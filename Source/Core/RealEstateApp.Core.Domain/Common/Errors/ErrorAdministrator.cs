namespace RealEstateApp.Core.Domain.Common.Errors
{
  
    //Mensajes permanentes del documento funcional para la pantalla "Mantenimiento de administradores". Solo contiene literales reales;
    // los errores de integración pendiente con Joel viven en ErrorPendingIntegration (Core.Application).
  
    public static class ErrorAdministrator
    {
        //  Campos requeridos
        public static Error RequiredFields =>
            new Error("Admin_01", "Debe completar todos los campos requeridos.");

        public static Error NotFound =>
            new Error("Admin_13", "El administrador seleccionado no existe.");

        //  Unicidad de datos 
        public static Error EmailDuplicate =>
            new Error("Admin_02", "Ya existe un usuario registrado con este correo electrónico.");

        public static Error UsernameDuplicate =>
            new Error("Admin_03", "Ya existe un usuario registrado con este nombre de usuario.");

        public static Error IDCardDuplicate =>
            new Error("Admin_04", "Ya existe un usuario registrado con esta cédula.");

        // ── Contraseña
        public static Error PasswordMismatch =>
            new Error("Admin_05", "La contraseña y la confirmación de contraseña no coinciden.");

        //  Reglas de auto-protección 
        public static Error SelfEdit =>
            new Error("Admin_06", "No puede editar su propio usuario desde este mantenimiento.");

        public static Error SelfInactivation =>
            new Error("Admin_07", "No puede inactivar a su propio usuario.");

        public static Error LastAdminRequired =>
            new Error("Admin_08", "Debe existir al menos un administrador activo en el sistema.");

        //  Mensajes de éxito
        public static Error Created =>
            new Error("Admin_09", "El administrador fue creado correctamente.");

        public static Error Updated =>
            new Error("Admin_10", "El administrador fue actualizado correctamente.");

        public static Error Activated =>
            new Error("Admin_11", "El administrador fue activado correctamente.");

        public static Error Inactivated =>
            new Error("Admin_12", "El administrador fue inactivado correctamente.");
    }
}
