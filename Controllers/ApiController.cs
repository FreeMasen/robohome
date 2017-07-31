using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoboHome.Data;

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

    }
}