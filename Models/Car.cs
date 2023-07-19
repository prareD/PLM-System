namespace PLM_System.Models
{
    public class Car
    {
        public int SpotId { get; set; }
        public string? TagNumber { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public DateTime? ElapsedTime { get; set; }
    }
}

