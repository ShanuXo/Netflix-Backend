namespace MongoWithDotNetAPI.DAta
{
    public class UserDataContext
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string UserCollectionName { get; set; } = null!;
        public string SubscriptionCollectionName { get; set; } = null!;
        public string MovieListCollectionName { get; set; } = null!;
        public string VideoCollectionName { get; set; } = null!;
    }

}
