using System;
using System.IO;

namespace Common.Actions
{
    public class LogManager
    {
        static string WindowsServiceLogPath = @"D:\\WebApiLogs\\WSLogs.txt";
        static string CommonLogPath = @"D:\\WebApiLogs\\CommonLogs.txt";
        static string MethodCallLogPath = @"D:\\WebApiLogs\\MethodCallLog.txt";
        public static void SetWindowsServiceLog(String LogText)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(WindowsServiceLogPath, true))
                {
                    sw.WriteLine(LogText + " - " + DateTime.Now.ToString());
                }
            }
            catch (Exception ex)
            { }
        }
        public static void SetCommonLog(String LogText)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(CommonLogPath, true))
                {
                    sw.WriteLine(LogText + " - " + DateTime.Now.ToString());
                }
            }
            catch (Exception ex)
            { 
            }

        }

        public static void MethodCallLog(String LogText)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(MethodCallLogPath, true))
                {
                    sw.WriteLine(LogText + " - " + DateTime.Now.ToString());
                }
            }
            catch (Exception ex)
            { }

        }

    }
}
