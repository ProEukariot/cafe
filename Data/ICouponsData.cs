public interface ICouponsData
{
    public Task<IEnumerable<Coupon>> GetByUsersCoupons(Guid userId, CancellationToken token = default);

    public Task<Coupon> Get(Guid id);

    public Task Create(Coupon coupon);

    public Task Update(Coupon coupon);
}