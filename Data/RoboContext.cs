using Microsoft.EntityFrameworkCore;
using RoboHome.Models;

namespace RoboHome.Data
{
    public class RoboContext: DbContext
    {
        public DbSet<Light> Lights;
        private DbSet<Flip> Flips;
    }
}