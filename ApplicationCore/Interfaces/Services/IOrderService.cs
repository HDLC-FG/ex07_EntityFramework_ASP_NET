using ApplicationCore.Models;

namespace ApplicationCore.Interfaces.Services
{
    public interface IOrderService
    {
        Task<IList<Order>> GetAll();
        Task<IList<Order>> GetAll(int page, int pageSize);
        Task<Order?> Get(int id);
        int GetTotalOrders();
        Task Add(Order order);
        Task Update(Order order);
        Task Delete(int orderId);
        Task<IList<Order>> GetAllOrdersByCustomer(int customerId);
        Task<IDictionary<int, double>> GetAverageArticlePerOrder();
        Task<double> GetAverageOrderValue();
    }
}
