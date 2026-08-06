using Microsoft.EntityFrameworkCore;
using parkingLotAPI.Data;
using parkingLotAPI.DTOs;
using parkingLotAPI.Models;
using parkingLotAPI.Utils;


namespace parkingLotAPI.Services
{
    public class ReservationService(AppDbContext dbContext) : IReservationService
    {
        public async Task<ServiceResponse<List<SpotInfo>>> GetAllSpotsAsync()
        {
            DateTime nowTime = DateTime.UtcNow;
            return new ServiceResponse<List<SpotInfo>>
            {
                ResponseData = await dbContext.ParkingSpots
                .Include(ps => ps.ReservationHistory)
                .Select(ps => new SpotInfo
                {
                    SpotID = ps.SpotID,
                    IsOccupied = ps.ReservationHistory.Any(r => r.EndTime > nowTime)
                })
                .ToListAsync(),
             
            };
            
        }

        public async Task<ServiceResponse<SpotInfo>> GetSpotByIdAsync(string id)
        {
            DateTime nowTime = DateTime.UtcNow;
            ParkingSpot? idSpot = await dbContext.ParkingSpots.FindAsync(id);

            if (!IsValidSpotId(id) || idSpot is null)
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
                    IsOccupied = ps.ReservationHistory.Any(r => r.EndTime > nowTime)
                })
                .FirstOrDefaultAsync(),
                ResponseStatus = ResponseStatus.OK
            };
        }

        public async Task<ServiceResponse> PostReservationAsync(CreateReservationInfo createData)
        {
            DateTime nowTime = DateTime.UtcNow;
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
            if (createData.StartTime < nowTime){
                return new ServiceResponse
                {
                    ResponseStatus = ResponseStatus.BAD_REQUEST,
                    Message = $"Start time should not be in the past"
                };
            }
            if (createData.EndTime < nowTime)
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
            var checkSpotResponse = await GetSpotByIdAsync(createData.SpotId);
            if (checkSpotResponse.ResponseData!.IsOccupied)
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

        public async Task<ServiceResponse<List<ReservationHistoryInfo>>> GetSpotHistoryAsync(string id)
        {
            var getResponse = await GetSpotByIdAsync(id);
            if (getResponse.ResponseData is null)
            {
                return new ServiceResponse<List<ReservationHistoryInfo>>
                {
                    ResponseData = null,
                    ResponseStatus = ResponseStatus.NOT_FOUND,
                    Message = $"Spot with ID: {id} not found"
                };
            }
            else
            {
                return new ServiceResponse<List<ReservationHistoryInfo>>
                {
                    ResponseStatus = ResponseStatus.OK,
                    ResponseData = await dbContext.Reservations
                        .Where(r=> r.SpotID == id)
                        .Select(r => new ReservationHistoryInfo
                        {
                            SpotID = r.SpotID,
                            CustomerName = r.CustomerName,
                            StartTime = r.StartTime,
                            EndTime = r.EndTime,

                        })
                        .ToListAsync()
                };
            }
        }
        public async Task<ServiceResponse> DeleteReservationAsync(int id)
        {
            Reservation? deleteEntry = await dbContext.Reservations.FindAsync(id);
            if (deleteEntry is null)
            {
                return new ServiceResponse
                {
                    ResponseStatus = ResponseStatus.NOT_FOUND,
                    Message = $"Reservation with ID: {id} not found"
                };
            }

            dbContext.Reservations.Remove(deleteEntry);
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
