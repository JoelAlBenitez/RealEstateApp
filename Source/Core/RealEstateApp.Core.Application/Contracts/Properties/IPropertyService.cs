using RealEstateApp.Core.Domain.Common.ValidationResult;

namespace RealEstateApp.Core.Application.Contracts.Properties
{
    // PENDIENTE DE CONFIRMAR CON SEBASTIÁN: El nombre de esta interfaz (IPropertyService) es
    // especulativo y no ha sido validado contra el código real del módulo de Sebastián. La firma
    // exacta del método confirmado en el documento de coordinación del equipo (distribucion-equipo.html)
    // es la siguiente:
    //   Task DeletePropertiesByAgentAsync(string agentId)
    // Sin embargo, adaptamos el tipo de retorno a Task<ValidationResult> para poder propagar errores
    // hacia DeleteAgentAsync. Si Sebastián expone el método con retorno void/Task sin resultado,
    // esta interfaz deberá ajustarse en consecuencia.
    public interface IPropertyService
    {
        // Nombre de método confirmado en distribucion-equipo.html (Sebastián → Adrián, prioridad 🟡 Media).
        // Encapsula la cascada completa: propiedades + imágenes + mejoras + ofertas + mensajes + favoritos.
        // PENDIENTE DE CONFIRMAR CON SEBASTIÁN: tipo de retorno y firma final contra el código real.
        Task<ValidationResult> DeletePropertiesByAgentAsync(string agentId);
    }
}