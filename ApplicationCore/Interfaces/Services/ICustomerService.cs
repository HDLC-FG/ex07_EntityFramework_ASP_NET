using ApplicationCore.Models;

namespace ApplicationCore.Interfaces.Services
{
    public interface ICustomerService
    {
        Task<IList<Customer>> GetAll();
    }
}
