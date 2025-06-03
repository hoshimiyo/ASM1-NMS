using NMS_API_FE.Models;

namespace NMS_API_FE.Services.Interfaces
{
    public interface ILecturerService
    {
        Task<IEnumerable<NewsArticleViewModel>> GetNewsArticles();
        Task<NewsArticleViewModel> GetNewsArticleById(string id);  

    }
}
