
using System.Data;
using Dapper;

public class CouponsData : ICouponsData
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public CouponsData(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }
    public async Task Create(Coupon coupon)
    {
        using IDbConnection db = await _connectionFactory.GetConnection();
        await db.ExecuteAsync("SQL_CMD", coupon);
    }

    public async Task<IEnumerable<Coupon>> GetByUsersCoupons(Guid userId, CancellationToken token)
    {
        using IDbConnection db = await _connectionFactory.GetConnection();

        var cmd = new CommandDefinition("SQL_CMD", parameters: new { userId }, commandType: CommandType.StoredProcedure, cancellationToken: token);

        return await db.QueryAsync<Coupon>(cmd);
    }

    public async Task<Coupon> Get(Guid id)
    {
        using IDbConnection db = await _connectionFactory.GetConnection();

        return await db.QuerySingleAsync<Coupon>("SQL_CMD", new { id });
    }

    public async Task Update(Coupon coupon)
    {
        using IDbConnection db = await _connectionFactory.GetConnection();

        // update only not null values
        // exception possible

        await db.ExecuteAsync("SQL_CMD", coupon);
    }
}
