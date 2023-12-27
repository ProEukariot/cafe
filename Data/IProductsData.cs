public interface IProductsData
{
    public Task<IEnumerable<Product>> Get(CancellationToken token = default);

    public Task<Product?> Get(Guid id);

    public Task Create(Product coffee);

    public Task Update(Product coffee);

    public Task Delete(Guid id);
}