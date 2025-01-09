using ApplicationCore.Models;

namespace ApplicationCore.Interfaces.Repositories
{
    public interface IArticleRepository : IRepository<Article>
    {
        Task<IList<Article>> GetArticlesBelowStock(int stock);
        Task<IDictionary<Article, decimal>> GetTotalSalesPerArticle();
        Task<Article> AddArticleStock(int itemId, int quantity);
        Task<Article> UpdateArticleStock(int itemId, int quantity);
    }
}
