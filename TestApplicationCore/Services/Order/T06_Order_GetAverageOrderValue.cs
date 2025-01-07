using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Services;
using Moq;

namespace TestApplicationCore.Services.Order
{
    [TestClass]
    public sealed class T06_Order_GetAverageOrderValue
    {
        [TestMethod]
        public void GetAverageOrderValue_ReturnOk()
        {
            var mock = new Mock<IOrderRepository>(MockBehavior.Strict);
            mock.Setup(x => x.GetAverageOrderValue()).ReturnsAsync(10);

            var service = new OrderService(mock.Object);

            var result = service.GetAverageOrderValue().Result;

            Assert.IsNotNull(result);
            Assert.AreEqual(10, result);
        }
    }
}
