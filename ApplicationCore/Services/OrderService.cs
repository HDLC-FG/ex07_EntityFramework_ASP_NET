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

        public async Task<IList<Order>> GetAll()
        {
            return await orderRepository.GetAll();
        }

        public async Task<IList<Order>> GetAll(int page, int pageSize)
        {
            return await orderRepository.GetAll(page, pageSize);
        }

        public async Task<Order?> Get(int id)
        {
            return await orderRepository.GetById(id);
        }

        public int GetTotalOrders()
        {
            return orderRepository.GetTotalOrders();
        }

        public async Task Add(Order order)
        {
            await orderRepository.Add(order);
        }

        public async Task Update(Order order)
        {
            await orderRepository.Update(order);
        }

        public async Task Delete(int id)
        {
            var order = await orderRepository.GetById(id);
            if(order != null)
            {
                await orderRepository.Delete(order);
            }
            else
            {
                throw new Exception("Order does not exist");
            }
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
