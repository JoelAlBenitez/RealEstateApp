namespace RealEstateApp.Core.Application.ViewsModel.Users.Consult
{
    // ViewModel para el listado de administradores
    public sealed class AdministratorListItemViewModel : UserConsultViewModel
    {
        public required string UserName { get; set; }
        public required string IdCard { get; set; }
        public required string Email { get; set; }
        public required bool IsActive { get; set; }
    }
}
