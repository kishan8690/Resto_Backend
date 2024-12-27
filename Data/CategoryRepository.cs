using Microsoft.Data.SqlClient;
using Resto_Backend.Model;

namespace Resto_Backend.Data
{
    public class CategoryRepository
    {
        public readonly IConfiguration _configuration;
        public CategoryRepository(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public List<CategoryModel> SelectAllCategory()
        {
            List<CategoryModel> categoryList = new List<CategoryModel>();
            using (SqlConnection sqlConnection = new SqlConnection(this._configuration.GetConnectionString("ConnectionString")))
            {
                SqlCommand sqlCommand = new SqlCommand("PR_Category_Select", sqlConnection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                sqlConnection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    CategoryModel category = new CategoryModel
                    {
                        CategoryID = Convert.ToInt32(reader["CategoryID"]),
                        CategoryName = reader["CategoryName"].ToString()
                    };
                    categoryList.Add(category);
                }
            }
            return categoryList;
        }
        public bool InsertCategory(CategoryModel category)
        {
            using (SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("ConnectionString")))
            {
                SqlCommand sqlCommand = new SqlCommand("PR_Category_Insert", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                sqlCommand.Parameters.AddWithValue("@CategoryName", category.CategoryName);
                conn.Open();
                int rowAffected = sqlCommand.ExecuteNonQuery();
                return rowAffected > 0;
            }
        }
        public bool UpdateCategory(CategoryModel category)
        {
            using (SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("ConnectionString")))
            {
                SqlCommand sqlCommand = new SqlCommand("PR_Category_Update", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                sqlCommand.Parameters.AddWithValue("@CategoryID", category.CategoryID);
                sqlCommand.Parameters.AddWithValue("@CategoryName", category.CategoryName);
                conn.Open();
                int rowAffected = sqlCommand.ExecuteNonQuery();
                return rowAffected > 0;
            }
        }
        public bool DeleteCategory(int CategoryID)
        {
            using (SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("ConnectionString")))
            {
                SqlCommand sqlCommand = new SqlCommand("PR_Category_Delete", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                sqlCommand.Parameters.AddWithValue("@CategoryID", CategoryID);
                conn.Open();
                int rowAffected = sqlCommand.ExecuteNonQuery();
                return rowAffected > 0;
            }
        }
        public CategoryModel SelectCategoryByPk(int CategoryID)
        {
            CategoryModel category = null;
            using (SqlConnection sqlConnection = new SqlConnection(this._configuration.GetConnectionString("ConnectionString")))
            {
                SqlCommand sqlCommand = new SqlCommand("PR_Category_Select_By_PK", sqlConnection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                sqlCommand.Parameters.AddWithValue("@CategoryID", CategoryID);
                sqlConnection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.Read())
                {
                    category = new CategoryModel
                    {
                        CategoryID = Convert.ToInt32(reader["CategoryID"]),
                        CategoryName = reader["CategoryName"].ToString()
                    };
                }
                return category;
            }
        }
    }
}
