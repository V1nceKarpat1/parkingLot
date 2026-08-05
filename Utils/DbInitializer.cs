using Microsoft.EntityFrameworkCore;
using parkingLotAPI.Data;
using parkingLotAPI.Models;

namespace parkingLotAPI.Utils
{
    public class DbInitializer
    {
        public static void Initialize(AppDbContext dbContext)
        {
            if (!dbContext.ParkingSpots.Any())
            {
              
                char level = 'A';
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        dbContext.ParkingSpots.Add(new ParkingSpot { SpotID = $"{(char)(level+i)}{j}" });
                    }
                }
                dbContext.SaveChanges();
            }

           
            if (!dbContext.Reservations.Any())
            {
                Reservation[] initReservations =
                {
                    new Reservation{ SpotID = "A4", StartTime = new DateTime(new DateOnly(2026,04,12),new TimeOnly(23,45,23)), EndTime = new DateTime(new DateOnly(2026,06,10),new TimeOnly(10,01,20)),CustomerName = "Frank"},
                    new Reservation{ SpotID = "B2", StartTime = new DateTime(new DateOnly(2026,03,10),new TimeOnly(10,01,20)), EndTime = new DateTime(new DateOnly(2026,07,10),new TimeOnly(10,01,20)),CustomerName = "Lisa"},
                    new Reservation{ SpotID = "A3", StartTime = new DateTime(new DateOnly(2026,07,23),new TimeOnly(23,32,54)), EndTime = DateTime.Now.AddHours(1),CustomerName = "Mary"},
                    new Reservation{ SpotID = "C6", StartTime = new DateTime(new DateOnly(2026,02,21),new TimeOnly(22,22,03)), EndTime = DateTime.Now.AddHours(3),CustomerName = "Dan"},
                    new Reservation{ SpotID = "E1", StartTime = new DateTime(new DateOnly(2026,03,10),new TimeOnly(03,23,54)), EndTime = DateTime.Now.AddDays(7),CustomerName = "Peter"}
                };
                dbContext.Reservations.AddRange(initReservations);
                dbContext.SaveChanges();
            }

          

        }

    }
}
