using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoboHome.Data;
using RoboHome.Models;

namespace RoboHome.Controllers
{
    public class ApiController: Controller
    {
        private readonly RoboContext _context;

        public ApiController(RoboContext context) {
            this._context = context;
        }

        [HttpGet("/api/remotes")]
        public async Task<IActionResult> Remotes() {
            System.Console.WriteLine("Remotes");
            try {
                var remotes = await this._context.Remotes
                                                .Include(r => r.Switches)
                                                .Include("Switches.Flips")
                                                .ToListAsync();
                return new ObjectResult(remotes);
            } catch(Exception ex) {
                return new ObjectResult(ex.Message);
            }
        }

        [HttpGet("/api/remote/{id}")]
        public async Task<IActionResult> Remote(int id)
        {
            try {
                var remote = await this._context.Remotes
                                                .Where(r => r.Id == id)
                                                .Include(r => r.Switches)
                                                .Include("Switches.Flips")
                                                .FirstAsync();
                return new ObjectResult(remote);
            } catch(Exception ex) {
                return new ObjectResult(ex.Message);
            }
        }

        [HttpPost("/api/remote")]
        public async Task<IActionResult> Remote([FromBody] Remote remote)
        {
            try
            {
                var dbRemote = await this._context
                                        .Remotes
                                        .Where(r => r.Id == remote.Id)
                                        .Include(r => r.Switches)
                                        .Include("Switches.Flips")
                                        .FirstAsync();
                if (dbRemote == null) {
                    await this._context.Remotes.AddAsync(remote);
                }
                else 
                {
                    await this.UpdateAndAddSwitches(remote.Switches);
                    this.RemoveSwitches(remote, dbRemote);
                    dbRemote.Location = remote.Location;
                }
                await this._context.SaveChangesAsync();
                return new ObjectResult(null);
            }
            catch (Exception ex) 
            {
                return new ObjectResult(ex.Message);
            }
        }

        private void RemoveSwitches(Remote remote, Remote dbRemote)
        {
            if (dbRemote.Switches.Count() > remote.Switches.Count())
            {
                var switchesToDelete = dbRemote.Switches.Where(s =>
                {
                    return !remote.Switches.Select(t => t.Id).Contains(s.Id);
                });
                this._context.RemoveRange(switchesToDelete);
            }
        }

        private async Task UpdateAndAddSwitches(List<Switch> clientSwitches) 
        {
            foreach (var sw in clientSwitches) 
            {
                var dbSwitch = await this._context
                                    .Switches
                                    .Where(s => s.Id == sw.Id)
                                    .FirstAsync();
                if (dbSwitch == null)
                {
                    await AddSwitch(sw);
                }
                else
                {
                    await UpdateSwitch(sw, dbSwitch);
                }
            }
        }

        private async Task AddSwitch(Switch sw)
        {
            await this._context.Flips.AddRangeAsync(sw.Flips);
            await this._context.Switches.AddAsync(sw);
        }

        private async Task UpdateSwitch(Switch sw, Switch dbSwitch)
        {
            await UpdateAndAddFlips(sw.Flips);
            dbSwitch.Name = sw.Name;
            dbSwitch.OffPin = sw.OffPin;
            dbSwitch.OnPin = sw.OnPin;
            dbSwitch.State = sw.State;
            RemoveFlips(sw, dbSwitch);
        }

        private void RemoveFlips(Switch sw, Switch dbSwitch)
        {
            if (dbSwitch.Flips.Count() > sw.Flips.Count())
            {
                var flipsToDelete = dbSwitch.Flips.Where(f =>
                {
                    return !sw.Flips.Select(t => t.Id).Contains(f.Id);
                });
                this._context.Flips.RemoveRange(flipsToDelete);
            }
        }

        private async Task UpdateAndAddFlips(List<Flip> flips)
        {
            foreach (var flip in flips)
            {
                var dbFlip = await this._context.Flips
                                                .Where(f => f.Id == flip.Id)
                                                .FirstOrDefaultAsync();
                if (dbFlip == null) 
                {
                    this._context.Flips.Add(flip);
                } 
                else
                {
                    UpdateFlip(flip, dbFlip);
                }
            }
        }

        private static void UpdateFlip(Flip flip, Flip dbFlip)
        {
            dbFlip.Direction = flip.Direction;
            dbFlip.Hour = flip.Hour;
            dbFlip.Minute = flip.Minute;
            dbFlip.TimeOfDay = flip.TimeOfDay;
        }
    }
}