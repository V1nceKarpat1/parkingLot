namespace parkingLotAPI.DTOs
{
    public class ReservationHistoryInfo
    {
      
        public string SpotID { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string CustomerName { get; set; } = string.Empty;
    }
}
