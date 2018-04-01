using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Design;
using Npgsql.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RoboHome.Models;

namespace RoboHome.Data
{
    public class RoboContext: DbContext
    {
        public DbSet<Remote> Remotes { get; set; }
        public DbSet<Switch> Switches { get; set; }

        public DbSet<Flip> Flips { get; set; }
        public DbSet<KeyTime> KeyTimes { get; set; }
        private RoboContext() {}
        public RoboContext(DbContextOptions<RoboContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<KeyTime>(t => {
                t.HasKey("Date");
            });

            builder.Entity<KeyTime>(t => {
                t.OwnsOne(c => c.Time);
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

            builder.Entity<Flip>(f => {
                f.OwnsOne(c => c.Time);
            });

            base.OnModelCreating(builder);
        }
    }

    public class RoboContextFactory : IDesignTimeDbContextFactory<RoboContext>
    {
        private string connectionString;
        public RoboContextFactory()
        {
            var path = Directory.GetCurrentDirectory() + "/appsettings.json";
            using (var file = File.Open(path, FileMode.Open))
            {
                using (var reader = new StreamReader(file)) 
                {
                    var text = reader.ReadToEnd();
                    var json = JObject.Parse(text);
                    var connectionString = (string)json["connectionString"];
                    if (connectionString != null) {
                        this.connectionString = connectionString;
                    } else {
                        throw new System.Exception($"{path} does not have a connectionString property");
                    }
                }
            }
        }
        RoboContext IDesignTimeDbContextFactory<RoboContext>.CreateDbContext(string[] args)
        {
            var ob = new DbContextOptionsBuilder<RoboContext>();
            ob.UseNpgsql(this.connectionString);
            return new RoboContext(ob.Options);
        }
    }
}