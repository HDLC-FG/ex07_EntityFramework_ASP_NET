using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Models;
using Moq;

namespace TestApplicationCore.Services.ArticleService
{
    [TestClass]
    public sealed class T01_ArticleService_Get
    {
        [TestMethod]
        public void Get_Id_ReturnNull()
        {
            var mock = new Mock<IArticleRepository>(MockBehavior.Strict);
            mock.Setup(x => x.GetById(1)).ReturnsAsync((Article)null);

            var service = new ApplicationCore.Services.ArticleService(mock.Object);

            var result = service.Get(1).Result;

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Get_Id_ReturnOk()
        {
            var mock = new Mock<IArticleRepository>(MockBehavior.Strict);
            mock.Setup(x => x.GetById(1)).ReturnsAsync(new Article { Id = 123 });

            var service = new ApplicationCore.Services.ArticleService(mock.Object);

            var result = service.Get(1).Result;

            Assert.IsNotNull(result);
            Assert.AreEqual(123, result.Id);
        }
    }
}
