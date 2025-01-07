using ApplicationCore.ValueObjects;

namespace ApplicationCore.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public Address Address { get; set; }
        public DateTime OrderDate { get; set; }
        public double TotalAmount { get; set; }
        public string OrderStatus { get; set; }
        public IList<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }
    }
}
