
using RealEstateApp.Core.Domain.Common.Errors;

namespace RealEstateApp.Core.Domain.Common.CodeErrors.Property
{
    public static class PropertyErrors
    {
        public static readonly Error NotFoundByCode = new("Propiedad.NoEncontradaPorCodigo","No se encontró ninguna propiedad disponible con el código ingresado.");

        public static readonly Error NotFoundByFilter = new("Propiedad.NoEncontradaPorFiltro", "No se encontraron propiedades disponibles con los filtros seleccionados.");

        public static readonly Error NoPropertiesFound = new("Propiedad.NoSeEncontraronPropiedades", "No tiene propiedades disponibles registradas en este momento.");
    }
}
