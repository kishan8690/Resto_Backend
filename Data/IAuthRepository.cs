namespace Resto_Backend.Data
{
    
        public interface IAuthRepository
        {
            Task<bool> ValidateUser(string username, string password);
        }
    
}
