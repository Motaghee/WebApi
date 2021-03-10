namespace Common.Models.Qccastt
{
    public class Qccastt
    {
        public string Id { get; set; }
        public double Srl { get; set; } = 0; //double
        public bool ValidFormat = false;// { get; set; } = false;
        public string Vin { get; set; } = ""; //double
        public string VinWithoutChar ; //{ get; set; } = ""; //double
        public string AreaDesc { get; set; }
        public string ModuleName { get; set; }
        public double QCMdult_Srl { get; set; }
        public double QCBadft_Srl { get; set; }
        public string DefectDesc { get; set; }
        public string CreatedDateFa { get; set; }
        public string CreatedDayFa { get; set; }
        public string StrenghtDesc { get; set; }
        public int IsRepaired { get; set; } 
        public double CreatedBy { get; set; } = 0;
        public double RepairedBy { get; set; }
        public string CreatedByDesc { get; set; }
        public string RepairedByDesc { get; set; }
        public string RepairedDateFa { get; set; }
        

        public double GrpCode { get; set; }
        public double BdMdlCode { get; set; }
        public double AreaType { get; set; }
        public double ShopCode { get; set; }
        public double QCAreat_Srl { get; set; }
    }
}