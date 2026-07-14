namespace RealEstateApp.Core.Domain.Common.Errors
{
    public static class ErrorSaleType
    {
        public static readonly Error RequiredFields = new("SaleType.RequiredFields", "Debe completar todos los campos requeridos.");
        public static readonly Error NameDuplicate = new("SaleType.NameDuplicate", "Ya existe un tipo de venta registrado con este nombre.");
        public static readonly Error NameDuplicateEdit = new("SaleType.NameDuplicateEdit", "Ya existe otro tipo de venta registrado con este nombre.");
        public static readonly Error Created = new("SaleType.Created", "El tipo de venta fue creado correctamente.");
        public static readonly Error Updated = new("SaleType.Updated", "El tipo de venta fue actualizado correctamente.");
        public static readonly Error NotExists = new("SaleType.NotExists", "No existen tipos de ventas registrados.");
        
        // Supuesto de Diseño — no citado literalmente en esta sección del documento, reutilizado por consistencia con ErrorPropertyType.NotFound
        public static readonly Error NotFound = new("SaleType.NotFound", "El tipo de venta seleccionado no existe.");
        
        // Supuesto de Diseño — mismo supuesto
        public static readonly Error DeleteFailed = new("SaleType.DeleteFailed", "No fue posible eliminar el tipo de venta. Intente nuevamente más tarde.");
        
        // Supuesto de Diseño — el documento no lo cita explícitamente para esta sección, pero es consistente con el patrón de éxito usado en Crear/Editar y con ErrorPropertyType.Deleted
        public static readonly Error Deleted = new("SaleType.Deleted", "El tipo de venta fue eliminado correctamente.");
    }
}
