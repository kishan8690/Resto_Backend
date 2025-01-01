using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using Resto_Backend.Utils;

namespace Resto_Backend.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly string _connectionString;
        public AuthRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionString");
        }


        public async Task<bool> ValidateUser(string username, string password)
        {
           // string hashPassword = PasswordIncryptDecrypt.ConvertToEncrypt(password);
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync(); // Use async version
                SqlCommand cmd = new SqlCommand("PR_User_Login", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@UserName", username);
                cmd.Parameters.AddWithValue("@Password", password);

                var result = await cmd.ExecuteScalarAsync(); // Use async version
                return result != null && Convert.ToInt32(result) > 0;
            }
        }

    }
}
