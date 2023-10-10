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
    public class ActivityController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly ActivityService activityService;

        public ActivityController(IConfiguration configuration)
        {
            _configuration = configuration;

            MongoClient client = new MongoClient(_configuration.GetValue<string>("ConnectionStrings:MongoString"));
            IMongoDatabase database = client.GetDatabase(_configuration.GetValue<string>("ConnectionStrings:MongoDB"));
            IMongoCollection<User> users = database.GetCollection<User>(_configuration.GetValue<string>("ConnectionStrings:UserCollection"));
            IMongoCollection<Activity> activities = database.GetCollection<Activity>("activity");

            activityService = new ActivityService(activities, users);
        }

        [Authorize(Roles = "admin,user")]
        [HttpGet("myactivities")]
        public async Task<IActionResult> GetProtectedData()
        {
            var id = HttpContext.Items["userId"];
            if (id != null)
            {
                List<Activity> myuser = await activityService.GetActivitiesByUserId($"{id}");
                return Ok(myuser);
            }

            return Unauthorized("User ID not found.");
        }

        [Authorize(Roles = "admin")]
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllActivities()
        {
            List<Activity> activities = await activityService.GetAllActivities();
            if (activities == null)
            {
                return NotFound();
            }

            return Ok(activities);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetActivityById(string id)
        {
            Activity activity = await activityService.GetActivityById(id);
            if (activity == null)
            {
                return NotFound();
            }

            return Ok(activity);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> PostUser(Activity activity)
        {
            if (activity == null)
            {
                return BadRequest();
            }
            else
            {
                await activityService.Create(activity);
                return CreatedAtAction(nameof(GetActivityById), new { id = activity.Id }, activity);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(string id)
        {
            await activityService.DeleteById(id);
            return Ok();
        }
    }
}
