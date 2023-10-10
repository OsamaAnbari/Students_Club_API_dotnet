﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace WebApplication1.Models
{
    public class Activity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name")]
        public string? Name { get; set; }

        [BsonElement("date")]
        public DateTime? Date { get; set; } = default(DateTime?);

        [BsonElement("teacher")]
        public string? Teacher { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("users")]
        public List<string?>? Users { get; set; }
    }
}
