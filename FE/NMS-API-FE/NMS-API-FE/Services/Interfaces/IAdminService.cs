using NMS_API_FE.DTOs;
using NMS_API_FE.Models;

namespace NMS_API_FE.Services.Interfaces
{
    public interface IAdminService
    {
        Task<IEnumerable<SystemAccountViewModel>> ManageAccounts();
        Task<SystemAccountViewModel> DetailsAccount(int id);
        Task CreateAccount(AccountCreateAdminDTO accountCreateAdminDTO);
        Task EditAccount(int id, AccountUpdateAdminDTO accountUpdateAdminDTO);
        Task<bool> DeleteAccount(int id);
        Task<List<NewsArticleViewModel>> Report(DateTime startDate, DateTime endDate);
        Task<IEnumerable<SystemAccountViewModel>> SearchAccount(string searchTerm);
    }
}
