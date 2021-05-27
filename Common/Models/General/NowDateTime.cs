using Common.Models.General;
using System;
using System.Globalization;

namespace Common.Models.General
{
    public class NowDateTime
    {
        public NowDateTime()
        {
            PersianCalendar pc = new PersianCalendar();
            Now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime dtN = DateTime.Now;
            NowDateFa = pc.GetYear(dtN).ToString() + "/" + pc.GetMonth(dtN).ToString().PadLeft(2, '0') + "/" + pc.GetDayOfMonth(dtN).ToString().PadLeft(2, '0'); 
            NowTime = dtN.ToString("HH:mm:ss"); 
            NowDateTimeFa = NowDateFa + " " + NowTime;
        }

        public double QCUsertSrl { get; set; }
        public String Now { get; set; }
        public string NowDateFa { get; set; }
        public string NowTime { get; set; }
        public string NowDateTimeFa { get; set; }
        public MessageCount MsgCount { get; set; }
    }




}