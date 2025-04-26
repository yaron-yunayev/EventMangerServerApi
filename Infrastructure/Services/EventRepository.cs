using EventMangerServerApi.Core.Dtos;
using EventMangerServerApi.Core.Interfaces;
using EventMangerServerApi.Core.Modles;
using EventMangerServerApi.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventMangerServerApi.Infrastructure.Services
{
    public class EventRepository : IEventRepository
    {
        private readonly EventProjectDbContext _context;

        public EventRepository(EventProjectDbContext context)
        {
            _context = context;
        }

        public async Task<List<Event>> GetAllEventsAsync()
        {
            return await _context.Events
                .Include(e => e.Manager)
                .Include(e => e.Suppliers)
                .ToListAsync();
        }

        public async Task<Event?> GetEventByIdAsync(int id)
        {
            return await _context.Events
                .Include(e => e.Manager)
                .Include(e => e.Suppliers)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<List<Event>> GetEventsBySupplierIdAsync(int supplierId)
        {
            return await _context.Events
                .Include(e => e.Manager)
                .Include(e => e.Suppliers)
                .Where(e => e.Suppliers.Any(s => s.Id == supplierId))
                .ToListAsync();
        }

        public async Task<List<Event>> GetEventsByManagerIdAsync(int managerId)
        {
            return await _context.Events
                .Include(e => e.Manager)
                .Include(e => e.Suppliers)
                .Where(e => e.ManagerId == managerId)
                .ToListAsync();
        }

       


        public async Task AddEventAsync(Event eventEntity)
        {
            await _context.Events.AddAsync(eventEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateEventAsync(Event eventEntity)
        {
            var existingEvent = await _context.Events
                .Include(e => e.Suppliers)
                .FirstOrDefaultAsync(e => e.Id == eventEntity.Id);

            if (existingEvent == null)
                return false;

            _context.Entry(existingEvent).CurrentValues.SetValues(eventEntity);

            foreach (var supplier in eventEntity.Suppliers)
            {
                if (!existingEvent.Suppliers.Any(s => s.Id == supplier.Id))
                {
                    existingEvent.Suppliers.Add(supplier);
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }



        public async Task<bool> DeleteEventAsync(int id)
        {
            var eventEntity = await _context.Events
                .Include(e => e.Suppliers)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (eventEntity == null)
                return false;

            _context.Events.Remove(eventEntity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Event?> GetEventByIdWithSuppliersAsync(int id)
        {
            return await _context.Events
                .Include(e => e.Suppliers)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<List<Supplier>> GetSuppliersByIdsAsync(List<int> supplierIds)
        {
            return await _context.Suppliers
                .Where(s => supplierIds.Contains(s.Id))
                .ToListAsync();
        }
    }
}
