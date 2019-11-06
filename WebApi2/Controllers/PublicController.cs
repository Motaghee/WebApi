using System;
using System.Globalization;
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
            MemoryCacher mc = new MemoryCacher();
            MessageCount oldmsc = (MessageCount)mc.GetValue("MessageCount");
            if ((oldmsc == null) || (oldmsc.InsDateFa != ndt.NowDateFa))
            {
                MessageCount msc = MessageUtility.GetSmsCountByDate(ndt.NowDateFa);
                ndt.MsgCount = msc;
                mc.Add("MessageCount", msc, DateTimeOffset.Now.AddSeconds(5));
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
            // String DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";
            //String mDateTime = DateUtils.formatDateTimeFromDate(DATE_FORMAT, Calendar.getInstance().getTime());
            ndt.Now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            PersianCalendar pc = new PersianCalendar();
            DateTime dtN = DateTime.Now;
            ndt.NowDateFa = pc.GetYear(dtN).ToString() + "/" + pc.GetMonth(dtN).ToString().PadLeft(2, '0') + "/" + pc.GetDayOfMonth(dtN).ToString().PadLeft(2, '0');
            ndt.NowTime = dtN.ToString("HH:mm:ss");
            ndt.NowDateTimeFa = ndt.NowDateFa + " " + ndt.NowTime;
            // ---
            MemoryCacher mc = new MemoryCacher();
            MessageCount oldmsc = (MessageCount)mc.GetValue("MessageCount");
            if ((oldmsc == null) || (oldmsc.InsDateFa != ndt.NowDateFa))
            {
                MessageCount msc = MessageUtility.GetSmsCountByDate(ndt.NowDateFa);
                ndt.MsgCount = msc;
                if (oldmsc != null)
                    mc.Delete("MessageCount");
                mc.Add("MessageCount", msc, DateTimeOffset.Now.AddSeconds(30));
            }
            else
                ndt.MsgCount = oldmsc;
            // ---
            return ndt;
            //ToShortDateString()+" " + DateTime.Now.ToShortTimeString();
        }




    }
}