using EventMangerServerApi.Core.Dtos;
using EventMangerServerApi.Core.Modles;
using System.Threading.Tasks;

namespace EventMangerServerApi.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserResponseDto?> RegisterUserAsync(RegisterUserDto userDto);
        Task<UserResponseDto?> GetUserByIdAsync(int userId);
        Task<UserResponseDto?> GetUserByEmailAsync(string email);
        Task<UserResponseDto?> UpdateUserAsync(int userId, UpdateUserDto userDto);
        Task<bool> DeleteUserAsync(int userId);
        Task<bool> UserExistsAsync(string email);
        Task<bool> AddFavoriteSupplierAsync(int userId, int supplierId);
        Task<bool> RemoveFavoriteSupplierAsync(int userId, int supplierId);
        Task<List<Supplier>> GetFavoriteSuppliersAsync(int userId);


    }
}
