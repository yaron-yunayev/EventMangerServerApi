using EventMangerServerApi.Core.Dtos;
using EventMangerServerApi.Core.Modles;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventMangerServerApi.Application.Interfaces
{
    public interface ISupplierService
    {
        Task<Supplier?> GetSupplierByEmailAsync(string email);
        Task AddSupplierAsync(CreateSupplierDto supplierDto);
        Task<List<Supplier>> GetAllSuppliersAsync();
        Task<Supplier?> GetSupplierByIdAsync(int id);

        Task<bool> UpdateSupplierAsync(int id, UpdateSupplierDto dto);
        Task<bool> DeleteSupplierAsync(int id);
    }
}
