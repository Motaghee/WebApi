using Common.Actions;
using Common.db;
using Common.Models;
using Common.Models.Qccastt;
using Common.Utility;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;

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

        public static List<BodyModelProductStatistics> GetLiveLdbBodyModelProductStatistics(string _Type)
        {
            try
            {
                //LogManager.SetCommonLog("GetLdbProductStatistics_request start");
                List<BodyModelProductStatistics> lstPS = new List<BodyModelProductStatistics>();
                // generate new statistic
                //List<ProductStatistics> lstNewPS = StatisticsActs.GetYearProdStatistics();
                // get instanse of ldb
                LiteDatabase db = new LiteDatabase(ldbConfig.ldbConnectionString);
                // get old ldb ps lst
                LiteCollection<BodyModelProductStatistics> dbPS = db.GetCollection<BodyModelProductStatistics>("BodyModelProductStatistics");
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
                LogManager.SetCommonLog("GetLdbBodyModelProductStatistics_" + ex.Message.ToString());
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

        public static List<GroupProductStatistics> GetArchiveLdbGroupProductStatistics(int FromLastDay)
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
                List<GroupProductStatistics> lstPS = new List<GroupProductStatistics>();
                // generate new statistic
                //List<ProductStatistics> lstNewPS = StatisticsActs.GetYearProdStatistics();
                // get instanse of ldb
                LiteDatabase db = new LiteDatabase(ldbConfig.ldbArchiveConnectionString);
                // get old ldb ps lst
                LiteCollection<GroupProductStatistics> dbPS = db.GetCollection<GroupProductStatistics>("GroupProductStatistics");
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
                LogManager.SetCommonLog("GetArchiveLdbGroupProductStatistics" + ex.Message.ToString());
                return null;
            }
        }

        public static List<QCStatistics> GetArchiveQCStatistics(int FromLastDay)
        {
            try
            {
                string strFromLastDayCondition = CommonUtility.GetLastDateFa(FromLastDay);
                //LogManager.SetCommonLog("GetArchiveLdbProductStatistics start");
                List<QCStatistics> lstPS = new List<QCStatistics>();
                LiteDatabase db;
                if (FromLastDay==0)
                    db = new LiteDatabase(ldbConfig.ldbQCStatisticsConnectionString);
                else
                    db = new LiteDatabase(ldbConfig.ldbArchiveQCStatisticsConnectionString);
                // get old ldb ps lst
                LiteCollection<QCStatistics> dbPS = db.GetCollection<QCStatistics>("QCStatistics");
                // Get old lst
                if (dbPS.Count() != 0)
                {
                    foreach (var item in dbPS.Find(Query.GTE("CreatedDateFa", strFromLastDayCondition)))
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
                LogManager.SetCommonLog("GetArchiveQCStatistics_Error_" + ex.Message.ToString());
                return null;
            }
        }

        public static object GetQCAreaStatistics(int FromLastDay, BsonArray _AreaType)
        {
            try
            {
                string strNowdtFa = CommonUtility.GetNowDateTimeFa();
                //string strFromLastDayCondition = CommonUtility.GetLastDateFa(FromLastDay);
                LiteDatabase db;
                if (FromLastDay == 0)
                    db = new LiteDatabase(ldbConfig.ldbQCStatisticsConnectionString);
                else
                    db = new LiteDatabase(ldbConfig.ldbArchiveQCStatisticsConnectionString);
                // get old ldb ps lst
                LiteCollection<QCStatistics> dbPS = db.GetCollection<QCStatistics>("QCStatistics");
                //{
                //int i = 1;
                var data = dbPS
                    .Find(Query.In("AreaType", _AreaType))  //(Query.GTE("CreatedDateFa", strFromLastDayCondition))
                    //.Select(g => new { g.CreatedDateFa, g.AreaCode, g.AreaDesc, g.AreaType, g.ShopName, g.PTShopCode, g.QCAreat_Srl, g.DetectCarCount, g.DefCarCount, g.StrCarCount, g.RegDefCount, g.StrCount, g.ASPCarCount, g.ASPRegDefCount, g.BPlusCarCount, g.BPlusRegDefCount })
                    //.Where(a=>a.AreaType == _AreaType)
                    .GroupBy(g => new {  g.CreatedDateFaNum,
                                         g.AreaType, g.ShopCode, g.QCAreat_Srl,
                                        g.GrpCode,g.BdMdlCode  })
                    .Select(c => new
                    {
                        //Id = i++,
                        U_DateTimeFa = CommonUtility.GetNowTime().Substring(0,5),
                        c.Key.CreatedDateFaNum,
                        c.Key.ShopCode,
                        c.Key.GrpCode,c.Key.BdMdlCode,
                        c.Key.QCAreat_Srl,c.Key.AreaType,
                        DetectCarCount = c.Sum(x => x.DetectCarCount),
                        DefCarCount = c.Sum(x => x.DefCarCount),
                        StrCarCount = c.Sum(x => x.StrCarCount),
                        RegDefCount = c.Sum(x => x.RegDefCount),
                        StrCount = c.Sum(x => x.StrCount),
                        ASPCarCount = c.Sum(x => x.ASPCarCount),
                        ASPRegDefCount = c.Sum(x => x.ASPRegDefCount),
                        BPlusCarCount = c.Sum(x => x.BPlusCarCount),
                        BPlusRegDefCount = c.Sum(x => x.BPlusRegDefCount)
                    }).ToList();
                     return data;
            }
            catch (Exception ex)
            {
                LogManager.SetCommonLog("GetArchiveQCStatistics_Error_" + ex.Message.ToString());
                return null;
            }
        }

        public static object GetAuditStatistics(int FromLastDay, BsonArray _AreaCodes)
        {
            try
            {
                string strNowdtFa = CommonUtility.GetNowDateTimeFa();
                //string strFromLastDayCondition = CommonUtility.GetLastDateFa(FromLastDay);
                LiteDatabase db;
                if (FromLastDay == 0)
                    db = new LiteDatabase(ldbConfig.ldbAuditStatisticsConnectionString);
                else
                    db = new LiteDatabase(ldbConfig.ldbArchiveAuditStatisticsConnectionString);
                // get old ldb ps lst
                LiteCollection<AuditStatistics> dbPS = db.GetCollection<AuditStatistics>("AuditStatistics");
                //{
                //int i = 1;
                var data = dbPS
                    .Find(Query.In("AreaCode", _AreaCodes))  //(Query.GTE("CreatedDateFa", strFromLastDayCondition))
                                                             //.Select(g => new { g.CreatedDateFa, g.AreaCode, g.AreaDesc, g.AreaType, g.ShopName, g.PTShopCode, g.QCAreat_Srl, g.DetectCarCount, g.DefCarCount, g.StrCarCount, g.RegDefCount, g.StrCount, g.ASPCarCount, g.ASPRegDefCount, g.BPlusCarCount, g.BPlusRegDefCount })
                                                             //.Where(a=>a.AreaType == _AreaType)
                        .Select(c => new
                        {       
                            U_DateTimeFa = CommonUtility.GetNowTime().Substring(0, 5),
                            c.AreaCode,
                            c.AuditDateFaNum,
                            c.AuditDate_Fa,
                            c.QCAreat_Srl,
                            c.GrpCode,
                            c.BdMdlCode,
                            c.BdStlCode,
                            c.CountOfModuleDefect,
                            c.AuditCarCount,
                            c.CountOfADefect,
                            c.CountOfSDefect,
                            c.CountOfPDefect,
                            c.CountOfBDefect,
                            c.CountOfCDefect,
                            c.CountOfBPlusDefect,
                            c.SumOfNegativeScoreValue
                        })
                        .ToList();
                return data;
            }
            catch (Exception ex)
            {
                LogManager.SetCommonLog("GetArchiveAuditStatistics_Error_" + ex.Message.ToString());
                return null;
            }
        }

        public static object GetAuditStatisticsMDTrend(BsonArray _QCAreatSrls)
        {
            try
            {
                string strNowdtFa = CommonUtility.GetNowDateTimeFa();
                //string strFromLastDayCondition = CommonUtility.GetLastDateFa(FromLastDay);
                LiteDatabase db;
                db = new LiteDatabase(ldbConfig.ldbArchiveAuditStatisticsConnectionString);
                // get old ldb ps lst
                LiteCollection<AuditStatisticsMDTrend> dbPS = db.GetCollection<AuditStatisticsMDTrend>("AuditStatisticsMDTrend");
                //{
                //int i = 1;
                var data = dbPS
                    .Find(Query.In("QCAreat_Srl", _QCAreatSrls))  //(Query.GTE("CreatedDateFa", strFromLastDayCondition))
                                                             //.Select(g => new { g.CreatedDateFa, g.AreaCode, g.AreaDesc, g.AreaType, g.ShopName, g.PTShopCode, g.QCAreat_Srl, g.DetectCarCount, g.DefCarCount, g.StrCarCount, g.RegDefCount, g.StrCount, g.ASPCarCount, g.ASPRegDefCount, g.BPlusCarCount, g.BPlusRegDefCount })
                                                             //.Where(a=>a.AreaType == _AreaType)
                        .Select(c => new
                        {
                            U_DateTimeFa = CommonUtility.GetNowTime().Substring(0, 5),
                            c.YearWeekNo,
                            c.QCAreat_Srl,
                            c.QcMdult_Srl,
                            c.QcBadft_Srl,
                            c.GrpCode,
                            c.BdMdlCode,
                            c.BdStlCode,
                            c.WeekNScoreSum,
                            c.AuditCarCount,
                            c.AVGNScore,
                            c.MDRegR,
                            c.WeekDetectMDCount
                        })
                        .ToList();
                return data;
            }
            catch (Exception ex)
            {
                LogManager.SetCommonLog("GetArchiveAuditStatisticsMDTrend_Error_" + ex.Message.ToString());
                return null;
            }
        }


        public static object GetQCHAreaStatistics()
        {
            try
            {
                LiteDatabase db = new LiteDatabase(ldbConfig.ldbQCStatisticsConnectionString);
                // get old ldb ps lst
                LiteCollection<QCHStatistics> dbPS = db.GetCollection<QCHStatistics>("QCHStatistics");
                //{
                //int i = 1;
                var data = dbPS.FindAll();
                    
                return data;
            }
            catch (Exception ex)
            {
                LogManager.SetCommonLog("GetArchiveQCHStatistics_Error_" + ex.Message.ToString());
                return null;
            }
        }

        public static object GetTodayASPCarDef()
        {
            try
            {
                LiteDatabase db = new LiteDatabase(ldbConfig.ldbQccasttConnectionString);
                // get old ldb ps lst
                LiteCollection<Qccastt> dbPS = db.GetCollection<Qccastt>("QCCASTT");
                //{
                //int i = 1;
                var data = dbPS.FindAll().Select(c => new
                { c.Vin,c.CreatedBy,c.RepairedBy,c.QCMdult_Srl,c.QCBadft_Srl,c.CreatedDateFa,c.StrenghtDesc,c.IsRepaired,c.CreatedByDesc,c.RepairedByDesc,c.QCAreat_Srl,c.GrpCode,c.BdMdlCode,c.ShopCode,c.AreaType
                }).OrderByDescending(o => o.CreatedDateFa).ToList();

                return data;
            }
            catch (Exception ex)
            {
                LogManager.SetCommonLog("GetTodayASPCarDef_Error" + ex.Message.ToString());
                return null;
            }
        }

        public static object GetCarStatus(FilterCarStatus _filterCarStatus)
        {
            try
            {
                LiteDatabase db;
                db = new LiteDatabase(ldbConfig.ldbCarStatusConnectionString);
                // get old ldb ps lst
                LiteCollection<CarStatus> dbPS = db.GetCollection<CarStatus>("CarStatus");
                //{
                int QCount = 2;
                if (_filterCarStatus.CompanyCode != 0)
                    QCount++;
                if (_filterCarStatus.FinqcCode != 0)
                    QCount++;
                if (_filterCarStatus.Status_Code != -1)
                    QCount++;
                int i = 0;
                Query[] lstQ = new Query[QCount];
                //--
                lstQ[i] = Query.GTE("JoineryDateFaNum", _filterCarStatus.JoineryStartDateFaNum);
                i++;
                lstQ[i] = Query.LTE("JoineryDateFaNum", _filterCarStatus.JoineryEndDateFaNum);
                if (_filterCarStatus.CompanyCode != 0)
                {
                    i++;
                    lstQ[i] = Query.EQ("CompanyCode", _filterCarStatus.CompanyCode);
                }
                if (_filterCarStatus.FinqcCode != 0)
                {
                    i++;
                    lstQ[i] = Query.EQ("FinqcCode", _filterCarStatus.FinqcCode);
                }
                if (_filterCarStatus.Status_Code != -1)
                {
                    i++;
                    lstQ[i] = Query.EQ("Status_Code", _filterCarStatus.Status_Code);
                }
                var data = dbPS.Find(Query.And(lstQ)).Select(c => new
                  {//Id = i++,
                      c.CompanyCode,
                      c.FinqcCode,
                      c.JoineryDateFaNum,
                      c.Status_Code,
                      c.Vin
                  }).ToList();
                if (_filterCarStatus.AutomationSendPid != 0)
                {
                    DataTable dt = DBHelper.ToDataTable(dbPS.Find(Query.And(lstQ)).Select(c => new
                    { c.CompanyName, c.FinqcCode, c.JoineryDateFa, c.Status_Code, c.Vin }).OrderBy(o => o.CompanyName).ToList());
                    dt.Columns["CompanyName"].ColumnName = "شرکت";
                    dt.Columns["FinqcCode"].ColumnName = "وضعیت کیفی";
                    dt.Columns["Status_Code"].ColumnName = "وضعیت فروش";
                    dt.Columns["JoineryDateFa"].ColumnName = "تاریخ تجارتی";
                    dt.Columns["Vin"].ColumnName = "شماره شاسی";
                    //dt.DefaultView.Sort = "CompanyName";
                    string[] s = new string[1];
                    s[0] = _filterCarStatus.AutomationSendPid.ToString();
                    CommonUtility.SendAutomationAtachExcel("گزارش وضعیت خودرو", _filterCarStatus.AutomationContentDesc.Replace("🔍",Environment.NewLine+ "🔍"),s,s,dt,"CarStatus");
                    List<CarStatus>  lst = new List<CarStatus>();
                    CarStatus cs1 = new CarStatus();
                    cs1.Vin = "sent";
                    lst.Add(cs1);
                    return lst;
                }
                return data;

            }
            catch (Exception ex)
            {
                LogManager.SetCommonLog("GetArchiveQCStatistics_Error_" + ex.Message.ToString());
                return null;
            }
        }



        


    }

   

}
