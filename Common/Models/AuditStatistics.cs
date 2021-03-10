using LiteDB;
using System.Collections.Generic;

namespace Common.Models
{
    public class AuditStatistics
    {
        //public string U_DateTime { get; set; }
        public string Id { get; set; }
        public string AuditDate_Fa { get; set; }
        //public string AuditDate { get; set; }
        public double QCAreat_Srl { get; set; }
        public double AreaCode { get; set; }
        public double AuditDateFaNum { get; set; }
        public double GrpCode { get; set; }
        public double BdMdlCode { get; set; }
        public double BdStlCode { get; set; }
        // counts
        public double CountOfModuleDefect { get; set; }
        public double AuditCarCount { get; set; }
        public double CountOfADefect { get; set; }
        public double CountOfSDefect { get; set; }
        public double CountOfPDefect { get; set; }
        public double CountOfBDefect { get; set; }
        public double CountOfCDefect { get; set; }
        public double CountOfBPlusDefect { get; set; }
        public double SumOfNegativeScoreValue { get; set; }
        
    }

}
