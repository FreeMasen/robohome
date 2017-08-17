using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Design;
using Npgsql.EntityFrameworkCore;
using RoboHome.Models;

namespace RoboHome.Data
{
    public class RoboContext: DbContext
    {
        public DbSet<Remote> Remotes { get; set; }
        public DbSet<Switch> Switches { get; set; }
        public DbSet<Flip> Flips { get; set; }
        public DbSet<Flip> PendingFlips { get; set; }
        public DbSet<KeyTimes> KeyTimes { get; set; }

        public RoboContext(DbContextOptions<RoboContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<KeyTimes>(t => {
                t.HasKey("Date");
            });

            builder.Entity<Remote>(r => {
                r.Property<int>("Id")
                    .UseNpgsqlSerialColumn();
            });

            builder.Entity<Switch>(s => {
                s.Property<int>("Id")
                    .UseNpgsqlSerialColumn();
            });
            builder.Entity<Flip>(f => {
                f.Property<int>("Id")
                    .UseNpgsqlSerialColumn();
            });
            base.OnModelCreating(builder);
        }
    }
}