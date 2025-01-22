using Microsoft.Data.SqlClient;
using Resto_Backend.Model;
using Resto_Backend.Utils;

namespace Resto_Backend.Data
{
    public class ItemRepository
    {
        public readonly IConfiguration _configuration;
        public ItemRepository(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public List<ItemModel> SelectAllItem()
        {
            List<ItemModel> ItemList = new List<ItemModel>();
            using (SqlConnection sqlConnection = new SqlConnection(this._configuration.GetConnectionString("ConnectionString")))
            {
                SqlCommand sqlCommand = new SqlCommand("PR_Item_Select", sqlConnection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                sqlConnection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    ItemModel item = new ItemModel
                    {
                        ItemID = Convert.ToInt32(reader["ItemID"]),
                        ItemName = reader["ItemName"].ToString(),
                        ItemDescription = reader["Description"].ToString(),
                        ItemPrice = Convert.ToDouble(reader["ItemPrice"]),
                        CategoryID = Convert.ToInt32(reader["CategoryID"]),
                        ChefName = reader["ChefName"].ToString(),
                        CategoryName = reader["CategoryName"].ToString(),
                        ItemImageUrl = reader["ImagePath"].ToString(),
                        ChefID = Convert.ToInt32(reader["ChefID"]),
             
                    };
                    ItemList.Add(item);
                }
            }
            return ItemList;
        }
        public List<ItemModel> SelectItemsByCategory(int categoryID)
        {
            List<ItemModel> ItemList = new List<ItemModel>();
            using (SqlConnection sqlConnection = new SqlConnection(this._configuration.GetConnectionString("ConnectionString")))
            {
                SqlCommand sqlCommand = new SqlCommand("PR_Item_Select_By_Category", sqlConnection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                sqlCommand.Parameters.AddWithValue("@CategoryID", categoryID);
                sqlConnection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    ItemModel item = new ItemModel
                    {
                        ItemID = Convert.ToInt32(reader["ItemID"]),
                        ItemName = reader["ItemName"].ToString(),
                        ItemDescription = reader["Description"].ToString(),
                        ItemPrice = Convert.ToDouble(reader["ItemPrice"]),
                        CategoryID = Convert.ToInt32(reader["CategoryID"]),
                        ChefName = reader["ChefName"].ToString(),
                        CategoryName = reader["CategoryName"].ToString(),
                        ItemImageUrl = reader["ImagePath"].ToString(),
                        ChefID = Convert.ToInt32(reader["ChefID"]),

                    };
                    ItemList.Add(item);
                }
            }
            return ItemList;
        }
        public ItemModel SelectItemByID(int ItemID)
        {
            ItemModel item = new ItemModel();
            using (SqlConnection sqlConnection = new SqlConnection(this._configuration.GetConnectionString("ConnectionString")))
            {
                SqlCommand sqlCommand = new SqlCommand("PR_Item_Select_By_PK", sqlConnection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                sqlCommand.Parameters.AddWithValue("@ItemID", ItemID);
                sqlConnection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    item.ItemID = Convert.ToInt32(reader["ItemID"]);
                    item.ItemName = reader["ItemName"].ToString();
                    item.ItemDescription = reader["Description"].ToString();
                    item.ItemPrice = Convert.ToDouble(reader["ItemPrice"]);
                    item.CategoryID = Convert.ToInt32(reader["CategoryID"]);
                    item.ChefName = reader["ChefName"].ToString();
                    item.CategoryName = reader["CategoryName"].ToString();
                    item.ItemImageUrl = reader["ImagePath"].ToString();
                    item.ChefID = Convert.ToInt32(reader["ChefID"]);
                }
            }
            return item;
        }

        public async Task<bool> InsertItem(ItemModel item)
        {
            using (SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("ConnectionString")))
            {
                SqlCommand sqlCommand = new SqlCommand("PR_Item_Insert", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                sqlCommand.Parameters.AddWithValue("@ItemName", item.ItemName);
                sqlCommand.Parameters.AddWithValue("@Description", item.ItemDescription);
                sqlCommand.Parameters.AddWithValue("@ItemPrice", item.ItemPrice);
                sqlCommand.Parameters.AddWithValue("@CategoryID", item.CategoryID);
                sqlCommand.Parameters.AddWithValue("@ChefID", item.ChefID);
                CloudinaryService cloudinaryService = new CloudinaryService(this._configuration);
                string url = await cloudinaryService.UploadFileAsync(item.ItemImage);
                Console.WriteLine(url);
                sqlCommand.Parameters.AddWithValue("@ImagePath", url);
                conn.Open();
                int rowAffected = sqlCommand.ExecuteNonQuery();
                return rowAffected > 0;
            }
        }
        public async Task<bool> UpdateItem(ItemModel item)
        {
            using (SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("ConnectionString")))
            {
                SqlCommand sqlCommand = new SqlCommand("PR_Item_Update", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                sqlCommand.Parameters.AddWithValue("@ItemID", item.ItemID);
                sqlCommand.Parameters.AddWithValue("@ItemName", item.ItemName);
                sqlCommand.Parameters.AddWithValue("@Description", item.ItemDescription);
                sqlCommand.Parameters.AddWithValue("@ItemPrice", item.ItemPrice);
                sqlCommand.Parameters.AddWithValue("@CategoryID", item.CategoryID);
                sqlCommand.Parameters.AddWithValue("@ChefID", item.ChefID);
                CloudinaryService cloudinaryService = new CloudinaryService(this._configuration);
                string url = await cloudinaryService.UploadFileAsync(item.ItemImage);
                Console.WriteLine(url);
                sqlCommand.Parameters.AddWithValue("@ImagePath", url);
                conn.Open();
                int rowAffected = sqlCommand.ExecuteNonQuery();
                return rowAffected > 0;
            }
        }
        public bool DeleteItem(int itemID)
        {
            using (SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("ConnectionString")))
            {
                SqlCommand sqlCommand = new SqlCommand("PR_Item_Delete", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                sqlCommand.Parameters.AddWithValue("@ItemID", itemID);
                conn.Open();
                int rowAffected = sqlCommand.ExecuteNonQuery();
                return rowAffected > 0;
            }
        }
    }
}
