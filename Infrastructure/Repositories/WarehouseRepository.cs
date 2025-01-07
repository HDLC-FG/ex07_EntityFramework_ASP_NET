using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class WarehouseRepository : IWarehouseRepository
    {
        private readonly ApplicationDbContext context;

        public WarehouseRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IList<Warehouse>> GetAll()
        {
            return await context.Warehouses.ToListAsync();
        }

        public async Task<Warehouse?> GetById(int id)
        {
            return await context.Warehouses.FindAsync(id);
        }

        public async Task Add(Warehouse entity)
        {
            await context.Warehouses.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task Update(Warehouse entity)
        {
            context.Warehouses.Update(entity);
            await context.SaveChangesAsync();
        }

        public async Task Delete(Warehouse entity)
        {
            context.Warehouses.Remove(entity);
            await context.SaveChangesAsync();
        }
    }
}
