namespace Web.ViewModels
{
    public class CustomerViewModel : IViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int OrdersCount { get; set; }
        public double OrderTotalAmount { get; set; }
        public double OrderAverageAmount { get; set; }
    }
}
