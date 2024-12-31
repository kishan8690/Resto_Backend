using Microsoft.Data.SqlClient;
using Resto_Backend.Model;
using Resto_Backend.Utils;

namespace Resto_Backend.Data
{
    public class ChefReposetory
    {
        public readonly IConfiguration _configuration;
        public ChefReposetory(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public List<ChefModel> SelectAllChef()
        {
            List<ChefModel> chefList = new List<ChefModel>();
            using (SqlConnection sqlConnection = new SqlConnection(this._configuration.GetConnectionString("ConnectionString")))
            {
                SqlCommand sqlCommand = new SqlCommand("PR_Chef_Select", sqlConnection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                sqlConnection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    ChefModel chef = new ChefModel
                    {
                        ChefID = Convert.ToInt32(reader["ChefID"]),
                        ChefName = reader["ChefName"].ToString(),
                        ChefSpeciality = reader["ChefSpeciality"].ToString(),
                        Experience = Convert.ToInt32(reader["Experience"]),
                        ChefImage = reader["ChefImage"].ToString(),
                        MobileNumber = reader["MobileNumber"].ToString(),
                        Address = reader["Address"].ToString(),
                        Designation = reader["Designation"].ToString(),
                        Salary = Convert.ToDecimal(reader["Salary"])
                    };
                    chefList.Add(chef);
                }
            }
            return chefList;
        }
        public async Task<bool> InsertChef(ChefModel chef)
        {
            using (SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("ConnectionString")))
            {
                SqlCommand sqlCommand = new SqlCommand("PR_Chef_Insert", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                sqlCommand.Parameters.AddWithValue("@ChefName", chef.ChefName);
                sqlCommand.Parameters.AddWithValue("@ChefSpeciality", chef.ChefSpeciality);
                sqlCommand.Parameters.AddWithValue("@Experience", chef.Experience);
                CloudinaryService cloudinaryService = new CloudinaryService(this._configuration);
                string url = await cloudinaryService.UploadFileAsync(chef.ChefImage);
                Console.WriteLine(url);
                sqlCommand.Parameters.AddWithValue("@ChefImage", url);
                sqlCommand.Parameters.AddWithValue("@MobileNumber", chef.MobileNumber);
                sqlCommand.Parameters.AddWithValue("@Address", chef.Address);
                sqlCommand.Parameters.AddWithValue("@Designation", chef.Designation);
                sqlCommand.Parameters.AddWithValue("@Salary", chef.Salary);
                conn.Open();
                int rowAffected = sqlCommand.ExecuteNonQuery();
                return rowAffected > 0;
            }   
        }
        public async Task<bool> UpdateChef(ChefModel chef)
        {
            using (SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("ConnectionString")))
            {
                SqlCommand sqlCommand = new SqlCommand("PR_Chef_Update", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                sqlCommand.Parameters.AddWithValue("@ChefID", chef.ChefID);
                sqlCommand.Parameters.AddWithValue("@ChefName", chef.ChefName);
                sqlCommand.Parameters.AddWithValue("@ChefSpeciality", chef.ChefSpeciality);
                sqlCommand.Parameters.AddWithValue("@Experience", chef.Experience);
                CloudinaryService cloudinaryService = new CloudinaryService(this._configuration);
                string url = await cloudinaryService.UploadFileAsync(chef.ChefImage);
                Console.WriteLine(url);
                sqlCommand.Parameters.AddWithValue("@ChefImage", url);
                sqlCommand.Parameters.AddWithValue("@MobileNumber", chef.MobileNumber);
                sqlCommand.Parameters.AddWithValue("@Address", chef.Address);
                sqlCommand.Parameters.AddWithValue("@Designation", chef.Designation);
                sqlCommand.Parameters.AddWithValue("@Salary", chef.Salary);
                conn.Open();
                int rowAffected = sqlCommand.ExecuteNonQuery();
                return rowAffected > 0;
            }
        }
        public bool DeleteChef(int ChefID)
        {
            using (SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("ConnectionString")))
            {
                SqlCommand sqlCommand = new SqlCommand("PR_Chef_Delete", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                sqlCommand.Parameters.AddWithValue("@ChefID", ChefID);
                conn.Open();
                int rowAffected = sqlCommand.ExecuteNonQuery();
                return rowAffected > 0;
            }
        }
        public ChefModel SelectChefByPk(int ChefID)
        {
            ChefModel chef = null;
            using (SqlConnection sqlConnection = new SqlConnection(this._configuration.GetConnectionString("ConnectionString")))
            {
                SqlCommand sqlCommand = new SqlCommand("PR_Chef_Select_By_PK", sqlConnection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                sqlCommand.Parameters.AddWithValue("@ChefID", ChefID);
                sqlConnection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.Read())
                {
                    chef = new ChefModel
                    {
                        ChefID = Convert.ToInt32(reader["ChefID"]),
                        ChefName = reader["ChefName"].ToString(),
                        ChefSpeciality = reader["ChefSpeciality"].ToString(),
                        Experience = Convert.ToInt32(reader["Experience"]),
                        ChefImage = reader["ChefImage"].ToString(),
                        MobileNumber = reader["MobileNumber"].ToString(),
                        Address = reader["Address"].ToString(),
                        Designation = reader["Designation"].ToString(),
                        Salary = Convert.ToDecimal(reader["Salary"])
                    };
                }
                return chef;
            }
        }
    }
}
