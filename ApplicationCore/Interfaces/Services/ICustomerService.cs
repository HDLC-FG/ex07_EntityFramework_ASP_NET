using ApplicationCore.Models;

namespace ApplicationCore.Interfaces.Services
{
    public interface ICustomerService
    {
        Task<IList<Customer>> GetAll();
        Task<IList<Customer>> GetAll(int page, int pageSize);
        int GetTotal();
    }
}
