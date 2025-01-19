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
        public IFormFile ProfileImage { get; set; }
        public string ProfileImageUrl { get; set; }
        public int IsAdmin { get; set; }   
    }
    public class LoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
