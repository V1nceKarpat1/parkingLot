
using Microsoft.EntityFrameworkCore;

using parkingLotAPI.Utils;
using parkingLotAPI.Data;
using parkingLotAPI.DTOs;
using parkingLotAPI.Models;
using parkingLotAPI.Services;

namespace parkingLotTest
{
    public class ParkingLotTest
    {
        private ParkingSpot[] InitSpotData()
        {
            DateTime nowTime = DateTime.UtcNow;
            return new ParkingSpot[]{
                new ParkingSpot
                {
                    SpotID = "A1",
                    ReservationHistory = new List<Reservation>
                    {
                        new Reservation {ReservationID = 1,EndTime = nowTime.AddMinutes(30)},
                        new Reservation {ReservationID = 2,EndTime = new DateTime(new DateOnly(2022,10,3),new TimeOnly(23,42,13))}
                    }
                },
                new ParkingSpot
                {
                    SpotID = "C1",
                    ReservationHistory = new List<Reservation>
                    {
                        new Reservation {ReservationID = 3, EndTime = new DateTime(new DateOnly(2023,12,3),new TimeOnly(20,5,13))}
                    },
                },
                new ParkingSpot
                {
                    SpotID = "D4",
                    ReservationHistory = new List<Reservation>
                    {
                        new Reservation {ReservationID = 4,EndTime = new DateTime(new DateOnly(2023,11,30),new TimeOnly(10,40,5))},
                        new Reservation {ReservationID = 5,EndTime = new DateTime(new DateOnly(2022,05,20),new TimeOnly(9,50,46))},
                        new Reservation {ReservationID = 6,EndTime = new DateTime(new DateOnly(2000,03,6),new TimeOnly(1,4,50))},
                        new Reservation {ReservationID = 7,EndTime = new DateTime(new DateOnly(2022,10,3),new TimeOnly(6,1,13))}
                    }
                },
                new ParkingSpot
                {
                    SpotID = "H4",
                },
                new ParkingSpot
                {
                    SpotID = "D9",
                    ReservationHistory = new List<Reservation>
                    {
                        new Reservation {ReservationID = 8, EndTime = new DateTime(new DateOnly(2023,11,30),new TimeOnly(10,40,5))},
                    }
                },

            };

        }

        private async Task<AppDbContext> GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);
            context.ParkingSpots.AddRange(InitSpotData());
            await context.SaveChangesAsync();
            return context;
        }
        [Fact]
        public async Task GetAllSpotsTest()
        {
            using var context = await GetInMemoryDbContext();
            var service = new ReservationService(context);


            var result = await service.GetAllSpotsAsync();

            Assert.NotNull(result.ResponseData);
            Assert.True(result.ResponseData[0].IsOccupied);
            Assert.False(result.ResponseData[1].IsOccupied);
        }
        [Fact]
        public async Task GetIdSpotTest()
        {
            using var context = await GetInMemoryDbContext();
            var service = new ReservationService(context);


            var resultTrue = await service.GetSpotByIdAsync("C1");
            var resultFalse = await service.GetSpotByIdAsync("F7");

            Assert.NotNull(resultTrue.ResponseData);
            Assert.True(resultTrue.ResponseData.SpotID == "C1");
            Assert.Null(resultFalse.ResponseData);
            Assert.True(resultFalse.Message == "Spot with ID: F7 not found");
        }
        [Fact]
        public async Task PostReservationTest()
        {
            DateTime nowTime = DateTime.UtcNow;
            using var context = await GetInMemoryDbContext();
            var service = new ReservationService(context);

            CreateReservationInfo validInfo = new CreateReservationInfo
            {
                SpotId = "H4",
                StartTime = nowTime.AddSeconds(30),
                EndTime = nowTime.AddHours(2),
                CustomerName = "Frank"
            };
            CreateReservationInfo inValidIdnfo = new CreateReservationInfo
            {
                SpotId = "H34",
                StartTime = nowTime.AddSeconds(30),
                EndTime = nowTime.AddHours(2),
                CustomerName = "Frank"
            };
            CreateReservationInfo inValidNameInfo = new CreateReservationInfo
            {
                SpotId = "H4",
                StartTime = nowTime.AddSeconds(30),
                EndTime = nowTime.AddHours(2)
            };
            CreateReservationInfo inValidStartInfo = new CreateReservationInfo
            {
                SpotId = "H4",
                StartTime = nowTime.AddHours(-1),
                EndTime = nowTime.AddHours(2),
                CustomerName = "Frank"
            };
            CreateReservationInfo inValidEndInfo = new CreateReservationInfo
            {
                SpotId = "H4",
                StartTime = nowTime.AddSeconds(30),
                EndTime = nowTime.AddHours(-1),
                CustomerName = "Frank"
            };
            CreateReservationInfo inValidDateInfo = new CreateReservationInfo
            {
                SpotId = "H4",
                StartTime = nowTime.AddHours(3),
                EndTime = nowTime.AddHours(2),
                CustomerName = "Frank"

            };
            CreateReservationInfo occupiedSpotInfo = new CreateReservationInfo
            {
                SpotId = "A1",
                StartTime = nowTime.AddSeconds(30),
                EndTime = nowTime.AddHours(2),
                CustomerName = "Frank"

            };

            var resultTrue = await service.PostReservationAsync(validInfo);
            var resultInvalidId = await service.PostReservationAsync(inValidIdnfo);
            var resultInvalidName = await service.PostReservationAsync(inValidNameInfo);
            var resultInvalidStart = await service.PostReservationAsync(inValidStartInfo);
            var resultInvalidEnd = await service.PostReservationAsync(inValidEndInfo);
            var resultInvalidDate = await service.PostReservationAsync(inValidDateInfo);
            var resultOccupiedSpot = await service.PostReservationAsync(occupiedSpotInfo);



            Assert.True(resultTrue.ResponseStatus == ResponseStatus.NO_CONTENT);
            var newEntry = await context.Reservations
              .Where(r => r.SpotID == "H4")
              .FirstOrDefaultAsync();

            Assert.NotNull(newEntry);
            Assert.True(newEntry.SpotID == "H4");
            Assert.True(newEntry.CustomerName == "Frank");

            Assert.True(resultInvalidName.Message == "Name cannot be empty");
            Assert.True(resultInvalidId.Message == "Invalid ID");
            Assert.True(resultInvalidStart.Message == "Start time should not be in the past");
            Assert.True(resultInvalidEnd.Message == "End time should not be in the past");
            Assert.True(resultInvalidDate.Message == "Start time should not be greater than end time");
            Assert.True(resultOccupiedSpot.Message == "Spot A1 is occupied");
        }
        [Fact]
        public async Task GetSpotHistoryTest()
        {
            using var context = await GetInMemoryDbContext();
            var service = new ReservationService(context);

            var validSpotHistory = await service.GetSpotHistoryAsync("D4");
            var invalidSpotHistory = await service.GetSpotHistoryAsync("E8");


            Assert.NotNull(validSpotHistory.ResponseData);
            Assert.True(validSpotHistory.ResponseData.Count == 4);
            Assert.Null(invalidSpotHistory.ResponseData);
            Assert.True(invalidSpotHistory.Message == "Spot with ID: E8 not found");
        }
        [Fact]
        public async Task DeleteReservationTest()
        {
            using var context = await GetInMemoryDbContext();
            var service = new ReservationService(context);

            var validDeleteResult = await service.DeleteReservationAsync(8);
            var inValidDeleteResult = await service.DeleteReservationAsync(100);

            Assert.True(validDeleteResult.ResponseStatus == ResponseStatus.NO_CONTENT);
            var deleteEntry = await context.ParkingSpots.FindAsync("D9");
            Assert.NotNull(deleteEntry);
            Assert.True(deleteEntry.ReservationHistory.Count == 0);

            Assert.True(inValidDeleteResult.Message == "Reservation with ID: 100 not found");
        }
    }
}
