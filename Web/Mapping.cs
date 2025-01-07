using ApplicationCore.Models;
using Web.ViewModels;

namespace Web
{
    public class Mapping
    {
        public static WarehouseViewModel ConvertToWarehouseVM(Warehouse warehouse)
        {
            var vm = new WarehouseViewModel();
            vm.Id = warehouse.Id;
            vm.Address = warehouse.Address;
            vm.Name = warehouse.Name;
            vm.PostalCode = warehouse.PostalCode;
            return vm;
        }

        public static Warehouse ToWarehouse(WarehouseViewModel wm)
        {
            var warehouse = new Warehouse();
            warehouse.Id = wm.Id;
            warehouse.Address = wm.Address;
            warehouse.Name = wm.Name;
            warehouse.PostalCode = wm.PostalCode;

            return warehouse;
        }
    }
}
