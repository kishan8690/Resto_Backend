using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

public interface IDashboardRepository
{
    Task<List<dynamic>> GetDashboardDataAsync();
}

public class DashboardRepository : IDashboardRepository
{
    private readonly string _connectionString;

    public DashboardRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("ConnectionString");
    }

    public async Task<List<dynamic>> GetDashboardDataAsync()
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var result = new List<dynamic>();

            using (var multi = await connection.QueryMultipleAsync("PR_DashBoardData", commandType: CommandType.StoredProcedure))
            {
                do
                {
                    var tableData = (await multi.ReadAsync()).ToList();
                    if (tableData.Any())
                    {
                        result.Add(tableData);
                    }
                } while (!multi.IsConsumed);
            }
            
            return result;
        }
    }
}
