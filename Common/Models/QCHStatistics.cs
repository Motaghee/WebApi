using LiteDB;
using System;
using System.Collections.Generic;

namespace Common.Models
{
    public class QCHStatistics
    {

        public string Id { get; set; }
        public string Hour { get; set; }
        public double GrpCode { get; set; }
        public double BdMdlCode { get; set; }
        public double AreaType { get; set; }
        public double ShopCode { get; set; }
        public double QCAreat_Srl { get; set; }
        public double DetectCarCount { get; set; }
        public double DefCarCount { get; set; }
        public double RegDefCount { get; set; }
    }

}
