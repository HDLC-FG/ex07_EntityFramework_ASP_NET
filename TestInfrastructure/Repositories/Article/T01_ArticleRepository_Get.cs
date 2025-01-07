using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace TestInfrastructure.Repositories.Article;

[TestClass]
public sealed class T01_ArticleRepository_Get
{
    private DbContextOptions<ApplicationDbContext> _dbContextOptions;

    public T01_ArticleRepository_Get()
    {
        // Générer un nom unique pour chaque test pour assurer l'isolement
        _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
    }

    [TestMethod]
    public void Get_Id_ReturnNull()
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

            var result = repository.GetById(2).Result;

            Assert.IsNull(result);
        }
    }

    [TestMethod]
    public void Get_Id_ReturnArticle()
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

            var result = repository.GetById(1).Result;

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("article 1", result.Description);
            Assert.AreEqual("name 1", result.Name);
            Assert.AreEqual(100, result.Price);
            Assert.AreEqual(10, result.StockQuantity);
        }
    }
}
