using Microsoft.EntityFrameworkCore;
using parkingLotAPI.Data;
using parkingLotAPI.Models;

namespace parkingLotAPI.Utils
{
    public class DbInitializer
    {
        public static void Initialize(AppDbContext dbContext)
        {
            if (dbContext.ParkingSpots.Any())
            {
                return;  
            }
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

    }
}
