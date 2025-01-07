using ApplicationCore.Models;
using ApplicationCore.ValueObjects;
using Infrastructure.Repositories;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace TestInfrastructure.Repositories.Order
{
    [TestClass]
    public sealed class T04_OrderRepository_GetAllOrdersByCustomer
    {
        private DbContextOptions<ApplicationDbContext> _dbContextOptions;

        public T04_OrderRepository_GetAllOrdersByCustomer()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }
        
        [TestMethod]
        public void GetAllOrdersByCustomer_Id_ReturnOk()
        {
            using (var context = new ApplicationDbContext(_dbContextOptions, true))
            {
                context.Orders.Add(new ApplicationCore.Models.Order
                {
                    Id = 1,
                    Customer = new Customer
                    {
                        Id = 2,
                        Email = string.Empty,
                        FirstName = "FirstName",
                        LastName = "LastName"
                    },
                    Address = new Address("Rue de la mairie", "Rennes", "France", "35000"),
                    OrderDate = new DateTime(2024, 1, 6),
                    TotalAmount = 100,
                    OrderStatus = "delivered",
                    OrderDetails = new List<ApplicationCore.Models.OrderDetail>
                    {
                        new ApplicationCore.Models.OrderDetail { Id = 3 }
                    },
                    Warehouse = new ApplicationCore.Models.Warehouse { Id = 4 }
                });
                context.SaveChangesAsync().Wait();

                var repository = new OrderRepository(context);

                var result = repository.GetAllOrdersByCustomer(2).Result;

                Assert.IsNotNull(result);
                Assert.AreEqual(1, result.Count);
                var order = result[0];
                Assert.AreEqual(1, order.Id);
                Assert.AreEqual(2, order.Customer.Id);
                Assert.AreEqual(string.Empty, order.Customer.Email);
                Assert.AreEqual("FirstName", order.Customer.FirstName);
                Assert.AreEqual("LastName", order.Customer.LastName);
                Assert.AreEqual("Rue de la mairie", order.Address.Street);
                Assert.AreEqual("Rennes", order.Address.City);
                Assert.AreEqual("France", order.Address.Country);
                Assert.AreEqual("35000", order.Address.ZipCode);
                Assert.AreEqual(new DateTime(2024, 1, 6), order.OrderDate);
                Assert.AreEqual(100, order.TotalAmount);
                Assert.AreEqual("delivered", order.OrderStatus);
                Assert.AreEqual(1, order.OrderDetails.Count);
                Assert.AreEqual(3, order.OrderDetails[0].Id);
                Assert.AreEqual(4, order.WarehouseId);
                Assert.AreEqual(4, order.Warehouse.Id);
            }
        }
    }
}
