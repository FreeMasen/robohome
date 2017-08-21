﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using RoboHome.Data;
using RoboHome.Models;
using System;

namespace projects.Migrations
{
    [DbContext(typeof(RoboContext))]
    [Migration("20170820044406_key_times")]
    partial class key_times
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452");

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

                    b.ToTable("Flip");
                });

            modelBuilder.Entity("RoboHome.Models.KeyTime", b =>
                {
                    b.Property<DateTime>("Date");

                    b.HasKey("Date");

                    b.ToTable("KeyTimes");
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

                    b.OwnsOne("RoboHome.Models.Time", "Time", b1 =>
                        {
                            b1.Property<int?>("FlipId");

                            b1.Property<int>("Hour");

                            b1.Property<int>("Minute");

                            b1.Property<int>("TimeOfDay");

                            b1.Property<int>("TimeType");

                            b1.ToTable("Flip");

                            b1.HasOne("RoboHome.Models.Flip")
                                .WithOne("Time")
                                .HasForeignKey("RoboHome.Models.Time", "FlipId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });

            modelBuilder.Entity("RoboHome.Models.KeyTime", b =>
                {
                    b.OwnsOne("RoboHome.Models.Time", "Time", b1 =>
                        {
                            b1.Property<DateTime>("KeyTimeDate");

                            b1.Property<int>("Hour");

                            b1.Property<int>("Minute");

                            b1.Property<int>("TimeOfDay");

                            b1.Property<int>("TimeType");

                            b1.ToTable("KeyTimes");

                            b1.HasOne("RoboHome.Models.KeyTime")
                                .WithOne("Time")
                                .HasForeignKey("RoboHome.Models.Time", "KeyTimeDate")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });

            modelBuilder.Entity("RoboHome.Models.Switch", b =>
                {
                    b.HasOne("RoboHome.Models.Remote")
                        .WithMany("Switches")
                        .HasForeignKey("RemoteId");
                });
#pragma warning restore 612, 618
        }
    }
}
