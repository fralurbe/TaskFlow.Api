using TaskFlow.Api.Data;
using TaskFlow.Api.DTOs;
using TaskFlow.Api.Models;
using TaskFlow.Api.Services;
using Microsoft.EntityFrameworkCore;

public class CategoryService : ICategoryService {
    private readonly ApplicationDbContext _context;

    public CategoryService(ApplicationDbContext context) {
        _context = context;
    }

    public async Task<IEnumerable<CategoryReadDto>> GetAllCategoriesAsync() {
        return await _context.Categories
            .Select(c => new CategoryReadDto { Id = c.Id, Name = c.Name })
            .ToListAsync<CategoryReadDto>();
    }

    public async Task<Category> CreateCategoryAsync(CategoryCreateDto categoryDto) {
        var nueva = new Category { Name = categoryDto.Name };
        _context.Categories.Add(nueva);
        await _context.SaveChangesAsync();
        return nueva;
    }

    public async Task<string> DeleteCategoryAsync(int id) {
        var categoria = await _context.Categories
            .Include(c => c.Tasks) // Necesitas cargar las tareas para contar
            .FirstOrDefaultAsync(c => c.Id == id);

        if (categoria == null) return "NOT_FOUND";

        // ¡Validación de lógica de negocio real!
        if (categoria.Tasks.Any()) {
            return "HAS_TASKS";
        }

        _context.Categories.Remove(categoria);
        await _context.SaveChangesAsync();

        return "SUCCESS";
    }

    public async Task<string> UpdateCategoryAsync(int id, CategoryUpdateDto updateDto) {
        var categoria = await _context.Categories.FindAsync(id);        
        if (categoria == null) return "NOT_FOUND";

        categoria.Name = updateDto.Name;
        _context.Categories.Update(categoria);
        await _context.SaveChangesAsync();

        return "SUCCESS";
    }

}