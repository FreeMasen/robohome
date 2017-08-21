using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using RoboHome.Data;
using RoboHome.Models;

namespace projects.Migrations
{
    [DbContext(typeof(RoboContext))]
    [Migration("20170719035859_pin_correction")]
    partial class pin_correction
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("RoboHome.Models.Flip", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Direction");

                    b.Property<int>("Hour");

                    b.Property<int?>("LightId");

                    b.Property<int>("Minute");

                    b.Property<int>("TimeOfDay");

                    b.HasKey("Id");

                    b.HasIndex("LightId");

                    b.ToTable("Flips");
                });

            modelBuilder.Entity("RoboHome.Models.Light", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<int>("OffPin");

                    b.Property<int>("OnPin");

                    b.Property<int>("State");

                    b.HasKey("Id");

                    b.ToTable("Lights");
                });

            modelBuilder.Entity("RoboHome.Models.Flip", b =>
                {
                    b.HasOne("RoboHome.Models.Light")
                        .WithMany("Flips")
                        .HasForeignKey("LightId");
                });
        }
    }
}
