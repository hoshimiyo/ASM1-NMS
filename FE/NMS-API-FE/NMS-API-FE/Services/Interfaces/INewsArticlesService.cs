using NMS_API_FE.DTOs;
using NMS_API_FE.Models;

namespace NMS_API_FE.Services.Interfaces
{
    public interface INewsArticlesService
    {
        Task<IEnumerable<NewsArticleViewModel>> GetNewsArticlesAsync();
        Task<NewsArticleViewModel> GetNewsArticleByIdAsync(string id);
        Task CreateNewsArticleAsync(NewsArticleCreateDTO dto);
        Task UpdateNewsArticleAsync(string id, NewsArticleUpdateDTO dto);
        Task DeleteNewsArticleAsync(string id);
        Task<IEnumerable<NewsArticleViewModel>> SearchNewsArticles(string searchTerm, int categoryId, int tagId);
    }
}
