using BookAPIProject.Controllres.Dtos;
using BookAPIProject.Models;
using BookAPIProject.Models.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace BookAPIProject.Controllres
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CategoryDtos>))]
        public IActionResult GetCategories()
        {
            var categories = _categoryRepository.GetCategories().ToList();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var categoriesDto = new List<CategoryDtos>();
            foreach (var category in categories)
            {
                categoriesDto.Add(new CategoryDtos
                {
                    Id = category.Id,
                    Name = category.Name
                });
            }
            return Ok(categoriesDto);
        }

        [HttpGet("{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(CategoryDtos))]
        public IActionResult GetCategory(int categoryId)
        {
            if (!_categoryRepository.CategoryExist(categoryId))
            {
                return NotFound();
            }
            var category = _categoryRepository.GetCategory(categoryId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var categoryDto = new CategoryDtos()
            {
                Id = category.Id,
                Name = category.Name
            };

            return Ok(categoryDto);
        }

        [HttpGet("Book/{bookId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CategoryDtos>))]
        public IActionResult GetCategoriesOfBook(int bookId)
        {
            if (!_categoryRepository.CategoryExist(bookId))
            {
                return NotFound();
            }
            var categories = _categoryRepository.GetAllCategoriesOfBook(bookId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var CategoryDto = new List<CategoryDtos>();
            foreach(var category in categories)
            {
                CategoryDto.Add(new CategoryDtos()
                {
                    Id = category.Id,
                    Name = category.Name
                });
            }
            

            return Ok(CategoryDto);
        }

        [HttpGet("{categoryId}/books")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(BookDto))]
        public IActionResult GetAllBooksForCategory(int categoryId)
        {
            if (!_categoryRepository.CategoryExist(categoryId))
            {
                return NotFound();
            }
            var books = _categoryRepository.GetAllBooksFromCategory(categoryId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var booksDto = new List<BookDto>();
            foreach (var book in books)
            {
                booksDto.Add(new BookDto
                {
                    Id = book.Id,
                    Title = book.Title,
                    Isbn = book.Isbn,
                    DatePublished = book.DatePublished
                });
            }
            return Ok(booksDto);
        }

        [HttpPost]
        public IActionResult CreateCategory([FromBody] Category Categorycreate)
        {
            if (Categorycreate == null)
                return BadRequest(ModelState);
            var category = _categoryRepository.GetCategories()
                .Where(c => c.Name.Trim().ToUpper() == Categorycreate.Name.Trim().ToUpper())
                .FirstOrDefault();
            if (category != null)
            {
                ModelState.AddModelError("", $"Country {Categorycreate.Name} already exist");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_categoryRepository.Add(Categorycreate))
            {
                ModelState.AddModelError("", $"Something went wrong saving {Categorycreate.Name}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetCountry", new { countryId = Categorycreate.Id }, Categorycreate);
        }
        [HttpPut("{CategoryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult UpdateCategory(int CategoryId, [FromBody] Category CategoryUpdate)
        {
            if (CategoryUpdate == null)
                return BadRequest(ModelState);

            if (CategoryId != CategoryUpdate.Id)
                return BadRequest(ModelState);

            if (!_categoryRepository.CategoryExist(CategoryId))
                return NotFound();
            if (_categoryRepository.IsDuplicateCountryName(CategoryId, CategoryUpdate.Name))
            {
                ModelState.AddModelError("", $"Country {CategoryUpdate.Name} already exist");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_categoryRepository.Update(CategoryUpdate))
            {
                ModelState.AddModelError("", $"Something went wrong updating {CategoryUpdate.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{CategoryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult DeleteCategory(int CategoryId)
        {
            if (!_categoryRepository.CategoryExist(CategoryId))
                return NotFound();
            var categoryToDelete = _categoryRepository.GetCategory(CategoryId);
            if (_categoryRepository.GetAllBooksFromCategory(CategoryId).Count > 0)
            {
                ModelState.AddModelError("", $"Country {categoryToDelete.Name}" + "can't be deleted because it is used by at least one auther");
                return StatusCode(409, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_categoryRepository.Delete(categoryToDelete))
            {
                ModelState.AddModelError("", $"Something went wrong deleting {categoryToDelete.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
