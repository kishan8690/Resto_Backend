using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resto_Backend.Data;
using Resto_Backend.Model;
using Resto_Backend.Utils;

namespace Resto_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueryController : ControllerBase
    {
        private readonly QueryReposetory queryReposetory;

        public QueryController(QueryReposetory queryReposetory)
        {
            this.queryReposetory = queryReposetory;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var queries = queryReposetory.SelectAllQuery();
            if (queries == null)
            {
                return NotFound();
            }
            return Ok(queries);
        }
        [HttpGet("{id}")]
        public IActionResult GetByID(int id)
        {
            var query = queryReposetory.SelectQueryByPk(id);
            if (query == null)
            {
                return NotFound();
            }
            return Ok(query);
        }
        [HttpPost]
        public IActionResult AddQuery([FromBody] QueryModel queryModel)
        {
            if (queryModel == null)
            {
                return BadRequest();
            }
            bool isInsert = queryReposetory.InsertQuery(queryModel);
            if (isInsert)
            {
                return Ok(new { Message = "Query is Inserted Successfully !" });
            }
            return StatusCode(500, "An Error occurred while Inserting");
        }
        [HttpPut("{id}")]
        public IActionResult UpdateQueryy([FromBody] QueryModel queryModel)
        {
            if (queryModel == null)
            {
                return BadRequest();
            }
            bool isUpdate = queryReposetory.UpdateQuery(queryModel);
            if (isUpdate)
            {
                return Ok(new { Message = "Query is Updated Successfully !" });
            }
            return StatusCode(500, "An Error occurred while Updating");
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteQuery(int id)
        {
            bool isDelete = queryReposetory.DeleteQuery(id);
            if (isDelete)
            {
                return Ok(new { Message = "Query is Deleted Successfully !" });
            }
            return StatusCode(500, "An Error occurred while Deleting");
        }
        [HttpPut("SendResponse")]
        public IActionResult SendResponse(MailResponse mailResponse,int id)
        {
            var query = queryReposetory.SelectQueryByPk(id);

            MailService mailService = new MailService();
            mailService.SendEmailNotification(mailResponse.ToMail,mailResponse.Subject, mailResponse.Message);
            return Ok(mailResponse);
        }
    }
}
