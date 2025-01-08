using ApplicationCore.Models;
using ApplicationCore.ValueObjects;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace TestInfrastructure.Repositories.Order
{
    [TestClass]
    public sealed class T02_OrderRepository_Add
    {
        private DbContextOptions<ApplicationDbContext> _dbContextOptions;

        public T02_OrderRepository_Add()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        [TestMethod]
        public void Add_Order_ReturnOk()
        {
            using (var context = new ApplicationDbContext(_dbContextOptions, true))
            {
                var repository = new OrderRepository(context);

                repository.Add(new ApplicationCore.Models.Order
                {
                    Id = 1,
                    Customer = new Customer
                    {
                        Id = 2,
                        Email = string.Empty,
                        FirstName = "FirstName",
                        LastName = "LastName"
                    },
                    Address = new Address("Rue de la mairie", "Rennes"),
                    OrderDate = new DateTime(2024, 1, 6),
                    TotalAmount = 100,
                    OrderStatus = "delivered",
                    OrderDetails = new List<ApplicationCore.Models.OrderDetail>
                    {
                        new ApplicationCore.Models.OrderDetail { Id = 3 }
                    },
                    Warehouse = new ApplicationCore.Models.Warehouse { Id = 4 }
                }).Wait();

                var result = context.Orders.First();
                Assert.IsNotNull(result);
                Assert.AreEqual(1, result.Id);
                Assert.AreEqual(2, result.Customer.Id);
                Assert.AreEqual(string.Empty, result.Customer.Email);
                Assert.AreEqual("FirstName", result.Customer.FirstName);
                Assert.AreEqual("LastName", result.Customer.LastName);
                Assert.AreEqual("Rue de la mairie", result.Address.Street);
                Assert.AreEqual("Rennes", result.Address.City);
                Assert.AreEqual(new DateTime(2024, 1, 6), result.OrderDate);
                Assert.AreEqual(100, result.TotalAmount);
                Assert.AreEqual("delivered", result.OrderStatus);
                Assert.AreEqual(1, result.OrderDetails.Count);
                Assert.AreEqual(3, result.OrderDetails[0].Id);
                Assert.AreEqual(4, result.WarehouseId);
                Assert.AreEqual(4, result.Warehouse.Id);
            }
        }
    }
}
