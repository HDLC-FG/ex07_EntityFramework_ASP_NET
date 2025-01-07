using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Models;

namespace Infrastructure.Repositories
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly ApplicationDbContext context;

        public OrderDetailRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<int> Add(OrderDetail orderDetail)
        {
            await context.OrderDetails.AddAsync(orderDetail);
            return await context.SaveChangesAsync();
        }
    }
}
