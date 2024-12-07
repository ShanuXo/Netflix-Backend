using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NetflixApi.Model
{
    public class MovieList
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public string UserId { get; set; }
        public List<string> MoviesId { get; set; }    
    }
}
