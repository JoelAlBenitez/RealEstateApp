using RealEstateApp.Core.Domain.Entities.Base;
using RealEstateApp.Core.Domain.Enums;

namespace RealEstateApp.Core.Domain.Entities
{
    public class Offer : BaseEntitie<int>
    {
        public required string ClientId { get; set; } // Cliente que oferta
        public int PropertyId { get; set; }
        public Property? Property { get; set; }
        public decimal Amount { get; set; } // DOP
        public OfferStatus Status { get; set; } // Enum (Pendiente, Aceptada, Rechazada)
    }
}
