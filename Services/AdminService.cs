using MongoDB.Bson;
using MongoDB.Driver;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class AdminService
    {
        IMongoCollection<Admin> admins;

        public AdminService(IMongoCollection<Admin> admins)
        {
            this.admins = admins;
        }

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

        public async Task<Admin> AuthUsename(Login model)
        {
            Admin admin = await admins.Find<Admin>(u => u.Tc == model.Username).FirstOrDefaultAsync();
            if(admin != null)
            {
                if (admin.Password == model.Password)
                {
                    return admin;
                }
            }
            return null;
        }
    }
}