using NMS_API_FE.Models;

namespace NMS_API_FE.Services.Interfaces
{
    public interface IGuestService
    {
        Task<IEnumerable<NewsArticleViewModel>> GetArticlesWithActiveCategories();
    }
}
