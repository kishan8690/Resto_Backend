using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resto_Backend.Data;
using Resto_Backend.Model;

namespace Resto_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryRepository categoryRepository;

        public CategoryController(CategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var categoryList = categoryRepository.SelectAllCategory();
            if (categoryList == null)
            {
                return NotFound();
            }
            return Ok(categoryList);
        }
        [HttpGet("{id}")]
        public IActionResult GetByID(int id)
        {
            var category = categoryRepository.SelectCategoryByPk(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }
        [HttpPost]
        public IActionResult AddCategory([FromBody] CategoryModel category)
        {
            if (category == null)
            {
                return BadRequest();
            }
            bool isInsert = categoryRepository.InsertCategory(category);
            if (isInsert)
            {
                return Ok(new { Message = "Category is Inserted Successfully !" });
            }
            return StatusCode(500, "An Error occurred while Inserting");
        }
        [HttpPut("{id}")]
        public IActionResult UpdateCategory([FromBody] CategoryModel category)
        {
            if (category == null)
            {
                return BadRequest();
            }
            bool isUpdate = categoryRepository.UpdateCategory(category);
            if (isUpdate)
            {
                return Ok(new { Message = "Category is Updated Successfully !" });
            }
            return StatusCode(500, "An Error occurred while Updating");
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            bool isDelete = categoryRepository.DeleteCategory(id);
            if (isDelete)
            {
                return Ok(new { Message = "Category is Deleted Successfully !" });
            }
            return StatusCode(500, "An Error occurred while Deleting");
        }
    }
}
