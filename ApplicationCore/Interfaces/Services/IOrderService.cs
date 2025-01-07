using ApplicationCore.Models;

namespace ApplicationCore.Interfaces.Services
{
    public interface IOrderService
    {
        // Method to get an order
        Task<Order?> Get(int id);

        // Method to add new Order
        Task Add(Order order);

        // Method to delete an order
        Task Delete(int orderId);

        // Method to fetch all orders made by a specific customer
        Task<IList<Order>> GetAllOrdersByCustomer(int customerId);

        // Method to get average order value
        Task<IDictionary<int, double>> GetAverageArticlePerOrder();

        // Method to get average number article by order
        Task<double> GetAverageOrderValue();
    }
}
