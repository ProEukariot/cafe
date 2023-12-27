public interface IUsersData
{
    public Task<User?> GetByUsername(string namename);

    public Task<bool> CheckIfExists(string username);

    public Task<User?> Get(Guid id);

    public Task Create(User user);

    public Task Update(User user);

    public Task Delete(Guid id);
}