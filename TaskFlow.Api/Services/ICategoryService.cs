using TaskFlow.Api.DTOs;
using TaskFlow.Api.Models;

public interface ICategoryService {
    Task<IEnumerable<CategoryReadDto>> GetAllCategoriesAsync();
    Task<Category> CreateCategoryAsync(CategoryCreateDto categoryDto);
    Task<string> DeleteCategoryAsync(int id);
    Task<string> UpdateCategoryAsync(int id, CategoryUpdateDto updateDto);
}