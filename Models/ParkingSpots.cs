namespace PLM_System.Models
{
    public class ParkingSpots
    {
        internal decimal totalAmount;
        public int SpotNumber { get; set; }
        public bool IsOccupied { get; set; }
        public int AvailableSpots { get; set; }
        public decimal Revenue { get; set; }
        public int AverageCarsPerDay { get; set; }
        public int AverageRevenuePerDay { get; set; }
    }
}