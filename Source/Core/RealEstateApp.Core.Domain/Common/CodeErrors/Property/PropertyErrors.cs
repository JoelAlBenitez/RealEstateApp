
using RealEstateApp.Core.Domain.Common.Errors;

namespace RealEstateApp.Core.Domain.Common.CodeErrors.Property
{
    public static class PropertyErrors
    {
        public static readonly Error NotFoundByCode = new("Property.NotFoundByCode","No se encontro ninguna propiedad disponible con el codigo ingresado");

        public static readonly Error NotFoundByFilter = new("Property.NotFoundByFilter", "No se encontro propiedades disponibles con los filtros seleccionados");

        public static readonly Error NoPropertiesFound = new("Property.NoPropertiesFound", "No tiene propieades disponibles registradas en este momento");
    }
}
