using LiteDB;
using System;
using System.Collections.Generic;

namespace Common.Models
{
    public class AuditStatisticsMDTrend
    {
        //public string U_DateTime { get; set; }
        public string Id { get; set; }
        public double YearWeekNo { get; set; }
        public double QCAreat_Srl { get; set; }
        public double QcMdult_Srl { get; set; }
        public double QcBadft_Srl { get; set; }
        public double GrpCode { get; set; }
        public double BdMdlCode { get; set; }
        public double BdStlCode { get; set; }
        // counts
        public double WeekNScoreSum { get; set; }
        public double AuditCarCount { get; set; }
        public double WeekDetectMDCount { get; set; }
        public double AVGNScore { get; set; }
        public double MDRegR { get; set; }



    }

}
