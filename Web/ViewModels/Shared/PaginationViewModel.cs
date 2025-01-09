namespace Web.ViewModels.Shared
{
    public class PaginationViewModel<T>
    {
        public IList<T> Items { get; set; } = new List<T>();
        public PaginationInfosViewModels Infos { get; set; } = new PaginationInfosViewModels();
    }
}
