using Dapper;
using Microsoft.Data.SqlClient;
using projectapi.Webapi.Interfaces;
//using projectapi.Webapi.Interfaces;

namespace projectapi.Webapi.Repositories
{
    public class Object2DRepository : IObject2DRepository
    {
        public string sqlConectionString;
        public Object2DRepository(string connectionString)
        {
            sqlConectionString = connectionString;
        }

        public async Task<Object2D?> ReadAsync(int id)
        {
            using (var sqlConnection = new SqlConnection(sqlConectionString))
            {
                return await sqlConnection.QuerySingleOrDefaultAsync<Object2D>("SELECT * FROM Object2D3", new { Id = id });
            }
        }
    }
}
