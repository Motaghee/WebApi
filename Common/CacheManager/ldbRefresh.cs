using Common.Actions;
using Common.Models;
using Common.db;
using LiteDB;
using System;
using System.Collections.Generic;
using Common.Models.Qccastt;
using System.Globalization;
using Common.Utility;
using Common.Models.General;
using System.Data;
using Oracle.ManagedDataAccess.Client;

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
                if ((lstNewPS != null) && (lstNewPS.Count != 0))
                {
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
                return false;
            }
            catch (Exception ex)
            {
                LogManager.SetCommonLog("RefreshLdbProductStatistics_" + ex.Message.ToString());
                return false;
            }
        }

        public static bool RefreshLiveLdbBodyModelProductStatistics(string _Type)
        {
            try
            {
                //LogManager.SetCommonLog("RefreshLdbProductStatistics_ starting..." );
                // generate new statistic
                List<BodyModelProductStatistics> lstNewPS = StatisticsActs.GetLiveBodyModelProdStatistics(_Type);
                if ((lstNewPS != null) && (lstNewPS.Count != 0))
                {

                    // get instanse of ldb
                    LiteDatabase db = new LiteDatabase(ldbConfig.ldbConnectionString);
                    // get old ldb ps lst
                    LiteCollection<BodyModelProductStatistics> dbPS = db.GetCollection<BodyModelProductStatistics>("BodyModelProductStatistics");
                    // delete old lst
                    dbPS.Delete(Query.EQ("DateIntervalType", _Type));
                    // insetr new lst
                    dbPS.Insert(lstNewPS);
                    //LogManager.SetCommonLog("RefreshLdbProductStatistics_ insert successfully" + lstNewPS.Count);
                    db.Dispose();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                //DBHelper.LogFile(ex);
                LogManager.SetCommonLog("RefreshLdbBodyModelProductStatistics_" + ex.Message.ToString());
                return false;
            }
        }

        public static bool RefreshArchiveLdbCompanyProductStatistics()
        {
            try
            {
                string strToday = CommonUtility.GetNowDateFaNum();
                ldbUpdStatus l = ldbRefresh.GetLdbUpdateStatus();
                if (l.ArchiveCompanyProductStatistics_UDate != strToday)
                {
                    //LogManager.SetCommonLog("RefreshLdbProductStatistics_ starting..." );
                    // generate new statistic
                    List<CompanyProductStatistics> lstNewPS = StatisticsActs.GetArchiveCompanyProdStatistics();
                    // get instanse of ldb
                    LiteDatabase db = new LiteDatabase(ldbConfig.ldbArchiveConnectionString);
                    // get old ldb ps lst
                    LiteCollection<CompanyProductStatistics> dbPS = db.GetCollection<CompanyProductStatistics>("CompanyProductStatistics");
                    // delete old lst
                    dbPS.Delete(Query.All());
                    // insetr new lst
                    dbPS.Insert(lstNewPS);
                    //LogManager.SetCommonLog("RefreshLdbCompanyProductStatistics_ insert successfully" + lstNewPS.Count);
                    db.Dispose();
                    l.ArchiveCompanyProductStatistics_UDate = strToday;
                    bool result = ldbRefresh.SetLdbUpdateStatus(l);
                    return true;
                }else
                    return true;
            }
            catch (Exception ex)
            {
                LogManager.SetCommonLog("RefreshArchiveLdbCompanyProductStatistics" + ex.Message.ToString());
                return false;
            }
        }

        public static bool RefreshArchiveLdbGroupProductStatistics()
        {
            try
            {
                string strToday = CommonUtility.GetNowDateFaNum();
                ldbUpdStatus l = ldbRefresh.GetLdbUpdateStatus();
                if (l.ArchiveGroupProductStatistics_UDate != strToday)
                {
                    //LogManager.SetCommonLog("RefreshLdbProductStatistics_ starting..." );
                    // generate new statistic
                    List<GroupProductStatistics> lstNewPS = StatisticsActs.GetArchiveGroupProdStatistics();
                // get instanse of ldb
                LiteDatabase db = new LiteDatabase(ldbConfig.ldbArchiveConnectionString);
                // get old ldb ps lst
                LiteCollection<GroupProductStatistics> dbPS = db.GetCollection<GroupProductStatistics>("GroupProductStatistics");
                // delete old lst
                dbPS.Delete(Query.All());
                // insetr new lst
                dbPS.Insert(lstNewPS);
                //LogManager.SetCommonLog("RefreshLdbGroupProductStatistics_ insert successfully" + lstNewPS.Count);
                db.Dispose();
                l.ArchiveGroupProductStatistics_UDate = strToday;
                bool result = ldbRefresh.SetLdbUpdateStatus(l);
                return true;
            }else
                return true;
        }
            catch (Exception ex)
            {
                LogManager.SetCommonLog("RefreshArchiveLdbGroupProductStatistics" + ex.Message.ToString());
                return false;
            }
        }

        public static bool RefreshLdbQCStatistics(bool JustTodayStatistics)
        {
            try
            {
                string strToday = CommonUtility.GetNowDateFaNum();
                ldbUpdStatus l = ldbRefresh.GetLdbUpdateStatus();
                if ((!JustTodayStatistics) &&(l.ArchiveQCStatistics_UDate == strToday))
                {
                    return true;
                }
                //LogManager.SetCommonLog("RefreshLdbQCStatistics starting... just today="+ JustTodayStatistics);
                // generate new statistic
                List<QCStatistics> lstNewPS = StatisticsActs.GetArchiveQCStatistics(JustTodayStatistics);
                // get instanse of ldb
                LiteDatabase db;
                if (JustTodayStatistics)
                    db = new LiteDatabase(ldbConfig.ldbQCStatisticsConnectionString);
                else
                    db = new LiteDatabase(ldbConfig.ldbArchiveQCStatisticsConnectionString);
                // get old ldb ps lst
                LiteCollection<QCStatistics> dbPS = db.GetCollection<QCStatistics>("QCStatistics");
                // delete old lst
                dbPS.Delete(Query.All());
                // insetr new lst
                dbPS.Insert(lstNewPS);
                //LogManager.SetCommonLog("RefreshLdbQCStatistics insert successfully" + lstNewPS.Count);
                db.Dispose();
                if (!JustTodayStatistics)
                {
                    l.ArchiveQCStatistics_UDate = strToday;
                    bool result = ldbRefresh.SetLdbUpdateStatus(l);
                }
                return true;
            }
            catch (Exception ex)
            {
                LogManager.SetCommonLog("RefreshArchiveLdbQCStatistics_err:" + ex.Message.ToString());
                return false;
            }
        }

        public static bool RefreshLdbAuditStatistics(bool JustTodayStatistics)
        {
            try
            {
                string strToday = CommonUtility.GetNowDateFaNum();
                ldbUpdStatus l = ldbRefresh.GetLdbUpdateStatus();
                if ((!JustTodayStatistics) && (l.ArchiveAuditStatistics_UDate == strToday))
                {
                    return true;
                }
                //LogManager.SetCommonLog("RefreshLdbQCStatistics starting... just today="+ JustTodayStatistics);
                // generate new statistic
                List<AuditStatistics> lstNewPS = StatisticsActs.GetArchiveAuditStatistics(JustTodayStatistics,"1000,1001,1002,1003");
                // get instanse of ldb
                LiteDatabase db;
                if (JustTodayStatistics)
                    db = new LiteDatabase(ldbConfig.ldbAuditStatisticsConnectionString);
                else
                    db = new LiteDatabase(ldbConfig.ldbArchiveAuditStatisticsConnectionString);
                // get old ldb ps lst
                LiteCollection<AuditStatistics> dbPS = db.GetCollection<AuditStatistics>("AuditStatistics");
                // delete old lst
                dbPS.Delete(Query.All());
                // insetr new lst
                dbPS.Insert(lstNewPS);
                //LogManager.SetCommonLog("RefreshLdbQCStatistics insert successfully" + lstNewPS.Count);
                db.Dispose();
                if (!JustTodayStatistics)
                {
                    l.ArchiveAuditStatistics_UDate = strToday;
                    bool result = ldbRefresh.SetLdbUpdateStatus(l);
                }
                return true;
            }
            catch (Exception ex)
            {
                LogManager.SetCommonLog("RefreshArchiveLdbAuditStatistics_err:" + ex.Message.ToString());
                return false;
            }
        }

        public static bool RefreshLdbAuditStatisticsMDTrend()
        {
            try
            {
                string strToday = CommonUtility.GetNowDateFaNum();
                ldbUpdStatus l = ldbRefresh.GetLdbUpdateStatus();
                //if (l.ArchiveAuditStatisticsMDTrend_UDate == strToday)
                //{
                //    return true;
                //}
                //LogManager.SetCommonLog("RefreshLdbQCStatistics starting... just today="+ JustTodayStatistics);
                // generate new statistic
                List<AuditStatisticsMDTrend> lstNewPS = StatisticsActs.GetArchiveMDTrendAuditStatistics("841,961,962,963");
                if (lstNewPS == null)
                {
                    // get instanse of ldb
                    LiteDatabase db;
                    db = new LiteDatabase(ldbConfig.ldbArchiveAuditStatisticsConnectionString);
                    // get old ldb ps lst
                    LiteCollection<AuditStatisticsMDTrend> dbPS = db.GetCollection<AuditStatisticsMDTrend>("AuditStatisticsMDTrend");
                    // delete old lst
                    dbPS.Delete(Query.All());
                    // insetr new lst
                    dbPS.Insert(lstNewPS);
                    //LogManager.SetCommonLog("RefreshLdbQCStatistics insert successfully" + lstNewPS.Count);
                    db.Dispose();
                    l.ArchiveAuditStatisticsMDTrend_UDate = strToday;
                    bool result = ldbRefresh.SetLdbUpdateStatus(l);
                }
                return true;
            }
            catch (Exception ex)
            {
                LogManager.SetCommonLog("RefreshLdbAuditStatisticsMDTrend:" + ex.Message.ToString());
                return false;
            }
        }

        public static bool RefreshLdbASPQCCASTT(bool JustTodayStatistics)
        {
            int trc = 1;
            try
            {
                string strToday = CommonUtility.GetNowDateFaNum();
                trc = 2;
                ldbUpdStatus l = ldbRefresh.GetLdbUpdateStatus();
                trc = 3;
                if ((!JustTodayStatistics) && (l.ArchiveASPQCCASTT == strToday))
                {
                    trc = 4;
                    return true;
                }
                trc = 5;
                List<Qccastt> lstNewPS = StatisticsActs.GetASPQccastt(JustTodayStatistics);
                trc = 6;
                // get instanse of ldb
                LiteDatabase db;
                trc = 7;
                if (JustTodayStatistics)
                    db = new LiteDatabase(ldbConfig.ldbQccasttConnectionString);
                else
                    db = new LiteDatabase(ldbConfig.ldbQCArchiveQccasttConnectionString);
                trc = 8;
                // get old ldb ps lst
                LiteCollection<Qccastt> dbPS = db.GetCollection<Qccastt>("QCCASTT");
                // delete old lst
                dbPS.Delete(Query.All());
                // insetr new lst
                dbPS.Insert(lstNewPS);
                trc = 9;
                //LogManager.SetCommonLog("RefreshLdbQCStatistics insert successfully" + lstNewPS.Count);
                db.Dispose();
                trc = 10;
                if (!JustTodayStatistics)
                {
                    trc = 10;
                    l.ArchiveASPQCCASTT = strToday;
                    bool result = ldbRefresh.SetLdbUpdateStatus(l);
                    trc = 11;
                }
                trc = 12;
                return true;
            }
            catch (Exception ex)
            {
                LogManager.SetCommonLog(trc.ToString()+"RefreshLdbASPQCCASTT_Err:" + JustTodayStatistics + ex.Message.ToString());
                return false;
            }
        }

        public static bool RefreshLdbQCHStatistics()
        {
            try
            {
                //LogManager.SetCommonLog("RefreshLdbQCStatistics starting... just today="+ JustTodayStatistics);
                // generate new statistic
                List<QCHStatistics> lstNewPS = StatisticsActs.GetHQCStatistics();
                // get instanse of ldb
                LiteDatabase db= new LiteDatabase(ldbConfig.ldbQCStatisticsConnectionString); 
                
                // get old ldb ps lst
                LiteCollection<QCHStatistics> dbPS = db.GetCollection<QCHStatistics>("QCHStatistics");
                // delete old lst
                dbPS.Delete(Query.All());
                // insetr new lst
                dbPS.Insert(lstNewPS);
                //LogManager.SetCommonLog("RefreshLdbQCStatistics insert successfully" + lstNewPS.Count);
                db.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                LogManager.SetCommonLog("RefreshLdbQCHStatistics_err:" + ex.Message.ToString());
                return false;
            }
        }

        public static bool RefreshLdbCarStatus()
        {
            try
            {
                //LogManager.SetCommonLog("RefreshLdbQCStatistics starting... just today="+ JustTodayStatistics);
                // generate new statistic
                List<CarStatus> lstNew = StatisticsActs.GetCarStatus();
                // get instanse of ldb
                LiteDatabase db;
                db = new LiteDatabase(ldbConfig.ldbCarStatusConnectionString);
                // get old ldb ps lst
                LiteCollection<CarStatus> dbPS = db.GetCollection<CarStatus>("CarStatus");
                // delete old lst
                dbPS.Delete(Query.All());
                // insetr new lst
                dbPS.Insert(lstNew);
                //LogManager.SetCommonLog("RefreshLdbQCStatistics insert successfully" + lstNewPS.Count);
                db.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                LogManager.SetCommonLog("RefreshLdbCarStatus_err:" + ex.Message.ToString());
                return false;
            }
        }

        public static ldbUpdStatus GetLdbUpdateStatus()
        {
            try
            {
                LiteDatabase db;
                db = new LiteDatabase(ldbConfig.ldbUpdateStatusConnectionString);
                // get old ldb ps lst
                LiteCollection<ldbUpdStatus> result = db.GetCollection<ldbUpdStatus>("ldbUpdStatus");
                if ((result != null) && (result.Count() != 0))
                {
                    ldbUpdStatus First = new ldbUpdStatus();
                    foreach (var item in result.FindAll())
                    {
                        return item;
                    }
                    ldbUpdStatus lus = new ldbUpdStatus();
                    lus.Id = "1";
                    return lus;
                }
                else
                {
                    ldbUpdStatus lus = new ldbUpdStatus();
                    lus.Id = "1";
                    return lus;
                }
                //db.Dispose();
            }
            catch (Exception ex)
            {
                LogManager.SetCommonLog("GetLdbUpdateStatus:" + ex.Message.ToString());
                return null;
            }
        }

        public static bool SetLdbUpdateStatus(ldbUpdStatus _ldbupdStause)
        {
            try
            {
                string strNow = CommonUtility.GetNowDateFaNum();
                LiteDatabase db;
                db = new LiteDatabase(ldbConfig.ldbUpdateStatusConnectionString);
                // get old ldb ps lst
                LiteCollection<ldbUpdStatus> result = db.GetCollection<ldbUpdStatus>("ldbUpdStatus");
                result.Delete(Query.All());
                result.Insert(_ldbupdStause);
                db.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                LogManager.SetCommonLog("SetLdbUpdateStatus:" + ex.Message.ToString());
                return false;
            }
        }

        public static bool GenerateQCMdDPU()
        {
            try
            {
                string strToday = CommonUtility.GetNowDateFaNum();
                ldbUpdStatus l = ldbRefresh.GetLdbUpdateStatus();
                if (l.ArchiveGenerateQCmdDPU == strToday)
                {
                    return true;
                }
                if (DBHelper.DBConnectionIns.State == ConnectionState.Open)
                    DBHelper.DBConnectionIns.Close();
                if (DBHelper.DBConnectionIns.State == ConnectionState.Closed)
                {
                    DBHelper.DBConnectionIns.ConnectionString = DBHelper.CnStrInsLive;
                    DBHelper.DBConnectionIns.Open();
                }
                OracleCommand cmd = new OracleCommand();
                OracleDataAdapter da = new OracleDataAdapter();
                cmd.Connection = DBHelper.DBConnectionIns;
                da.SelectCommand = cmd;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_GenerateQCmdDPU";
                cmd.ExecuteNonQuery();
                l.ArchiveGenerateQCmdDPU = strToday;
                bool result = ldbRefresh.SetLdbUpdateStatus(l);
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                DBHelper.DBConnectionIns.Close();
            }

        }

        public static bool GenerateQCCaridDetailsNonBrand()
        {
            try
            {
                OracleCommand cmd = new OracleCommand();
                OracleDataAdapter da = new OracleDataAdapter();
                cmd.Connection = DBHelper.LiveDBConnectionIns;
                if (DBHelper.LiveDBConnectionIns.State != ConnectionState.Open)
                    DBHelper.LiveDBConnectionIns.Open();
                da.SelectCommand = cmd;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_GenerateQCCaridDetailsNonBrand";
                cmd.ExecuteNonQuery();
                //l.ArchiveGenerateQCmdDPU = strToday;
                //bool result = ldbRefresh.SetLdbUpdateStatus(l);
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                DBHelper.DBConnectionIns.Close();
            }

        }

        public static bool GenerateQCCaridDetailsOnlineSync(int pQuickNewVinSync)
        {
            try
            {
                OracleCommand cmd = new OracleCommand();
                OracleDataAdapter da = new OracleDataAdapter();
                cmd.Connection = DBHelper.LiveDBConnectionIns;
                if (DBHelper.LiveDBConnectionIns.State != ConnectionState.Open)
                    DBHelper.LiveDBConnectionIns.Open();
                da.SelectCommand = cmd;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_GenerateQCCaridDetailsOnlineSync";
                cmd.Parameters.Add(new OracleParameter {
                    OracleDbType = OracleDbType.Int32,
                    Direction = ParameterDirection.Input,
                    ParameterName = "pQuickNewVinSync",
                    Value = pQuickNewVinSync
                });
                cmd.ExecuteNonQuery();
                //l.ArchiveGenerateQCmdDPU = strToday;
                //bool result = ldbRefresh.SetLdbUpdateStatus(l);
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                DBHelper.DBConnectionIns.Close();
            }

        }
    }
}

