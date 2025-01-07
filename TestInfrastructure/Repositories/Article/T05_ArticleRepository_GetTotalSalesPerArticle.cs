using Infrastructure.Repositories;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace TestInfrastructure.Repositories.Article
{
    [TestClass]
    public sealed class T05_ArticleRepository_GetTotalSalesPerArticle
    {
        private DbContextOptions<ApplicationDbContext> _dbContextOptions;

        public T05_ArticleRepository_GetTotalSalesPerArticle()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        [TestMethod]
        public void GetTotalSalesPerArticle_ReturnTotal()
        {
            using (var context = new ApplicationDbContext(_dbContextOptions, true))
            {
                var article1 = new ApplicationCore.Models.Article
                {
                    Id = 1,
                    Description = "article 1",
                    Name = "name 1",
                    Price = 100m,
                    StockQuantity = 10
                };
                var article2 = new ApplicationCore.Models.Article
                {
                    Id = 2,
                    Description = "article 2",
                    Name = "name 2",
                    Price = 200m,
                    StockQuantity = 20
                };
                context.Articles.Add(article1);
                context.Articles.Add(article2);

                context.OrderDetails.Add(new ApplicationCore.Models.OrderDetail
                {
                    Article = article1,
                    Quantity = 1,
                    UnitPrice = 10,
                });
                context.OrderDetails.Add(new ApplicationCore.Models.OrderDetail
                {
                    Article = article2,
                    Quantity = 2,
                    UnitPrice = 20
                });

                context.SaveChangesAsync().Wait();
                var repository = new ArticleRepository(context);

                var result = repository.GetTotalSalesPerArticle().Result;

                Assert.IsNotNull(result);
                Assert.AreEqual(2, result.Count);
                Assert.AreEqual(10, result[article1]);
                Assert.AreEqual(40, result[article2]);
            }
        }
    }
}
