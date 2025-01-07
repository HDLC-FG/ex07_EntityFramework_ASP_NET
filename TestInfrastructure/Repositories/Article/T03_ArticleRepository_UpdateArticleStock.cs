using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace TestInfrastructure.Repositories.Article
{
    [TestClass]
    public sealed class T03_ArticleRepository_UpdateArticleStock
    {
        private DbContextOptions<ApplicationDbContext> _dbContextOptions;

        public T03_ArticleRepository_UpdateArticleStock()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        [TestMethod]
        public void UpdateArticleStock_Id_Quantity_ArticleNotExist_ThrowException()
        {
            using (var context = new ApplicationDbContext(_dbContextOptions, true))
            {
                var repository = new ArticleRepository(context);

                var result = Assert.ThrowsExceptionAsync<Exception>(() =>
                {
                    return repository.UpdateArticleStock(1, 15);
                }).Result;

                Assert.IsNotNull(result);
                Assert.AreEqual("Article does not exist", result.Message);
            }
        }

        [TestMethod]
        public void UpdateArticleStock_Id_Quantity_ReturnArticle()
        {
            using (var context = new ApplicationDbContext(_dbContextOptions, true))
            {
                context.Articles.Add(new ApplicationCore.Models.Article
                {
                    Id = 1,
                    Description = "article 1",
                    Name = "name 1",
                    Price = 100m,
                    StockQuantity = 10
                });
                context.SaveChangesAsync().Wait();
                var repository = new ArticleRepository(context);

                var result = repository.UpdateArticleStock(1, 15).Result;

                Assert.IsNotNull(result);
                Assert.AreEqual(1, result.Id);
                Assert.AreEqual("article 1", result.Description);
                Assert.AreEqual("name 1", result.Name);
                Assert.AreEqual(100, result.Price);
                Assert.AreEqual(25, result.StockQuantity);
            }
        }
    }
}
