using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
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

        public async Task<User> GetUserById(string id)
        {
            return await users.Find(u =>  u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<User> Create(User user)
        {
            await users.InsertOneAsync(user);
            return user;
        }

        public async Task UpdateById(string id, User user)
        {
            await users.ReplaceOneAsync(u => u.Id == id, user);
        }

        public async Task DeleteById(string id)
        {
            await users.DeleteManyAsync(u => u.Id == id);
        }
    }
}