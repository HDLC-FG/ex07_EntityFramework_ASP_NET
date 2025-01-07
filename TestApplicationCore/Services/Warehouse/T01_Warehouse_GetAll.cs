using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Models;
using Moq;

namespace TestApplicationCore.Services.Warehouse
{
    [TestClass]
    public sealed class T01_Warehouse_GetAll
    {
        [TestMethod]
        public void GetAll_ReturnNull()
        {
            var mock = new Mock<IWarehouseRepository>(MockBehavior.Strict);
            mock.Setup(x => x.GetAll()).ReturnsAsync((List<ApplicationCore.Models.Warehouse>)null);

            var service = new ApplicationCore.Services.WarehouseService(mock.Object);

            var result = service.GetAll().Result;

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetAll_ReturnOk()
        {
            var mock = new Mock<IWarehouseRepository>(MockBehavior.Strict);
            mock.Setup(x => x.GetAll()).ReturnsAsync(new List<ApplicationCore.Models.Warehouse>
            {
                new ApplicationCore.Models.Warehouse { Id = 11 },
                new ApplicationCore.Models.Warehouse { Id = 22 }
            });

            var service = new ApplicationCore.Services.WarehouseService(mock.Object);

            var result = service.GetAll().Result;

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(11, result[0].Id);
            Assert.AreEqual(22, result[1].Id);
        }
    }
}
