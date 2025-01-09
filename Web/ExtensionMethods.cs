using ApplicationCore.Models;
using ApplicationCore.ValueObjects;
using Web.ViewModels;
using static ApplicationCore.Enums;

namespace Web
{
    public static class ExtensionMethods
    {
        public static OrderViewModel ToViewModel(this Order order)
        {
            if (order == null) return new OrderViewModel();

            return new OrderViewModel
            {
                Id = order.Id,
                CustomerName = CustomerNameFormmatter(order.Customer),
                Email = order.Customer?.Email,
                ShippingAddress = AddressFormatter(order.Address),
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                OrderStatus = Enum.Parse<OrderStatus>(order.OrderStatus),
                OrderDetails = order.OrderDetails.Select(x => x.ToViewModel()).ToList(),
                WarehouseId = order.WarehouseId
            };
        }

        public static OrderDetailViewModel ToViewModel(this OrderDetail orderDetail)
        {
            if (orderDetail == null) return new OrderDetailViewModel();

            return new OrderDetailViewModel
            {
                Id = orderDetail.Id,
                ArticleId = orderDetail.ArticleId,
                OrderId = orderDetail.OrderId,
                Quantity = orderDetail.Quantity,
                UnitPrice = orderDetail.UnitPrice
            };
        }

        public static ArticleViewModel ToViewModel(this Article article)
        {
            if (article == null) return new ArticleViewModel();

            return new ArticleViewModel
            {
                Id = article.Id,
                Name = article.Name,
                Description = article.Description,
                Price = article.Price,
                StockQuantity = article.StockQuantity
            };
        }

        public static CustomerViewModel ToViewModel(this Customer customer)
        {
            if (customer == null) return new CustomerViewModel();

            int ordersCount = 0;
            double orderTotalAmount = 0;
            double orderAverageAmount = 0;
            if (customer.Orders != null && customer.Orders.Count > 0)
            {
                ordersCount = customer.Orders.Count;
                orderTotalAmount = customer.Orders.Sum(x => x.TotalAmount);
                orderAverageAmount = customer.Orders.Select(x => x.TotalAmount).Average();
            }

            return new CustomerViewModel
            {
                Id = customer.Id,
                Name = CustomerNameFormmatter(customer),
                Address = AddressFormatter(customer.Address),
                OrdersCount = ordersCount,
                OrderTotalAmount = orderTotalAmount,
                OrderAverageAmount = orderAverageAmount
            };
        }

        private static string CustomerNameFormmatter(Customer customer)
        {
            return customer != null ? $"{customer.FirstName} {customer.LastName}" : string.Empty;
        }

        private static string AddressFormatter(Address address)
        {
            return $"{address.Street}, {address.City}";
        }
    }
}
