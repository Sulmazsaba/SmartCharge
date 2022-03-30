using Microsoft.EntityFrameworkCore;
using SmartCharge.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCharge.Infrastructure.Persistence
{
    public class SmartChargeContext : DbContext
    {
        public SmartChargeContext(DbContextOptions<SmartChargeContext> options) : base(options)
        {

        }

        public DbSet<Group> Groups { get; set; }
        public DbSet<ChargeStation> ChargeStations { get; set; }
        public DbSet<Connector> Connectors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChargeStation>()
                .HasOne(s => s.Group)
                .WithMany(a => a.ChargeStations)
                .IsRequired()
               .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Connector>()
            .HasKey(nameof(Connector.Id), nameof(Connector.ChargeStationId));

            base.OnModelCreating(modelBuilder);
        }


    }
}
