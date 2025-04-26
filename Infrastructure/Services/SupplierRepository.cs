using EventMangerServerApi.Core.Interfaces;
using EventMangerServerApi.Core.Modles;
using EventMangerServerApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EventMangerServerApi.Infrastructure.Services
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly EventProjectDbContext _context;

        public SupplierRepository(EventProjectDbContext context)
        {
            _context = context;
        }

        public async Task<Supplier> GetSupplierByEmailAsync(string email)
        {
            return await _context.Suppliers.FirstOrDefaultAsync(s => s.Email == email);
        }

        public async Task<Supplier?> GetSupplierByIdAsync(int id)
        {
            return await _context.Suppliers.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<Supplier>> GetSuppliersByIdsAsync(List<int> supplierIds)
        {
            return await _context.Suppliers
                .Where(s => supplierIds.Contains(s.Id))
                .ToListAsync();
        }

        public async Task AddSupplierAsync(Supplier supplier)
        {
            await _context.Suppliers.AddAsync(supplier);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSupplierAsync(Supplier supplier)
        {
            _context.Suppliers.Update(supplier);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSupplierAsync(Supplier supplier)
        {
            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Supplier>> GetAllSuppliersAsync()
        {
            return await _context.Suppliers.ToListAsync();
        }
    }
}
