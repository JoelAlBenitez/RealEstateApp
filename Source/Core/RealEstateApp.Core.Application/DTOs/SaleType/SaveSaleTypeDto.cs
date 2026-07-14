namespace RealEstateApp.Core.Application.DTOs.SaleType
{
    public sealed record SaveSaleTypeDto
    {
        // Nullable: null en creación, con valor en edición — necesario porque
        // IGenericServices.UpdateAsync no recibe TKey por separado.
        public int? Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
    }
}
