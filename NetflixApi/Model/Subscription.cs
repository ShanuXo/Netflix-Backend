using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace NetflixApi.Model
{
    public class Subscription
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId _id { get; set; }
        
        [BsonElement("UserId")]
        public string UserId { get; set; }

        [BsonElement("PaymentMethodToken")]
        public string PaymentMethodToken { get; set; }

        [BsonElement("Amount")]
        public decimal Amount { get; set; }

        [BsonElement("StartDate")]
        public DateTime StartDate { get; set; }

        [BsonElement("EndDate")]
        public DateTime EndDate { get; set; }


    }
}
