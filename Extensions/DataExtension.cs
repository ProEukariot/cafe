public static class DataExtension
{
    public static IServiceCollection AddData(this IServiceCollection services)
    {
        return services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>()
        .AddSingleton<IProductsData, ProductsData>()
        .AddSingleton<ICouponsData, CouponsData>()
        .AddSingleton<IUsersData, UsersData>();
    }
}