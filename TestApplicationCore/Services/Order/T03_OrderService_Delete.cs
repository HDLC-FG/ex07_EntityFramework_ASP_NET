using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Services;
using Moq;

namespace TestApplicationCore.Services.Order
{
    [TestClass]
    public sealed class T03_OrderService_Delete
    {
        [TestMethod]
        public void Delete_Id_ReturnOk()
        {
            var mock = new Mock<IOrderRepository>(MockBehavior.Strict);
            var order = new ApplicationCore.Models.Order { Id = 1 };
            mock.Setup(x => x.GetById(1)).ReturnsAsync(order);
            mock.Setup(x => x.Delete(order)).Returns(Task.CompletedTask);
            
            var service = new OrderService(mock.Object);

            service.Delete(1).Wait();
        }
    }
}
