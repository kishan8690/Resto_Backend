using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using Resto_Backend.Utils;
using Resto_Backend.Model;

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
            
            UserModel user = null;
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand("PR_User_SelectByUserName", sqlConnection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                sqlCommand.Parameters.AddWithValue("@UserName", username);
                sqlConnection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.Read())
                {
                    user = new UserModel
                    {
                        UserID = Convert.ToInt32(reader["UserID"]),
                        UserName = reader["UserName"].ToString(),
                        Password = reader["Password"].ToString(),
                        Email = reader["Email"].ToString(),
                        MobileNumber = reader["MobileNumber"].ToString(),
                        Address = reader["Address"].ToString(),
                        ProfileImage = reader["ProfileImage"].ToString(),
                        IsAdmin = Convert.ToInt32(reader["IsAdmin"]),
                    };
                }

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync(); // Use async version
                    SqlCommand cmd = new SqlCommand("PR_User_Login", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@UserName", username);
                    cmd.Parameters.AddWithValue("@Password", user.Password);

                    var result = await cmd.ExecuteScalarAsync(); // Use async version
                    return result != null && Convert.ToInt32(result) > 0;
                }
            }

        }
    }
}
