using LiteDB;
using System;
using System.Collections.Generic;

namespace Common.Models
{
    public class QCStatistics
    {

        public string Id { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public string CreatedDateFa { get; set; }
        public double CreatedDateFaNum { get; set; }
        //public string U_DateTimeFa { get; set; }
        //public double CompanyCode { get; set; }
        public double GrpCode { get; set; }
        //public string GrpName { get; set; }
        public double BdMdlCode { get; set; }
        //public string AliasName { get; set; }
        //public string CommonBodyModelName { get; set; }
        public double AreaType { get; set; }
        //public string ShopName { get; set; }
        //public double AreaCode { get; set; }
        public double ShopCode { get; set; }
        //public string AreaDesc { get; set; }
        public double QCAreat_Srl { get; set; }
        // counts
        public double DetectCarCount { get; set; }
        public double DefCarCount { get; set; }
        public double StrCarCount { get; set; }
        public double RegDefCount { get; set; }
        public double StrCount { get; set; }
        public double ASPCarCount { get; set; }
        public double ASPRegDefCount { get; set; }
        public double BPlusCarCount { get; set; }
        public double BPlusRegDefCount { get; set; }
        //public double QCMdult_Srl { get; set; }
        //public double QCBadft_Srl { get; set; }
        //public double CreatedBy { get; set; }
    }

}
