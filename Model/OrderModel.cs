namespace Resto_Backend.Model
{
    public class OrderModel
    {
        public int OrderID { get; set; }
        public DateTime OrderDate{ get; set; }
        public int UserID{ get; set; }
        public int TotalQuantity { get; set; }
        public decimal TotalPrice{ get; set; }
    }
}
