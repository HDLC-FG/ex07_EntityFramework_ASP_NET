using ApplicationCore.Models;

namespace ApplicationCore.Interfaces.Repositories
{
    public interface IOrderDetailRepository
    {
        Task<int> Add(OrderDetail orderDetail);
    }
}
