namespace RealEstateApp.Core.Domain.Entities.Base
{
    public  class BaseEntitie <TKey>
    {
        public TKey? Id { get; set; }
        public required DateTimeOffset CreateAt { get; set; }
        public required DateTimeOffset UpdateAt { get; set; }
    }
}
