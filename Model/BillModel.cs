namespace Resto_Backend.Model
{
    public class BillModel
    {
        public int BillID { get; set; }
        public int OrderID { get; set; }
        public string PaymentMode { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime PaymentData{ get; set; }
    }
}
