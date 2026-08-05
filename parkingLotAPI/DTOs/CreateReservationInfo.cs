namespace parkingLotAPI.DTOs
{
    public class CreateReservationInfo
    {
        public string SpotId { get; set; }  = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string CustomerName { get; set; } = string.Empty;
    }
}
