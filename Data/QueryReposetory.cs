using Microsoft.Data.SqlClient;
using Resto_Backend.Model;

namespace Resto_Backend.Data
{

    public class QueryReposetory
    {
        public readonly IConfiguration _configuration;
        public QueryReposetory(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public List<QueryModel> SelectAllQuery()
        {
            List<QueryModel> queryList = new List<QueryModel>();
            using (SqlConnection sqlConnection = new SqlConnection(this._configuration.GetConnectionString("ConnectionString")))
            {
                SqlCommand sqlCommand = new SqlCommand("PR_Query_Select", sqlConnection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                sqlConnection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    QueryModel query = new QueryModel
                    {
                        QueryID = Convert.ToInt32(reader["QueryID"]),
                        Name = reader["Name"].ToString(),
                        Email = reader["Email"].ToString(),
                        Subject = reader["Subject"].ToString(),
                        Message = reader["Message"].ToString()
                    };
                    queryList.Add(query);
                }
            }
            return queryList;
        }
        public QueryModel SelectQueryByPk(int queryID)
        {
            QueryModel query = new QueryModel();
            using (SqlConnection sqlConnection = new SqlConnection(this._configuration.GetConnectionString("ConnectionString")))
            {
                SqlCommand sqlCommand = new SqlCommand("PR_Query_Select_By_PK", sqlConnection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                sqlCommand.Parameters.AddWithValue("@QueryID", queryID);
                sqlConnection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    query.QueryID = Convert.ToInt32(reader["QueryID"]);
                    query.Name = reader["Name"].ToString();
                    query.Email = reader["Email"].ToString();
                    query.Subject = reader["Subject"].ToString();
                    query.Message = reader["Message"].ToString();
                }
            }
            return query;
        }
        public bool InsertQuery(QueryModel query)
        {
            using (SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("ConnectionString")))
            {
                SqlCommand sqlCommand = new SqlCommand("PR_Query_Insert", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                sqlCommand.Parameters.AddWithValue("@Name", query.Name);
                sqlCommand.Parameters.AddWithValue("@Email", query.Email);
                sqlCommand.Parameters.AddWithValue("@Subject", query.Subject);
                sqlCommand.Parameters.AddWithValue("@Message", query.Message);
                conn.Open();
                int rowAffected = sqlCommand.ExecuteNonQuery();
                return rowAffected > 0;
            }
        }
        public bool UpdateQuery(QueryModel query)
        {
            using (SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("ConnectionString")))
            {
                SqlCommand sqlCommand = new SqlCommand("PR_Query_Update", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                sqlCommand.Parameters.AddWithValue("@QueryID", query.QueryID);
                sqlCommand.Parameters.AddWithValue("@Name", query.Name);
                sqlCommand.Parameters.AddWithValue("@Email", query.Email);
                sqlCommand.Parameters.AddWithValue("@Subject", query.Subject);
                sqlCommand.Parameters.AddWithValue("@Message", query.Message);
                conn.Open();
                int rowAffected = sqlCommand.ExecuteNonQuery();
                return rowAffected > 0;
            }
        }
        public bool DeleteQuery(int queryID)
        {
            using (SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("ConnectionString")))
            {
                SqlCommand sqlCommand = new SqlCommand("PR_Query_Delete", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                sqlCommand.Parameters.AddWithValue("@QueryID", queryID);
                conn.Open();
                int rowAffected = sqlCommand.ExecuteNonQuery();
                return rowAffected > 0;
            }
        }

    }
}
