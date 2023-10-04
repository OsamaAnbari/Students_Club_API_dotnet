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

        [BsonElement("tc")]
        public string? Tc { get; set; }

        [BsonElement("password")]
        public string? Password { get; set; }
    }
}