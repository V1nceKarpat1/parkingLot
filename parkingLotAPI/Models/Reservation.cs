namespace parkingLotAPI.Models
{
    public class Reservation
    {
        public int ReservationID { get; set; } //p key
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string CustomerName { get; set; } = string.Empty;


        public string SpotID { get; set; } = string.Empty;
        public ParkingSpot ParkingSpot { get; set; } = null!;
    }
}
