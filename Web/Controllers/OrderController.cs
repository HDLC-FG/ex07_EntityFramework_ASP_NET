using System.Linq;
using ApplicationCore.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Web.ViewModels;
using Web.ViewModels.Shared;

namespace Web.Controllers
{
    public class OrderController : Controller
    {
        private const int pageSize = 20;
        private readonly IOrderService orderService;
        private readonly IOrderDetailService orderDetailService;
        private readonly IArticleService articleService;
        private static List<ArticleSelectedViewModel> articlesSelectedViewModel = new List<ArticleSelectedViewModel>();

        public OrderController(IOrderService orderService,
            IOrderDetailService orderDetailService,
            IArticleService articleService)
        {
            var context = new Infrastructure.ApplicationDbContext();
            this.orderService = orderService;
            this.orderDetailService = orderDetailService;
            this.articleService = articleService;
        }

        // GET: Order?page=1
        public async Task<ActionResult> Index(int page = 1)
        {
            var totalOrders = orderService.GetTotal();
            var totalPages = Math.Ceiling(totalOrders / (double)pageSize);   //Round up to the calculation result (exemple : 45 orders, page max to draw 20 (45/20 = 2.25), result = 3 (pages needed))

            var orders = await orderService.GetAll(page, pageSize);

            //pour l'exemple : la ligne suivante compile
            IViewModel test = new OrderViewModel();
            //mais la ligne suivante ne compile pas ! : (visiblement problème de covariance)
            //IList<IViewModel> test2 = new List<OrderViewModel>();
            //alors que celle là oui mais elle ne résoud pas le problème
            IEnumerable<IViewModel> test2 = new List<OrderViewModel>();
            //celle là ne résoud pas le problème non plus
            IList<IViewModel> test3 = new List<OrderViewModel>().ToList<IViewModel>();

            var paginationViewModel = new PaginationViewModel<OrderViewModel>
            {
                Items = orders.Select(x => x.ToViewModel()).ToList(),
                CurrentPage = page,
                TotalPages = totalPages
            };

            return View(paginationViewModel);
        }

        // GET: Order/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var order = await orderService.Get(id);
            return View(order?.ToViewModel());
        }

        // GET: Order/Create
        public async Task<ActionResult> Create()
        {
            var orderDetails = await orderDetailService.GetAll();
            ViewData["OrderDetails"] = orderDetails.Select(x => x.ToViewModel()).ToList();
            var articles = await articleService.GetAll();
            ViewData["Articles"] = articles.Select(x => x.ToViewModel()).ToList();
            ViewData["ArticlesSelected"] = articlesSelectedViewModel;
            return View(new OrderViewModel());
        }

        // POST: Order/Create
        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(OrderViewModel viewModel)
        {
            if (articlesSelectedViewModel.Count > 0)
            {
                ModelState["ListArticlesSelected"].ValidationState = ModelValidationState.Valid;
            }

            var articles = await articleService.GetAll();

            if (!ModelState.IsValid)
            {
                var orderDetails = await orderDetailService.GetAll();
                ViewData["OrderDetails"] = orderDetails.Select(x => x.ToViewModel()).ToList();
                ViewData["Articles"] = articles.Select(x => x.ToViewModel()).ToList();
                ViewData["ArticlesSelected"] = articlesSelectedViewModel;
                return View(viewModel);
            }

            foreach (var articleSelected in articlesSelectedViewModel)
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
                articlesSelectedViewModel.Add(articleSelected);
                success = true;

                await articleService.AddArticleStock(articleSelected.Id, -articleSelected.Qte);
            }

            ViewData["ArticlesSelected"] = articlesSelectedViewModel;
            return Json(new
            {
                Success = success,
                TotalAmount = articlesSelectedViewModel.Sum(x => x.Price * x.Qte),
                PartialView = RenderViewToString("_ListeArticles.cshtml", articlesSelectedViewModel)
            });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteArticle(int Id, int Qte)
        {
            await articleService.AddArticleStock(Id, Qte);
            var articleToRemove = articlesSelectedViewModel.Find(x => x.Id == Id);
            articlesSelectedViewModel.Remove(articleToRemove);
            return Json(new
            {
                TotalAmount = articlesSelectedViewModel.Sum(x => x.Price * x.Qte),
                PartialView = RenderViewToString("_ListeArticles.cshtml", articlesSelectedViewModel)
            });
        }

        // GET: Order/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var order = await orderService.Get(id);
            return View(order?.ToViewModel());
        }

        // POST: Order/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, OrderViewModel viewModel)
        {
            ModelState["ListArticlesSelected"].ValidationState = ModelValidationState.Valid;

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

        // GET: Order/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var order = await orderService.Get(id);
            return View(order?.ToViewModel());
        }

        // POST: Order/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, OrderViewModel viewModel)
        {
            ModelState["ListArticlesSelected"].ValidationState = ModelValidationState.Valid;

            if (!ModelState.IsValid) return View();

            try
            {
                await orderService.Delete(viewModel.Id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Article
        public async Task<IActionResult> Statistics()
        {
            var orders = await orderService.GetAll();
            var articles = await articleService.GetAll();

            var viewModel = new StatisticsViewModel
            {
                AverageTotalAmount = Math.Round(orders.Select(x => x.TotalAmount).Average()),
                AverageTotalArticle = Math.Round(orders.Select(x => x.OrderDetails.Count).Average(), 2),
                BestSellingArticle = orders.SelectMany(x => x.OrderDetails).GroupBy(x => x.Article).OrderByDescending(x => x.Count()).FirstOrDefault()?.Key.ToViewModel()
            };
            return View(viewModel);
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
