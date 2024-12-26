using Microsoft.Data.SqlClient;
using Resto_Backend.Model;
using Resto_Backend.Utils;

namespace Resto_Backend.Data
{
    public class UserRepository
    {
        private readonly IConfiguration _configuration;
        public UserRepository(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public async Task<bool> Register(UserModel user)
        {
            using (SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("ConnectionString")))
            {
                SqlCommand sqlCommand = new SqlCommand("PR_User_Register", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                sqlCommand.Parameters.AddWithValue("@Username", user.UserName);
                string hashPassword = PasswordIncryptDecrypt.ConvertToEncrypt(user.Password);
                sqlCommand.Parameters.AddWithValue("@Password", hashPassword);
                sqlCommand.Parameters.AddWithValue("@Email", user.Email);
                sqlCommand.Parameters.AddWithValue("@MobileNumber", user.MobileNumber);
                sqlCommand.Parameters.AddWithValue("@Address", user.Address);
                CloudinaryService cloudinaryService = new CloudinaryService(this._configuration);
                string url = await cloudinaryService.UploadFileAsync(user.ProfileImage);
                Console.WriteLine(url);
                sqlCommand.Parameters.AddWithValue("@ProfileImage", url);
                conn.Open();
                int rowAffected = sqlCommand.ExecuteNonQuery();

                return rowAffected > 0;
            }
        }
        public UserModel SelectUserByPk(int userID)
        {
            UserModel user = null;
            using (SqlConnection sqlConnection = new SqlConnection(this._configuration.GetConnectionString("ConnectionString")))
            {
                SqlCommand sqlCommand = new SqlCommand("PR_User_SelectByPK", sqlConnection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                sqlCommand.Parameters.AddWithValue("@UserID", userID);
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
                return user;
            }
        }
        public bool UpdatePassword(int UserID, string NewPassword)
        {
            using (SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("ConnectionString")))
            {
                SqlCommand sqlCommand = new SqlCommand("PR_User_UpdatePassword", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                sqlCommand.Parameters.AddWithValue("@UserID", UserID);



                conn.Open();
                int rowAffected = sqlCommand.ExecuteNonQuery();

                return rowAffected > 0;
            }
        }
    }
}
