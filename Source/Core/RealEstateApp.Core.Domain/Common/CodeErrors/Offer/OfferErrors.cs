using RealEstateApp.Core.Domain.Common.Errors;

namespace RealEstateApp.Core.Domain.Common.CodeErrors.Offer
{
    public static class OfferErrors
    {
        public static readonly Error PendingOfferExists = new("Offer.PendingOfferExists", "Ya tiene una oferta pendiente para esta propiedad.");

        public static readonly Error PropertyHasAcceptedOffer = new("Offer.PropertyHasAcceptedOffer", "Esta propiedad ya tiene una oferta aceptada y no permite nuevas ofertas.");

        public static readonly Error OfferAlreadyAnswered = new("Offer.PropertyAlreadyAnswered", "Esta oferta ya fue respondida.");

        public static readonly Error PropertyAlreadySold = new("Offer.PropertyAlreadySold", "No se puede aceptar una oferta de una propiedad que ya fue vendida.");

        public static readonly Error InvalidAmount = new("Offer.InvalidAmount", "El monto de la oferta debe ser un valor numérico mayor que cero.");
    }
}
