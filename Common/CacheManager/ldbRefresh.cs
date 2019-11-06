using Common.Actions;
using Common.Models;
using LiteDB;
using System;
using System.Collections.Generic;

namespace Common.CacheManager
{
    public class ldbRefresh
    {

        public static bool RefreshLiveLdbProductStatistics(string _Type)
        {
            try
            {
                //LogManager.SetCommonLog("RefreshLdbProductStatistics_ starting..." );
                // generate new statistic
                List<ProductStatistics> lstNewPS = StatisticsActs.GetLiveProdStatistics(_Type);
                // get instanse of ldb
                LiteDatabase db = new LiteDatabase(ldbConfig.ldbConnectionString);
                // get old ldb ps lst
                LiteCollection<ProductStatistics> dbPS = db.GetCollection<ProductStatistics>("ProductStatistics");
                // delete old lst
                dbPS.Delete(Query.EQ("DateIntervalType", _Type));
                // insetr new lst
                dbPS.Insert(lstNewPS);
                //LogManager.SetCommonLog("RefreshLdbProductStatistics_ insert successfully" + lstNewPS.Count);
                db.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                LogManager.SetCommonLog("RefreshLdbProductStatistics_" + ex.Message.ToString());
                return false;
            }
        }

        public static bool RefreshArchiveLdbProductStatistics()
        {
            try
            {
                //LogManager.SetCommonLog("RefreshLdbProductStatistics_ starting..." );
                // generate new statistic
                List<ProductStatistics> lstNewPS = StatisticsActs.GetArchiveProdStatistics();
                // get instanse of ldb
                LiteDatabase db = new LiteDatabase(ldbConfig.ldbArchiveConnectionString);
                // get old ldb ps lst
                LiteCollection<ProductStatistics> dbPS = db.GetCollection<ProductStatistics>("ProductStatistics");
                // delete old lst
                dbPS.Delete(Query.All());
                // insetr new lst
                dbPS.Insert(lstNewPS);
                //LogManager.SetCommonLog("RefreshLdbProductStatistics_ insert successfully" + lstNewPS.Count);
                db.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                LogManager.SetCommonLog("RefreshLdbProductStatistics_" + ex.Message.ToString());
                return false;
            }
        }
    }
}

