using ApplicationCore.Models;
using ApplicationCore.ValueObjects;
using Infrastructure.Repositories;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace TestInfrastructure.Repositories.Order
{
    [TestClass]
    public sealed class T05_OrderRepository_GetAverageArticlePerOrder
    {
        private DbContextOptions<ApplicationDbContext> _dbContextOptions;

        public T05_OrderRepository_GetAverageArticlePerOrder()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        [TestMethod]
        public void GetAverageArticlePerOrder_ReturnOk()
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
                        new ApplicationCore.Models.OrderDetail { Id = 31, Article = new ApplicationCore.Models.Article { Id = 1, Description = "", Name = "" }  },
                        new ApplicationCore.Models.OrderDetail { Id = 32, Article = new ApplicationCore.Models.Article { Id = 2, Description = "", Name = "" } },
                        new ApplicationCore.Models.OrderDetail { Id = 33, Article = new ApplicationCore.Models.Article { Id = 3, Description = "", Name = "" } }
                    },
                    Warehouse = new ApplicationCore.Models.Warehouse { Id = 4 }
                });
                context.SaveChangesAsync().Wait();

                var repository = new OrderRepository(context);

                var result = repository.GetAverageArticlePerOrder().Result;

                Assert.IsNotNull(result);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(3, result[1]);
            }
        }
    }
}
