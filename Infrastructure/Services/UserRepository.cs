using EventMangerServerApi.Core.Interfaces;
using EventMangerServerApi.Core.Modles;
using EventMangerServerApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EventMangerServerApi.Infrastructure.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly EventProjectDbContext _projectDbContext;

        public UserRepository(EventProjectDbContext context)
        {
            _projectDbContext = context;
        }

        public async Task<User?> CreateUserAsync(User user)
        {
            try
            {
                _projectDbContext.Users.Add(user);
                await _projectDbContext.SaveChangesAsync();
                return user;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            try
            {
                return await _projectDbContext.Users
                    .Include(u => u.FavoriteSuppliers)
                    .FirstOrDefaultAsync(u => u.Email == email);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }


        public async Task<bool> UserExistsAsync(string email)
        {
            var user = await _projectDbContext.Users
                .FirstOrDefaultAsync(u => u.Email == email);
            return user != null;
        }

      public async Task<User?> UpdateUserAsync(User user)
{
    try
    {
        var existingUser = await _projectDbContext.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
        if (existingUser == null)
            return null;

        existingUser.FirstName = user.FirstName;
        existingUser.LastName = user.LastName;
        existingUser.PhoneNumber = user.PhoneNumber;
        existingUser.Age = user.Age;
        existingUser.Address = user.Address;

        if (!string.IsNullOrEmpty(user.Role) && user.Role != existingUser.Role)
        {
            existingUser.Role = user.Role;

            switch (user.Role)
            {
                case "EventManager":
                    existingUser.IsEventManager = true;
                    existingUser.EventDomain = user.EventDomain ?? existingUser.EventDomain; 
                    existingUser.IsraeliID = user.IsraeliID ?? existingUser.IsraeliID;
                    break;

                case "Admin":
                    existingUser.IsEventManager = false;
                    existingUser.EventDomain = null;
                    existingUser.IsraeliID = null;
                    break;

                default:
                    existingUser.IsEventManager = false; // ברירת מחדל למי שאינו Admin או EventManager
                    break;
            }
        }

        await _projectDbContext.SaveChangesAsync();
        return existingUser;
    }
    catch (Exception e)
    {
        Console.WriteLine($"Error updating user: {e.Message}");
        return null;
    }
}


        public async Task<bool> DeleteUserAsync(int userId)
        {
            try
            {
                var user = await _projectDbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                    return false;

                _projectDbContext.Users.Remove(user);
                await _projectDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _projectDbContext.Users.ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _projectDbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<List<User>> GetUsersByRoleAsync(string role)
        {
            try
            {
                return await _projectDbContext.Users
                    .Where(u => u.Role == role)
                    .ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<User>();
            }
        }

        public async Task<bool> AddFavoriteSupplierAsync(int userId, int supplierId)
        {
            var exists = await _projectDbContext.Set<UserFavoriteSupplier>()
                .AnyAsync(f => f.UserId == userId && f.SupplierId == supplierId);

            if (exists) return false;

            _projectDbContext.Set<UserFavoriteSupplier>().Add(new UserFavoriteSupplier
            {
                UserId = userId,
                SupplierId = supplierId
            });

            await _projectDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveFavoriteSupplierAsync(int userId, int supplierId)
        {
            var fav = await _projectDbContext.Set<UserFavoriteSupplier>()
                .FirstOrDefaultAsync(f => f.UserId == userId && f.SupplierId == supplierId);

            if (fav == null) return false;

            _projectDbContext.Set<UserFavoriteSupplier>().Remove(fav);
            await _projectDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Supplier>> GetFavoriteSuppliersAsync(int userId)
        {
            return await _projectDbContext.Set<UserFavoriteSupplier>()
                .Where(f => f.UserId == userId)
                .Select(f => f.Supplier)
                .ToListAsync();
        }

        public async Task<bool> UserHasRoleAsync(int userId, string role)
        {
            try
            {
                var user = await _projectDbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
                return user != null && user.Role == role;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public async Task<List<User>> GetEventManagersAsync()
        {
            return await GetUsersByRoleAsync("EventManager");
        }

        public async Task<List<User>> GetAdminsAsync()
        {
            return await GetUsersByRoleAsync("Admin");
        }
    }
}
