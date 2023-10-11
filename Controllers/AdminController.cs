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
    public class AdminController : ControllerBase
    {
        private readonly AdminService userService;

        public AdminController(IConfiguration configuration)
        {
            userService = new AdminService(configuration);
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll()
        {
            List<Admin> myuser = await userService.GetAll();
            if (myuser == null)
            {
                return NotFound();
            }

            return Ok(myuser);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(string id)
        {
            Admin myuser = await userService.GetById(id);
            if (myuser == null)
            {
                return NotFound();
            }

            return Ok(myuser);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> PostUser(Admin myuser)
        {
            if (myuser == null)
            {
                return BadRequest();
            }
            else
            {
                await userService.Create(myuser);
                return CreatedAtAction(nameof(GetById), new { id = myuser.Id }, myuser);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(string id, Admin newuser)
        {
            Admin exisiting = await userService.GetById(id);

            if (newuser == null)
            {
                return BadRequest();
            }
            if (exisiting == null)
            {
                return NotFound();
            }

            newuser.Id = exisiting.Id;
            await userService.UpdateById(id, newuser);

            return Ok(newuser);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(string id)
        {
            Admin myuser = await userService.GetById(id);

            if (myuser == null)
            {
                return NotFound();
            }

            await userService.DeleteById(id);

            return Ok(myuser);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("protected")]
        public IActionResult gg()
        {
            return Ok();
        }
    }
}
