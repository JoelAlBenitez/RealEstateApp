namespace RealEstateApp.Core.Application.ViewsModel.Users.Consult
{
    // ViewModel para las pantallas de confirmación (activar/inactivar y eliminar) de un agente.
    public sealed class AgentActionConfirmViewModel
    {
        public required string Id { get; set; }
        public required string FullName { get; set; }
        public required bool IsActive { get; set; }
        public int Properties { get; set; }
    }
}
