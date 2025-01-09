using ApplicationCore.Models;

namespace ApplicationCore.Interfaces.Services
{
    public interface IArticleService
    {
        Task<IList<Article>> GetAll();
        Task<Article?> Get(int id);
        Task Add(Article article);
        Task<Article> AddArticleStock(int itemId, int quantity);
        Task<Article> UpdateArticleStock(int itemId, int quantity);
        Task<IList<Article>> GetArticlesBelowStock(int stock);
        Task<IDictionary<Article, decimal>> GetTotalSalesPerArticle();
    }
}
