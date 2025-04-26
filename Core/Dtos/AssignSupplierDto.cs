namespace EventMangerServerApi.Core.Dtos
{
    public class AssignSuppliersDto
    {
        public int EventId { get; set; }
        public List<int> SupplierIds { get; set; }
    }

}
