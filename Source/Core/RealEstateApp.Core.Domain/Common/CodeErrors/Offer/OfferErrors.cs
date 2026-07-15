using RealEstateApp.Core.Domain.Common.Errors;

namespace RealEstateApp.Core.Domain.Common.CodeErrors.Offer
{
    public static class OfferErrors
    {
        public static readonly Error PendingOfferExists = new("Oferta.OfertaPendienteExiste", "Ya tiene una oferta pendiente para esta propiedad.");

        public static readonly Error PropertyHasAcceptedOffer = new("Oferta.PropiedadTieneOfertaAceptada", "Esta propiedad ya tiene una oferta aceptada y no permite nuevas ofertas.");

        public static readonly Error OfferAlreadyAnswered = new("Oferta.OfertaYaRespondida", "Esta oferta ya fue respondida.");

        public static readonly Error PropertyAlreadySold = new("Oferta.PropiedadYaVendida", "No se puede aceptar una oferta de una propiedad que ya fue vendida.");

        public static readonly Error InvalidAmount = new("Oferta.MontoInvalido", "El monto de la oferta debe ser un valor numérico mayor que cero.");
    }
}
