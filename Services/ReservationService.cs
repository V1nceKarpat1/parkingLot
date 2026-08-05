using Microsoft.EntityFrameworkCore;
using parkingLotAPI.Data;
using parkingLotAPI.DTOs;
using parkingLotAPI.Models;
using parkingLotAPI.Utils;
using System.ComponentModel;

namespace parkingLotAPI.Services
{
    public class ReservationService(AppDbContext dbContext) : IReservationService
    {
        public async Task<ServiceResponse<List<SpotInfo>>> GetSpotsAsync()
        {
            return new ServiceResponse<List<SpotInfo>>
            {
                ResponseData = await dbContext.ParkingSpots
                .Include(ps => ps.ReservationHistory)
                .Select(ps => new SpotInfo
                {
                    SpotID = ps.SpotID,
                    IsOccupied = ps.ReservationHistory.Any(r => r.EndTime > DateTime.Now)
                })
                .ToListAsync(),
             
            };
            
        }

        
           
              
        


      
    }
}
