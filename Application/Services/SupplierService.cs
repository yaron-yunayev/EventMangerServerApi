using EventMangerServerApi.Application.Interfaces;
using EventMangerServerApi.Core.Dtos;
using EventMangerServerApi.Core.Interfaces;
using EventMangerServerApi.Core.Modles;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventMangerServerApi.Application.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;

        public SupplierService(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public async Task<Supplier?> GetSupplierByEmailAsync(string email)
        {
            return await _supplierRepository.GetSupplierByEmailAsync(email);
        }
        public async Task<Supplier?> GetSupplierByIdAsync(int id)
        {
            return await _supplierRepository.GetSupplierByIdAsync(id);
        }


        public async Task AddSupplierAsync(CreateSupplierDto supplierDto)
        {
            var existingSupplier = await _supplierRepository.GetSupplierByEmailAsync(supplierDto.Email);
            if (existingSupplier != null)
            {
                throw new Exception("Supplier already exists with this email.");
            }

            var supplier = new Supplier
            {
                Name = supplierDto.Name,
                PhoneNumber = supplierDto.PhoneNumber,
                Email = supplierDto.Email,
                Category = supplierDto.Category.ToString(), 
                Description = supplierDto.Description,
                Address = supplierDto.Address
            };

            await _supplierRepository.AddSupplierAsync(supplier);
        }

        public async Task<List<Supplier>> GetAllSuppliersAsync()
        {
            return await _supplierRepository.GetAllSuppliersAsync();
        }

        public async Task<bool> UpdateSupplierAsync(int id, UpdateSupplierDto dto)
        {
            var existing = await _supplierRepository.GetSupplierByIdAsync(id);
            if (existing == null)
            {
                return false;
            }

            existing.Name = dto.Name;
            existing.PhoneNumber = dto.PhoneNumber;
            existing.Email = dto.Email;
            existing.Category = dto.Category.ToString();
            existing.Description = dto.Description;
            existing.Address = dto.Address;

            await _supplierRepository.UpdateSupplierAsync(existing);
            return true;
        }

        public async Task<bool> DeleteSupplierAsync(int id)
        {
            var existing = await _supplierRepository.GetSupplierByIdAsync(id);
            if (existing == null)
            {
                return false;
            }

            await _supplierRepository.DeleteSupplierAsync(existing); 
            return true;
        }
    }
}
