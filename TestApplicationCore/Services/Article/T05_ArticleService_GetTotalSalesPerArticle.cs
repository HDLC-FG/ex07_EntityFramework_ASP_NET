using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Models;
using Moq;

namespace TestApplicationCore.Services.ArticleService
{
    [TestClass]
    public sealed class T05_ArticleService_GetTotalSalesPerArticle
    {
        [TestMethod]
        public void GetTotalSalesPerArticle_Article_ReturnOk()
        {
            var mock = new Mock<IArticleRepository>(MockBehavior.Strict);
            var article1 = new Article { Id = 102 };
            var article2 = new Article { Id = 304 };

            mock.Setup(x => x.GetTotalSalesPerArticle()).ReturnsAsync(new Dictionary<Article, decimal>
            {
                { article1, 12 },
                { article2, 34 }
            });

            var service = new ApplicationCore.Services.ArticleService(mock.Object);

            var result = service.GetTotalSalesPerArticle().Result;

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(12, result[article1]);
            Assert.AreEqual(34, result[article2]);
        }
    }
}
