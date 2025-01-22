namespace Resto_Backend.Model
{
    public class ItemModel
    {
        public int ItemID { get; set; }
        public string  ItemName { get; set; }
        public string ItemDescription { get; set; }
        public double ItemPrice { get; set; }
        public int CategoryID{ get; set; }
        public string? CategoryName { get; set; }
        public IFormFile ItemImage { get; set; }
        public string? ItemImageUrl { get; set; }
        public int ChefID{ get; set; }
        public string? ChefName{ get; set; }
    }
}
