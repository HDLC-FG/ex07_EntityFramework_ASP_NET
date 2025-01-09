using ApplicationCore.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Web.ViewModels;
using Web.ViewModels.Shared;

namespace Web.Controllers
{
    public class CustomerController : Controller
    {
        private const int pageSize = 20;
        private readonly ICustomerService customerService;

        public CustomerController(ICustomerService customerService)
        {
            this.customerService = customerService;
        }

        // GET: Customer
        public async Task<ActionResult> Index(int page = 1)
        {
            var totalOrders = customerService.GetTotal();
            var totalPages = Math.Ceiling(totalOrders / (double)pageSize);   //Round up to the calculation result (exemple : 25 customers, page max to draw 20 (25/20 = 1.25), result = 2 (pages needed))

            var orders = await customerService.GetAll(page, pageSize);
            var paginationViewModel = new PaginationViewModel<CustomerViewModel>
            {
                Items = orders.Select(x => x.ToViewModel()).ToList(),
                Infos = new PaginationInfosViewModels
                {
                    CurrentPage = page,
                    TotalPages = totalPages
                }                
            };

            return View(paginationViewModel);
        }
    }
}