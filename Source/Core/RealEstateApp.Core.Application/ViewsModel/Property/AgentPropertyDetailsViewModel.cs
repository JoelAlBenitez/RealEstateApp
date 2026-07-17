using RealEstateApp.Core.Application.ViewsModel.Offer;
using RealEstateApp.Core.Application.ViewsModel.MessageAtC;

namespace RealEstateApp.Core.Application.ViewsModel.Property
{
    public class AgentPropertyDetailsViewModel
    {
        public PropertyDetailViewModel Property { get; set; } = null!;
        public List<OfferViewModel> Offers { get; set; } = new();
        public List<MessageAtCViewModel> Conversations { get; set; } = new();
    }
}
