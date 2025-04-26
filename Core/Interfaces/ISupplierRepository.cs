using EventMangerServerApi.Core.Modles;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventMangerServerApi.Core.Interfaces
{
    public interface ISupplierRepository
    {
        Task<Supplier> GetSupplierByEmailAsync(string email);
        Task<Supplier?> GetSupplierByIdAsync(int id);
        Task AddSupplierAsync(Supplier supplier);
        Task UpdateSupplierAsync(Supplier supplier);
        Task DeleteSupplierAsync(Supplier supplier);
        Task<List<Supplier>> GetAllSuppliersAsync();
        Task<List<Supplier>> GetSuppliersByIdsAsync(List<int> supplierIds);
    }
}
