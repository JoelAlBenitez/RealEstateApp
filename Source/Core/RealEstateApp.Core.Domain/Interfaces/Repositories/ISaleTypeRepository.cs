using System.Threading.Tasks;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Domain.Interfaces.GenericRepository;

namespace RealEstateApp.Core.Domain.Interfaces.Repositories
{
    public interface ISaleTypeRepository : IGenericRepository<SaleType, int>
    {
        Task<int> CountBySaleTypeAsync(int saleTypeId);
        // TODO PENDIENTE DE CONFIRMAR: misma estrategia de cascada pendiente que
        // PropertyType — depende de que Sebastián configure OnDelete(DeleteBehavior.Cascade)
        // en la FK Property.SaleTypeId.
        Task<bool> ExistNameAsync(string name, int id = 0);
    }
}
