using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Models;
using ApplicationCore.Services;
using Moq;

namespace TestApplicationCore.Services.Order
{
    [TestClass]
    public sealed class T01_Order_Get
    {
        [TestMethod]
        public void Get_Id_ReturnNull()
        {
            var mock = new Mock<IOrderRepository>(MockBehavior.Strict);
            mock.Setup(x => x.GetById(1)).ReturnsAsync((ApplicationCore.Models.Order)null);

            var service = new OrderService(mock.Object);

            var result = service.Get(1).Result;

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Get_Id_ReturnOk()
        {
            var mock = new Mock<IOrderRepository>(MockBehavior.Strict);
            mock.Setup(x => x.GetById(1)).ReturnsAsync(new ApplicationCore.Models.Order { Id = 123 });

            var service = new OrderService(mock.Object);

            var result = service.Get(1).Result;

            Assert.IsNotNull(result);
            Assert.AreEqual(123, result.Id);
        }
    }
}
