namespace RealEstateApp.Core.Domain.Common.Errors
{
    
    // Mensajes permanentes del documento funcional para la pantalla
    // "Mantenimiento de desarrolladores". Idénticos a ErrorAdministrator en
    // campos/duplicados/contraseña, pero SIN las 3 reglas de auto-protección
    // (auto-edición, auto-inactivación, mínimo activo) — confirmado en especificación:
    //esas reglas NO aplican al mantenimiento de desarrolladores.
    //  Los errores de integración pendiente con Joel viven en ErrorPendingIntegration (Core.Application).
  
    public static class ErrorDeveloper
    {
        // ── Campos requeridos ──────────────────────────────────────────────
        public static Error RequiredFields =>
            new Error("Dev_01", "Debe completar todos los campos requeridos.");

        // ── Unicidad de datos ──────────────────────────────────────────────
        public static Error EmailDuplicate =>
            new Error("Dev_02", "Ya existe un usuario registrado con este correo electrónico.");

        public static Error UsernameDuplicate =>
            new Error("Dev_03", "Ya existe un usuario registrado con este nombre de usuario.");

        public static Error IDCardDuplicate =>
            new Error("Dev_04", "Ya existe un usuario registrado con esta cédula.");

        // ── Contraseña ─────────────────────────────────────────────────────
        public static Error PasswordMismatch =>
            new Error("Dev_05", "La contraseña y la confirmación de contraseña no coinciden.");

        // ── Mensajes de éxito ──────────────────────────────────────────────
        public static Error Created =>
            new Error("Dev_06", "El desarrollador fue creado correctamente.");

        public static Error Updated =>
            new Error("Dev_07", "El desarrollador fue actualizado correctamente.");

        public static Error Activated =>
            new Error("Dev_08", "El desarrollador fue activado correctamente.");

        public static Error Inactivated =>
            new Error("Dev_09", "El desarrollador fue inactivado correctamente.");
    }
}
