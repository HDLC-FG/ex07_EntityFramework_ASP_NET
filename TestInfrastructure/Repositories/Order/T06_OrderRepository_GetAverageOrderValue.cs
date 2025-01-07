using ApplicationCore.Models;
using ApplicationCore.ValueObjects;
using Infrastructure.Repositories;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace TestInfrastructure.Repositories.Order
{
    [TestClass]
    public sealed class T06_OrderRepository_GetAverageOrderValue
    {
        private DbContextOptions<ApplicationDbContext> _dbContextOptions;

        public T06_OrderRepository_GetAverageOrderValue()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        [TestMethod]
        public void GetAverageOrderValue_ReturnOk()
        {
            using (var context = new ApplicationDbContext(_dbContextOptions, true))
            {
                var customer = new Customer
                {
                    Id = 2,
                    Email = string.Empty,
                    FirstName = "FirstName",
                    LastName = "LastName"
                };
                var orderDetails = new List<ApplicationCore.Models.OrderDetail>
                {
                    new ApplicationCore.Models.OrderDetail { Id = 3 }
                };
                var warehouse = new ApplicationCore.Models.Warehouse { Id = 4 };
                context.Orders.Add(new ApplicationCore.Models.Order
                {
                    Id = 1,
                    Customer = customer,
                    Address = new Address("Rue de la mairie", "Rennes", "France", "35000"),
                    OrderDate = new DateTime(2024, 1, 6),
                    TotalAmount = 100,
                    OrderStatus = "delivered",
                    OrderDetails = orderDetails,
                    Warehouse = warehouse
                });
                context.Orders.Add(new ApplicationCore.Models.Order
                {
                    Id = 2,
                    Customer = customer,
                    Address = new Address("Rue de la mairie", "Rennes", "France", "35000"),
                    OrderDate = new DateTime(2024, 1, 6),
                    TotalAmount = 200,
                    OrderStatus = "delivered",
                    OrderDetails = orderDetails,
                    Warehouse = warehouse
                });
                context.SaveChangesAsync().Wait();

                var repository = new OrderRepository(context);

                var result = repository.GetAverageOrderValue().Result;

                Assert.IsNotNull(result);
                Assert.AreEqual(150, result);
            }
        }
    }
}
