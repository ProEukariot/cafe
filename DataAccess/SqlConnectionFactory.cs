using System.Data;
using Microsoft.Data.SqlClient;

public sealed class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly IConfiguration _config;

    public SqlConnectionFactory(IConfiguration config)
    {
        _config = config;
    }

    public async Task<IDbConnection> GetConnection(string connectionId)
    {
        //ARG-NULL-EXC
        var connection = new SqlConnection(_config.GetConnectionString(connectionId));

        await connection.OpenAsync();

        return connection;
    }
}