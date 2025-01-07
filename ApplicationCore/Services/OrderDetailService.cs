using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Models;

namespace ApplicationCore.Services
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IOrderDetailRepository orderDetailRepository;

        public OrderDetailService(IOrderDetailRepository orderDetailRepository)
        {
            this.orderDetailRepository = orderDetailRepository;
        }

        public async Task<IList<OrderDetail>> GetAll()
        {
            return await orderDetailRepository.GetAll();
        }

        public async Task Add(OrderDetail orderDetail)
        {
            await orderDetailRepository.Add(orderDetail);
        }
    }
}
