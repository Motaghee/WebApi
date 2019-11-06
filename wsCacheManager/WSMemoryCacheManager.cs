using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Common.Models;
using Common.Actions;
using Common.CacheManager;

namespace WinSrvCacheManager
{
    public partial class WSMemoryCacheManager : ServiceBase
    {
        Timer PSCreatorTimer;
        Timer PSInitialTimer;
        public WSMemoryCacheManager()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                LogManager.SetWindowsServiceLog("OnStart_Statrt WSMemoryCacheManager Service");
                PSInitialTimer = new Timer();
                PSInitialTimer.Interval = 5000; //5 Min
                PSInitialTimer.Elapsed += new ElapsedEventHandler(this.OnPSInitialTimer);
                PSInitialTimer.Start();
                //---
                PSCreatorTimer = new Timer();
                PSCreatorTimer.Interval = 360000; //5 Min
                PSCreatorTimer.Elapsed += new ElapsedEventHandler(this.OnPSCreatorTimer);
                PSCreatorTimer.Start();
                //---

            }
            catch (Exception ex)
            {
                LogManager.SetWindowsServiceLog("OnStart_Statrt Error " +ex.Message);
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
                bool RefRsltY = ldbRefresh.RefreshLdbProductStatistics("Y");
                bool RefRsltM = ldbRefresh.RefreshLdbProductStatistics("M");
                bool RefRsltD = ldbRefresh.RefreshLdbProductStatistics("D");
                //LogManager.SetWindowsServiceLog("OnPSCreatorTimer_ result RefreshLdb.RefreshLdbProductStatistics()Result=" + RefRslt.ToString());
            }
            catch (Exception ex)
            {
                LogManager.SetWindowsServiceLog("OnPSCreatorTimer"+ex.Message.ToString());
            }

        }

        public void OnPSInitialTimer(object sender, ElapsedEventArgs args)
        {
            try
            {
                PSInitialTimer.Stop();
                bool RefRsltY = ldbRefresh.RefreshLdbProductStatistics("Y");
                bool RefRsltM = ldbRefresh.RefreshLdbProductStatistics("M");
                bool RefRsltD = ldbRefresh.RefreshLdbProductStatistics("D");
                LogManager.SetWindowsServiceLog("OnPSInitialTimer Initialise cache db= " + RefRsltY + RefRsltM + RefRsltD);
                //LogManager.SetWindowsServiceLog("OnPSCreatorTimer_ result RefreshLdb.RefreshLdbProductStatistics()Result=" + RefRslt.ToString());
            }
            catch (Exception ex)
            {
                LogManager.SetWindowsServiceLog("OnPSInitialTimer" + ex.Message.ToString());
            }

        }





    }
}
