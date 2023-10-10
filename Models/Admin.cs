using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Admin
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name")]
        [Required(ErrorMessage = "First name is required")]
        public string? Name { get; set; }

        [BsonElement("surname")]
        public string? Surname { get; set; }

        [BsonElement("tc")]
        public string? Tc { get; set; }

        [BsonElement("password")]
        public string? Password { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("sent_messages")]
        public List<string?>? SentMessages { get; set; } = new List<string>();
    }
}