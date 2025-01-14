namespace Web.ViewModels.Shared
{
    public class PaginationViewModel<T>
    {
        //Le problème est ici pour Items
        public IList<T> Items { get; set; } = new List<T>();
        public int CurrentPage { get; set; }
        public double TotalPages { get; set; }
    }
}
