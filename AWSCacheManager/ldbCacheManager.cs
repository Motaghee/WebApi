using Common.Actions;
using Common.CacheManager;
using System;
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

        public ldbCacheManager()
        {
            InitializeComponent();
            _SetArchiveTimer = new System.Timers.Timer();
            _scheduleTime = DateTime.Today.AddDays(1).AddHours(4); // Schedule to run once a day at 7:00 a.m.
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                LogManager.SetWindowsServiceLog("OnStart_Statrt WSMemoryCacheManager Service");
                PSInitialTimer = new Timer();
                PSInitialTimer.Interval = 5000;
                PSInitialTimer.Elapsed += new ElapsedEventHandler(this.OnPSInitialTimer);
                PSInitialTimer.Start();
                //---
                PSCreatorTimer = new Timer();
                PSCreatorTimer.Interval = 900000; //15 Min
                PSCreatorTimer.Elapsed += new ElapsedEventHandler(this.OnPSCreatorTimer);
                PSCreatorTimer.Start();
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
                bool RefRsltY = ldbRefresh.RefreshLiveLdbProductStatistics("Y");
                bool RefRsltM = ldbRefresh.RefreshLiveLdbProductStatistics("M");
                bool RefRsltD = ldbRefresh.RefreshLiveLdbProductStatistics("D");
                bool RefRsltYD = ldbRefresh.RefreshLiveLdbProductStatistics("YD");
                bool RefRsltO30D = ldbRefresh.RefreshLiveLdbProductStatistics("O30D");
                bool RefRsltO90D = ldbRefresh.RefreshLiveLdbProductStatistics("O90D");
                bool RefRsltO180D = ldbRefresh.RefreshLiveLdbProductStatistics("O180D");

                //LogManager.SetWindowsServiceLog("OnPSCreatorTimer_ result RefreshLdb.RefreshLdbProductStatistics()Result=" + RefRslt.ToString());
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
                PSInitialTimer.Stop();
                bool RefRsltY = ldbRefresh.RefreshLiveLdbProductStatistics("Y");
                bool RefRsltM = ldbRefresh.RefreshLiveLdbProductStatistics("M");
                bool RefRsltD = ldbRefresh.RefreshLiveLdbProductStatistics("D");
                bool RefRsltYD = ldbRefresh.RefreshLiveLdbProductStatistics("YD");
                bool RefRsltO30D = ldbRefresh.RefreshLiveLdbProductStatistics("O30D");
                bool RefRsltO90D = ldbRefresh.RefreshLiveLdbProductStatistics("O90D");
                bool RefRsltO180D = ldbRefresh.RefreshLiveLdbProductStatistics("O180D");
                bool RefRsltA = ldbRefresh.RefreshArchiveLdbProductStatistics();
                LogManager.SetWindowsServiceLog("OnPSInitialTimer Initialise cache db= " + RefRsltY + RefRsltM + RefRsltD + RefRsltYD + RefRsltO30D + RefRsltO90D + RefRsltO180D + RefRsltA);
                //LogManager.SetWindowsServiceLog("OnPSCreatorTimer_ result RefreshLdb.RefreshLdbProductStatistics()Result=" + RefRslt.ToString());
            }
            catch (Exception ex)
            {
                LogManager.SetWindowsServiceLog("OnPSInitialTimer" + ex.Message.ToString());
            }
            finally
            {
                // set archive schedule
                _SetArchiveTimer.Enabled = true;
                _SetArchiveTimer.Interval = _scheduleTime.Subtract(DateTime.Now).TotalSeconds * 1000;
                _SetArchiveTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnArchivePS);
            }

        }

        public void OnArchivePS(object sender, ElapsedEventArgs args)
        {
            try
            {
                //run every day on 04:00 am
                bool RefRsltA = ldbRefresh.RefreshArchiveLdbProductStatistics();
                LogManager.SetWindowsServiceLog("OnArchivePS cache db= " + RefRsltA );
            }
            catch (Exception ex)
            {
                LogManager.SetWindowsServiceLog("OnArchivePS_" + ex.Message.ToString());
            }
            finally
            {
                //  If tick for the first time, reset next run to every 24 hours
                if (_SetArchiveTimer.Interval != 24 * 60 * 60 * 1000)
                {
                    _SetArchiveTimer.Interval = 24 * 60 * 60 * 1000;
                }
            }

        }


    }
}
