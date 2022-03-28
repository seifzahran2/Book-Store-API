using System.Collections.Generic;

namespace BookAPIProject.Models.Services
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();
        Category GetCategory(int ContryId);
        ICollection<Category> GetAllCategoriesOfBook(int BookId);
        ICollection<Book> GetAllBooksFromCategory(int CategoryId);
        bool CategoryExist(int CategoryId);
        bool IsDuplicateCountryName(int CategoryId, string CategoryName);
        bool Add(Category category);
        bool Delete(Category category);
        bool Update(Category category);
        bool Save();
    }
}
