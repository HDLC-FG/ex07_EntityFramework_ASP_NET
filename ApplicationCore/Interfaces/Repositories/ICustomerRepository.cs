using ApplicationCore.Models;

namespace ApplicationCore.Interfaces.Repositories
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<IList<Customer>> GetAll(int page, int pageSize);
        int GetTotal();
    }
}
