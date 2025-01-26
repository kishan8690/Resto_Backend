namespace Resto_Backend.Models
{
    public class BookingModel
    {
        public int? BookingID{ get; set; }
        public DateTime BookingDate{ get; set; }
        public int UserID{ get; set; }
        public string? UserName { get; set; }
        public int NumberOfPerson { get; set; }
        public string? BookingStatus { get; set; }
    }
    public class BookingStatusModel
    {
        public int BookingID { get; set; }
        public string BookingStatus { get; set; }
    }
}
