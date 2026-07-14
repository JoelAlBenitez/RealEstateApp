
namespace RealEstateApp.Core.Application.ViewsModel.Users.Consult
{
    // ViewModel para el listado de desarrolladores
    public sealed class DeveloperListItemViewModel : UserConsultViewModel
    {
        public required string UserName { get; set; }
        public required string IdCard { get; set; }
        public required string Email { get; set; }
        public required bool IsActive { get; set; }
    }
}
