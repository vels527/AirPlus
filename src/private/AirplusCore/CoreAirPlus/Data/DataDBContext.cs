using Microsoft.EntityFrameworkCore;
using CoreAirPlus.Entities;
namespace CoreAirPlus.Data
{
    public class DataDBContext : DbContext
    {
        public DbSet<CCompany> ccompanies { get; set; }
        public DbSet<Guest> guests { get; set; }
        public DbSet<Host> hosts { get; set; }
        public DbSet<Property> properties { get; set; }
        public DbSet<Reservation> reservations { get; set; }

        public DataDBContext(DbContextOptions<DataDBContext> options):base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Guest>().HasMany(c => c.reservations).WithOne(e => e.guest).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Property>().HasMany(c => c.reservations).WithOne(e => e.property).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<CCompany>().HasMany(c => c.reservations).WithOne(e => e.cCompany).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Reservation>().HasKey(t=>new { t.GuestId,t.PropertyId,t.CheckIn,t.CheckOut});
            modelBuilder.Entity<Host>().HasMany(c => c.properties).WithOne(e => e.host).OnDelete(DeleteBehavior.Cascade);
            //foreach(var relationship in modelBuilder.Model.GetEntityTypes().SE)
        }
    }
}