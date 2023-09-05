
namespace RedisCache.Repositories.Interfaces
{
    public interface ICacheRepository<T> where T : class
    {
        Task<T> GetValueAsync(string Id);
        Task<T> UpdateValueAsync(T entity);
        Task DeleteValueAsync(string Id);
    }
}
