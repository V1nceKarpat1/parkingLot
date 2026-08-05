using parkingLotAPI.DTOs;

namespace parkingLotAPI.Services
{
    public interface IReservationService
    {
        Task<List<SpotInfo>> GetSpotInfosAsync();
    }
}
