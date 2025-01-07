using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Models;

namespace ApplicationCore.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public async Task<Order?> Get(int id)
        {
            return await orderRepository.GetById(id);
        }

        public async Task Add(Order order)
        {
            await orderRepository.Add(order);
        }

        public async Task Delete(int id)
        {
            await orderRepository.Delete(id);
        }

        public async Task<IList<Order>> GetAllOrdersByCustomer(int customerId)
        {
            return await orderRepository.GetAllOrdersByCustomer(customerId);
        }

        public async Task<IDictionary<int, double>> GetAverageArticlePerOrder()
        {
            return await orderRepository.GetAverageArticlePerOrder();
        }

        public async Task<double> GetAverageOrderValue()
        {
            return await orderRepository.GetAverageOrderValue();
        }
    }
}
