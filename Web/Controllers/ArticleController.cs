using ApplicationCore.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Web.ViewModels;

namespace Web.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleService articleService;

        public ArticleController(IArticleService articleService)
        {
            this.articleService = articleService;
        }

        // GET: Article
        public async Task<IActionResult> Index()
        {
            var customers = await articleService.GetAll();
            return View(customers.OrderBy(x => x.Name).Select(x => x.ToViewModel()));
        }

        // GET: Article/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var article = await articleService.Get(id);
            return View(article?.ToViewModel());
        }

        // POST: Article/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ArticleUpdateStockQuantityViewModel viewModel)
        {
            if (!ModelState.IsValid) return View();

            try
            {
                await articleService.UpdateArticleStock(viewModel.Id, viewModel.StockQuantity);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
