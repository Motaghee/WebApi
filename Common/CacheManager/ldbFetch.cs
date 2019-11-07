using Common.Actions;
using Common.Models;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Globalization;

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


        public static List<ProductStatistics> GetArchiveLdbProductStatistics(int FromLastDay)
        {
            try
            {
                string Y, M, D;
                DateTime dtOldDay = DateTime.Now.AddDays(-FromLastDay);
                PersianCalendar pc = new PersianCalendar();
                Y = pc.GetYear(dtOldDay).ToString();
                M = pc.GetMonth(dtOldDay).ToString().PadLeft(2, '0');
                D = pc.GetDayOfMonth(dtOldDay).ToString().PadLeft(2, '0');
                string strFromLastDayCondition = Y + "/" + M + "/" + D;
                //LogManager.SetCommonLog("GetArchiveLdbProductStatistics start");
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
                    foreach (var item in dbPS.Find(Query.GT("ProdDateFa", strFromLastDayCondition)))
                    {
                        lstPS.Add(item);
                    }
                }
                // insetr new lst
                //LogManager.SetCommonLog("GetArchiveLdbProductStatistics="+ dbPS.Count());
                return lstPS;
            }
            catch (Exception ex)
            {
                LogManager.SetCommonLog("GetArchiveLdbProductStatistics_Error_" + ex.Message.ToString());
                return null;
            }
        }


        public static List<CompanyProductStatistics> GetArchiveLdbCompanyProductStatistics(int FromLastDay)
        {
            try
            {
                string Y, M, D;
                DateTime dtOldDay = DateTime.Now.AddDays(-FromLastDay);
                PersianCalendar pc = new PersianCalendar();
                Y = pc.GetYear(dtOldDay).ToString();
                M = pc.GetMonth(dtOldDay).ToString().PadLeft(2, '0');
                D = pc.GetDayOfMonth(dtOldDay).ToString().PadLeft(2, '0');
                string strFromLastDayCondition = Y + "/" + M + "/" + D;
                //LogManager.SetCommonLog("GetArchiveLdbProductStatistics start");
                List<CompanyProductStatistics> lstPS = new List<CompanyProductStatistics>();
                // generate new statistic
                //List<ProductStatistics> lstNewPS = StatisticsActs.GetYearProdStatistics();
                // get instanse of ldb
                LiteDatabase db = new LiteDatabase(ldbConfig.ldbArchiveConnectionString);
                // get old ldb ps lst
                LiteCollection<CompanyProductStatistics> dbPS = db.GetCollection<CompanyProductStatistics>("CompanyProductStatistics");
                // Get old lst
                if (dbPS.Count() != 0)
                {
                    foreach (var item in dbPS.Find(Query.GT("ProdDateFa", strFromLastDayCondition)))
                    {
                        lstPS.Add(item);
                    }
                }
                // insetr new lst
                //LogManager.SetCommonLog("GetArchiveLdbProductStatistics="+ dbPS.Count());
                return lstPS;
            }
            catch (Exception ex)
            {
                LogManager.SetCommonLog("GetArchiveLdbCompanyProductStatistics" + ex.Message.ToString());
                return null;
            }
        }

    }
}
