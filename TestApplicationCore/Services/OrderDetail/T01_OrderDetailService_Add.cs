using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Models;
using Moq;

namespace TestApplicationCore.Services.OrderDetailService
{
    [TestClass]
    public sealed class T01_OrderDetailService_Add
    {
        [TestMethod]
        public void Add_Article_ReturnOk()
        {
            var mock = new Mock<IOrderDetailRepository>(MockBehavior.Strict);

            var orderDetail = new OrderDetail();
            mock.Setup(x => x.Add(orderDetail)).ReturnsAsync(1);

            var service = new ApplicationCore.Services.OrderDetailService(mock.Object);

            var result = service.Add(orderDetail).Result;

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result);
        }
    }
}
