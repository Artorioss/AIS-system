﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using WpfAppMVVM.Model.Entities;
using WpfAppMVVM.Models.Entities;

namespace WpfAppMVVM.Models
{
    public class TransportationEntities: DbContext
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<RoutePoint> RoutePoints { get; set; }
        public DbSet<StateOrder> StateOrders { get; set; }
        public DbSet<Trailler> Traillers { get; set; }
        public DbSet<Transportation> Transportations { get; set; }
        public DbSet<TransportCompany> TransportCompanies { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Database=Transportations;Username=postgres;Password=qwerty");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            //var converter = new ValueConverter<DateTime, DateTime>
            //    (
            //        toDb => toDb.Date,
            //        fromDb => fromDb
            //    );

            //modelBuilder.Entity<Transportation>().Property(t => t.DateLoading).HasConversion(converter);
        }
    }
}
