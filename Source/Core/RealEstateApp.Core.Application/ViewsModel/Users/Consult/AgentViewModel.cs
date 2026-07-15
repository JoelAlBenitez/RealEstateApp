using RealEstateApp.Core.Application.ViewsModel.Users.Consult.Base;

namespace RealEstateApp.Core.Application.ViewsModel.Users.Consult
{
    public sealed class AgentViewModel : UserConsultViewModel
    {
        public required string ProfileImgAgent {  get; set; }
    }
}
