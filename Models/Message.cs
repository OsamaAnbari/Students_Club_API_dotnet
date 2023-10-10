using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace WebApplication1.Models
{
    public class Message
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("date")]
        public DateTime? Date { get; set; } = default(DateTime?);

        [BsonElement("subject")]
        public string? Subject { get; set; }

        [BsonElement("content")]
        public string? Content { get; set; }

        [BsonElement("sender_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? SenderId { get; set;}

        [BsonElement("receiver_ids")]
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string>? ReceiverIds { get; set; } = new List<string>();
    }
}
