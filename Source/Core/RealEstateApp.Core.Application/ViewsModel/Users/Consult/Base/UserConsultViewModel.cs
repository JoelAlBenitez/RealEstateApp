namespace RealEstateApp.Core.Application.ViewsModel.Users.Consult.Base
{
    public abstract class UserConsultViewModel 
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string LastName { get; set; }
    }
}
