namespace Resto_Backend.Model
{
    public class OrderDetailsModel
    {
        public int OrderDetailID { get; set; }
        public int OrderID { get; set; }
        public int ItemID { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal { get; set; }
    }
}
