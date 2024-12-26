namespace Resto_Backend.Model
{
    public class UserModel
    {
        public int? UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public string ProfileImage { get; set; }
        public int IsAdmin { get; set; }   
    }
}
