using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Models;

namespace ApplicationCore.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository articleRepository;

        public ArticleService(IArticleRepository articleRepository)
        {
            this.articleRepository = articleRepository;
        }

        public async Task<Article?> Get(int id)
        {
            return await articleRepository.GetById(id);
        }

        public async Task Add(Article article)
        {
            await articleRepository.Add(article);
        }

        public async Task<Article> UpdateArticleStock(int itemId, int quantity)
        {
            return await articleRepository.UpdateArticleStock(itemId, quantity);
        }

        public async Task<IList<Article>> GetArticlesBelowStock(int stock)
        {
            return await articleRepository.GetArticlesBelowStock(stock);
        }

        public async Task<IDictionary<Article, decimal>> GetTotalSalesPerArticle()
        {
            return await articleRepository.GetTotalSalesPerArticle();
        }
    }
}
