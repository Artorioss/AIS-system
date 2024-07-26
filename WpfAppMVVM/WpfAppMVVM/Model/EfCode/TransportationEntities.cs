using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using WpfAppMVVM.Model.EfCode.Entities;

namespace WpfAppMVVM.Model.EfCode
{
    public class TransportationEntities : DbContext
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarBrand> CarBrands { get; set; }
        public DbSet<TraillerBrand> TraillerBrands { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<RoutePoint> RoutePoints { get; set; }
        public DbSet<StateOrder> StateOrders { get; set; }
        public DbSet<Trailler> Traillers { get; set; }
        public DbSet<Transportation> Transportations { get; set; }
        public DbSet<TransportCompany> TransportCompanies { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<StateFilter> StateFilter { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Database=Transportations;Username=postgres;Password=qwerty");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
            v => v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime(),
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            var nullableDateTimeConverter = new ValueConverter<DateTime?, DateTime?>(
                v => v.HasValue ? v.Value.Kind == DateTimeKind.Utc ? v : v.Value.ToUniversalTime() : v,
                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);

            modelBuilder.Entity<Transportation>()
                .Property(t => t.DateLoading)
                .HasConversion(nullableDateTimeConverter);

            modelBuilder.Entity<Transportation>().HasQueryFilter(t => !t.SoftDeleted);
            modelBuilder.Entity<CarBrand>().HasQueryFilter(t => !t.SoftDeleted);
            modelBuilder.Entity<TraillerBrand>().HasQueryFilter(t => !t.SoftDeleted);
            modelBuilder.Entity<Customer>().HasQueryFilter(t => !t.SoftDeleted);
            modelBuilder.Entity<Driver>().HasQueryFilter(t => !t.SoftDeleted);
            modelBuilder.Entity<Route>().HasQueryFilter(t => !t.SoftDeleted);
            modelBuilder.Entity<RoutePoint>().HasQueryFilter(t => !t.SoftDeleted);
            modelBuilder.Entity<StateOrder>().HasQueryFilter(t => !t.SoftDeleted);
            modelBuilder.Entity<Trailler>().HasQueryFilter(t => !t.SoftDeleted);
            modelBuilder.Entity<TransportCompany>().HasQueryFilter(t => !t.SoftDeleted);
            modelBuilder.Entity<PaymentMethod>().HasQueryFilter(t => !t.SoftDeleted);
            modelBuilder.Entity<Car>().HasQueryFilter(t => !t.SoftDeleted);
            modelBuilder.Entity<StateFilter>().HasQueryFilter(t => !t.SoftDeleted);
        }
    }
}
