using Microsoft.AspNetCore.Mvc;
using NMS_API_FE.ApiResponseModels;
using NMS_API_FE.DTOs;

namespace NMS_API_FE.Services.Interfaces
{
    public interface IAccountService
    {
        Task<LoginApiResponse> Login(AccountLoginDTO accountLoginDTO);
        Task Register(AccountCreateDTO registerDTO);
        Task Logout();
    }
}
