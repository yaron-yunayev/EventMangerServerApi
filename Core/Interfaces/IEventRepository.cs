using EventMangerServerApi.Core.Modles;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventMangerServerApi.Core.Interfaces
{
    public interface IEventRepository
    {
        Task<List<Event>> GetAllEventsAsync();
        Task<Event?> GetEventByIdAsync(int id);
        Task<List<Event>> GetEventsBySupplierIdAsync(int supplierId);
        Task<List<Event>> GetEventsByManagerIdAsync(int managerId); // ✅ פונקציה חדשה
        Task<Event?> GetEventByIdWithSuppliersAsync(int id); // נוספה לצורך עבודה עם ספקים

        Task AddEventAsync(Event eventEntity);
        Task<bool> UpdateEventAsync(Event eventEntity);
        Task<bool> DeleteEventAsync(int id);

        // פונקציה לטעינת ספקים לפי מזהים
        Task<List<Supplier>> GetSuppliersByIdsAsync(List<int> supplierIds);
    }
}
