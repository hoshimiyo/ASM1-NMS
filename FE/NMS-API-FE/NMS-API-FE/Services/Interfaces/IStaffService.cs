using NMS_API_FE.DTOs;
using NMS_API_FE.Models;

namespace NMS_API_FE.Services.Interfaces
{
    public interface IStaffService
    {
        Task<AccountDTO> GetMyProfile(int userId);
        Task UpdateMyProfile(int userId, AccountDTO dto);
        Task<IEnumerable<IEnumerable<NewsArticleViewModel>>> GetNewsHistory();
    }
}
