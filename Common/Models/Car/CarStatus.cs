using LiteDB;
using System;
using System.Collections.Generic;

namespace Common.Models
{
    public class CarStatus
    {
        public string Id { get; set; }
        public string Vin { get; set; }
        public double JoineryDateFaNum { get; set; }
        public string JoineryDateFa { get; set; }
        public double GrpCode { get; set; }
        public double BdMdlCode { get; set; }
        public double FinqcCode { get; set; }
        public double Status_Code { get; set; }
        public double CompanyCode { get; set; }
        public string CompanyName { get; set; }
        

    }

}
