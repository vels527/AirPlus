using Microsoft.EntityFrameworkCore;
using CoreAirPlus.Entities;
using CoreAirPlus.ViewModel;
namespace CoreAirPlus.Data
{
    public class DataDBContext : DbContext
    {
        public DbSet<CleaningCompany> ccompanies { get; set; }
        public DbSet<Guest> guests { get; set; }
        public DbSet<Host> hosts { get; set; }
        public DbSet<Property> properties { get; set; }
        public DbSet<Reservation> reservations { get; set; }
        public DbSet<CalendarPrice> calendarPrices { get; set; }
        public DbSet<Listing> Listings { get; set; }

        public DataDBContext(DbContextOptions<DataDBContext> options):base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Guest>().HasMany(c => c.reservations).WithOne(e => e.guest).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Property>().HasMany(c => c.reservations).WithOne(e => e.property).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<CleaningCompany>().HasMany(c => c.reservations).WithOne(e => e.CleaningCompany).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Reservation>().HasKey(t=>new { t.GuestId,t.PropertyId,t.CheckIn,t.CheckOut});
            modelBuilder.Entity<CalendarPrice>().HasKey(c => new { c.ListingId,c.CalendarDate});
            modelBuilder.Entity<Listing>().HasKey(c=>new { c.ListingId,c.PropertyId});
            modelBuilder.Entity<ReservationViewModel>().HasKey(t => new { t.GuestId, t.PropertyId, t.CheckIn, t.CheckOut });
            modelBuilder.Entity<HostViewModel>().HasKey(t=>new { t.FirstName,t.LastName,t.Email,t.Password});
            modelBuilder.Entity<Host>().HasMany(c => c.properties).WithOne(e => e.host).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Property>().HasMany(c => c.Listings).WithOne(e => e.property).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Listing>().HasMany(c => c.CalendarDetail).WithOne(e => e.ListingDetail).HasForeignKey(e=>new { e.ListingId,e.PropertyId}).OnDelete(DeleteBehavior.SetNull);
            //foreach(var relationship in modelBuilder.Model.GetEntityTypes().SE)
        }

        public DbSet<CoreAirPlus.ViewModel.ReservationViewModel> ReservationViewModel { get; set; }
        public DbSet<CoreAirPlus.ViewModel.HostViewModel> HostsViewModel { get; set; }
    }
}