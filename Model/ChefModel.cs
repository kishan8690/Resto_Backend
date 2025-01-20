namespace Resto_Backend.Model
{
    public class ChefModel
    {
        public int ChefID { get; set; }
        public string ChefName { get; set; }
        public string ChefSpeciality { get; set; }
        public IFormFile ChefImage { get; set; }
        public string? ChefImageUrl { get; set; }
        public int Experience{ get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public string Designation { get; set; } 
        public decimal Salary { get; set; }
    }
}
