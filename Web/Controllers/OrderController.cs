using ApplicationCore.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Web.ViewModels;

namespace Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService orderService;
        private readonly IOrderDetailService orderDetailService;
        private readonly IArticleService articleService;
        private static List<ArticleSelectedViewModel> articleSelectedViewModels = new List<ArticleSelectedViewModel>();

        public OrderController(IOrderService orderService,
            IOrderDetailService orderDetailService,
            IArticleService articleService)
        {
            var context = new Infrastructure.ApplicationDbContext();
            this.orderService = orderService;
            this.orderDetailService = orderDetailService;
            this.articleService = articleService;
        }

        // GET: OrderController
        public async Task<ActionResult> Index()
        {
            var orders = await orderService.GetAll();
            return View(orders.Select(x => x.ToViewModel()));
        }

        // GET: OrderController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var order = await orderService.Get(id);
            return View(order?.ToViewModel());
        }

        // GET: OrderController/Create
        public async Task<ActionResult> Create()
        {
            var orderDetails = await orderDetailService.GetAll();
            ViewData["OrderDetails"] = orderDetails.Select(x => x.ToViewModel()).ToList();
            var articles = await articleService.GetAll();
            ViewData["Articles"] = articles.Select(x => x.ToViewModel()).ToList();
            ViewData["ArticlesSelected"] = articleSelectedViewModels;
            return View(new OrderViewModel());
        }

        // POST: OrderController/Create
        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(OrderViewModel viewModel)
        {
            if (articleSelectedViewModels.Count > 0)
            {
                ModelState["ListArticlesSelected"].ValidationState = ModelValidationState.Valid;
            }

            var articles = await articleService.GetAll();

            if (!ModelState.IsValid)
            {
                var orderDetails = await orderDetailService.GetAll();
                ViewData["OrderDetails"] = orderDetails.Select(x => x.ToViewModel()).ToList();
                ViewData["Articles"] = articles.Select(x => x.ToViewModel()).ToList();
                ViewData["ArticlesSelected"] = articleSelectedViewModels;
                return View(viewModel);
            }

            foreach (var articleSelected in articleSelectedViewModels)
            {
                var article = articles.FirstOrDefault(x => x.Id == articleSelected.Id)!;
                if (article.StockQuantity >= articleSelected.Qte)
                {
                    viewModel.OrderDetails.Add(new OrderDetailViewModel
                    {
                        ArticleId = articleSelected.Id,
                        Quantity = articleSelected.Qte,
                        UnitPrice = article.Price
                    });
                }
            }

            try
            {
                await orderService.Add(viewModel.ToModel());
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateArticle(ArticleSelectedViewModel articleSelected)
        {
            var articles = await articleService.GetAll();
            var article = articles.FirstOrDefault(x => x.Id == articleSelected.Id);
            var success = false;
            if (article?.StockQuantity >= articleSelected.Qte)
            {
                articleSelected.Price = article.Price;
                articleSelectedViewModels.Add(articleSelected);
                success = true;

                await articleService.UpdateArticleStock(articleSelected.Id, -articleSelected.Qte);
            }

            ViewData["ArticlesSelected"] = articleSelectedViewModels;
            return Json(new
            {
                Success = success,
                TotalAmount = articleSelectedViewModels.Sum(x => x.Price * x.Qte),
                PartialView = RenderViewToString("_ListeArticles.cshtml", articleSelectedViewModels)
            });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteArticle(int Id, int Qte)
        {
            await articleService.UpdateArticleStock(Id, Qte);
            var articleToRemove = articleSelectedViewModels.Find(x => x.Id == Id);
            articleSelectedViewModels.Remove(articleToRemove);
            return Json(new
            {
                TotalAmount = articleSelectedViewModels.Sum(x => x.Price * x.Qte),
                PartialView = RenderViewToString("_ListeArticles.cshtml", articleSelectedViewModels)
            });
        }

        // GET: OrderController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var order = await orderService.Get(id);
            return View(order?.ToViewModel());
        }

        // POST: OrderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, OrderViewModel viewModel)
        {
            if (!ModelState.IsValid) return View();

            try
            {
                await orderService.Update(viewModel.ToModel());
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: OrderController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var order = await orderService.Get(id);
            return View(order?.ToViewModel());
        }

        // POST: OrderController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, OrderViewModel viewModel)
        {
            if (!ModelState.IsValid) return View();

            try
            {
                await orderService.Delete(viewModel.Id!.Value);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private string RenderViewToString(string viewName, object model)
        {
            var controllerContext = ControllerContext;
            var viewEngine = controllerContext.HttpContext.RequestServices.GetRequiredService<IRazorViewEngine>();
            var tempDataProvider = controllerContext.HttpContext.RequestServices.GetRequiredService<ITempDataProvider>();

            // Obtient l'action et la vue à partir du moteur de vues Razor
            var viewResult = viewEngine.GetView("Views/Shared/", viewName, false);
            if (!viewResult.Success)
            {
                throw new InvalidOperationException($"View {viewName} not found.");
            }

            // Crée un StringWriter pour capter le contenu HTML rendu
            var stringWriter = new StringWriter();

            var viewContext = new ViewContext(
                controllerContext,
                viewResult.View,
                new ViewDataDictionary(new EmptyModelMetadataProvider(), controllerContext.ModelState) { Model = model },
                new TempDataDictionary(controllerContext.HttpContext, tempDataProvider),
                stringWriter,
                new HtmlHelperOptions()
            );

            // Rendre la vue en HTML dans le StringWriter
            viewResult.View.RenderAsync(viewContext).GetAwaiter().GetResult();

            // Retourner le contenu HTML généré sous forme de chaîne
            return stringWriter.ToString();
        }
    }
}
