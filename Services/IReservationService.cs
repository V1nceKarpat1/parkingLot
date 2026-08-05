using parkingLotAPI.DTOs;
using parkingLotAPI.Utils;

namespace parkingLotAPI.Services
{
    public interface IReservationService
    {
        Task<ServiceResponse<List<SpotInfo>>> GetSpotsAsync();
        Task<ServiceResponse<SpotInfo>> GetSpotByIdAsync(string id);
        Task<ServiceResponse> NewReservation(CreateReservation createData);


    }
}
