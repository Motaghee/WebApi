using Common.Actions;
using Common.CacheManager;
using System;
using System.Globalization;
using System.ServiceProcess;
using System.Timers;

namespace AWSCacheManager
{
    public partial class ldbCacheManager : ServiceBase
    {
        // - archive variables
        System.Timers.Timer _SetArchiveTimer;
        DateTime _scheduleTime;
        // -
        Timer PSCreatorTimer;
        Timer PSInitialTimer;
        Timer PSArchiveTimer;
        Timer QuickOnLineJob;

        public ldbCacheManager()
        {
            InitializeComponent();
            _SetArchiveTimer = new System.Timers.Timer();
            _scheduleTime = DateTime.Today.AddDays(1).AddHours(1) ; // Schedule to run once a day at 23:59:59 a.m.
        }

        string strArchiveRunDay="";
        protected override void OnStart(string[] args)
        {
            try
            {
                DateTime dtN = DateTime.Now;
                PersianCalendar pc = new PersianCalendar();
                string Y = pc.GetYear(dtN).ToString();
                string M = pc.GetMonth(dtN).ToString().PadLeft(2, '0');
                string D = pc.GetDayOfMonth(dtN).ToString().PadLeft(2, '0');
                string strNow = Y + M + D;
                LogManager.SetWindowsServiceLog("OnStart_Statrt WSMemoryCacheManager Service:strNow"+ strNow);
                PSInitialTimer = new Timer();
                PSInitialTimer.Interval = TimeSpan.FromSeconds(5).TotalMilliseconds; ;
                PSInitialTimer.Elapsed += new ElapsedEventHandler(this.OnPSInitialTimer);
                PSInitialTimer.Start();
                // ----
                QuickOnLineJob = new Timer();
                QuickOnLineJob.Interval = TimeSpan.FromMinutes(2).TotalMilliseconds; ;
                QuickOnLineJob.Elapsed += new ElapsedEventHandler(this.QuickOnLineTask);
                QuickOnLineJob.Start();

            }
            catch (Exception ex)
            {
                LogManager.SetWindowsServiceLog("OnStart_Statrt Error " + ex.Message);
            }
        }

        protected override void OnStop()
        {
            LogManager.SetWindowsServiceLog("OnStop_Stop WSMemoryCacheManager Service");
        }


        public void OnPSCreatorTimer(object sender, ElapsedEventArgs args)
        {
            try
            {
                //LogManager.SetWindowsServiceLog("OnPSCreatorTimer Start_On");
                // QCToday Statistic
                bool RefNonBrand = ldbRefresh.GenerateQCCaridDetailsNonBrand();
                bool RefAuditToday = ldbRefresh.RefreshLdbAuditStatistics(true);
                bool RefQCToday = ldbRefresh.RefreshLdbQCStatistics(true);
                bool RefQCHToday = ldbRefresh.RefreshLdbQCHStatistics();
                bool RefQccasttTodaye = ldbRefresh.RefreshLdbASPQCCASTT(true);
                // PS
                bool RefRsltY = ldbRefresh.RefreshLiveLdbProductStatistics("Y");
                bool RefRsltM = ldbRefresh.RefreshLiveLdbProductStatistics("M");
                bool RefRsltD = ldbRefresh.RefreshLiveLdbProductStatistics("D");
                bool RefRsltYD = ldbRefresh.RefreshLiveLdbProductStatistics("YD");
                bool RefRsltO30D = ldbRefresh.RefreshLiveLdbProductStatistics("O30D");
                bool RefRsltO90D = ldbRefresh.RefreshLiveLdbProductStatistics("O90D");
                bool RefRsltO180D = ldbRefresh.RefreshLiveLdbProductStatistics("O180D");
                // bodymodel PS
                bool RefRsltBMY = ldbRefresh.RefreshLiveLdbBodyModelProductStatistics("Y");
                bool RefRsltBMM = ldbRefresh.RefreshLiveLdbBodyModelProductStatistics("M");
                bool RefRsltBMD = ldbRefresh.RefreshLiveLdbBodyModelProductStatistics("D");
                bool RefRsltBMYD = ldbRefresh.RefreshLiveLdbBodyModelProductStatistics("YD");
                bool RefRsltBMO30D = ldbRefresh.RefreshLiveLdbBodyModelProductStatistics("O30D");
                bool RefRsltBMO90D = ldbRefresh.RefreshLiveLdbBodyModelProductStatistics("O90D");
                bool RefRsltBMO180D = ldbRefresh.RefreshLiveLdbBodyModelProductStatistics("O180D");
                //---
                bool RefCarSatus = ldbRefresh.RefreshLdbCarStatus();
                
                //MemoryCacher.Delete("O180DProductStatistics");
                //LogManager.SetCommonLog("OnPSCreatorTimer_request catch DeleteAll" );
                //LogManager.SetWindowsServiceLog("OnPSCreatorTimer_ result RefreshLdb.GenerateQCCaridDetailsNonBrand()Result=" + RefNonBrand.ToString());
            }
            catch (Exception ex)
            {
                LogManager.SetWindowsServiceLog("OnPSCreatorTimer" + ex.Message.ToString());
            }

        }

        public void OnPSInitialTimer(object sender, ElapsedEventArgs args)
        {
            try
            {
                DateTime dtN = DateTime.Now;
                PersianCalendar pc = new PersianCalendar();
                string Y = pc.GetYear(dtN).ToString();
                string M = pc.GetMonth(dtN).ToString().PadLeft(2, '0');
                string D = pc.GetDayOfMonth(dtN).ToString().PadLeft(2, '0');
                string NowDay = Y + M + D;
                strArchiveRunDay = NowDay;
                LogManager.SetWindowsServiceLog("Initialise LiveLdb Start NowDay is:" + NowDay);
                PSInitialTimer.Stop();
                bool RefQccasttTodaye = ldbRefresh.RefreshLdbASPQCCASTT(true);
                bool refreshNonBrand = ldbRefresh.GenerateQCCaridDetailsNonBrand();
                bool RefAuditToday = ldbRefresh.RefreshLdbAuditStatistics(true);
                bool RefQCToday = ldbRefresh.RefreshLdbQCStatistics(true);
                // --
                bool RefQCHToday = ldbRefresh.RefreshLdbQCHStatistics();
                bool RefRsltY = ldbRefresh.RefreshLiveLdbProductStatistics("Y");
                bool RefRsltM = ldbRefresh.RefreshLiveLdbProductStatistics("M");
                bool RefRsltD = ldbRefresh.RefreshLiveLdbProductStatistics("D");
                bool RefRsltYD = ldbRefresh.RefreshLiveLdbProductStatistics("YD");
                bool RefRsltO30D = ldbRefresh.RefreshLiveLdbProductStatistics("O30D");
                bool RefRsltO90D = ldbRefresh.RefreshLiveLdbProductStatistics("O90D");
                bool RefRsltO180D = ldbRefresh.RefreshLiveLdbProductStatistics("O180D");
                //LogManager.SetWindowsServiceLog("OnPSInitialTimer Initialise LiveLdb ProductStatistics cache db= " + RefRsltY + RefRsltM + RefRsltD + RefRsltYD + RefRsltO30D + RefRsltO90D + RefRsltO180D);
                bool RefRsltBMY = ldbRefresh.RefreshLiveLdbBodyModelProductStatistics("Y");
                bool RefRsltBMM = ldbRefresh.RefreshLiveLdbBodyModelProductStatistics("M");
                bool RefRsltBMD = ldbRefresh.RefreshLiveLdbBodyModelProductStatistics("D");
                bool RefRsltBMYD = ldbRefresh.RefreshLiveLdbBodyModelProductStatistics("YD");
                bool RefRsltBMO30D = ldbRefresh.RefreshLiveLdbBodyModelProductStatistics("O30D");
                bool RefRsltBMO90D = ldbRefresh.RefreshLiveLdbBodyModelProductStatistics("O90D");
                bool RefRsltBMO180D = ldbRefresh.RefreshLiveLdbBodyModelProductStatistics("O180D");
                //LogManager.SetWindowsServiceLog("OnPSInitialTimer Initialise LiveLdb BodyModelProductStatistics cache db= " + RefRsltBMY + RefRsltBMM + RefRsltBMD + RefRsltBMYD + RefRsltBMO30D + RefRsltBMO90D + RefRsltBMO180D);
                // bool RefRsltA = ldbRefresh.RefreshArchiveLdbProductStatistics();
                bool RefAuditArchive = ldbRefresh.RefreshLdbAuditStatistics(false);
                bool RefASPArchive = ldbRefresh.RefreshLdbASPQCCASTT(false);
                bool RefRsltAC = ldbRefresh.RefreshArchiveLdbCompanyProductStatistics();
                bool RefRsltAG = ldbRefresh.RefreshArchiveLdbGroupProductStatistics();
                bool RefQCArchive = ldbRefresh.RefreshLdbQCStatistics(false);
                //bool result = ldbRefresh.GenerateQCMdDPU();
                bool RefAuditMDTrendArchive = ldbRefresh.RefreshLdbAuditStatisticsMDTrend();
                LogManager.SetWindowsServiceLog("OnPSInitialTimer_finish archives result RefreshLdb.RefreshLdbProductStatistics()ResultQC=" + refreshNonBrand);
                bool RefCarSatus = ldbRefresh.RefreshLdbCarStatus();
                //LogManager.SetWindowsServiceLog("OnPSInitialTimer  Finished"+ RefCarSatus);
            }
            catch (Exception ex)
            {
                LogManager.SetWindowsServiceLog("OnPSInitialTimer" + ex.Message.ToString());
            }
            finally
            {
                //---
                PSCreatorTimer = new Timer();
                PSCreatorTimer.Interval = TimeSpan.FromMinutes(20).TotalMilliseconds;
                PSCreatorTimer.Elapsed += new ElapsedEventHandler(this.OnPSCreatorTimer);
                PSCreatorTimer.Start();

            }

        }

        public void OnArchivePS(object sender, ElapsedEventArgs args)
        {
            try
            {
                PSArchiveTimer.Stop();
                LogManager.SetWindowsServiceLog("start Archive Run");
                //bool RefRsltA = ldbRefresh.RefreshArchiveLdbProductStatistics();
                bool RefAuditArchive = ldbRefresh.RefreshLdbAuditStatistics(false);
                bool RefAuditMDTrendArchive = ldbRefresh.RefreshLdbAuditStatisticsMDTrend();
                bool RefRsltAC = ldbRefresh.RefreshArchiveLdbCompanyProductStatistics();
                bool RefRsltAG = ldbRefresh.RefreshArchiveLdbGroupProductStatistics();
                bool RefQCArchive=ldbRefresh.RefreshLdbQCStatistics(false);
                bool RefASPArchive = ldbRefresh.RefreshLdbASPQCCASTT(false);
                //bool result = ldbRefresh.GenerateQCMdDPU();

                LogManager.SetWindowsServiceLog("finish Archive new day cache db= " +  RefRsltAC + RefRsltAG+ RefQCArchive+ RefAuditArchive+ RefAuditMDTrendArchive);
            }
            catch (Exception ex)
            {
                LogManager.SetWindowsServiceLog("OnArchivePS_" + ex.Message.ToString());
            }

        }


        public void QuickOnLineTask(object sender, ElapsedEventArgs args)
        {
            string trace = "0";
            bool RefQuickNewVinOnlineSync = false;
            try
            {
                //run every 1 min 
                RefQuickNewVinOnlineSync = ldbRefresh.GenerateQCCaridDetailsOnlineSync(1);
                trace += "1";
                bool RefQccasttArchive = ldbRefresh.RefreshLdbASPQCCASTT(true);
                //trace += "2->"+ RefQccasttArchive.ToString();
                //LogManager.SetWindowsServiceLog("OnArchivePS cache db= " +  RefRsltAC + RefRsltAG);
                DateTime dtN = DateTime.Now;
                PersianCalendar pc = new PersianCalendar();
                string Y = pc.GetYear(dtN).ToString();
                string M = pc.GetMonth(dtN).ToString().PadLeft(2, '0');
                string D = pc.GetDayOfMonth(dtN).ToString().PadLeft(2, '0');
                string NowDay = Y + M + D;
                trace += "3";
                if (NowDay!=strArchiveRunDay)
                {
                    trace += "4";
                    strArchiveRunDay = NowDay;
                    PSArchiveTimer = new Timer();
                    PSArchiveTimer.Interval = TimeSpan.FromSeconds(5).TotalMilliseconds; ;
                    PSArchiveTimer.Elapsed += new ElapsedEventHandler(this.OnArchivePS);
                    PSArchiveTimer.Start();
                    trace += "5";
                }
            }
            catch (Exception ex)
            {
                LogManager.SetWindowsServiceLog("QuickOnLineTask_"+ trace+ "_RefQuickNewVinOnlineSync:"+ RefQuickNewVinOnlineSync + "_" + ex.Message.ToString());
            }

        }


    }
}
