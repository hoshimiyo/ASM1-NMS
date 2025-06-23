using BLL.DTOs;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IAccountService
    {
        Task<string> AuthenticateAsync(AccountLoginDTO dto);
        Task<IEnumerable<SystemAccount>> GetAllAccountsAsync();
        Task<IEnumerable<SystemAccount>> GetAllAccountsForManageAsync();
        Task<SystemAccount> GetAccountByIdAsync(int id);
        Task CreateAccountAsync(AccountCreateDTO account);
        Task<SystemAccount> CreateAccountAsync(AccountCreateAdminDTO account);
        Task UpdateAccountAsync(int id, AccountDTO account);
        Task UpdateAccountAsync(int id, AccountUpdateAdminDTO account);
        Task<(bool Success, string Message)> DeleteAccountAsync(int id);

        
    }
}
