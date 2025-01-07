using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Services;
using Moq;

namespace TestApplicationCore.Services.Order
{
    [TestClass]
    public sealed class T04_Order_GetAllOrdersByCustomer
    {
        [TestMethod]
        public void GetAllOrdersByCustomer_CustomerId_ReturnOk()
        {
            var mock = new Mock<IOrderRepository>(MockBehavior.Strict);
            mock.Setup(x => x.GetAllOrdersByCustomer(1)).ReturnsAsync(new List<ApplicationCore.Models.Order>
            {
                new ApplicationCore.Models.Order { Id = 12 },
                new ApplicationCore.Models.Order { Id = 34 }
            });

            var service = new OrderService(mock.Object);

            var result = service.GetAllOrdersByCustomer(1).Result;

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(12, result[0].Id);
            Assert.AreEqual(34, result[1].Id);
        }
    }
}
