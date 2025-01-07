using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly ApplicationDbContext context;

        public OrderDetailRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IList<OrderDetail>> GetAll()
        {
            return await context.OrderDetails.ToListAsync();
        }

        public async Task<OrderDetail?> GetById(int id)
        {
            return await context.OrderDetails.FindAsync(id);
        }

        public async Task Add(OrderDetail entity)
        {
            await context.OrderDetails.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task Update(OrderDetail entity)
        {
            context.OrderDetails.Update(entity);
            await context.SaveChangesAsync();
        }

        public async Task Delete(OrderDetail entity)
        {
            context.OrderDetails.Remove(entity);
            await context.SaveChangesAsync();
        }
    }
}
