using ApplicationCore.Models;

namespace ApplicationCore.Interfaces.Services
{
    public interface IWarehouseService
    {
        Task<IList<Warehouse>> GetAll();
        Task<Warehouse?> Get(int id);
        Task Add(Warehouse warehouse);
        Task Update(Warehouse warehouse);
        Task Delete(int id);
    }
}
