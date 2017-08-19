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
    partial class RoboContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("RoboHome.Models.Flip", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<int>("Direction");

                    b.Property<int>("Hour");

                    b.Property<int>("Minute");

                    b.Property<int?>("SwitchId");

                    b.Property<int>("TimeOfDay");

                    b.HasKey("Id");

                    b.HasIndex("SwitchId");

                    b.ToTable("Flips");
                });

            modelBuilder.Entity("RoboHome.Models.Remote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<string>("Location");

                    b.HasKey("Id");

                    b.ToTable("Remotes");
                });

            modelBuilder.Entity("RoboHome.Models.Switch", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<string>("Name");

                    b.Property<int>("OffPin");

                    b.Property<int>("OnPin");

                    b.Property<int?>("RemoteId");

                    b.Property<int>("State");

                    b.HasKey("Id");

                    b.HasIndex("RemoteId");

                    b.ToTable("Switches");
                });

            modelBuilder.Entity("RoboHome.Models.Flip", b =>
                {
                    b.HasOne("RoboHome.Models.Switch")
                        .WithMany("Flips")
                        .HasForeignKey("SwitchId");
                });

            modelBuilder.Entity("RoboHome.Models.Switch", b =>
                {
                    b.HasOne("RoboHome.Models.Remote")
                        .WithMany("Switches")
                        .HasForeignKey("RemoteId");
                });
        }
    }
}
