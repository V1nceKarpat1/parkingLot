namespace parkingLotAPI.Models
{
    public class ParkingSpot
    {
        public string SpotID { get; set; } = string.Empty; //p key
        public List<Reservation> ReservationHistory { get; set; } = new List<Reservation>();
    }
}
