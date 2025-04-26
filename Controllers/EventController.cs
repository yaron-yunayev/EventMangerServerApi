using EventMangerServerApi.Application.Interfaces;
using EventMangerServerApi.Core.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventMangerServerApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        public async Task<ActionResult<List<EventResponseDto>>> GetAllEvents()
        {
            var events = await _eventService.GetAllEventsAsync();
            return Ok(events);
        }

        [HttpGet("my-events")]
        [Authorize]
        public async Task<ActionResult<List<EventResponseDto>>> GetMyEvents()
        {
            var userId = GetUserIdFromClaims();
            if (userId == null)
                return Unauthorized("User ID claim is missing or invalid.");

            var events = await _eventService.GetEventsByManagerIdAsync(userId.Value);
            return Ok(events);
        }

        [HttpGet("manager/{managerId}")]
        public async Task<ActionResult<List<EventResponseDto>>> GetEventsByManager(int managerId)
        {
            var events = await _eventService.GetEventsByManagerIdAsync(managerId);
            if (events == null || !events.Any())
                return NotFound("No events found for this manager.");

            return Ok(events);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EventResponseDto>> GetEventById(int id)
        {
            var eventDto = await _eventService.GetEventByIdAsync(id);
            if (eventDto == null)
                return NotFound("Event not found.");

            return Ok(eventDto);
        }

        [HttpGet("supplier/{supplierId}")]
        public async Task<ActionResult<List<EventResponseDto>>> GetEventsBySupplier(int supplierId)
        {
            var events = await _eventService.GetEventsBySupplierIdAsync(supplierId);
            return Ok(events);
        }


        [HttpGet("{eventId}/suppliers")]
        public async Task<ActionResult<List<SupplierDto>>> GetSuppliersForEvent(int eventId)
        {
            var suppliers = await _eventService.GetSuppliersForEventAsync(eventId);
            if (suppliers == null || !suppliers.Any())
                return NotFound("No suppliers found for this event.");

            return Ok(suppliers);
        }
 
        [HttpPost]
        [Authorize(Policy = "MustBeEventManager")]
        public async Task<ActionResult> CreateEvent([FromBody] CreateEventDto eventDto)
        {
            var userId = GetUserIdFromClaims();
            if (userId == null)
                return Unauthorized("User ID claim is missing or invalid.");

            await _eventService.AddEventAsync(eventDto, userId.Value);
            return CreatedAtAction(nameof(GetEventById), new { id = eventDto.ManagerId }, eventDto);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "MustBeEventManager")]
        public async Task<ActionResult> UpdateEvent(int id, [FromBody] CreateEventDto eventDto)
        {
            var userId = GetUserIdFromClaims();
            if (userId == null)
                return Unauthorized("User ID claim is missing or invalid.");

            var success = await _eventService.UpdateEventAsync(id, eventDto, userId.Value);
            if (!success)
                return Forbid("You do not have permission to update this event.");

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "MustBeEventManager")]
        public async Task<ActionResult> DeleteEvent(int id)
        {
            var userId = GetUserIdFromClaims();
            if (userId == null)
                return Unauthorized("User ID claim is missing or invalid.");

            var success = await _eventService.DeleteEventAsync(id, userId.Value);
            if (!success)
                return Forbid("You do not have permission to delete this event.");

            return NoContent();
        }

        [HttpPost("assign-suppliers")]
        [Authorize(Policy = "MustBeEventManager")]
        public async Task<IActionResult> AssignSuppliersToEvent([FromBody] AssignSuppliersDto dto)
        {
            try
            {
                var success = await _eventService.AssignSuppliersToEventAsync(dto.EventId, dto.SupplierIds);
                if (!success)
                    return NotFound("Event or one of the suppliers not found.");

                return Ok("✅ All suppliers assigned successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }


        private int? GetUserIdFromClaims()
        {
            var userIdClaim = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                return null;

            return userId;
        }
    }
}
