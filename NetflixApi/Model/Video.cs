using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace NetflixApi.Model
{
    public class Video
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        

        public string name { get; set; }
        //new
        public string Title { get; set; } = string.Empty; 
        public string VideoPath { get; set; } = string.Empty;


        [BsonElement("data")]
        public BsonBinaryData Data { get; set; }
    }

   
}
