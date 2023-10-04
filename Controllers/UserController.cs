using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService userService = new UserService();

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllUsers()
        {
            List<User> myuser = await userService.GetAllUsers();
            if (myuser == null)
            {
                return NotFound();
            }
            
            return Ok(myuser);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserById(string id)
        {
            User myuser = await userService.GetUserById(id);
            if (myuser == null)
            {
                return NotFound();
            }

            return Ok(myuser);
        }

        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> PostUser(User myuser)
        {
            if (myuser == null)
            {
                return BadRequest();
            }
            else
            {
                await userService.Create(myuser);
                return CreatedAtAction(nameof(GetUserById), new { id = myuser.Id }, myuser);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(string id, User newuser)
        {
            User exisiting = await userService.GetUserById(id);

            if (newuser == null)
            {
                return BadRequest();
            }
            if (exisiting == null)
            {
                return NotFound();
            }

            await userService.UpdateById(id, newuser);

            return Ok(newuser);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(string id)
        {
            User myuser = await userService.GetUserById(id);

            if(myuser == null)
            {
                return NotFound();
            }

            await userService.DeleteById(id);

            return Ok(myuser);
        }
    }
}