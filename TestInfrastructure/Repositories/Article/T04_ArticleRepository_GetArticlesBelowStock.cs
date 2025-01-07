using Infrastructure.Repositories;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace TestInfrastructure.Repositories.Article
{
    [TestClass]
    public sealed class T04_ArticleRepository_GetArticlesBelowStock
    {
        private DbContextOptions<ApplicationDbContext> _dbContextOptions;

        public T04_ArticleRepository_GetArticlesBelowStock()
        {
            // Générer un nom unique pour chaque test pour assurer l'isolement
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        [TestMethod]
        public void GetArticlesBelowStock_notEnoughStock_ReturnNull()
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

                var result = repository.GetArticlesBelowStock(7).Result;

                Assert.IsNotNull(result);
                Assert.AreEqual(0, result.Count);
            }
        }

        [TestMethod]
        public void GetArticlesBelowStock_averageStock_ReturnOneArticle()
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
                context.Articles.Add(new ApplicationCore.Models.Article
                {
                    Id = 2,
                    Description = "article 2",
                    Name = "name 2",
                    Price = 200m,
                    StockQuantity = 20
                });
                context.SaveChangesAsync().Wait();
                var repository = new ArticleRepository(context);

                var result = repository.GetArticlesBelowStock(15).Result;

                Assert.IsNotNull(result);
                Assert.AreEqual(1, result.Count);
                var article = result[0];
                Assert.IsNotNull(article);
                Assert.AreEqual(1, article.Id);
                Assert.AreEqual("article 1", article.Description);
                Assert.AreEqual("name 1", article.Name);
                Assert.AreEqual(100, article.Price);
                Assert.AreEqual(10, article.StockQuantity);
            }
        }

        [TestMethod]
        public void GetArticlesBelowStock_maxStock_ReturnAllArticles()
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
                context.Articles.Add(new ApplicationCore.Models.Article
                {
                    Id = 2,
                    Description = "article 2",
                    Name = "name 2",
                    Price = 200m,
                    StockQuantity = 20
                });
                context.SaveChangesAsync().Wait();
                var repository = new ArticleRepository(context);

                var result = repository.GetArticlesBelowStock(20).Result;

                Assert.IsNotNull(result);
                Assert.AreEqual(2, result.Count);
                var article1 = result[0];
                Assert.IsNotNull(article1);
                Assert.AreEqual(1, article1.Id);
                Assert.AreEqual("article 1", article1.Description);
                Assert.AreEqual("name 1", article1.Name);
                Assert.AreEqual(100, article1.Price);
                Assert.AreEqual(10, article1.StockQuantity);
                var article2 = result[1];
                Assert.IsNotNull(article2);
                Assert.AreEqual(2, article2.Id);
                Assert.AreEqual("article 2", article2.Description);
                Assert.AreEqual("name 2", article2.Name);
                Assert.AreEqual(200, article2.Price);
                Assert.AreEqual(20, article2.StockQuantity);
            }
        }
    }
}
