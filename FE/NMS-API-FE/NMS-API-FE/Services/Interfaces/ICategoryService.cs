using NMS_API_FE.Models;

namespace NMS_API_FE.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryViewModel>> GetAllCategoriesAsync();
        Task<CategoryViewModel> GetCategoryByIdAsync(int id);
        Task CreateCategoryAsync(CategoryViewModel category);
        Task UpdateCategoryAsync(int id, CategoryViewModel category);
        Task DeleteCategoryAsync(int id);

    }
}
