using LiteDB;

namespace Common.CacheManager
{
    public class ldbConfig
    {
        public static ConnectionString ldbConnectionString = new ConnectionString(@"C:\MobAppCache\ProductStatisticsCache.bdl")
        {
            Password = "H@med110Rez@"
        };

        public static ConnectionString ldbArchiveConnectionString = new ConnectionString(@"C:\MobAppCache\ProductStatisticsArchiveCache.bdl")
        {
            Password = "H@med110Rez@"
        };

        public static ConnectionString ldbQCStatisticsConnectionString = new ConnectionString(@"C:\MobAppCache\QCStatisticsTodayCache.bdl")
        {
            Password = "H@med110Rez@"
        };
        public static ConnectionString ldbArchiveQCStatisticsConnectionString = new ConnectionString(@"C:\MobAppCache\QCStatisticsArchiveCache.bdl")
        {
            Password = "H@med110Rez@"
        };
        public static ConnectionString ldbAuditStatisticsConnectionString = new ConnectionString(@"C:\MobAppCache\AuditStatisticsTodayCache.bdl")
        {
            Password = "H@med110Rez@"
        };
        public static ConnectionString ldbArchiveAuditStatisticsConnectionString = new ConnectionString(@"C:\MobAppCache\AuditStatisticsArchiveCache.bdl")
        {
            Password = "H@med110Rez@"
        };



        public static ConnectionString ldbCarStatusConnectionString = new ConnectionString(@"C:\MobAppCache\CarStatusCache.bdl")
        {
            Password = "H@med110Rez@"
        };
        public static ConnectionString ldbQccasttConnectionString = new ConnectionString(@"C:\MobAppCache\ASPQccasttCache.bdl")
        {
            Password = "H@med110Rez@"
        };
        public static ConnectionString ldbQCArchiveQccasttConnectionString = new ConnectionString(@"C:\MobAppCache\ASPQccasttArchiveCache.bdl")
        {
            Password = "H@med110Rez@"
        };
        public static ConnectionString ldbUpdateStatusConnectionString = new ConnectionString(@"C:\MobAppCache\ldbUpdStatus.bdl")
        {
            Password = "H@med110Rez@"
        };
        public static ConnectionString ldbUserConnectionString = new ConnectionString(@"C:\MobAppCache\users\x.bdl")
        {
            Password = "H@med110Rez@"
        };

        public static ConnectionString ldbOnlineUsersConnectionString = new ConnectionString(@"C:\MobAppCache\OnlineUsers.bdl")
        {
            Password = "H@med110Rez@"
        };

        public static ConnectionString ldbSMessageConnectionString = new ConnectionString(@"C:\MobAppCache\SMessage.bdl")
        {
            Password = "H@med110Rez@"
        };

        public static ConnectionString GetUserConnectionString(string _userid)
        {
            string path = string.Format(@"C:\MobAppCache\users\{0}.bdl", _userid);
            ConnectionString ldbUserConnectionString = new ConnectionString(path);
            ldbUserConnectionString.Password = "H@med110Rez@";
            return ldbUserConnectionString;
        }



    }
}
