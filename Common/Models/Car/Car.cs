using Common.Models.QccasttModels;
using System;
using System.Collections.Generic;

namespace Common.Models.Car
{
    public class Car
    {
        public string Vin { get; set; }
        public double Prodno { get; set; }
        public string VinWithoutChar;
        public bool ValidFormat = false;
        public bool AuditEditable = false;
        public string msg = "";
        public string JoineryDate_Fa { get; set; }
        public DateTime JoineryDate { get; set; }
        public double bdmdlCode { get; set; }
        //public double CarMdlCode { get; set; }
        public string bdmdlAliasName { get; set; }
        public string BdstlAliasName { get; set; }
        public double ForExport { get; set; }
        public double FiTypeCode { get; set; }
        public string FiTypeName { get; set; }
        public double GearBoxTypeCode { get; set; }
        public string GearBoxTypeDesc { get; set; }
        public double ShopCode { get; set; }
        public string ShopName { get; set; }
        public double FinQcCode { get; set; }
        public string ClrAlias { get; set; }
        public string JoinaryTeamDesc { get; set; }
        public string JoinaryTeam { get; set; }
        public string ProdEndDate_Fa { get; set; }
        public double GrpCode { get; set; }
        //public double CarImageCount { get; set; }
        //public string CarImageSrls { get; set; }
        public string GrpName { get; set; }
        //public string ReCallDesc { get; set; } 
        //public string LendDesc { get; set; } 
        //public string AsmShopCode { get; set; } 
        //public string PTCurrentShopCode { get; set; } 
        public double CompanyCode { get; set; }
        //public string ToAreaEnterNum { get; set; }
        //public string CarTypeDesc { get; set; }
        //public string PTTrace { get; set; }
        //public string QCTrace;
        public string BodyShopProdDate { get; set; }
        public string PaintShopProdDate { get; set; }
        public string ASMShopProdDate { get; set; }
        public double CurAreaSrl { get; set; }
        public int ActAreaSrl { get; set; }
        public int ActBy { get; set; }
        public int PTCurrentShopCode { get; set; }
        public int CurrentAreaDefCount { get; set; }
        public IEnumerable<Qcqctrt> lstQcqctrt { get; set; }
        public IEnumerable<Qccastt> lstQccastt { get; set; }




    }
}