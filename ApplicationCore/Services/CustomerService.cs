using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Models;

namespace ApplicationCore.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }

        public async Task<IList<Customer>> GetAll()
        {
            return await customerRepository.GetAll();
        }

        public async Task<IList<Customer>> GetAll(int page, int pageSize)
        {
            return await customerRepository.GetAll(page, pageSize);
        }

        public int GetTotal()
        {
            return customerRepository.GetTotal();
        }
    }
}
