using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Services;
using Moq;

namespace TestApplicationCore.Services.Order
{
    [TestClass]
    public sealed class T02_OrderService_Add
    {
        [TestMethod]
        public void Add_Article_ReturnOk()
        {
            var mock = new Mock<IOrderRepository>(MockBehavior.Strict);

            var order = new ApplicationCore.Models.Order();
            mock.Setup(x => x.Add(order)).Returns(Task.CompletedTask);

            var service = new OrderService(mock.Object);

            service.Add(order).Wait();
        }
    }
}
