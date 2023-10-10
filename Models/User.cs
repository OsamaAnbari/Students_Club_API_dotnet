using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("role")]
        public string Role = "user";

        [BsonElement("name")]
        [Required(ErrorMessage = "First name is required")]
        public string? Name { get; set; }

        [BsonElement("surname")]
        public string? Surname { get; set; }

        [BsonElement("tc")]
        public string? Tc { get; set; }

        [BsonElement("department")]
        public string? Department { get; set; }

        [BsonElement("email")]
        public string? Email { get; set; }

        [BsonElement("image")]
        public string? Image { get; set; }

        [BsonElement("mobile")]
        public string? Mobile { get; set; }

        [BsonElement("password")]
        public string? Password { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("admin")]
        public List<string?>? AdminId { get; set; } = new List<string>();

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("activities")]
        public List<string?>? Activities { get; set; } = new List<string>();

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("sent_messages")]
        public List<string?>? SentMessages { get; set; } = new List<string>();

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("received_messages")]
        public List<string?>? ReceivedMessages { get; set; } = new List<string>();
    }
}