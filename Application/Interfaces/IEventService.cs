using EventMangerServerApi.Core.Dtos;
using EventMangerServerApi.Core.Modles;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventMangerServerApi.Application.Interfaces
{
    public interface IEventService
    {
        Task<List<EventResponseDto>> GetAllEventsAsync();
        Task<EventResponseDto?> GetEventByIdAsync(int id);
        Task<List<EventResponseDto>> GetEventsBySupplierIdAsync(int supplierId);
        Task<List<EventResponseDto>> GetEventsByManagerIdAsync(int managerId); // ✅ הוספה
        Task<bool> AssignSuppliersToEventAsync(int eventId, List<int> supplierIds);

        Task<List<SupplierDto>> GetSuppliersForEventAsync(int eventId);
        Task AddEventAsync(CreateEventDto eventDto, int userId);
        Task<bool> UpdateEventAsync(int id, CreateEventDto eventDto, int userId);
        Task<bool> DeleteEventAsync(int eventId, int userId);
    }
}
