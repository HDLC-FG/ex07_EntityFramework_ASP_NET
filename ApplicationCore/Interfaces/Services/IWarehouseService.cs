using ApplicationCore.Models;

namespace ApplicationCore.Interfaces.Services
{
    public interface IWarehouseService
    {
        // Method to fetch all warehouses
        Task<IList<Warehouse>> GetAll();
    }
}
