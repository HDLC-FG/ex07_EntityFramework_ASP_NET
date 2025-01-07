using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Models;
using Moq;

namespace TestApplicationCore.Services.ArticleService
{
    [TestClass]
    public sealed class T03_ArticleService_UpdateArticleStock
    {
        [TestMethod]
        public void UpdateArticleStock_Article_ReturnOk()
        {
            var mock = new Mock<IArticleRepository>(MockBehavior.Strict);

            var article = new Article { Id = 123 };
            mock.Setup(x => x.UpdateArticleStock(1, 456)).ReturnsAsync(article);

            var service = new ApplicationCore.Services.ArticleService(mock.Object);

            var result = service.UpdateArticleStock(1, 456).Result;

            Assert.IsNotNull(result);
            Assert.AreEqual(123, result.Id);
        }
    }
}
