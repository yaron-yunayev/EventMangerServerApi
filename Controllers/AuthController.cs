using EventMangerServerApi.Application.Interfaces;
using EventMangerServerApi.Application.Services;
using EventMangerServerApi.Core.Dtos;
using EventMangerServerApi.Core.Interfaces;
using EventMangerServerApi.Core.Modles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EventMangerServerApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHashingService _passwordHashService;
        private readonly IUserRepository _userRepository;

        public AuthController(IUserService userService, ITokenService tokenService, IPasswordHashingService passwordHashService, IUserRepository userRepository)
        {
            _userService = userService;
            _tokenService = tokenService;
            _passwordHashService = passwordHashService;
            _userRepository = userRepository;
        }

        // 🔹 הרשמת משתמש חדש
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = await _userRepository.GetUserByEmailAsync(userDto.Email);
            if (existingUser != null)
            {
                return Conflict("Email is already taken.");
            }

            // מיפוי של RegisterUserDto ל-User
            var user = new User
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Age = userDto.Age,
                Email = userDto.Email,
                Password = _passwordHashService.HashPassword(userDto.Password),
                IsEventManager = userDto.IsEventManager,
                EventDomain = userDto.EventDomain,
                IsraeliID = userDto.IsraeliID,
                Address = userDto.Address,
                PhoneNumber = userDto.PhoneNumber
            };

            var createdUser = await _userRepository.CreateUserAsync(user);
            if (createdUser == null)
            {
                return StatusCode(500, "User could not be created.");
            }

            return Ok(new
            {
                Message = "User created successfully",
                User = new
                {
                    createdUser.Id,
                    createdUser.FirstName,
                    createdUser.LastName,
                    createdUser.Email,
                    createdUser.IsEventManager,
                    createdUser.EventDomain,
                    createdUser.IsraeliID,
                    createdUser.Address,
                    createdUser.PhoneNumber,
                    createdUser.Role
                }
            });
        }

        // 🔹 התחברות וקבלת טוקן
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userRepository.GetUserByEmailAsync(login.Email);
            if (user == null)
            {
                return Unauthorized("Invalid credentials.");
            }

            if (!_passwordHashService.VerifyPassword(user.Password, login.Password))
            {
                return Unauthorized("Invalid credentials.");
            }

            var token = _tokenService.GenerateJwtToken(user);


            return Ok(new
            {
                Token = token,
                Id = user.Id,
                Email = user.Email,
                Role = user.Role,
                IsEventManager = user.IsEventManager,
                FavoriteSupplierIds = user.FavoriteSuppliers.Select(fs => fs.SupplierId).ToList()
            });


        }


        [Authorize(Roles = "Admin")]
        [HttpGet("admin-only")]
        public IActionResult AdminOnly()
        {
            return Ok("Welcome, Admin!");
        }

        [Authorize(Roles = "EventManager")]
        [HttpGet("event-manager-only")]
        public IActionResult EventManagerOnly()
        {
            return Ok("Welcome, Event Manager!");
        }

        [Authorize]
        [HttpGet("user-only")]
        public IActionResult UserOnly()
        {
            return Ok("Welcome, User!");
        }
    }
}
