using EventMangerServerApi.Application.Interfaces;
using EventMangerServerApi.Core.Dtos;
using EventMangerServerApi.Core.Interfaces;
using EventMangerServerApi.Core.Modles;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EventMangerServerApi.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHashingService _passwordHashService;

        public UserService(IUserRepository userRepository, IPasswordHashingService passwordHashService)
        {
            _userRepository = userRepository;
            _passwordHashService = passwordHashService;
        }

        public async Task<UserResponseDto?> RegisterUserAsync(RegisterUserDto userDto)
        {
            if (await _userRepository.UserExistsAsync(userDto.Email))
                return null;

            var newUser = new User
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Age = userDto.Age,
                Email = userDto.Email,
                Password = _passwordHashService.HashPassword(userDto.Password), // הצפנת סיסמה
                IsEventManager = userDto.IsEventManager,
                EventDomain = userDto.EventDomain,
                IsraeliID = userDto.IsraeliID,
                Address = userDto.Address,
                PhoneNumber = userDto.PhoneNumber

            };

            var createdUser = await _userRepository.CreateUserAsync(newUser);
            return createdUser == null ? null : MapToDto(createdUser);
        }

        public async Task<UserResponseDto?> UpdateUserAsync(int userId, UpdateUserDto userDto)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(userId);
            if (existingUser == null) return null;

            existingUser.FirstName = userDto.FirstName;
            existingUser.LastName = userDto.LastName;
            existingUser.Age = userDto.Age;
            existingUser.Email = userDto.Email;
            existingUser.IsEventManager = userDto.IsEventManager;
            existingUser.EventDomain = userDto.EventDomain;
            existingUser.IsraeliID = userDto.IsraeliID;
            existingUser.Address = userDto.Address;
            existingUser.PhoneNumber = userDto.PhoneNumber;

            var updatedUser = await _userRepository.UpdateUserAsync(existingUser);
            return updatedUser == null ? null : MapToDto(updatedUser);
        }

        public async Task<UserResponseDto?> GetUserByIdAsync(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            return user == null ? null : MapToDto(user);
        }

        public async Task<UserResponseDto?> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            return user == null ? null : MapToDto(user);
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            return await _userRepository.DeleteUserAsync(userId);
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            return await _userRepository.UserExistsAsync(email);
        }

        public async Task<bool> AddFavoriteSupplierAsync(int userId, int supplierId)
        {
            return await _userRepository.AddFavoriteSupplierAsync(userId, supplierId);
        }

        public async Task<bool> RemoveFavoriteSupplierAsync(int userId, int supplierId)
        {
            return await _userRepository.RemoveFavoriteSupplierAsync(userId, supplierId);
        }

        public async Task<List<Supplier>> GetFavoriteSuppliersAsync(int userId)
        {
            return await _userRepository.GetFavoriteSuppliersAsync(userId);
        }




        private UserResponseDto MapToDto(User user)
        {
            return new UserResponseDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Age = user.Age,
                Email = user.Email,
                IsEventManager = user.IsEventManager,
                EventDomain = user.EventDomain,
                IsraeliID = user.IsraeliID,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role,

                FavoriteSupplierIds = user.FavoriteSuppliers?
                    .Select(f => f.SupplierId)
                    .ToList() ?? new List<int>()
            };
        }

    }
}

