using System.Data;
using Dapper;

public class UsersData : IUsersData
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public UsersData(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<User?> GetByUsername(string name)
    {
        using IDbConnection db = await _connectionFactory.GetConnection();

        return await db.QuerySingleOrDefaultAsync<User>("SP_Users_GetByUsername", new { Username = name });
    }

    public async Task<bool> CheckIfExists(string name)
    {
        using IDbConnection db = await _connectionFactory.GetConnection();

        byte res = await db.QuerySingleAsync<byte>("SP_Users_IfExists", new { Username = name });

        bool userExists = res > 0;

        return userExists;
    }

    public async Task<User?> Get(Guid id)
    {
        using IDbConnection db = await _connectionFactory.GetConnection();

        return await db.QuerySingleAsync<User>("SP_Users_Get", new { id });
    }


    public async Task Create(User user)
    {
        using IDbConnection db = await _connectionFactory.GetConnection();

        await db.ExecuteAsync("SP_Users_Insert", user);
    }

    public async Task Update(User user)
    {
        using IDbConnection db = await _connectionFactory.GetConnection();

        // update only not null values
        // exception possible

        await db.ExecuteAsync("SP_Users_Update", user);
    }

    public async Task Delete(Guid id)
    {
        using IDbConnection db = await _connectionFactory.GetConnection();

        await db.ExecuteAsync("SP_Users_Delete", new { id });
    }
}
