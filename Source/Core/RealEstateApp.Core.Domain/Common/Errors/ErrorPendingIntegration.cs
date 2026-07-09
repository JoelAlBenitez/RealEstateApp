

using RealEstateApp.Core.Domain.Common.Errors;

namespace RealEstateApp.Core.Application.Common.Errors
{
    
    public static class ErrorPendingIntegration
    {
        // ── Dashboard ──────────────────────────────────────────────────────
        /// <summary>
        /// PENDIENTE: GetTotalsByStatusAsync() de Sebastián + GetUserCountersAsync() de Joel.
        /// </summary>
        public static Error DashboardStats =>
            new Error("PENDING_Dashboard_01",
                "La obtención de estadísticas del dashboard está pendiente de integración con los servicios de Joel (usuarios) y Sebastián (propiedades).");

        // ── Agentes ────────────────────────────────────────────────────────
        /// <summary>
        /// PENDIENTE: GetAgentsAsync() de IOperationalAccountWebApp (Joel).
        /// </summary>
        public static Error AgentList =>
            new Error("PENDING_Agent_01",
                "El listado de agentes está pendiente de integración con el servicio de cuentas de Joel.");

        /// <summary>
        /// PENDIENTE: ChangeUserStatusAsync() de IOperationalAccountWebApp (Joel).
        /// </summary>
        public static Error AgentToggle =>
            new Error("PENDING_Agent_02",
                "La activación o inactivación del agente está pendiente de integración con el servicio de cuentas de Joel.");

        /// <summary>
        /// PENDIENTE: DeletePropertiesByAgentAsync() de IPropertyService (Sebastián) +
        /// DeleteUserAsync() de IOperationalAccountWebApp (Joel), dentro de una transacción explícita.
        /// </summary>
        public static Error AgentDelete =>
            new Error("PENDING_Agent_03",
                "La eliminación del agente y sus propiedades está pendiente de integración con los servicios de Sebastián (propiedades) y Joel (cuenta de usuario).");

        // ── Administradores ────────────────────────────────────────────────
        /// <summary>
        /// PENDIENTE: GetUsersByRoleAsync(TypeUsers.Administrator) de IOperationalAccountWebApp (Joel).
        /// </summary>
        public static Error AdminList =>
            new Error("PENDING_Admin_01",
                "El listado de administradores está pendiente de integración con el servicio de cuentas de Joel.");

        /// <summary>
        /// PENDIENTE: RegisterInternalUserAsync() de IOperationalAccountWebApp (Joel).
        /// </summary>
        public static Error AdminCreate =>
            new Error("PENDING_Admin_02",
                "La creación de administradores está pendiente de integración con el servicio de cuentas de Joel.");

        /// <summary>
        /// PENDIENTE: EditInternalUserAsync() de IOperationalAccountWebApp (Joel).
        /// </summary>
        public static Error AdminEdit =>
            new Error("PENDING_Admin_03",
                "La edición de administradores está pendiente de integración con el servicio de cuentas de Joel.");

        /// <summary>
        /// PENDIENTE: ChangeUserStatusAsync() de IOperationalAccountWebApp (Joel).
        /// Incluye validar mínimo un admin activo, que Joel maneja internamente.
        /// </summary>
        public static Error AdminToggle =>
            new Error("PENDING_Admin_04",
                "La activación o inactivación del administrador está pendiente de integración con el servicio de cuentas de Joel.");

        // ── Desarrolladores ────────────────────────────────────────────────
        /// <summary>
        /// PENDIENTE: GetUsersByRoleAsync(TypeUsers.Developer) de IOperationalAccountWebApp (Joel).
        /// </summary>
        public static Error DeveloperList =>
            new Error("PENDING_Dev_01",
                "El listado de desarrolladores está pendiente de integración con el servicio de cuentas de Joel.");

        /// <summary>
        /// PENDIENTE: RegisterInternalUserAsync() de IOperationalAccountWebApp (Joel).
        /// </summary>
        public static Error DeveloperCreate =>
            new Error("PENDING_Dev_02",
                "La creación de desarrolladores está pendiente de integración con el servicio de cuentas de Joel.");

        /// <summary>
        /// PENDIENTE: EditInternalUserAsync() de IOperationalAccountWebApp (Joel).
        /// </summary>
        public static Error DeveloperEdit =>
            new Error("PENDING_Dev_03",
                "La edición de desarrolladores está pendiente de integración con el servicio de cuentas de Joel.");

        /// <summary>
        /// PENDIENTE: ChangeUserStatusAsync() de IOperationalAccountWebApp (Joel).
        /// </summary>
        public static Error DeveloperToggle =>
            new Error("PENDING_Dev_04",
                "La activación o inactivación del desarrollador está pendiente de integración con el servicio de cuentas de Joel.");
    }
}
