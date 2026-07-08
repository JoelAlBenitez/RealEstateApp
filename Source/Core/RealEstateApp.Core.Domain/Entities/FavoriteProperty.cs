using RealEstateApp.Core.Domain.Entities.Base;

namespace RealEstateApp.Core.Domain.Entities
{
    public class FavoriteProperty : BaseEntitie<int>
    {
        public required string ClientId { get; set; } // Cliente autenticado
        public int PropertyId { get; set; }
        public Property? Property { get; set; }
    }
}
