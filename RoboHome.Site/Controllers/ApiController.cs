using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoboHome.Data;
using RoboHome.Models;
using RoboHome.Services;

namespace RoboHome.Controllers
{
    public class ApiController: Controller
    {
        private readonly RoboContext _context;
        private readonly Messenger _messenger;

        public ApiController(RoboContext context, IMqClient mqClient) {
            this._context = context;
            this._messenger = (Messenger)mqClient;
        }

        [HttpGet("/api/remotes")]
        public async Task<IActionResult> Remotes() {
            try {
                var remotes = await this._context.Remotes
                                                .Include(r => r.Switches)
                                                .Include("Switches.Flips")
                                                .ToListAsync();
                return new ObjectResult(remotes);
            } catch(Exception ex) {
                return new BadRequestObjectResult(ex.Message);
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
                                                .Include("Switches.Flips.Time")
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
                if (remote.Id == -1) {
                    var newEntity = await this._context.Remotes
                                        .AddAsync(new Remote(){
                                            Location = remote.Location,
                                            Switches = remote.Switches
                                        });
                    await this._context.SaveChangesAsync();
                    Console.WriteLine("Remote added");
                    return new ObjectResult(newEntity.Entity);
                }
                else
                {
                    var dbRemote = await this._context
                                            .Remotes
                                            .Where(r => r.Id == remote.Id)
                                            .Include(r => r.Switches)
                                            .Include("Switches.Flips")
                                            .FirstOrDefaultAsync();
                    _context.Entry(dbRemote).CurrentValues.SetValues(remote);
                    foreach (var sw in dbRemote.Switches.ToList()) {
                        if (!remote.Switches.Any(s => s.Id == sw.Id)) {
                            _context.Switches.Remove(sw);
                        }
                    }
                    foreach (var sw in remote.Switches)
                    {
                        var dbSwitch = _context.Switches.SingleOrDefault(s => s.Id == sw.Id);
                        if (dbSwitch == null)
                        {
                            dbRemote.Switches.Add(new Switch() {
                                Name = sw.Name,
                                Number = sw.Number,
                                Flips = sw.Flips,
                                OffPin = sw.OffPin,
                                OnPin = sw.OnPin,
                                State = sw.State,
                                RemoteId = dbRemote.Id
                            });
                        }
                        else
                        {
                            foreach (var flip in sw.Flips)
                            {
                                var dbFlip = _context.Flips.SingleOrDefault(f => f.Id == flip.Id);
                                if (dbFlip == null)
                                {
                                    dbSwitch.Flips.Add(new Flip() {
                                        Direction = flip.Direction,
                                        Time = new Time() {
                                            Hour = flip.Time.Hour,
                                            Minute = flip.Time.Minute,
                                            TimeOfDay = flip.Time.TimeOfDay
                                        },
                                    });
                                }
                                else {
                                    if (flip.SwitchId != sw.Id)
                                    {
                                        flip.SwitchId = sw.Id;
                                    }
                                    _context.Entry(dbFlip).CurrentValues.SetValues(flip);
                                    _context.Entry(dbFlip.Time).CurrentValues.SetValues(flip.Time);
                                }
                            }
                            if (sw.RemoteId != remote.Id)
                            {
                                sw.RemoteId = remote.Id;
                            }
                            _context.Entry(dbSwitch).CurrentValues.SetValues(sw);
                        }

                    }
                    await _context.SaveChangesAsync();
                    return new ObjectResult(dbRemote);
                }
            }
            catch (Exception ex)
            {
                var msg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                Console.WriteLine("Error saving changes {0}", msg);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("/api/delete/remote/{id}")]
        public async Task<IActionResult> DeleteRemote(int id)
        {
            try {
                var remote = await this._context.Remotes
                                        .Where(r => r.Id == id)
                                        .Include(r => r.Switches)
                                        .Include("Switches.Flips")
                                        .FirstOrDefaultAsync();
                Console.WriteLine("Remote to delete {0}", remote);
                foreach (var sw in remote.Switches) {
                    this._context.Switches.Remove(sw);
                }
                this._context.Remotes.Remove(remote);
                await this._context.SaveChangesAsync();
                return new ObjectResult(null);
            } catch (Exception ex) {
                Console.WriteLine("Error deleting remote {0}", ex.Message);
                return new ObjectResult(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Flip(int switchId, SwitchState newState)
        {
            try 
            {
                var remote = this._context.Remotes
                                            .Include(r => r.Switches)
                                            .Where(r => r.Switches
                                                        .Select(s => s.Id)
                                                        .Contains(switchId))
                                            .FirstOrDefault();
                var sw = remote.Switches.Where(s => s.Id == switchId).FirstOrDefault();
                sw.State = newState;
                _context.Entry(sw).CurrentValues.SetValues(sw);
                this._messenger.SendMessage(remote.Id, new {switch_id = sw.Number, direction = newState});
                this._context.SaveChanges();
                return new ObjectResult(null);
            } 
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
                return new ObjectResult(ex.Message);
            }
        }

#region Helpers
        private void RemoveSwitches(Remote remote, Remote dbRemote)
        {
            Console.WriteLine("RemoteSwitches");
            if (dbRemote.Switches.Count() > remote.Switches.Count())
            {
                var switchesToDelete = dbRemote.Switches.Where(s =>
                {
                    return !remote.Switches.Select(t => t.Id).Contains(s.Id);
                });
                this._context.RemoveRange(switchesToDelete);
            }
        }

        private static void UpdateFlip(Flip flip, Flip dbFlip)
        {
            Console.WriteLine("UpdateFlip");
            dbFlip.Direction = flip.Direction;
            dbFlip.Time.Hour = flip.Time.Hour;
            dbFlip.Time.Minute = flip.Time.Minute;
            dbFlip.Time.TimeOfDay = flip.Time.TimeOfDay;
        }
    }
#endregion
}