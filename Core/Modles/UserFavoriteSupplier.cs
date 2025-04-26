namespace EventMangerServerApi.Core.Modles
{
    public class UserFavoriteSupplier
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }
    }

}
