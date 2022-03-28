using System.Collections.Generic;
using System.Linq;

namespace BookAPIProject.Models.Services
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BookDbContext _categoryDbContext;
        public CategoryRepository(BookDbContext CategoryDbContext)
        {
            _categoryDbContext = CategoryDbContext;
        }
        public bool CategoryExist(int CategoryId)
        {
            return _categoryDbContext.Categories.Any(c=>c.Id == CategoryId);
        }

        public ICollection<Book> GetAllBooksFromCategory(int CategoryId)
        {
            return _categoryDbContext.BookCategories.Where(c => c.CategoryId == CategoryId)
                .Select(b => b.Book).ToList();
        }

        public ICollection<Category> GetCategories()
        {
            return _categoryDbContext.Categories.OrderBy(c => c.Name).ToList();
        }

        public ICollection<Category> GetAllCategoriesOfBook(int BookId)
        {
            return _categoryDbContext.BookCategories.Where(b => b.BookId == BookId)
                .Select(c => c.Category).ToList();
        }

        public Category GetCategory(int CategoryId)
        {
            return _categoryDbContext.Categories.Where(c => c.Id == CategoryId).FirstOrDefault();
        }

        public bool IsDuplicateCountryName(int CategoryId, string CategoryName)
        {
            var Category = _categoryDbContext.Categories.Where(c => c.Name.Trim().ToUpper() == CategoryName.Trim().ToUpper() && c.Id != CategoryId).FirstOrDefault();
            return Category == null ? false : true;
        }

        public bool Add(Category category)
        {
            _categoryDbContext.Add(category);
            return Save();
        }

        public bool Update(Category category)
        {
            _categoryDbContext.Update(category);
            return Save();
        }

        public bool Delete(Category category)
        {
            _categoryDbContext.Remove(category);
            return Save();
        }

        public bool Save()
        {
            var saved = _categoryDbContext.SaveChanges();
            return saved >= 0 ? true : false;
        }
    }
}
