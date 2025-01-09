namespace Web.ViewModels
{
    public class PaginationViewModel<T>
    {
        public IList<T> Items { get; set; } = new List<T>();
        public int CurrentPage { get; set; }
        public double TotalPages { get; set; }
    }
}
