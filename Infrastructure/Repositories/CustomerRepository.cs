using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext context;

        public CustomerRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Task Add(Customer entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Customer entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<Customer>> GetAll()
        {
            return await context.Customers.Include(x => x.Orders).ToListAsync();
        }

        public async Task<IList<Customer>> GetAll(int page, int pageSize)
        {
            return await context.Customers
                .OrderBy(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(x => x.Orders)
                .ToListAsync();
        }

        public int GetTotal()
        {
            return context.Customers.Count();
        }

        public Task<Customer?> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task Update(Customer entity)
        {
            throw new NotImplementedException();
        }
    }
}
