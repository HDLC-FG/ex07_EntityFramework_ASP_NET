using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace TestInfrastructure.Repositories.Article
{
    [TestClass]
    public sealed class T02_ArticleRepository_Add
    {
        private DbContextOptions<ApplicationDbContext> _dbContextOptions;

        public T02_ArticleRepository_Add()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        [TestMethod]
        public void Add_Article_ReturnOk()
        {
            using (var context = new ApplicationDbContext(_dbContextOptions, true))
            {
                var repository = new ArticleRepository(context);

                repository.Add(new ApplicationCore.Models.Article
                {
                    Id = 1,
                    Description = "article",
                    Name = "name",
                    Price = 100m,
                    StockQuantity = 10
                }).Wait();

                var result = context.Articles.First();
                Assert.IsNotNull(result);
                Assert.AreEqual(1, result.Id);
                Assert.AreEqual("article", result.Description);
                Assert.AreEqual("name", result.Name);
                Assert.AreEqual(100, result.Price);
                Assert.AreEqual(10, result.StockQuantity);
            }
        }
    }
}
