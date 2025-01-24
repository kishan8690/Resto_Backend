using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resto_Backend.Data;
using Resto_Backend.Model;

namespace Resto_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly ItemRepository itemRepository;

        public ItemController(ItemRepository itemRepository)
        {
            this.itemRepository = itemRepository;
        }
        [HttpGet]
        public IActionResult GetAllItem()
        {
            var items = itemRepository.SelectAllItem();
            if (items == null)
            {
                return NotFound();
            }
            return Ok(items);
        }
        [HttpGet("category/{chategoryid}")]
        public IActionResult GetItemByCategory(int chategoryid)
        {
            var items = itemRepository.SelectItemsByCategory(chategoryid);
            if (items == null)
            {
                return NotFound();
            }
            return Ok(items);
        }
        [HttpGet("{id}")]
        public IActionResult GetItemByID(int id)
        {
            var items = itemRepository.SelectItemByID(id);
            if (items == null)
            {
                return NotFound();
            }
            return Ok(items);
        }
        [HttpPost]
        public async Task<IActionResult> AddItem([FromForm] ItemModel item)
        {
            Console.WriteLine("Item Name: " + item.ItemName);
            Console.WriteLine("Description: " + item.ItemDescription);
            Console.WriteLine("Price: " + item.ItemPrice);
            Console.WriteLine("CategoryID: " + item.CategoryID);
            Console.WriteLine("ChefID: " + item.ChefID);

            if (item == null)
            {
                return BadRequest();
            }
            bool isInsert = await itemRepository.InsertItem(item);
            if (isInsert)
            {
                return Ok(new { Message = "Item is Inserted Successfully !" });
            }
            return StatusCode(500, "An Error occurred while Inserting");
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem([FromForm] ItemModel item)
        {
            if (item == null)
            {
                return BadRequest();
            }
            bool isUpdate = await itemRepository.UpdateItem(item);
            if (isUpdate)
            {
                return Ok(new { Message = "Item is Updated Successfully !" });
            }
            return StatusCode(500, "An Error occurred while Updating");
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteItem(int id)
        {
            bool isDelete = itemRepository.DeleteItem(id);
            if (isDelete)
            {
                return Ok(new { Message = "Item is Deleted Successfully !" });
            }
            return StatusCode(500, "An Error occurred while Deleting");
        }
    }
}
