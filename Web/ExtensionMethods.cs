﻿using ApplicationCore.Models;
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
                CustomerName = order.Customer != null ? $"{order.Customer.FirstName} {order.Customer.LastName}" : string.Empty,
                Email = order.Customer?.Email,
                ShippingAddress = $"{order.Address.Street}, {order.Address.ZipCode} {order.Address.City} - {order.Address.Country}",
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                OrderStatus = Enum.Parse<OrderStatus>(order.OrderStatus),
                OrderDetails = order.OrderDetails.Select(x => x.ToViewModel()).ToList()
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
    }
}
