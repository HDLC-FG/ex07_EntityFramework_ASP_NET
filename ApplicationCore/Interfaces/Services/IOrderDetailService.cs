using ApplicationCore.Models;

namespace ApplicationCore.Interfaces.Services
{
    public interface IOrderDetailService
    {
        Task<IList<OrderDetail>> GetAll();
        Task Add(OrderDetail orderDetail);
    }
}
