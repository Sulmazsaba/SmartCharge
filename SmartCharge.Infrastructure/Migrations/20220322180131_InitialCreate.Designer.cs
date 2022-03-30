﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SmartCharge.Infrastructure.Persistence;

#nullable disable

namespace SmartCharge.Infrastructure.Migrations
{
    [DbContext(typeof(SmartChargeContext))]
    [Migration("20220322180131_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("SmartCharge.Domain.Entities.ChargeStation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("ChargeStations");
                });

            modelBuilder.Entity("SmartCharge.Domain.Entities.Connector", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<int>("ChargeStationId")
                        .HasColumnType("int");

                    b.Property<int>("MaxCurrentInAmps")
                        .HasColumnType("int");

                    b.HasKey("Id", "ChargeStationId");

                    b.HasIndex("ChargeStationId");

                    b.ToTable("Connectors");
                });

            modelBuilder.Entity("SmartCharge.Domain.Entities.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CapacityInAmps")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("SmartCharge.Domain.Entities.ChargeStation", b =>
                {
                    b.HasOne("SmartCharge.Domain.Entities.Group", "Group")
                        .WithMany("ChargeStations")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");
                });

            modelBuilder.Entity("SmartCharge.Domain.Entities.Connector", b =>
                {
                    b.HasOne("SmartCharge.Domain.Entities.ChargeStation", "ChargeStation")
                        .WithMany("Connectors")
                        .HasForeignKey("ChargeStationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ChargeStation");
                });

            modelBuilder.Entity("SmartCharge.Domain.Entities.ChargeStation", b =>
                {
                    b.Navigation("Connectors");
                });

            modelBuilder.Entity("SmartCharge.Domain.Entities.Group", b =>
                {
                    b.Navigation("ChargeStations");
                });
#pragma warning restore 612, 618
        }
    }
}