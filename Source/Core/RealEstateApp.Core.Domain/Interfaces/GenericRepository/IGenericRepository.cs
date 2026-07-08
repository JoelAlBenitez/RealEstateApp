namespace RealEstateApp.Core.Domain.Interfaces.GenericRepository
{
    public interface IGenericRepository<TEntity, TKey> where  TEntity : class
    {
        Task<bool> SaveAsync(TEntity entity);
        Task<bool> UpdateAsync(TEntity entity);
        Task<IReadOnlyCollection<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(TKey key);
        Task<bool> DeleteAsync(TEntity entity);
    }
}
