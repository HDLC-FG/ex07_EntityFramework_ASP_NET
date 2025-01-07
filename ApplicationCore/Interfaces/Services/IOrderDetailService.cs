using ApplicationCore.Models;

namespace ApplicationCore.Interfaces.Services
{
    public interface IOrderDetailService
    {
        Task<int> Add(OrderDetail orderDetail);
    }
}
