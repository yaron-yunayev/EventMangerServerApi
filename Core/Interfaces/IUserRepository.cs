using EventMangerServerApi.Core.Modles;

public interface IUserRepository
{
    Task<User?> CreateUserAsync(User user);
    Task<User?> GetUserByEmailAsync(string email);
    Task<User?> UpdateUserAsync(User user);
    Task<bool> DeleteUserAsync(int userId);
    Task<List<User>> GetAllUsersAsync();
    Task<bool> UserExistsAsync(string email);
    Task<User?> GetUserByIdAsync(int userId);
    Task<List<User>> GetEventManagersAsync();
    Task<List<User>> GetAdminsAsync();


    Task<bool> AddFavoriteSupplierAsync(int userId, int supplierId);
    Task<bool> RemoveFavoriteSupplierAsync(int userId, int supplierId);
    Task<List<Supplier>> GetFavoriteSuppliersAsync(int userId);

}
