using System.Data;

public interface ISqlConnectionFactory
{
    Task<IDbConnection> GetConnection(string connectionId = "Default");
}