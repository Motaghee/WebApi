using System;
using System.IO;

namespace Common.Actions
{
    public class LogManager
    {
        static string WindowsServiceLogPath = @"D:\\MobAppLogs\\WSLogs.txt";
        static string CommonLogPath = @"D:\\MobAppLogs\\CommonLogs.txt";
        public static void SetWindowsServiceLog(String LogText)
        {
            using (StreamWriter sw = new StreamWriter(WindowsServiceLogPath, true))
            {
                sw.WriteLine(LogText + " - " + DateTime.Now.ToString());
            }
        }
        public static void SetCommonLog(String LogText)
        {
            using (StreamWriter sw = new StreamWriter(CommonLogPath, true))
            {
                sw.WriteLine(LogText + " - " + DateTime.Now.ToString());
            }
        }

    }
}
