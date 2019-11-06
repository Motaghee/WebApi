using Common.Actions;
using Common.Models;
using LiteDB;
using System;
using System.Collections.Generic;

namespace Common.CacheManager
{
    public class ldbFetch
    {
        public static List<ProductStatistics> GetLiveLdbProductStatistics(string _Type)
        {
            try
            {
                //LogManager.SetCommonLog("GetLdbProductStatistics_request start");
                List<ProductStatistics> lstPS = new List<ProductStatistics>();
                // generate new statistic
                //List<ProductStatistics> lstNewPS = StatisticsActs.GetYearProdStatistics();
                // get instanse of ldb
                LiteDatabase db = new LiteDatabase(ldbConfig.ldbConnectionString);
                // get old ldb ps lst
                LiteCollection<ProductStatistics> dbPS = db.GetCollection<ProductStatistics>("ProductStatistics");
                // Get old lst
                if (dbPS.Count() != 0)
                {
                    foreach (var item in dbPS.Find(Query.EQ("DateIntervalType", _Type)))
                    {
                        lstPS.Add(item);
                    }
                }
                // insetr new lst
                //LogManager.SetCommonLog("GetLdbProductStatistics_result="+ dbPS.Count());
                return lstPS;
            }
            catch (Exception ex)
            {
                LogManager.SetCommonLog("GetLdbProductStatistics_" + ex.Message.ToString());
                return null;
            }
        }


        public static List<ProductStatistics> GetArchiveLdbProductStatistics()
        {
            try
            {
                LogManager.SetCommonLog("GetArchiveLdbProductStatistics start");
                List<ProductStatistics> lstPS = new List<ProductStatistics>();
                // generate new statistic
                //List<ProductStatistics> lstNewPS = StatisticsActs.GetYearProdStatistics();
                // get instanse of ldb
                LiteDatabase db = new LiteDatabase(ldbConfig.ldbArchiveConnectionString);
                // get old ldb ps lst
                LiteCollection<ProductStatistics> dbPS = db.GetCollection<ProductStatistics>("ProductStatistics");
                // Get old lst
                if (dbPS.Count() != 0)
                {
                    foreach (var item in dbPS.Find(Query.All()))
                    {
                        lstPS.Add(item);
                    }
                }
                // insetr new lst
                LogManager.SetCommonLog("GetArchiveLdbProductStatistics="+ dbPS.Count());
                return lstPS;
            }
            catch (Exception ex)
            {
                LogManager.SetCommonLog("GetArchiveLdbProductStatistics_Error_" + ex.Message.ToString());
                return null;
            }
        }

    }
}
