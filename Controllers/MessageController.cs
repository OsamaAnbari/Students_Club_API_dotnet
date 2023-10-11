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
        private readonly MessageService messageService;

        public MessageController(IConfiguration configuration)
        {
            messageService = new MessageService(configuration);
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
        [HttpGet("getreceived")]
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
            Message message = await messageService.GetById(id);
            if (message == null)
            {
                return NotFound();
            }

            await messageService.DeleteById(id);
            return Ok();
        }

        [Authorize(Roles = "user")]
        [HttpDelete("deletefromuser{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteInUserById(string id)
        {
            Message message = await messageService.GetById(id);
            if (message == null)
            {
                return NotFound();
            }

            await messageService.DeleteInUserById(id);
            return Ok();
        }
    }
}
