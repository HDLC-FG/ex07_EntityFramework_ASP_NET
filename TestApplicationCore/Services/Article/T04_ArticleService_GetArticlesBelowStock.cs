using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Models;
using Moq;

namespace TestApplicationCore.Services.ArticleService
{
    [TestClass]
    public sealed class T04_ArticleService_GetArticlesBelowStock
    {
        [TestMethod]
        public void GetArticlesBelowStock_Article_ReturnOk()
        {
            var mock = new Mock<IArticleRepository>(MockBehavior.Strict);

            var article = new List<Article>
            {
                new Article{ Id = 12 },
                new Article{ Id = 34 }
            };
            mock.Setup(x => x.GetArticlesBelowStock(456)).ReturnsAsync(article);

            var service = new ApplicationCore.Services.ArticleService(mock.Object);

            var result = service.GetArticlesBelowStock(456).Result;

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(12, result[0].Id);
            Assert.AreEqual(34, result[1].Id);
        }
    }
}
