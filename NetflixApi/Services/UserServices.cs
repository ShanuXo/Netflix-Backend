using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoWithDotNetAPI.DAta;
using NetflixApi.Model;

namespace NetflixApi.Services
{
    public class UserServices
    {
       
        private readonly IMongoCollection<LocalUsers> _userCollection;
        public UserServices(IOptions<UserDataContext> userDataContext)
        {
            var mongoClient = new MongoClient(userDataContext.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(userDataContext.Value.DatabaseName);
            _userCollection = mongoDatabase.GetCollection<LocalUsers>(userDataContext.Value.UserCollectionName);

        }


        /*public UserServices(IOptions<UserDataContext> _userDataContext)
        {
            //_configuration = configuration;
            var connectionString = _userDataContext.GetConnectionString("MoviesandSeriesDatabase:ConnectionString");
          *//*  if (connectionString == null)
            {
                throw new ArgumentNullException(nameof(connectionString), "Connection string is missing in appsettings.json.");
            }*//*

            var databaseName = _configuration["MoviesandSeriesDatabase:DatabaseName"];
            var collectionName = _configuration["MoviesandSeriesDatabase:UserCollectionName"];

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _userCollection = database.GetCollection<LocalUsers>(collectionName);
        }*/

        //user registration this is to register individual user 
        public async Task<LocalUsers> SignupAsync(RegistrationRequest newUser)
        {     //mapping the requried model with local users
            LocalUsers user = new LocalUsers()
            {
                Email = newUser.Email,
                Password = newUser.Password,

            };
            await _userCollection.InsertOneAsync(user);

            return user;
        }

        // User Login Find a user by username and password 
        public async Task<LocalUsers?> LoginAsync(string username, string password)

        {

            var user = await _userCollection.Find(x => x.Email == username && x.Password == password).FirstOrDefaultAsync();

            return user;
        }
        public async Task<LocalUsers> GetAsync(string username)
        {
            // Define a filter to search for the user by username
            var filter = Builders<LocalUsers>.Filter.Eq(x => x.Email, username);

            try
            {
                // Use the MongoDB collection to find the user matching the filter
                var user = await _userCollection.Find(filter).FirstOrDefaultAsync();

                return user; // Return the user if found, or null if not found
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
