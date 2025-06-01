using AutoMapper;
using NMS_API_FE.DTOs;
using NMS_API_FE.Models;

namespace BLL.Utils
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CategoryViewModel, CategoryDTO>();
            CreateMap<SystemAccountViewModel, AccountDTO>()
                .ForMember(dest => dest.Password, opt => opt.Ignore());

            CreateMap<AccountDTO, SystemAccountViewModel>()
                .ForMember(dest => dest.AccountPasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedArticles, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedArticles, opt => opt.Ignore());

            CreateMap<AccountUpdateAdminDTO, SystemAccountViewModel>()
                .ForMember(dest => dest.AccountPasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedArticles, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedArticles, opt => opt.Ignore());

            CreateMap<SystemAccountViewModel, AccountUpdateAdminDTO>()
                .ForMember(dest => dest.Password, opt => opt.Ignore()); // Ignore password for security
        }
    }
}
