using System.Data;
using Dapper;

public class ProductsData : IProductsData
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public ProductsData(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Product>> Get(CancellationToken token)
    {
        using IDbConnection db = await _connectionFactory.GetConnection();

        var cmd = new CommandDefinition("SP_Products_GetAll", commandType: CommandType.StoredProcedure, cancellationToken: token);

        return await db.QueryAsync<Product>(cmd);
    }

    public async Task<Product?> Get(Guid id)
    {
        using IDbConnection db = await _connectionFactory.GetConnection();

        return await db.QuerySingleOrDefaultAsync<Product>("SP_Products_Get", new { id });
    }

    public async Task Create(Product product)
    {
        using IDbConnection db = await _connectionFactory.GetConnection();

        await db.ExecuteAsync("SP_Products_Insert", product);
    }

    public async Task Update(Product product)
    {
        using IDbConnection db = await _connectionFactory.GetConnection();

        // update only not null values
        // exception possible

        await db.ExecuteAsync("SP_Products_Update", product);
    }

    public async Task Delete(Guid id)
    {
        using IDbConnection db = await _connectionFactory.GetConnection();

        await db.ExecuteAsync("SP_Products_Delete", new { id });
    }
}