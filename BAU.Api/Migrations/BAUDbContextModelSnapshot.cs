﻿// <auto-generated />
using BAU.Api.DAL.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace BAU.Api.Migrations
{
    [DbContext(typeof(BAUDbContext))]
    partial class BAUDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BAU.Api.DAL.Models.Engineer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Engineers");
                });

            modelBuilder.Entity("BAU.Api.DAL.Models.EngineerShift", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date")
                        .HasColumnType("Date");

                    b.Property<byte>("Duration");

                    b.Property<int>("EngineerId");

                    b.HasKey("Id");

                    b.HasIndex("EngineerId");

                    b.ToTable("EngineersShifts");
                });

            modelBuilder.Entity("BAU.Api.DAL.Models.EngineerShift", b =>
                {
                    b.HasOne("BAU.Api.DAL.Models.Engineer", "Engineer")
                        .WithMany("Shifts")
                        .HasForeignKey("EngineerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
