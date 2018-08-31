﻿// <auto-generated />
using CoreAirPlus.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace CoreAirPlus.Migrations
{
    [DbContext(typeof(DataDBContext))]
    [Migration("20180831173947_CalendarDays")]
    partial class CalendarDays
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CoreAirPlus.Entities.CalendarPrice", b =>
                {
                    b.Property<long>("ListingID");

                    b.Property<DateTime>("CalendarDate");

                    b.Property<bool>("IsAvailable");

                    b.Property<decimal>("Price");

                    b.Property<int?>("PropertyId");

                    b.HasKey("ListingID", "CalendarDate");

                    b.HasAlternateKey("CalendarDate", "ListingID");

                    b.HasIndex("PropertyId");

                    b.ToTable("calendarPrices");
                });

            modelBuilder.Entity("CoreAirPlus.Entities.CleaningCompany", b =>
                {
                    b.Property<int>("CleaningCompanyId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Phone");

                    b.Property<string>("Url");

                    b.HasKey("CleaningCompanyId");

                    b.ToTable("ccompanies");
                });

            modelBuilder.Entity("CoreAirPlus.Entities.Guest", b =>
                {
                    b.Property<int>("GuestId")
                        .ValueGeneratedOnAdd();

                    b.Property<byte?>("Age");

                    b.Property<DateTime?>("DOB");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("FullName")
                        .IsRequired();

                    b.Property<string>("LastName");

                    b.Property<string>("Phone");

                    b.Property<string>("Remarks");

                    b.Property<string>("Tag");

                    b.HasKey("GuestId");

                    b.ToTable("guests");
                });

            modelBuilder.Entity("CoreAirPlus.Entities.Host", b =>
                {
                    b.Property<int>("HostId")
                        .ValueGeneratedOnAdd();

                    b.Property<byte?>("Age");

                    b.Property<DateTime?>("DOB");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName");

                    b.Property<string>("FullName");

                    b.Property<string>("LastName");

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<string>("Phone");

                    b.Property<string>("Remarks");

                    b.Property<string>("Username")
                        .IsRequired();

                    b.HasKey("HostId");

                    b.ToTable("hosts");
                });

            modelBuilder.Entity("CoreAirPlus.Entities.Property", b =>
                {
                    b.Property<int>("PropertyId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address")
                        .IsRequired();

                    b.Property<int>("HostId");

                    b.Property<string>("IcalUrl");

                    b.Property<long>("ListingId");

                    b.HasKey("PropertyId");

                    b.HasIndex("HostId");

                    b.ToTable("properties");
                });

            modelBuilder.Entity("CoreAirPlus.Entities.Reservation", b =>
                {
                    b.Property<int>("GuestId");

                    b.Property<int>("PropertyId");

                    b.Property<DateTime>("CheckIn");

                    b.Property<DateTime>("CheckOut");

                    b.Property<int?>("CleaningCompanyId");

                    b.Property<DateTime?>("CleaningTime");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<DateTime?>("RCheckIn");

                    b.Property<DateTime?>("RCheckOut");

                    b.Property<string>("Remarks");

                    b.Property<string>("status");

                    b.HasKey("GuestId", "PropertyId", "CheckIn", "CheckOut");

                    b.HasAlternateKey("CheckIn", "CheckOut", "GuestId", "PropertyId");

                    b.HasIndex("CleaningCompanyId");

                    b.HasIndex("PropertyId");

                    b.ToTable("reservations");
                });

            modelBuilder.Entity("CoreAirPlus.ViewModel.ReservationViewModel", b =>
                {
                    b.Property<int>("GuestId");

                    b.Property<int>("PropertyId");

                    b.Property<DateTime>("CheckIn");

                    b.Property<DateTime>("CheckOut");

                    b.Property<int?>("CleaningCompanyId");

                    b.Property<DateTime?>("CleaningTime");

                    b.Property<string>("GuestName");

                    b.Property<string>("RCheckIn");

                    b.Property<string>("RCheckOut");

                    b.Property<string>("Remarks");

                    b.Property<string>("Status");

                    b.HasKey("GuestId", "PropertyId", "CheckIn", "CheckOut");

                    b.HasAlternateKey("CheckIn", "GuestId", "PropertyId");

                    b.ToTable("ReservationViewModel");
                });

            modelBuilder.Entity("CoreAirPlus.Entities.CalendarPrice", b =>
                {
                    b.HasOne("CoreAirPlus.Entities.Property", "property")
                        .WithMany("CalendarPrices")
                        .HasForeignKey("PropertyId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("CoreAirPlus.Entities.Property", b =>
                {
                    b.HasOne("CoreAirPlus.Entities.Host", "host")
                        .WithMany("properties")
                        .HasForeignKey("HostId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CoreAirPlus.Entities.Reservation", b =>
                {
                    b.HasOne("CoreAirPlus.Entities.CleaningCompany", "CleaningCompany")
                        .WithMany("reservations")
                        .HasForeignKey("CleaningCompanyId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("CoreAirPlus.Entities.Guest", "guest")
                        .WithMany("reservations")
                        .HasForeignKey("GuestId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CoreAirPlus.Entities.Property", "property")
                        .WithMany("reservations")
                        .HasForeignKey("PropertyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}