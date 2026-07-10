using RealEstateApp.Core.Domain.Common.Errors;

namespace RealEstateApp.Core.Domain.Common.CodeErrors.Favorite
{
    public static class FavoriteErrors
    {
        public static readonly Error AlreadyFavorite = new("Favorite.AlreadyFavorite","Esta propiedad ya está agregada a sus favoritos.");
    }
}
