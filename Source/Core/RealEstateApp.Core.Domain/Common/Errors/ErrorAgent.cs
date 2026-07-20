namespace RealEstateApp.Core.Domain.Common.Errors
{
    /// <summary>
    /// Mensajes permanentes del documento funcional para la pantalla
    /// "Listado de los agentes". Solo contiene literales reales;
    /// los errores de integración pendiente con Joel/Sebastián viven en ErrorPendingIntegration (Core.Application).
    /// </summary>
    public static class ErrorAgent
    {
      

        // ── Errores de operación ───────────────────────────────────────────
        public static Error NotFound =>
            new Error("Agent_04", "El agente seleccionado no existe.");

        public static Error DeleteFailed =>
            new Error("Agent_05", "No fue posible eliminar el agente. Intente nuevamente más tarde.");

        // ── Mensajes de éxito ──────────────────────────────────────────────
        public static Error Activated =>
            new Error("Agent_06", "El agente fue activado correctamente.");

        public static Error Inactivated =>
            new Error("Agent_07", "El agente fue inactivado correctamente.");

        public static Error Deleted =>
            new Error("Agent_08", "El agente fue eliminado correctamente.");
    }
}
