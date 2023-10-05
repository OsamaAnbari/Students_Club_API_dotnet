using MongoDB.Bson;
using MongoDB.Driver;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class AdminService
    {
        static MongoClient client = new MongoClient("mongodb+srv://osama3nbri13:asdrasdr1@cluster0.j3gm3vp.mongodb.net/");
        static IMongoDatabase database = client.GetDatabase("asptest");
        IMongoCollection<Admin> admins = database.GetCollection<Admin>("admin");

        public async Task<List<Admin>> GetAll()
        {
            return await admins.Find(u => true).ToListAsync();
        }

        public async Task<Admin> GetById(string id)
        {
            return await admins.Find(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Admin> Create(Admin admin)
        {
            await admins.InsertOneAsync(admin);
            return admin;
        }

        public async Task UpdateById(string id, Admin admin)
        {
            await admins.ReplaceOneAsync(u => u.Id == id, admin);
        }

        public async Task DeleteById(string id)
        {
            await admins.DeleteManyAsync(u => u.Id == id);
        }
    }
}