using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.ViewsModel.Offer
{
    public class CreateOfferViewModel
    {
        public int PropertyId { get; set; }

        [Required(ErrorMessage = "Debe ingresar el monto de la oferta.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto de la oferta debe ser un valor numérico mayor que cero.")]
        public decimal Amount { get; set; }
    }
}
