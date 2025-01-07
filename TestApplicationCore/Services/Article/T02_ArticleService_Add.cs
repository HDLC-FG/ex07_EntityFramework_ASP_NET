using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Models;
using Moq;

namespace TestApplicationCore.Services.ArticleService
{
    [TestClass]
    public sealed class T02_ArticleService_Add
    {
        [TestMethod]
        public void Add_Article_ReturnOk()
        {
            var mock = new Mock<IArticleRepository>(MockBehavior.Strict);

            var article = new Article();
            mock.Setup(x => x.Add(article)).Returns(Task.CompletedTask);

            var service = new ApplicationCore.Services.ArticleService(mock.Object);

            service.Add(article).Wait();
        }
    }
}
