
using System.ServiceProcess;

namespace AWSCacheManager
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new ldbCacheManager()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
