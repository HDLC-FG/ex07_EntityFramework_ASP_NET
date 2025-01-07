using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Models;

namespace ApplicationCore.Services
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IWarehouseRepository warehouseRepository;

        public WarehouseService(IWarehouseRepository warehouseRepository)
        {
            this.warehouseRepository = warehouseRepository;
        }

        public async Task<IList<Warehouse>> GetAll()
        {
            return await warehouseRepository.GetAll();
        }

        public async Task<Warehouse?> Get(int id)
        {
            return await warehouseRepository.GetById(id);
        }

        public async Task Add(Warehouse warehouse)
        {
            await warehouseRepository.Add(warehouse);
        }

        public async Task Update(Warehouse warehouse)
        {
            await warehouseRepository.Update(warehouse);
        }

        public async Task Delete(int id)
        {
            var warehouse = await warehouseRepository.GetById(id);
            if (warehouse != null)
            {
                await warehouseRepository.Delete(warehouse);
            }
            else
            {
                throw new Exception("Warehouse does not exist");
            }
        }
    }
}
