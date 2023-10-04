using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set;  }

        [BsonElement("name")]
        public string? Name { get; set; }

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

        [BsonElement("surname")]
        public string? Surname { get; set; }

        [BsonElement("role")]
        public string? Role { get; set; }

        [BsonElement("password")]
        public string? Password { get; set; }

    }
}