using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoWithDotNetAPI.DAta;
using NetflixApi.Model;

namespace NetflixApi.Services
{
    public class SubscriptionServices
    {
        public readonly IMongoCollection<Subscription> _subscriptionCollection;

        public SubscriptionServices(IOptions<UserDataContext> userDataContext)
        {
            var mongoClient = new MongoClient(userDataContext.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(userDataContext.Value.DatabaseName);
            _subscriptionCollection= mongoDatabase.GetCollection<Subscription>(userDataContext.Value.SubscriptionCollectionName);
        }


        public async Task<List<Subscription>> GetSubscriptionsByUserIdAsync(string userId)
        {
            var filter = Builders<Subscription>.Filter.Eq(s => s.UserId, userId);
            var subscriptions = await _subscriptionCollection.Find(filter).ToListAsync();
            return subscriptions;
        }
        public void CreateSubscription(Subscription subscription)
        {
            _subscriptionCollection.InsertOne(subscription);
        }
    }
}
