﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using WpfAppMVVM.Models.Entities;

namespace WpfAppMVVM.Models
{
    public class TransportationEntities: DbContext
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarBrand> CarBrands { get; set; }
        public DbSet<TraillerBrand> TraillerBrands { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<RoutePoint> RoutePoints { get; set; }
        public DbSet<RussianBrandName> RussianBrandNames { get; set; }
        public DbSet<StateOrder> StateOrders { get; set; }
        public DbSet<Trailler> Traillers { get; set; }
        public DbSet<Transportation> Transportations { get; set; }
        public DbSet<TransportCompany> TransportCompanies { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Database=Transportations;Username=postgres;Password=qwerty");
        }
    }
}
