using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext context;

        public OrderRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IList<Order>> GetAll()
        {
            return await context.Orders.ToListAsync();
        }

        public async Task<Order?> GetById(int id)
        {
            return await context.Orders.FindAsync(id);
        }

        public async Task Add(Order entity)
        {
            await context.Orders.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task Update(Order entity)
        {
            context.Orders.Update(entity);
            await context.SaveChangesAsync();
        }

        public async Task Delete(Order entity)
        {
            context.Orders.Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task<IList<Order>> GetAllOrdersByCustomer(int customerId)
        {
            return await context.Orders.Where(x => x.CustomerId == customerId).ToListAsync();
        }

        public async Task<IDictionary<int, double>> GetAverageArticlePerOrder()
        {
            var tmp = context.Orders.GroupBy(x => x, y => y, (x, y) => new
            {
                Order = x.Id,
                AverageArticle = y.Average(a => a.OrderDetails.Count)
            });
            return await tmp.ToDictionaryAsync(x => x.Order, y => y.AverageArticle);
        }

        public async Task<double> GetAverageOrderValue()
        {
            return await context.Orders.AverageAsync(x => x.TotalAmount);
        }
    }
}
