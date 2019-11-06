using LiteDB;

namespace Common.CacheManager
{
    class ldbConfig
    {
        public static ConnectionString ldbConnectionString = new ConnectionString(@"C:\MobAppCache\Cache.bdl")
        {
            Password = "H@med110Rez@"
        };

        public static ConnectionString ldbArchiveConnectionString = new ConnectionString(@"C:\MobAppCache\ArchiveCache.bdl")
        {
            Password = "H@med110Rez@"
        };

    }
}
