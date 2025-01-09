using ApplicationCore.Models;

namespace ApplicationCore.Interfaces.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IList<Order>> GetAll(int page, int pageSize);
        int GetTotalOrders();
        Task<IList<Order>> GetAllOrdersByCustomer(int customerId);
        Task<IDictionary<int, double>> GetAverageArticlePerOrder();
        Task<double> GetAverageOrderValue();
    }
}
