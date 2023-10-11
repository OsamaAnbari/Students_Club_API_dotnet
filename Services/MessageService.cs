using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class MessageService
    {
        IMongoCollection<User> users;
        IMongoCollection<Admin> admins;
        IMongoCollection<Message> messages;

        public MessageService(IConfiguration configuration)
        {
            MongoClient client = new MongoClient(configuration.GetValue<string>("ConnectionStrings:MongoString"));
            IMongoDatabase database = client.GetDatabase(configuration.GetValue<string>("ConnectionStrings:MongoDB"));
            messages = database.GetCollection<Message>(configuration.GetValue<string>("ConnectionStrings:MessageCollection"));
            users = database.GetCollection<User>(configuration.GetValue<string>("ConnectionStrings:UserCollection"));
            admins = database.GetCollection<Admin>(configuration.GetValue<string>("ConnectionStrings:AdminCollection"));
        }

        public async Task<Message> GetById(string? id)
        {
            Message message = await messages.Find(u => u.Id == id).FirstOrDefaultAsync();
            return message;
        }

        public async Task<List<Message>> GetSentByUserId(string? userId)
        {
            Admin admin = await admins.Find(u => u.Id == userId).FirstOrDefaultAsync();
            List<string?>? myMessages = admin.SentMessages.ToList();
            var filter = Builders<Message>.Filter.In("Id", myMessages);

            return await messages.Find(filter).ToListAsync();
        }

        public async Task<List<Message>> GetReceivedByUserId(string? userId)
        {
            User user = await users.Find(u => u.Id == userId).FirstOrDefaultAsync();
            List<string?>? myMessages = user.ReceivedMessages.ToList();
            var filter = Builders<Message>.Filter.In("Id", myMessages);

            return await messages.Find(filter).ToListAsync();
        }

        public async Task<Message> Create(Message message)
        {
            await messages.InsertOneAsync(message);

            var update1 = Builders<User>.Update.Push(u => u.ReceivedMessages, message.Id);
            var filter1 = Builders<User>.Filter.In("Id", message.ReceiverIds);

            var update2 = Builders<Admin>.Update.Push(u => u.SentMessages, message.Id);
            var filter2 = Builders<Admin>.Filter.Eq("Id", message.SenderId);

            await users.UpdateManyAsync(filter1, update1);
            await admins.UpdateManyAsync(filter2, update2);

            return message;
        }

        public async Task DeleteById(string id)
        {
            Message myMessage = await messages.Find(u => u.Id == id).FirstOrDefaultAsync();
            List<string?>? myUsers = myMessage.ReceiverIds.ToList();
            string? myAdmin = myMessage.SenderId;

            var filter1 = Builders<User>.Filter.In("Id", myUsers);
            var update1 = Builders<User>.Update.Pull(u => u.ReceivedMessages, id);

            var filter2 = Builders<Admin>.Filter.Eq("Id", myAdmin);
            var update2 = Builders<Admin>.Update.Pull(u => u.SentMessages, id);

            await messages.DeleteManyAsync(u => u.Id == id);
            await users.UpdateManyAsync(filter1, update1);
            await admins.UpdateManyAsync(filter2, update2);
        }

        public async Task DeleteInUserById(string id)
        {
            Message myMessage = await messages.Find(u => u.Id == id).FirstOrDefaultAsync();
            List<string?>? myUsers = myMessage.ReceiverIds.ToList();

            var filter1 = Builders<User>.Filter.In("Id", myUsers);
            var update1 = Builders<User>.Update.Pull(u => u.ReceivedMessages, id);

            await users.UpdateManyAsync(filter1, update1);
        }
    }
}