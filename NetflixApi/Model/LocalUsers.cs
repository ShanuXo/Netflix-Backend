using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace NetflixApi.Model
{
    public class LocalUsers
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonElement("Email")]
        public string Email { get; set; }
        [BsonElement("Password")]
        public string Password { get; set; }
       
    }
}
