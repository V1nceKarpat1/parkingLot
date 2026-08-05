namespace parkingLotAPI.Models
{
    public class ParkingSpot
    {
        public string SpotID { get; set; } = string.Empty;
        public int? ReservationID { get; set; }
    }
}
