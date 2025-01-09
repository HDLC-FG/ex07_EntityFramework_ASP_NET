using ApplicationCore.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService customerService;

        public CustomerController(ICustomerService customerService)
        {
            this.customerService = customerService;
        }

        // GET: Customer
        public async Task<ActionResult> Index()
        {
            var customers = await customerService.GetAll();
            return View(customers.OrderByDescending(x => x.Orders.Sum(y => y.TotalAmount)).Select(x => x.ToViewModel()));
        }
    }
}