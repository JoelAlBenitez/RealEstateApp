namespace RealEstateApp.Core.Domain.Common.Errors
{
    public static class ErrorImprovement
    {
        public static readonly Error RequiredFields = new("Improvement.RequiredFields", "Debe completar todos los campos requeridos.");
        public static readonly Error NameDuplicate = new("Improvement.NameDuplicate", "Ya existe una mejora registrada con este nombre.");
        public static readonly Error NameDuplicateEdit = new("Improvement.NameDuplicateEdit", "Ya existe otra mejora registrada con este nombre.");
        public static readonly Error Created = new("Improvement.Created", "La mejora fue creada correctamente.");
        public static readonly Error Updated = new("Improvement.Updated", "La mejora fue actualizada correctamente.");
        public static readonly Error NotExists = new("Improvement.NotExists", "No existen mejoras registradas.");
        public static readonly Error Forbidden = new("Improvement.Forbidden", "No tiene permisos para realizar esta acción.");

        // Supuesto de Diseño — no citado literalmente en esta sección, reutilizado por consistencia
        public static readonly Error NotFound = new("Improvement.NotFound", "La mejora seleccionada no existe.");
        
        // Supuesto de Diseño — mismo supuesto
        public static readonly Error DeleteFailed = new("Improvement.DeleteFailed", "No fue posible eliminar la mejora. Intente nuevamente más tarde.");
        
        // Supuesto de Diseño — consistente con el patrón de éxito
        public static readonly Error Deleted = new("Improvement.Deleted", "La mejora fue eliminada correctamente.");
    }
}
