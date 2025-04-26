using EventMangerServerApi.Application.Interfaces;
using EventMangerServerApi.Core.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventMangerServerApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdUser = await _userService.RegisterUserAsync(userDto);
            if (createdUser == null)
                return Conflict("Email is already taken.");

            return Ok(createdUser);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedUser = await _userService.UpdateUserAsync(id, userDto);
            if (updatedUser == null)
                return NotFound("User not found.");

            return Ok(updatedUser);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var isDeleted = await _userService.DeleteUserAsync(id);
            if (!isDeleted)
                return NotFound("User not found.");

            return Ok("User deleted successfully.");
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound("User not found.");

            return Ok(user);
        }

        [HttpPost("{userId}/favorites/{supplierId}")]
        [Authorize]
        public async Task<IActionResult> AddFavoriteSupplier(int userId, int supplierId)
        {
            var success = await _userService.AddFavoriteSupplierAsync(userId, supplierId);
            if (!success)
                return BadRequest("Unable to add supplier to favorites.");

            return Ok("Supplier added to favorites.");
        }

        [HttpDelete("{userId}/favorites/{supplierId}")]
        [Authorize]
        public async Task<IActionResult> RemoveFavoriteSupplier(int userId, int supplierId)
        {
            var success = await _userService.RemoveFavoriteSupplierAsync(userId, supplierId);
            if (!success)
                return BadRequest("Unable to remove supplier from favorites.");

            return Ok("Supplier removed from favorites.");
        }

        [HttpGet("{userId}/favorites")]
        [Authorize]
        public async Task<IActionResult> GetFavoriteSuppliers(int userId)
        {
            var suppliers = await _userService.GetFavoriteSuppliersAsync(userId);
            return Ok(suppliers);
        }
    }
}
