﻿using Microsoft.Data.SqlClient;
using Resto_Backend.Model;
using Resto_Backend.Utils;

namespace Resto_Backend.Data
{
    public class UserRepository
    {
        public readonly IConfiguration _configuration;
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

                // Upload Profile Image to Cloudinary
                CloudinaryService cloudinaryService = new CloudinaryService(this._configuration);
                string url = await cloudinaryService.UploadFileAsync(user.ProfileImage);
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
                        ProfileImageUrl = reader["ProfileImage"].ToString(),
                        IsAdmin = Convert.ToInt32(reader["IsAdmin"]),
                    };
                }
                return user;
            }
        }
        public IEnumerable<UserModel> SelectAllUsers()
        {
            List<UserModel> users = new List<UserModel>();
            using (SqlConnection sqlConnection = new SqlConnection(this._configuration.GetConnectionString("ConnectionString")))
            {
                SqlCommand sqlCommand = new SqlCommand("PR_User_SelectAll", sqlConnection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                
                sqlConnection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    UserModel user = new UserModel
                    {
                        UserID = Convert.ToInt32(reader["UserID"]),
                        UserName = reader["UserName"].ToString(),
                        Password = reader["Password"].ToString(),
                        Email = reader["Email"].ToString(),
                        MobileNumber = reader["MobileNumber"].ToString(),
                        Address = reader["Address"].ToString(),
                        ProfileImageUrl = reader["ProfileImage"].ToString(),
                        IsAdmin = Convert.ToInt32(reader["IsAdmin"]),
                    };
                    users.Add(user);
                }
                return users;
            }

        }
        public UserModel SelectUserByUserName(string userName)
        {
            UserModel user = null;
            using (SqlConnection sqlConnection = new SqlConnection(this._configuration.GetConnectionString("ConnectionString")))
            {
                SqlCommand sqlCommand = new SqlCommand("PR_User_SelectByUserName", sqlConnection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                sqlCommand.Parameters.AddWithValue("@UserName", userName);
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
                        ProfileImageUrl = reader["ProfileImage"].ToString(),
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
                string hashPassword = PasswordIncryptDecrypt.ConvertToEncrypt(NewPassword);
                sqlCommand.Parameters.AddWithValue("@Password", hashPassword);
                conn.Open();
                int rowAffected = sqlCommand.ExecuteNonQuery();

                return rowAffected > 0;
            }
        }
    }
}
