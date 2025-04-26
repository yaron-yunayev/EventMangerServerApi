using EventMangerServerApi.Application.Interfaces;
using EventMangerServerApi.Core.Dtos;
using EventMangerServerApi.Core.Interfaces;
using EventMangerServerApi.Core.Modles;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventMangerServerApi.Application.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISupplierRepository _supplierRepository;

        public EventService(IEventRepository eventRepository, IUserRepository userRepository, ISupplierRepository supplierRepository)
        {
            _eventRepository = eventRepository;
            _userRepository = userRepository;
            _supplierRepository = supplierRepository;
        }

        public async Task<List<EventResponseDto>> GetAllEventsAsync()
        {
            var events = await _eventRepository.GetAllEventsAsync();

            return events.Select(e => new EventResponseDto
            {
                Id = e.Id,
                Name = e.Name,
                Date = e.Date,
                Location = e.Location,
                Description = e.Description,
                NumberOfGuests = e.NumberOfGuests,
                Manager = e.Manager != null ? new ManagerDto
                {
                    Id = e.Manager.Id,
                    FirstName = e.Manager.FirstName,
                    LastName = e.Manager.LastName
                } : null,
                Suppliers = e.Suppliers.Select(s => new SupplierDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Category = s.Category
                }).ToList()
            }).ToList();
        }

        public async Task<List<SupplierDto>> GetSuppliersForEventAsync(int eventId)
        {
            var @event = await _eventRepository.GetEventByIdWithSuppliersAsync(eventId);
            if (@event == null) return null;

            // המרת הספקים ל-Dto
            return @event.Suppliers.Select(supplier => new SupplierDto
            {
                Id = supplier.Id,
                Name = supplier.Name,
                Category = supplier.Category
            }).ToList();
        }


        public async Task<EventResponseDto?> GetEventByIdAsync(int id)
        {
            var eventEntity = await _eventRepository.GetEventByIdAsync(id);
            if (eventEntity == null) return null;

            return new EventResponseDto
            {
                Id = eventEntity.Id,
                Name = eventEntity.Name,
                Date = eventEntity.Date,
                Location = eventEntity.Location,
                Description = eventEntity.Description,
                NumberOfGuests = eventEntity.NumberOfGuests,
                Manager = eventEntity.Manager != null ? new ManagerDto
                {
                    Id = eventEntity.Manager.Id,
                    FirstName = eventEntity.Manager.FirstName,
                    LastName = eventEntity.Manager.LastName
                } : null,
                Suppliers = eventEntity.Suppliers.Select(s => new SupplierDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Category = s.Category
                }).ToList()
            };
        }

        public async Task<bool> AssignSuppliersToEventAsync(int eventId, List<int> supplierIds)
        {
            var @event = await _eventRepository.GetEventByIdWithSuppliersAsync(eventId);
            if (@event == null)
                return false;

            var suppliers = await _eventRepository.GetSuppliersByIdsAsync(supplierIds);
            if (suppliers == null || !suppliers.Any())
                return false;

            foreach (var supplier in suppliers)
            {
                if (!@event.Suppliers.Any(s => s.Id == supplier.Id))
                {
                    @event.Suppliers.Add(supplier);  // הוספת ספקים לאירוע
                }
            }

            return await _eventRepository.UpdateEventAsync(@event);
        }



        public async Task<List<EventResponseDto>> GetEventsBySupplierIdAsync(int supplierId)
        {
            var events = await _eventRepository.GetEventsBySupplierIdAsync(supplierId);

            return events.Select(e => new EventResponseDto
            {
                Id = e.Id,
                Name = e.Name,
                Date = e.Date,
                Location = e.Location,
                Description = e.Description,
                NumberOfGuests = e.NumberOfGuests,
                Manager = e.Manager != null ? new ManagerDto
                {
                    Id = e.Manager.Id,
                    FirstName = e.Manager.FirstName,
                    LastName = e.Manager.LastName
                } : null,
                Suppliers = e.Suppliers.Select(s => new SupplierDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Category = s.Category
                }).ToList()
            }).ToList();
        }

        public async Task AddEventAsync(CreateEventDto eventDto, int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null || !user.IsEventManager)
            {
                throw new UnauthorizedAccessException("You are not authorized to create an event.");
            }

            var suppliers = await _eventRepository.GetSuppliersByIdsAsync(eventDto.SupplierIds);

            var eventEntity = new Event
            {
                Name = eventDto.Name,
                Date = eventDto.Date,
                Location = eventDto.Location,
                Description = eventDto.Description,
                NumberOfGuests = eventDto.NumberOfGuests,
                ManagerId = userId,
                Suppliers = suppliers
            };

            await _eventRepository.AddEventAsync(eventEntity);
        }

        public async Task<bool> UpdateEventAsync(int id, CreateEventDto eventDto, int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null || !user.IsEventManager)
            {
                throw new UnauthorizedAccessException("You are not authorized to update this event.");
            }

            var existingEvent = await _eventRepository.GetEventByIdAsync(id);
            if (existingEvent == null || existingEvent.ManagerId != userId)
            {
                throw new UnauthorizedAccessException("You are not allowed to update this event.");
            }

            existingEvent.Name = eventDto.Name;
            existingEvent.Date = eventDto.Date;
            existingEvent.Location = eventDto.Location;
            existingEvent.Description = eventDto.Description;
            existingEvent.NumberOfGuests = eventDto.NumberOfGuests;

            var suppliers = await _eventRepository.GetSuppliersByIdsAsync(eventDto.SupplierIds);
            existingEvent.Suppliers = suppliers;

            return await _eventRepository.UpdateEventAsync(existingEvent);
        }

        public async Task<List<EventResponseDto>> GetEventsByManagerIdAsync(int managerId)
        {
            var events = await _eventRepository.GetEventsByManagerIdAsync(managerId);

            return events.Select(e => new EventResponseDto
            {
                Id = e.Id,
                Name = e.Name,
                Date = e.Date,
                Location = e.Location,
                Description = e.Description,
                NumberOfGuests = e.NumberOfGuests,
                Manager = e.Manager != null ? new ManagerDto
                {
                    Id = e.Manager.Id,
                    FirstName = e.Manager.FirstName,
                    LastName = e.Manager.LastName
                } : null,
                Suppliers = e.Suppliers.Select(s => new SupplierDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Category = s.Category
                }).ToList()
            }).ToList();
        }

        public async Task<bool> DeleteEventAsync(int eventId, int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null || !user.IsEventManager)
            {
                throw new UnauthorizedAccessException("You are not authorized to delete this event.");
            }

            var eventEntity = await _eventRepository.GetEventByIdAsync(eventId);
            if (eventEntity == null || eventEntity.ManagerId != userId)
            {
                throw new UnauthorizedAccessException("You are not allowed to delete this event.");
            }

            return await _eventRepository.DeleteEventAsync(eventId);
        }
    }
}
