namespace EventMangerServerApi.Core.Dtos
{
    public class AddSupplierToEventDto
    {
        public int EventId { get; set; }
        public int SupplierId { get; set; }
    }

}