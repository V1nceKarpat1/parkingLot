using Microsoft.EntityFrameworkCore;
using parkingLotAPI.Models;

namespace parkingLotAPI.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<ParkingSpot> ParkingSpots { get; set; }
        public DbSet<Reservation> Reservations { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<ParkingSpot>().HasKey("SpotID");

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.ParkingSpot)
                .WithMany(p => p.ReservationHistory)
                .HasForeignKey(r => r.SpotID);
          
        }
    }
}
