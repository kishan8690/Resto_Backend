using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resto_Backend.Data;
using Resto_Backend.Model;

namespace Resto_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChefController : ControllerBase
    {
        private readonly ChefReposetory chefReposetory;

        public ChefController(ChefReposetory chefReposetory)
        {
            this.chefReposetory = chefReposetory;
        }
        [HttpGet]   
        public IActionResult GetAll()
        {
            var chefs = chefReposetory.SelectAllChef();
            if (chefs == null)
            {
                return NotFound();
            }
            return Ok(chefs);
        }
        [HttpGet("{id}")]
        public IActionResult GetByID(int id)
        {
            var chef = chefReposetory.SelectChefByPk(id);
            if (chef == null)
            {
                return NotFound();
            }
            return Ok(chef);
        }
        [HttpPost]
        public async Task<IActionResult> AddChef([FromBody] ChefModel chef)
        {
            if (chef == null)
            {
                return BadRequest();
            }
            bool isInsert =await chefReposetory.InsertChef(chef);
            if (isInsert)
            {
                return Ok(new { Message = "Chef is Inserted Successfully !" });
            }
            return StatusCode(500, "An Error occurred while Inserting");
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateChef([FromBody] ChefModel chef)
        {
            if (chef == null)
            {
                return BadRequest();
            }
            bool isUpdate =await chefReposetory.UpdateChef(chef);
            if (isUpdate)
            {
                return Ok(new { Message = "Chef is Updated Successfully !" });
            }
            return StatusCode(500, "An Error occurred while Updating");
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteChef(int id)
        {
            bool isDelete = chefReposetory.DeleteChef(id);
            if (isDelete)
            {
                return Ok(new { Message = "Chef is Deleted Successfully !" });
            }
            return StatusCode(500, "An Error occurred while Deleting");
        }
    }
}
