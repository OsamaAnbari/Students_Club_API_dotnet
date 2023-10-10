using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class UserService
    {
        IMongoCollection<User> users;

        public UserService(IMongoCollection<User> users)
        {
            this.users = users;
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await users.Find(u => true).ToListAsync();
        }

        public async Task<User> GetUserById(string? id)
        {
            return await users.Find(u =>  u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<User> Create(User user)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password, workFactor: 10);
            await users.InsertOneAsync(user);
            return user;
        }

        public async Task UpdateById(string id, User user)
        {
            var filter = Builders<User>.Filter.Eq("id", user.Id);
            var update = Builders<User>.Update;
            var updateList = new List<UpdateDefinition<User>>();

            if(user.Name != null)
                updateList.Add(update.Set(x => x.Name, user.Name));
            if (user.Name != null)
                updateList.Add(update.Set(x => x.Surname, user.Surname));
            if (user.Name != null)
                updateList.Add(update.Set(x => x.Tc, user.Tc));
            if (user.Name != null)
                updateList.Add(update.Set(x => x.Department, user.Department));
            if (user.Password != null)
            {
                var newpass = BCrypt.Net.BCrypt.HashPassword(user.Password, workFactor: 10);
                updateList.Add(update.Set(x => x.Password, newpass));
            }

            if (updateList.Count > 0)
            {
                var combinedUpdate = update.Combine(updateList);
                await users.UpdateOneAsync(u => u.Id == user.Id, combinedUpdate);
            }
        }

        public async Task DeleteById(string id)
        {
            await users.DeleteManyAsync(u => u.Id == id);
        }

        public async Task<User?> Login(Login model)
        {
            User user = await users.Find<User>(u => u.Tc == model.Username).FirstOrDefaultAsync();

            if(user != null)
            {
                if (BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
                {
                    return user;
                }
            }
            return null;
        }
    }
}