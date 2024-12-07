using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoWithDotNetAPI.DAta;
using NetflixApi.Model;

namespace NetflixApi.Services
{
    public class VideoServices
    {
        private readonly IMongoCollection<Video> _videoCollection;
        public VideoServices(IOptions<UserDataContext> userDataContext)
        {
            var mongoClient = new MongoClient(userDataContext.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(userDataContext.Value.DatabaseName);
            _videoCollection = mongoDatabase.GetCollection<Video>(userDataContext.Value.VideoCollectionName);

        }
        public async Task<Video> GetVideoByIdAsync(string videoId)
        {
            if (!ObjectId.TryParse(videoId, out ObjectId objectId))
            {
                return null;
            }

            return await _videoCollection.Find(video => video.Id == objectId).FirstOrDefaultAsync();
        }
    }
}
