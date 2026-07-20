namespace RealEstateApp.Core.Domain.Common.Errors
{
    public static class ErrorPropertyType
    {
        public static readonly Error RequiredFields = new("PropertyType.RequiredFields", "Debe completar todos los campos requeridos.");
        public static readonly Error NameDuplicate = new("PropertyType.NameDuplicate", "Ya existe un tipo de propiedad registrado con este nombre.");
        public static readonly Error NameDuplicateEdit = new("PropertyType.NameDuplicateEdit", "Ya existe otro tipo de propiedad registrado con este nombre.");
        public static readonly Error Created = new("PropertyType.Created", "El tipo de propiedad fue creado correctamente.");
        public static readonly Error Updated = new("PropertyType.Updated", "El tipo de propiedad fue actualizado correctamente.");
        public static readonly Error Deleted = new("PropertyType.Deleted", "El tipo de propiedad fue eliminado correctamente.");
        public static readonly Error NotFound = new("PropertyType.NotFound", "El tipo de propiedad seleccionado no existe.");
        public static readonly Error DeleteFailed = new("PropertyType.DeleteFailed", "No fue posible eliminar el tipo de propiedad. Intente nuevamente más tarde.");
        public static readonly Error NotExists = new("PropertyType.NotExists", "No existen tipos de propiedades registrados.");
        public static readonly Error Forbidden = new("PropertyType.Forbidden", "No tiene permisos para realizar esta acción.");
    }
}
