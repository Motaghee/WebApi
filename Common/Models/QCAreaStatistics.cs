using LiteDB;
using System.Collections.Generic;

namespace Common.Models
{
    public class QCAreaStatistics
    {
        //public string U_DateTime { get; set; }
        public string Id { get; set; }
        public string CreatedDateFa { get; set; }
        public string U_DateTimeFa { get; set; }
        public double CreatedDateFaNum { get; set; }
        public double AreaType { get; set; }
        public double AreaCode { get; set; }
        public string ShopName { get; set; }
        public double PTShopCode { get; set; }
        public string AreaDesc { get; set; }
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
        //public double  { get; set; }
    }

}
