using System.Globalization;
using ApplicationCore.Models;
using ApplicationCore.Services;
using ApplicationCore.ValueObjects;
using Infrastructure;
using Infrastructure.Repositories;

namespace Web
{
    internal class Program
    {
        private static ArticleService articleService;
        private static OrderService orderService;
        private static OrderDetailService orderDetailService;

        static void Main(string[] args)
        {
            InsertDefaultData();
            //DisplayAllArticles();
            var dbContext = new ApplicationDbContext();
            var articleRepository = new ArticleRepository(dbContext);
            var orderRepository = new OrderRepository(dbContext);
            var orderDetailRepository = new OrderDetailRepository(dbContext);
            articleService = new ArticleService(articleRepository);
            orderService = new OrderService(orderRepository);
            orderDetailService = new OrderDetailService(orderDetailRepository);

            ReadCSV("C:\\Users\\henri.libault-de-la-\\Downloads\\export (1).csv", ',', new CultureInfo("en-US")).Wait();
        }

        private static async Task ReadCSV(string filePath, char delimiter, IFormatProvider numberFormatProvider)
        {
            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string ligne;
                    string currentModel = string.Empty;
                    string[] columns = [];

                    while ((ligne = reader.ReadLine()) != null)
                    {
                        string[] valeurs = ligne.Split(delimiter);

                        if (valeurs.Length == 1)
                        {
                            currentModel = valeurs[0];
                            continue;
                        }
                        if (valeurs[0] == "Id")
                        {
                            columns = valeurs;
                            continue;
                        }

                        if (!string.IsNullOrEmpty(currentModel) && columns.Length > 0)
                        {
                            switch (currentModel)
                            {
                                case "# Articles":
                                    await AddArticle(valeurs, columns, numberFormatProvider);
                                    break;
                                case "# Orders":
                                    await AddOrder(valeurs, columns, numberFormatProvider);
                                    break;
                                case "# OrderDetails":
                                    await AddOrderDetail(valeurs, columns, numberFormatProvider);
                                    break;
                                default:
                                    throw new Exception("Model inconnu");
                            }
                        }
                        else
                        {
                            throw new Exception("Aucun Model trouvé");
                        }

                        Console.WriteLine();
                    }
                }
            }
            else
            {
                throw new Exception("Le fichier n'existe pas");
            }
        }

        private static async Task AddArticle(string[] valeurs, string[] columns, IFormatProvider numberFormatProvider)
        {
            var article = new Article
            {
                Name = valeurs[Array.IndexOf(columns, nameof(Article.Name))],
                Description = valeurs[Array.IndexOf(columns, nameof(Article.Description))],
                Price = decimal.Parse(valeurs[Array.IndexOf(columns, nameof(Article.Price))], numberFormatProvider),
                StockQuantity = int.Parse(valeurs[Array.IndexOf(columns, nameof(Article.StockQuantity))]),
            };

            await articleService.Add(article);

            Console.Write($"Article : {article.Id} {article.Name}  {article.Description}  {article.Price}  {article.StockQuantity}");
        }

        private static async Task AddOrder(string[] valeurs, string[] columns, IFormatProvider numberFormatProvider)
        {
            var order = new Order
            {
                Customer = new Customer
                {
                    FirstName = string.Empty,
                    LastName = string.Empty,
                    Email = valeurs[Array.IndexOf(columns, nameof(Customer.Email))]
                },
                Address = new Address(
                    valeurs[Array.IndexOf(columns, "ShippingAddress")],
                    valeurs[Array.IndexOf(columns, nameof(Address.City))],
                    string.Empty,
                    string.Empty
                ),
                OrderDate = DateTime.Parse(valeurs[Array.IndexOf(columns, nameof(Order.OrderDate))]),
                TotalAmount = double.Parse(valeurs[Array.IndexOf(columns, nameof(Order.TotalAmount))], numberFormatProvider),
                OrderStatus = valeurs[Array.IndexOf(columns, nameof(Order.OrderStatus))],
                WarehouseId = int.Parse(valeurs[Array.IndexOf(columns, nameof(Order.WarehouseId))])
            };

            await orderService.Add(order);

            Console.Write($"Order : {order.Id} {order.CustomerId} {order.Address.Street} {order.Address.City} {order.OrderDate} {order.TotalAmount} {order.OrderStatus} {order.WarehouseId}");
        }

        private static async Task AddOrderDetail(string[] valeurs, string[] columns, IFormatProvider numberFormatProvider)
        {
            var orderDetail = new OrderDetail
            {
                Order = await orderService.Get(int.Parse(valeurs[Array.IndexOf(columns, nameof(OrderDetail.OrderId))])),
                Article = await articleService.Get(int.Parse(valeurs[Array.IndexOf(columns, nameof(OrderDetail.ArticleId))])),
                Quantity = int.Parse(valeurs[Array.IndexOf(columns, nameof(OrderDetail.Quantity))]),
                UnitPrice = decimal.Parse(valeurs[Array.IndexOf(columns, nameof(OrderDetail.UnitPrice))], numberFormatProvider)
            };

            await orderDetailService.Add(orderDetail);

            Console.Write($"OrderDetails : {orderDetail.Id} {orderDetail.OrderId} {orderDetail.ArticleId} {orderDetail.Quantity} {orderDetail.UnitPrice}");
        }

        private static void DisplayAllArticles()
        {
            using (var context = new ApplicationDbContext())
            {
                var articles = context.Articles.ToList();
                foreach (var article in articles)
                {
                    Console.WriteLine($"ID: {article.Id}, Name: {article.Name}, Price: {article.Price}, NbStock: {article.StockQuantity}");
                }
            }
        }

        private static void InsertDefaultData()
        {
            using (var context = new ApplicationDbContext())
            {
                var warehouse1 = new Warehouse
                {
                    Name = "Entrepot de Paris",
                    Address = "10 rue du csharp",
                    PostalCode = 75000,
                    CodeAccesMD5 = new List<string>
                    {
                        "840e998a22948adf5de39bd4f2b35da7",
                        "74b87337454200d4d33f80c4663dc5e5"
                    }
                };

                var warehouse2 = new Warehouse
                {
                    Name = "Entrepot de Rennes",
                    Address = "10 rue du wpf",
                    PostalCode = 35000,
                    CodeAccesMD5 = new List<string>
                    {
                        "68bbc76015c15ccf567bd797472ad1c4",
                        "8e06e600bdc53eaf6173694e7d155211"
                    }
                };

                context.Warehouses.Add(warehouse1);
                context.Warehouses.Add(warehouse2);
                context.SaveChanges();
            }
        }
    }
}
