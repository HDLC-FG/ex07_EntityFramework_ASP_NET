using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace TestInfrastructure.Repositories.OrderDetail
{
    [TestClass]
    public sealed class T01_OrderDetailRepository_Add
    {
        private DbContextOptions<ApplicationDbContext> _dbContextOptions;

        public T01_OrderDetailRepository_Add()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        [TestMethod]
        public void Add_OrderDetail_ReturnOk()
        {
            using (var context = new ApplicationDbContext(_dbContextOptions, true))
            {
                var repository = new OrderDetailRepository(context);

                _ = repository.Add(new ApplicationCore.Models.OrderDetail
                {
                    Id = 1,
                    Order = new ApplicationCore.Models.Order
                    {
                        Id = 2,
                        OrderStatus = "delivered"
                    },
                    Article = new ApplicationCore.Models.Article
                    {
                        Id = 3,
                        Description = "description",
                        Name = "name"
                    },
                    Quantity = 10,
                    UnitPrice = 100
                }).Result;

                var result = context.OrderDetails.First();
                Assert.IsNotNull(result);
                Assert.AreEqual(1, result.Id);
                Assert.AreEqual(2, result.OrderId);
                Assert.AreEqual(2, result.Order.Id);
                Assert.AreEqual("delivered", result.Order.OrderStatus);
                Assert.AreEqual(3, result.ArticleId);
                Assert.AreEqual(3, result.Article.Id);
                Assert.AreEqual("description", result.Article.Description);
                Assert.AreEqual("name", result.Article.Name);
                Assert.AreEqual(10, result.Quantity);
                Assert.AreEqual(100, result.UnitPrice);
            }
        }
    }
}
