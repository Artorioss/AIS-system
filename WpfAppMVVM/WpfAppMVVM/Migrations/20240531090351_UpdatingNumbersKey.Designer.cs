﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WpfAppMVVM.Models;

#nullable disable

namespace WpfAppMVVM.Migrations
{
    [DbContext(typeof(TransportationEntities))]
    [Migration("20240531090351_UpdatingNumbersKey")]
    partial class UpdatingNumbersKey
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CarDriver", b =>
                {
                    b.Property<string>("CarsNumber")
                        .HasColumnType("character varying(9)");

                    b.Property<int>("DriversDriverId")
                        .HasColumnType("integer");

                    b.HasKey("CarsNumber", "DriversDriverId");

                    b.HasIndex("DriversDriverId");

                    b.ToTable("CarDriver");
                });

            modelBuilder.Entity("DriverTrailler", b =>
                {
                    b.Property<int>("DriversDriverId")
                        .HasColumnType("integer");

                    b.Property<string>("TraillersNumber")
                        .HasColumnType("character varying(8)");

                    b.HasKey("DriversDriverId", "TraillersNumber");

                    b.HasIndex("TraillersNumber");

                    b.ToTable("DriverTrailler");
                });

            modelBuilder.Entity("RouteRoutePoint", b =>
                {
                    b.Property<int>("RoutePointsRoutePointId")
                        .HasColumnType("integer");

                    b.Property<int>("RoutesRouteId")
                        .HasColumnType("integer");

                    b.HasKey("RoutePointsRoutePointId", "RoutesRouteId");

                    b.HasIndex("RoutesRouteId");

                    b.ToTable("RouteRoutePoint");
                });

            modelBuilder.Entity("WpfAppMVVM.Model.Entities.Brand", b =>
                {
                    b.Property<int>("BrandId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("BrandId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.Property<string>("RussianBrandName")
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.HasKey("BrandId");

                    b.ToTable("Brands");
                });

            modelBuilder.Entity("WpfAppMVVM.Models.Entities.Car", b =>
                {
                    b.Property<string>("Number")
                        .HasMaxLength(9)
                        .HasColumnType("character varying(9)");

                    b.Property<int?>("BrandId")
                        .HasColumnType("integer");

                    b.Property<bool>("IsTruck")
                        .HasColumnType("boolean");

                    b.HasKey("Number");

                    b.HasIndex("BrandId");

                    b.ToTable("Cars");
                });

            modelBuilder.Entity("WpfAppMVVM.Models.Entities.Customer", b =>
                {
                    b.Property<int>("CustomerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("CustomerId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.HasKey("CustomerId");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("WpfAppMVVM.Models.Entities.Driver", b =>
                {
                    b.Property<int>("DriverId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("DriverId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.Property<int>("TransportCompanyId")
                        .HasColumnType("integer");

                    b.HasKey("DriverId");

                    b.HasIndex("TransportCompanyId");

                    b.ToTable("Drivers");
                });

            modelBuilder.Entity("WpfAppMVVM.Models.Entities.Route", b =>
                {
                    b.Property<int>("RouteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("RouteId"));

                    b.Property<string>("RouteName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.HasKey("RouteId");

                    b.ToTable("Routes");
                });

            modelBuilder.Entity("WpfAppMVVM.Models.Entities.RoutePoint", b =>
                {
                    b.Property<int>("RoutePointId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("RoutePointId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.HasKey("RoutePointId");

                    b.ToTable("RoutePoints");
                });

            modelBuilder.Entity("WpfAppMVVM.Models.Entities.StateOrder", b =>
                {
                    b.Property<int>("StateOrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("StateOrderId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.HasKey("StateOrderId");

                    b.ToTable("StateOrders");
                });

            modelBuilder.Entity("WpfAppMVVM.Models.Entities.Trailler", b =>
                {
                    b.Property<string>("Number")
                        .HasMaxLength(8)
                        .HasColumnType("character varying(8)");

                    b.Property<int?>("BrandId")
                        .HasColumnType("integer");

                    b.HasKey("Number");

                    b.HasIndex("BrandId");

                    b.ToTable("Traillers");
                });

            modelBuilder.Entity("WpfAppMVVM.Models.Entities.TransportCompany", b =>
                {
                    b.Property<int>("TransportCompanyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("TransportCompanyId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.HasKey("TransportCompanyId");

                    b.ToTable("TransportCompanies");
                });

            modelBuilder.Entity("WpfAppMVVM.Models.Entities.Transportation", b =>
                {
                    b.Property<int>("TransportationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("TransportationId"));

                    b.Property<string>("CarNumber")
                        .HasColumnType("character varying(9)");

                    b.Property<int>("CustomerId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("DateLoading")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("DriverId")
                        .HasColumnType("integer");

                    b.Property<decimal?>("PaymentToDriver")
                        .HasColumnType("numeric");

                    b.Property<decimal?>("Price")
                        .HasColumnType("numeric");

                    b.Property<int?>("RouteId")
                        .HasColumnType("integer");

                    b.Property<string>("RouteName")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)");

                    b.Property<int>("StateOrderId")
                        .HasColumnType("integer");

                    b.Property<string>("TraillerNumber")
                        .HasColumnType("character varying(8)");

                    b.Property<int?>("TransportCompanyId")
                        .HasColumnType("integer");

                    b.HasKey("TransportationId");

                    b.HasIndex("CarNumber");

                    b.HasIndex("CustomerId");

                    b.HasIndex("DriverId");

                    b.HasIndex("RouteId");

                    b.HasIndex("StateOrderId");

                    b.HasIndex("TraillerNumber");

                    b.HasIndex("TransportCompanyId");

                    b.ToTable("Transportations");
                });

            modelBuilder.Entity("CarDriver", b =>
                {
                    b.HasOne("WpfAppMVVM.Models.Entities.Car", null)
                        .WithMany()
                        .HasForeignKey("CarsNumber")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WpfAppMVVM.Models.Entities.Driver", null)
                        .WithMany()
                        .HasForeignKey("DriversDriverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DriverTrailler", b =>
                {
                    b.HasOne("WpfAppMVVM.Models.Entities.Driver", null)
                        .WithMany()
                        .HasForeignKey("DriversDriverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WpfAppMVVM.Models.Entities.Trailler", null)
                        .WithMany()
                        .HasForeignKey("TraillersNumber")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RouteRoutePoint", b =>
                {
                    b.HasOne("WpfAppMVVM.Models.Entities.RoutePoint", null)
                        .WithMany()
                        .HasForeignKey("RoutePointsRoutePointId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WpfAppMVVM.Models.Entities.Route", null)
                        .WithMany()
                        .HasForeignKey("RoutesRouteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WpfAppMVVM.Models.Entities.Car", b =>
                {
                    b.HasOne("WpfAppMVVM.Model.Entities.Brand", "Brand")
                        .WithMany("Cars")
                        .HasForeignKey("BrandId");

                    b.Navigation("Brand");
                });

            modelBuilder.Entity("WpfAppMVVM.Models.Entities.Driver", b =>
                {
                    b.HasOne("WpfAppMVVM.Models.Entities.TransportCompany", "TransportCompany")
                        .WithMany("Drivers")
                        .HasForeignKey("TransportCompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TransportCompany");
                });

            modelBuilder.Entity("WpfAppMVVM.Models.Entities.Trailler", b =>
                {
                    b.HasOne("WpfAppMVVM.Model.Entities.Brand", "Brand")
                        .WithMany("Traillers")
                        .HasForeignKey("BrandId");

                    b.Navigation("Brand");
                });

            modelBuilder.Entity("WpfAppMVVM.Models.Entities.Transportation", b =>
                {
                    b.HasOne("WpfAppMVVM.Models.Entities.Car", "Car")
                        .WithMany()
                        .HasForeignKey("CarNumber");

                    b.HasOne("WpfAppMVVM.Models.Entities.Customer", "Customer")
                        .WithMany("Transportations")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WpfAppMVVM.Models.Entities.Driver", "Driver")
                        .WithMany("Transportation")
                        .HasForeignKey("DriverId");

                    b.HasOne("WpfAppMVVM.Models.Entities.Route", "Route")
                        .WithMany("Transportation")
                        .HasForeignKey("RouteId");

                    b.HasOne("WpfAppMVVM.Models.Entities.StateOrder", "StateOrder")
                        .WithMany("Transportation")
                        .HasForeignKey("StateOrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WpfAppMVVM.Models.Entities.Trailler", "Trailler")
                        .WithMany()
                        .HasForeignKey("TraillerNumber");

                    b.HasOne("WpfAppMVVM.Models.Entities.TransportCompany", "TransportCompany")
                        .WithMany()
                        .HasForeignKey("TransportCompanyId");

                    b.Navigation("Car");

                    b.Navigation("Customer");

                    b.Navigation("Driver");

                    b.Navigation("Route");

                    b.Navigation("StateOrder");

                    b.Navigation("Trailler");

                    b.Navigation("TransportCompany");
                });

            modelBuilder.Entity("WpfAppMVVM.Model.Entities.Brand", b =>
                {
                    b.Navigation("Cars");

                    b.Navigation("Traillers");
                });

            modelBuilder.Entity("WpfAppMVVM.Models.Entities.Customer", b =>
                {
                    b.Navigation("Transportations");
                });

            modelBuilder.Entity("WpfAppMVVM.Models.Entities.Driver", b =>
                {
                    b.Navigation("Transportation");
                });

            modelBuilder.Entity("WpfAppMVVM.Models.Entities.Route", b =>
                {
                    b.Navigation("Transportation");
                });

            modelBuilder.Entity("WpfAppMVVM.Models.Entities.StateOrder", b =>
                {
                    b.Navigation("Transportation");
                });

            modelBuilder.Entity("WpfAppMVVM.Models.Entities.TransportCompany", b =>
                {
                    b.Navigation("Drivers");
                });
#pragma warning restore 612, 618
        }
    }
}