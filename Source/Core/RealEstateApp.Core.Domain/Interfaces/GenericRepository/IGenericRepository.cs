namespace RealEstateApp.Core.Domain.Interfaces.GenericRepository
{
    public interface IGenericRepository<TEntity, TKey> where  TEntity : class
    {
        Task AddAsync(TEntity entity);
        Task<int> SaveAsync();
        Task<bool> UpdateAsync(TEntity entity);
        Task<IReadOnlyCollection<TEntity>> GetAllAsync();
        Task<bool> DeleteAsync(TEntity entity);
        Task<TEntity> GetByIdAsync(TKey key);
    }
}
