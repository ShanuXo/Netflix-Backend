using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoWithDotNetAPI.DAta;
using NetflixApi.Model;

namespace NetflixApi.Services
{
    public class MovieListServices
    {
        private readonly IMongoCollection<MovieList> _movieListCollection;
        public MovieListServices(IOptions<UserDataContext> userDataContext)
        {
            var mongoClient = new MongoClient(userDataContext.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(userDataContext.Value.DatabaseName);
            _movieListCollection = mongoDatabase.GetCollection<MovieList>(userDataContext.Value.MovieListCollectionName);

        }
        public async Task<bool> AddMovieAsync(string userId, string newMovieId)
        {
            var filter = Builders<MovieList>.Filter.Eq(x => x.UserId, userId);
            var update = Builders<MovieList>.Update.Push(x => x.MoviesId, newMovieId);

            // Check if the document exists for the user
            var existingDocument = await _movieListCollection.Find(filter).FirstOrDefaultAsync();

            if (existingDocument != null)
            {
                // Update the existing document
                var result = await _movieListCollection.UpdateOneAsync(filter, update);
                return result.ModifiedCount > 0;
            }
            else
            {
                // Create a new document for the user with the new movie
                var newDocument = new MovieList
                {
                    UserId = userId,
                    MoviesId = new List<string> { newMovieId }
                };

                await _movieListCollection.InsertOneAsync(newDocument);
                return true;
            }
        }
        public async Task<bool> UpdateMovieNamesAsync(string userId, List<string> newMoviesID)
        {
            var filter = Builders<MovieList>.Filter.Eq(x => x.UserId, userId);
            var update = Builders<MovieList>.Update.PushEach(x => x.MoviesId, newMoviesID);

            // Check if the document exists for the user
            var existingDocument = await _movieListCollection.Find(filter).FirstOrDefaultAsync();

            if (existingDocument != null)
            {
                // Update the existing document
                var result = await _movieListCollection.UpdateOneAsync(filter, update);
                return result.ModifiedCount > 0;
            }
            else
            {
                // Create a new document for the user
                var newDocument = new MovieList
                {
                    UserId = userId,
                    MoviesId = newMoviesID
                };

                await _movieListCollection.InsertOneAsync(newDocument);
                return true;
            }
        }
        public async Task<MovieList> GetUserMovieListAsync(string userId)
        {
            var filter = Builders<MovieList>.Filter.Eq(x => x.UserId, userId);
            var userMovieList = await _movieListCollection.Find(filter).FirstOrDefaultAsync();

            return userMovieList;
        }
    }
}




//using Microsoft.Extensions.Options;
//using MongoDB.Bson;
//using MongoDB.Driver;
//using MongoWithDotNetAPI.DAta;
//using NetflixApi.Model;

//namespace NetflixApi.Services
//{
//    public class MovieListServices
//    {
//        private readonly IMongoCollection<MovieList> _movieListCollection;
//        public MovieListServices(IOptions<UserDataContext> userDataContext )
//        {
//            var mongoClient = new MongoClient(userDataContext.Value.ConnectionString);
//            var mongoDatabase = mongoClient.GetDatabase(userDataContext.Value.DatabaseName);
//            _movieListCollection = mongoDatabase.GetCollection<MovieList>(userDataContext.Value.MovieListCollectionName);

//        }
//        public async Task<bool> UpdateMovieNamesAsync(string userId, List<string> newMoviesID)
//        {
//            var filter = Builders<MovieList>.Filter.Eq(x => x.UserId, userId);
//            var update = Builders<MovieList>.Update.PushEach(x => x.MoviesId, newMoviesID);

//            // Check if the document exists for the user
//            var existingDocument = await _movieListCollection.Find(filter).FirstOrDefaultAsync();

//            if (existingDocument != null)
//            {
//                // Update the existing document
//                var result = await _movieListCollection.UpdateOneAsync(filter, update);
//                return result.ModifiedCount > 0;
//            }
//            else
//            {
//                // Create a new document for the user
//                var newDocument = new MovieList
//                {
//                    UserId = userId,
//                    MoviesId = newMoviesID
//                };

//                await _movieListCollection.InsertOneAsync(newDocument);
//                return true;
//            }
//        }
//        public async Task<MovieList> GetUserMovieListAsync(string userId)
//        {
//            var filter = Builders<MovieList>.Filter.Eq(x => x.UserId, userId);
//            var userMovieList = await _movieListCollection.Find(filter).FirstOrDefaultAsync();

//            return userMovieList;
//        }
//    }
//}
