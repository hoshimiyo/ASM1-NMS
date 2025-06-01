
using NMS_API_FE.DTOs;
using NMS_API_FE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMS_API_FE.Services.Interfaces
{
    public interface ITagService
    {
        Task CreateTagAsync(TagDTO dto);
        Task UpdateTagAsync(int TagId, TagDTO dto);
        Task DeleteTagAsync(int TagId);
        Task<TagViewModel> GetTagAsync(int TagId);
        Task<IEnumerable<TagViewModel>> GetAllTagsAsync();

    }
}
