using RealEstateApp.Core.Domain.Common.Errors;

namespace RealEstateApp.Core.Domain.Common.CodeErrors.Message
{
    public static class MessageErrors
    {
        public static readonly Error EmptyMessage = new("Mensaje.MensajeVacio", "El contenido del mensaje no puede estar vacío.");
    }
}
