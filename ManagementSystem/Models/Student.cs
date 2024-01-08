using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ManagementSystem.Models
{
    [BsonIgnoreExtraElements]
    public class Student
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Number { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public int Age { get; set; }
    }
}
