using Microsoft.AspNetCore.Mvc;
using NMS_API_FE.DTOs;

namespace NMS_API_FE.Services.Interfaces
{
    public interface IAccountService
    {
        Task Login(AccountLoginDTO loginDTO);
        Task Register(AccountCreateDTO registerDTO);
        Task Logout();
    }
}
