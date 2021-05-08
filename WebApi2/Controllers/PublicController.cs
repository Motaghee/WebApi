using Common.Actions;
using Common.CacheManager;
using Common.db;
using Common.Models;
using Common.Models.General;
using Common.Utility;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using WebApi.OutputCache.V2;
using WebApi2.Controllers.Utility;
using WebApi2.Models;

namespace WebApi2.Controllers
{
    public class PublicController : ApiController
    {

        [HttpGet]
        [Route("api/Public/GetNow")]
        [CacheOutput(ClientTimeSpan = 5, ServerTimeSpan = 5)]
        public NowDateTime GetNow()
        //public DateTime GetNow()
        {
            NowDateTime ndt = new NowDateTime();
            ndt.Now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            PersianCalendar pc = new PersianCalendar();
            DateTime dtN = DateTime.Now;
            ndt.NowDateFa = pc.GetYear(dtN).ToString() + "/" + pc.GetMonth(dtN).ToString().PadLeft(2, '0') + "/" + pc.GetDayOfMonth(dtN).ToString().PadLeft(2, '0');
            ndt.NowTime = dtN.ToString("HH:mm:ss");
            ndt.NowDateTimeFa = ndt.NowDateFa + " " + ndt.NowTime;
            // ---
            MessageCount oldmsc = (MessageCount)MemoryCacher.GetValue("MessageCount");
            if ((oldmsc == null) || (oldmsc.InsDateFa != ndt.NowDateFa))
            {
                MessageCount msc = MessageUtility.GetSmsCountByDate(ndt.NowDateFa);
                ndt.MsgCount = msc;
                MemoryCacher.Add("MessageCount", msc, DateTimeOffset.Now.AddSeconds(5));
            }
            else
                ndt.MsgCount = oldmsc;
            // ---
            return ndt;
        }


        // POST
        [HttpPost]
        [Route("api/Public/GetNowU")]
        [Authorize]
        public NowDateTime GetNowU([FromBody] NowDateTime ndt)
        //public DateTime GetNow()
        {
            User user = new User();
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            user.AppName = claims.FirstOrDefault(x => x.Type == "AppName").Value.ToString();
            user.USERID = Convert.ToInt32(claims.FirstOrDefault(x => x.Type == "UserId").Value.ToString());
            //---
            // String DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";
            //String mDateTime = DateUtils.formatDateTimeFromDate(DATE_FORMAT, Calendar.getInstance().getTime());
            ndt.Now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            PersianCalendar pc = new PersianCalendar();
            DateTime dtN = DateTime.Now;
            ndt.NowDateFa = pc.GetYear(dtN).ToString() + "/" + pc.GetMonth(dtN).ToString().PadLeft(2, '0') + "/" + pc.GetDayOfMonth(dtN).ToString().PadLeft(2, '0');
            ndt.NowTime = dtN.ToString("HH:mm:ss");
            ndt.NowDateTimeFa = ndt.NowDateFa + " " + ndt.NowTime;
            // ---
            //MessageCount oldmsc = (MessageCount)MemoryCacher.GetValue("MessageCount");
            //if ((oldmsc == null) || (oldmsc.InsDateFa != ndt.NowDateFa))
            //{
            //    MessageCount msc = MessageUtility.GetSmsCountByDate(ndt.NowDateFa);
            //    ndt.MsgCount = msc;
            //    if (oldmsc != null)
            //        MemoryCacher.Delete("MessageCount");
            //    MemoryCacher.Add("MessageCount", msc, DateTimeOffset.Now.AddSeconds(30));
            //}
            //else
            //    ndt.MsgCount = oldmsc;
            // ---
            try
            {
                //LogManager.SetCommonLog(String.Format(@"GetNowU time:{0} From:{1} by:{2} UserId:{3}", ndt.NowDateFa, user.AppName, ndt.QCUsertSrl.ToString(), user.USERID));
                GeneralUtility.UpdateUserData(user, ndt, 1);
            }
            catch (Exception e)
            {
                DBHelper.LogFile(e);

            }
            return ndt;
            //ToShortDateString()+" " + DateTime.Now.ToShortTimeString();
        }

        [HttpGet]
        [Route("api/Public/GetOnlineUsers")]
        public List<OnlineUsers> tstGetOnlineUsers()
        {
            // get instanse of ldb
            ConnectionString cn = ldbConfig.ldbOnlineUsersConnectionString;
            LiteDatabase db = new LiteDatabase(cn);
            // get old ldb ps lst
            List<OnlineUsers> lst = new List<OnlineUsers>();
            LiteCollection <OnlineUsers> dbUD = db.GetCollection<OnlineUsers>("OnlineUsers");
            lst = dbUD.FindAll().ToList<OnlineUsers>();
            return lst;
            //OnlineUsers[] res= result.Cast<OnlineUsers>().ToList();

            //User user = new User();
            //user.USERID = 1000861;
            //NowDateTime ndt = new NowDateTime();
            //ndt.Now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //PersianCalendar pc = new PersianCalendar();
            //DateTime dtN = DateTime.Now;
            //ndt.NowDateFa = pc.GetYear(dtN).ToString() + "/" + pc.GetMonth(dtN).ToString().PadLeft(2, '0') + "/" + pc.GetDayOfMonth(dtN).ToString().PadLeft(2, '0');
            //ndt.NowTime = dtN.ToString("HH:mm:ss");
            //ndt.NowDateTimeFa = ndt.NowDateFa + " " + ndt.NowTime;
            //GeneralUtility.UpdateUserData(user, ndt, 1);
        }
       
    }



}