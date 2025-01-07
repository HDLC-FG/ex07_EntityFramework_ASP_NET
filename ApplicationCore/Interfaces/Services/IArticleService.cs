using ApplicationCore.Models;

namespace ApplicationCore.Interfaces.Services
{
    public interface IArticleService
    {
        // Method to get an article by id
        Task<Article?> Get(int id);

        // Method to add new article
        Task Add(Article article);

        // Method to update the stock quantity of an Article
        Task<Article> UpdateArticleStock(int itemId, int quantity);

        // Method to fetch all Articles that are below given stock
        Task<IList<Article>> GetArticlesBelowStock(int stock);

        // Method to get total sales for each Article
        Task<IDictionary<Article, decimal>> GetTotalSalesPerArticle();
    }
}
