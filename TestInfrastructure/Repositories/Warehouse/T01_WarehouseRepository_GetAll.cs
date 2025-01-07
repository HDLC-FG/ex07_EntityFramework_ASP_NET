using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace TestInfrastructure.Repositories.Warehouse
{
    [TestClass]
    public sealed class T01_WarehouseRepository_GetAll
    {
        private DbContextOptions<ApplicationDbContext> _dbContextOptions;

        public T01_WarehouseRepository_GetAll()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        [TestMethod]
        public void GetAll_ReturnOk()
        {
            using (var context = new ApplicationDbContext(_dbContextOptions, true))
            {
                context.Warehouses.Add(new ApplicationCore.Models.Warehouse
                {
                    Id = 1,
                    Name = "Warehouse",
                    Address = "Warehouse address",
                    PostalCode = 1234,
                    CodeAccesMD5 = new List<string> { "code1", "code2" },
                    Orders = new List<ApplicationCore.Models.Order>
                    {
                        new ApplicationCore.Models.Order { Id = 2, OrderStatus = "status1" },
                        new ApplicationCore.Models.Order { Id = 3, OrderStatus = "status2" }
                    }
                });
                context.SaveChangesAsync().Wait();

                var repository = new WarehouseRepository(context);

                var result = repository.GetAll().Result;

                Assert.IsNotNull(result);
                Assert.AreEqual(1, result.Count);
                var warehouse = result[0];
                Assert.IsNotNull(warehouse);
                Assert.AreEqual(1, warehouse.Id);
                Assert.AreEqual("Warehouse", warehouse.Name);
                Assert.AreEqual("Warehouse address", warehouse.Address);
                Assert.AreEqual(1234, warehouse.PostalCode);
                Assert.IsNotNull(warehouse.CodeAccesMD5);
                Assert.AreEqual(2, warehouse.CodeAccesMD5.Count);
                Assert.AreEqual("code1", warehouse.CodeAccesMD5[0]);
                Assert.AreEqual("code2", warehouse.CodeAccesMD5[1]);
                Assert.IsNotNull(warehouse.Orders);
                Assert.AreEqual(2, warehouse.Orders.Count);
                Assert.AreEqual(2, warehouse.Orders[0].Id);
                Assert.AreEqual("status1", warehouse.Orders[0].OrderStatus);
                Assert.AreEqual(3, warehouse.Orders[1].Id);
                Assert.AreEqual("status2", warehouse.Orders[1].OrderStatus);
            }
        }
    }
}
