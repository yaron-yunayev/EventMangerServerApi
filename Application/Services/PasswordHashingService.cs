using EventMangerServerApi.Core.Modles;
using Microsoft.AspNetCore.Identity;

namespace EventMangerServerApi.Application.Services
{
    public interface IPasswordHashingService
    {
        string HashPassword(string password);
        bool VerifyPassword(string hashedPassword, string password);
    }

    public class PasswordHashingService : IPasswordHashingService
    {
        private readonly PasswordHasher<User> _passwordHasher;

        public PasswordHashingService()
        {
            _passwordHasher = new PasswordHasher<User>();
        }

        public string HashPassword(string password)
        {
            var user = new User(); 
            return _passwordHasher.HashPassword(user, password);
        }

        public bool VerifyPassword(string hashedPassword, string password)
        {
            var user = new User(); 
            var verificationResult = _passwordHasher.VerifyHashedPassword(user, hashedPassword, password);
            return verificationResult == PasswordVerificationResult.Success;
        }
    }
}


