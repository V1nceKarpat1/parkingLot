using Microsoft.EntityFrameworkCore;
using parkingLotAPI.Data;
using parkingLotAPI.DTOs;
using System.ComponentModel;

namespace parkingLotAPI.Services
{
    public class ReservationService(AppDbContext dbContext) : IReservationService
    {
        public async Task<List<SpotInfo>> GetSpotInfosAsync()
        {
       
            return await dbContext.ParkingSpots
                .Include(ps => ps.ReservationHistory)
                .Select(ps => new SpotInfo
                {
                    SpotID = ps.SpotID,
                    IsOccupied = ps.ReservationHistory.Any(r => r.EndTime > DateTime.Now)
                })
                .ToListAsync();
        }
    }
}
