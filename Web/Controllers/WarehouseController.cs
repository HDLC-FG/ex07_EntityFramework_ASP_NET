using ApplicationCore.Interfaces.Services;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class WarehouseController : Controller
    {
        private readonly IWarehouseService warehouseService;

        public WarehouseController(IWarehouseService warehouseService)
        {
            this.warehouseService = warehouseService;
        }

        // GET: WarehouseController
        public async Task<ActionResult> Index()
        {
            var warehouses = await warehouseService.GetAll();
            return View(warehouses.Select(x => Mapping.ConvertToWarehouseVM(x)));
        }

        // GET: WarehouseController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var warehouse = await warehouseService.Get(id);
            return View(Mapping.ConvertToWarehouseVM(warehouse));
        }

        // GET: WarehouseController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WarehouseController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection)
        {
            try
            {
                //int newId = WarehouseController.Warehouses.Select(w => w.Id).Aggregate((previusMax, current) => { return Math.Max(previusMax, current); }) + 1;
                Warehouse warehouse = new Warehouse();
                ApplyFormCollectionToWarehouse(collection, warehouse);
                await warehouseService.Add(warehouse);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: WarehouseController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            // Search throught warehouses list the target warehouse by id
            var warehouse = await warehouseService.Get(id);
            return View(Mapping.ConvertToWarehouseVM(warehouse));
        }

        // POST: WarehouseController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, IFormCollection collection)
        {
            try
            {
                // Search throught warehouses list the target warehouse by id
                var warehouse = await warehouseService.Get(id);
                if (warehouse is null)
                {
                    throw new Exception();
                }
                ApplyFormCollectionToWarehouse(collection, warehouse);
                await warehouseService.Update(warehouse);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private void ApplyFormCollectionToWarehouse(IFormCollection collection, Warehouse foundWarehouse)
        {
            foreach (var field in collection)
            {
                if (field.Key == nameof(foundWarehouse.Id))
                {
                    foundWarehouse.Id = int.Parse(field.Value);
                }
                else if (field.Key == nameof(foundWarehouse.Name))
                {
                    foundWarehouse.Name = field.Value;
                }
                else if (field.Key == nameof(foundWarehouse.Address))
                {
                    foundWarehouse.Address = field.Value;
                }
                else if (field.Key == nameof(foundWarehouse.PostalCode))
                {
                    foundWarehouse.PostalCode = int.Parse(field.Value);
                }
            }
        }

        // GET: WarehouseController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            // Search throught warehouses list the target warehouse by id
            var warehouse = await warehouseService.Get(id);
            return View(Mapping.ConvertToWarehouseVM(warehouse));
        }

        // POST: WarehouseController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                await warehouseService.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
