using NMS_API_FE.DTOs;
using NMS_API_FE.Models;

namespace NMS_API_FE.Services.Interfaces
{
    public interface IStaffService
    {
        Task<IEnumerable<CategoryService>> GetActiveCategories();
        Task CreateCategory(CategoryDTO categoryViewModel);
        Task UpdateCategory(int id, CategoryViewModel categoryViewModel);
        Task DeleteCategory(int id);
        Task<IEnumerable<NewsArticleViewModel>> GetNewsArticles();
        Task CreateNewsArticle(NewsArticleCreateDTO dto);
        Task<NewsArticleViewModel> GetNewsArticleById(string id);
        Task UpdateNewsArticle(string id, NewsArticleUpdateDTO dto);
        Task DeleteNewsArticle(string id);
        Task<AccountDTO> GetMyProfile();
        Task UpdateMyProfile(AccountDTO dto);
        Task<IEnumerable<IEnumerable<NewsArticleViewModel>>> GetNewsHistory();
    }
}
