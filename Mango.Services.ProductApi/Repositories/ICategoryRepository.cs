using Mango.Services.ProductApi.Models.Dtos;

namespace Mango.Services.ProductApi.Repositories;

public interface ICategoryRepository
{
    Task<IEnumerable<CategoryDto>> GetCategories();
    Task<CategoryDto> GetCategoryById(int categoryId);
    Task<CategoryDto> CreateUpdateCategory(CategoryDto categoryDto);
    Task<bool> DeleteCategory(int categoryId);
}