using MongoDB.Bson;
using MongoDB.Driver;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class ActivityService
    {
        IMongoCollection<Activity> activities;
        IMongoCollection<User> users;

        public ActivityService(IMongoCollection<Activity> activities, IMongoCollection<User> users)
        {
            this.activities = activities;
            this.users = users;
        }

        public async Task<List<Activity>> GetAllActivities()
        {
            return await activities.Find(u => true).ToListAsync();
        }

        public async Task<Activity> GetActivityById(string? id)
        {
            return await activities.Find(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Activity>> GetActivitiesByUserId(string? userId)
        {
            User user = await users.Find(u => u.Id == userId).FirstOrDefaultAsync();
            List<string?>? myActivities = user.Activities.ToList();
            var filter = Builders<Activity>.Filter.In("Id", myActivities);

            return await activities.Find(filter).ToListAsync();
        }

        public async Task<Activity> Create(Activity activity)
        {
            await activities.InsertOneAsync(activity);

            var update = Builders<User>.Update.Push(u => u.Activities, activity.Id);
            var filter = Builders<User>.Filter.In("Id", activity.Users);

            await users.UpdateManyAsync(filter, update);
            return activity;
        }

        public async Task UpdateById(string id, Activity activity)
        {
            await activities.ReplaceOneAsync(u => u.Id == id, activity);
        }

        public async Task DeleteById(string id)
        {
            Activity myactivity = await activities.Find(u => u.Id == id).FirstOrDefaultAsync();
            List<string?>? myusers = myactivity.Users.ToList();

            var filter = Builders<User>.Filter.In("Id", myusers);
            var update = Builders<User>.Update.Pull(u => u.Activities, id);

            await activities.DeleteManyAsync(u => u.Id == id);
            await users.UpdateManyAsync(filter, update);
        }
    }
}
