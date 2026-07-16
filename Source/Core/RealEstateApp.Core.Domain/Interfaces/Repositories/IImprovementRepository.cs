using System.Threading.Tasks;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Domain.Interfaces.GenericRepository;

namespace RealEstateApp.Core.Domain.Interfaces.Repositories
{
    public interface IImprovementRepository : IGenericRepository<Improvement, int>
    {
        Task<int> CountByImprovementAsync(int improvementId);
        
        // Al eliminar, solo se remueve la relación PropertyImprovement, NO las
        // propiedades (el doocumento dice : "Las propiedades asociadas a la mejora no
        // deben eliminarse"). PENDIENTE: confirmar con Sebastián cómo se maneja
        // la tabla puente PropertyImprovement.
    }
}
