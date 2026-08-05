using parkingLotAPI.DTOs;
using parkingLotAPI.Utils;

namespace parkingLotAPI.Services
{
    public interface IReservationService
    {
        Task<ServiceResponse<List<SpotInfo>>> GetAllSpotsAsync();
        Task<ServiceResponse<SpotInfo>> GetSpotByIdAsync(string id);
        Task<ServiceResponse> PostReservationAsync(CreateReservationInfo createData);
        Task<ServiceResponse<List<ReservationHistoryInfo>>> GetSpotHistoryAsync(string id);
        Task<ServiceResponse> DeleteReservationAsync(int id);

    }
}
