using Microsoft.EntityFrameworkCore;
using RealEstateApp.Core.Domain.Interfaces.GenericRepository;
using RealEstateApp.Infraestructure.Persistence.Context;

namespace RealEstateApp.Infraestructure.Persistence.Repositories.Generic
{
    public abstract class GenericRepository<TEntity, Tkey> 
        : IGenericRepository<TEntity, Tkey>
        where TEntity : class
    {

        protected readonly DbContextRealEstateApp _context;

        public GenericRepository(DbContextRealEstateApp context)
        {
            _context = context;
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
        }

        public virtual async Task<bool> DeleteAsync(TEntity entity)
        {
           _context.Set<TEntity>().Remove(entity);
          return await _context.SaveChangesAsync() > 0; 
     
        }

        public  virtual async Task<IReadOnlyCollection<TEntity>> GetAllAsync()
        {
            return await _context.Set<TEntity>().AsNoTracking().ToListAsync();
        }

        public virtual async Task<TEntity> GetByIdAsync(Tkey key)
        {
            return await _context.Set<TEntity>().FindAsync(key) ?? null!;
        }

        public  Task<int> SaveAsync()
        {
           return  _context.SaveChangesAsync();
        }

        public virtual async Task<bool> UpdateAsync(TEntity entity)
        {
           _context.Set<TEntity>().Update(entity);
          return await _context.SaveChangesAsync() > 0;

        }
    }
}
