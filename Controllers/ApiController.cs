using System;
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
                foreach (var sw in remote.Switches) 
                {
                    
                    var dbSwitch = await this._context
                                        .Switches
                                        .Where(s => s.Id == sw.Id)
                                        .FirstAsync();
                    if (dbSwitch == null) {
                        await this._context.Flips.AddRangeAsync(sw.Flips);
                        await this._context.Switches.AddAsync(sw);
                    } else {
                        foreach (var flip in sw.Flips)
                        {
                            var dbFlip = await this._context.Flips
                                                            .Where(f => f.Id == flip.Id)
                                                            .FirstOrDefaultAsync();
                            if (dbFlip == null) 
                            {
                                dbSwitch.Flips.Add(flip);
                            } else {
                                dbFlip.Direction = flip.Direction;
                                dbFlip.Hour = flip.Hour;
                                dbFlip.Minute = flip.Minute;
                                dbFlip.TimeOfDay = flip.TimeOfDay;
                            }
                        }
                        dbSwitch.Name = sw.Name;
                        dbSwitch.OffPin = sw.OffPin;
                        dbSwitch.OnPin = sw.OnPin;
                        dbSwitch.State = sw.State;
                    }
                }
                dbRemote.Location = remote.Location;
                await this._context.SaveChangesAsync();
                return new ObjectResult(null);
            }
             catch (Exception ex) 
            {
                return new ObjectResult(ex.Message);
            }
        }

    }
}