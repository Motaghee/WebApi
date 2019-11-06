namespace WebApi2.Models
{
    public class Qccastt
    {
        public double Srl { get; set; } = 0; //double
        public bool ValidFormat = false;// { get; set; } = false;
        public string Vin { get; set; } = ""; //double
        public string VinWithoutChar = ""; //{ get; set; } = ""; //double
        public string AreaDesc { get; set; }
        public string ModuleName { get; set; }
        public string DefectDesc { get; set; }
        public string CreatedDateFa { get; set; }
        public string StrenghtDesc { get; set; }
        public double IsRepaired { get; set; } = 0;
        public double CreatedBy { get; set; } = 0;
        public double RepairedBy { get; set; }
        public string CreatedByDesc { get; set; }
        public string RepairedByDesc { get; set; }
    }
}