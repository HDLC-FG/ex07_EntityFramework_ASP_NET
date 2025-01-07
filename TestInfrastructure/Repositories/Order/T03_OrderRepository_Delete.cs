using ApplicationCore.Models;
using ApplicationCore.ValueObjects;
using Infrastructure.Repositories;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace TestInfrastructure.Repositories.Order
{
    [TestClass]
    public class T03_OrderRepository_Delete
    {
        private DbContextOptions<ApplicationDbContext> _dbContextOptions;
        
        public T03_OrderRepository_Delete()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        [TestMethod]
        public void Delete_Id_ReturnOk()
        {
            using (var context = new ApplicationDbContext(_dbContextOptions, true))
            {
                var order = new ApplicationCore.Models.Order
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
                };
                context.Orders.Add(order);
                context.SaveChangesAsync().Wait();

                var repository = new OrderRepository(context);

                repository.Delete(order).Wait();

                var result = context.Orders;
                Assert.IsNotNull(result);
                Assert.AreEqual(0, result.Count());
            }
        }
    }
}
