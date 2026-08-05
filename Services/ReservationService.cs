using Microsoft.EntityFrameworkCore;
using parkingLotAPI.Data;
using parkingLotAPI.DTOs;
using parkingLotAPI.Models;
using parkingLotAPI.Utils;
using System.ComponentModel;
using System.Diagnostics;

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

        public async Task<ServiceResponse<SpotInfo>> GetSpotByIdAsync(string id)
        {
            ParkingSpot? idSpot = await dbContext.ParkingSpots.FindAsync(id);

            if (idSpot is null)
            {
                return new ServiceResponse<SpotInfo>
                {
                    ResponseData = null,
                    ResponseStatus = ResponseStatus.NOT_FOUND,
                    Message = $"Spot with ID: {id} not found"
                };
            }



            return new ServiceResponse<SpotInfo>
            {
                ResponseData = await dbContext.ParkingSpots
                .Include(ps => ps.ReservationHistory)
                .Where(ps=> id == ps.SpotID)
                .Select(ps => new SpotInfo
                {
                    SpotID = ps.SpotID,
                    IsOccupied = ps.ReservationHistory.Any(r => r.EndTime > DateTime.Now)
                })
                .FirstOrDefaultAsync(),
                ResponseStatus = ResponseStatus.OK
            };
        }

        public async Task<ServiceResponse> NewReservation(CreateReservation createData)
        {  
            if (string.IsNullOrWhiteSpace(createData.CustomerName)){
                return new ServiceResponse
                {
                    ResponseStatus = ResponseStatus.BAD_REQUEST,
                    Message = $"Name cannot be empty"
                };
            }
            if (!IsValidSpotId(createData.SpotId))
            {
                return new ServiceResponse
                {
                    ResponseStatus = ResponseStatus.BAD_REQUEST,
                    Message = $"Invalid ID"
                };
            }
            if (createData.StartTime < DateTime.Now){
                return new ServiceResponse
                {
                    ResponseStatus = ResponseStatus.BAD_REQUEST,
                    Message = $"Start time should not be in the past"
                };
            }
            if (createData.StartTime < DateTime.Now)
            {
                return new ServiceResponse
                {
                    ResponseStatus = ResponseStatus.BAD_REQUEST,
                    Message = $"End time should not be in the past"
                };
            }
            if (createData.StartTime > createData.EndTime)
            {
                return new ServiceResponse
                {
                    ResponseStatus = ResponseStatus.BAD_REQUEST,
                    Message = $"Start time should not be greater than end time"
                };
            }
            var checkSpot = await GetSpotByIdAsync(createData.SpotId);
            if (checkSpot.ResponseData!.IsOccupied)
            {
                return new ServiceResponse
                {
                    ResponseStatus = ResponseStatus.BAD_REQUEST,
                    Message = $"Spot {createData.SpotId} is occupied"
                };
            }

            Reservation newEntry = new Reservation
            {
                SpotID = createData.SpotId,
                StartTime = createData.StartTime,
                EndTime = createData.EndTime,
                CustomerName  = createData.CustomerName
            };

            dbContext.Reservations.Add(newEntry);
            await dbContext.SaveChangesAsync();

            return new ServiceResponse
            {
                ResponseStatus = ResponseStatus.NO_CONTENT,
            };
        }



        private bool IsValidSpotId(string id)
        {

            
            return id.Length == 2
                && id[0] >= 'A'
                && id[0] <= 'J'
                && char.IsDigit(id[1]);

        }
              
        


      
    }
}
