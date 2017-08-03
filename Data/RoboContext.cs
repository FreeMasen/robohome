using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Npgsql.EntityFrameworkCore;
using RoboHome.Models;

namespace RoboHome.Data
{
    public class RoboContext: DbContext
    {
        public DbSet<Remote> Remotes { get; set; }
        public DbSet<Switch> Switches { get; set; }
        public DbSet<Flip> Flips { get; set; }

        public RoboContext(DbContextOptions<RoboContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}