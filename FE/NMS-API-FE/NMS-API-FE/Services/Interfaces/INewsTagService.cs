using NMS_API_FE.Models;

namespace NMS_API_FE.Services.Interfaces
{
    public interface INewsTagService
    {
        Task AddNewsTagAsync(string NewsArticleId, int TagId);
        Task<IEnumerable<TagViewModel>> GetTagsOfArticleAsync(string NewsArticleId);
        Task<IEnumerable<NewsArticleViewModel>> GetArticlesFromTagAsync(int TagId);
        Task DeleteNewsTagAsync(string NewsArticleId, int TagId);
    }
}
