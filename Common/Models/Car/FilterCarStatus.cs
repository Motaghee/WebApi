using LiteDB;
using System;
using System.Collections.Generic;

namespace Common.Models
{
    public class FilterCarStatus
    {
        public string Id { get; set; }
        public string Vin { get; set; }
        public double JoineryDateFaNum { get; set; }
        public double GrpCode { get; set; }
        public double BdMdlCode { get; set; }
        public double FinqcCode { get; set; }
        public double Status_Code { get; set; }
        public double CompanyCode { get; set; }
        
        public double JoineryStartDateFaNum { get; set; }
        public double JoineryEndDateFaNum { get; set; }

        public double AutomationSendPid { get; set; }
        public string AutomationContentDesc { get; set; }
    }

}
