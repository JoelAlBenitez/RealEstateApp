namespace RealEstateApp.Core.Domain.Common.Errors
{
    public static class DomainErrors
    {
        public static class PropertyErrors
        {
            public static readonly Error NotFoundByCode = new(
                "Property.NotFoundByCode", 
                "No se encontró ninguna propiedad disponible con el código ingresado."
            );

            public static readonly Error NotFoundByFilter = new(
                "Property.NotFoundByFilter", 
                "No se encontraron propiedades disponibles con los filtros seleccionados."
            );

            public static readonly Error NoPropertiesFound = new(
                "Property.NoPropertiesFound", 
                "No tiene propiedades disponibles registradas en este momento."
            );
        }

        public static class OfferErrors
        {
            public static readonly Error PendingOfferExists = new(
                "Offer.PendingOfferExists", 
                "Ya tiene una oferta pendiente para esta propiedad."
            );

            public static readonly Error PropertyHasAcceptedOffer = new(
                "Offer.PropertyHasAcceptedOffer", 
                "Esta propiedad ya tiene una oferta aceptada y no permite nuevas ofertas."
            );

            public static readonly Error OfferAlreadyAnswered = new(
                "Offer.AlreadyAnswered", 
                "Esta oferta ya fue respondida."
            );

            public static readonly Error PropertyAlreadySold = new(
                "Offer.PropertyAlreadySold", 
                "No se puede aceptar una oferta para una propiedad que ya fue vendida."
            );

            public static readonly Error InvalidAmount = new(
                "Offer.InvalidAmount", 
                "El monto de la oferta debe ser un valor numérico mayor que cero."
            );
        }

        public static class FavoriteErrors
        {
            public static readonly Error AlreadyFavorite = new(
                "Favorite.AlreadyFavorite", 
                "Esta propiedad ya está agregada a sus favoritos."
            );
        }
        
        public static class MessageErrors
        {
            public static readonly Error EmptyMessage = new(
                "Message.EmptyMessage", 
                "El contenido del mensaje no puede estar vacío."
            );
        }
    }
}
