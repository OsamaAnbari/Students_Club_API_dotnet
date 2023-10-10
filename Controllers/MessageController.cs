using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly MessageService messageService;

        public MessageController(IConfiguration configuration)
        {
            _configuration = configuration;

            MongoClient client = new MongoClient(_configuration.GetValue<string>("ConnectionStrings:MongoString"));
            IMongoDatabase database = client.GetDatabase(_configuration.GetValue<string>("ConnectionStrings:MongoDB"));
            IMongoCollection<Message> messages = database.GetCollection<Message>(_configuration.GetValue<string>("ConnectionStrings:MessageCollection"));
            IMongoCollection<User> users = database.GetCollection<User>(_configuration.GetValue<string>("ConnectionStrings:UserCollection"));
            IMongoCollection<Admin> admins = database.GetCollection<Admin>(_configuration.GetValue<string>("ConnectionStrings:AdminCollection"));

            messageService = new MessageService(admins, users, messages);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("getsent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSentByUserId()
        {
            var id = HttpContext.Items["userId"];
            List<Message> myMessages = await messageService.GetSentByUserId($"{id}");
            if (myMessages == null)
            {
                return NotFound();
            }

            return Ok(myMessages);
        }

        [Authorize(Roles = "user")]
        [Route("getreceived")]
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetReceivedByUserId()
        {
            var id = HttpContext.Items["userId"];
            List<Message> myMessages = await messageService.GetReceivedByUserId($"{id}");
            if (myMessages == null)
            {
                return NotFound();
            }

            return Ok(myMessages);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> PostUser(Message myMessage)
        {
            if (myMessage == null)
            {
                return BadRequest();
            }
            else
            {
                var id = HttpContext.Items["userId"];
                myMessage.SenderId = $"{id}";
                await messageService.Create(myMessage);
                return Ok(myMessage);
            }
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteById(string id)
        {
            await messageService.DeleteById(id);
            return Ok();
        }

        [Authorize(Roles = "user")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteInUserById(string id)
        {
            await messageService.DeleteInUserById(id);
            return Ok();
        }
    }
}
