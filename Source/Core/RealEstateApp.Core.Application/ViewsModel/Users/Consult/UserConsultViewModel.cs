namespace RealEstateApp.Core.Application.ViewsModel.Users.Consult
{
    // ViewModel base para consulta de usuarios, unificando los listados
    public class UserConsultViewModel
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string LastName { get; set; }
    }
}
