namespace Common.Models.Audit
{
    public class AuditCarDetail
    {
        public double Srl { get; set; } = 0; //double

        public bool ValidFormat = false;// { get; set; } = false;
        public string Vin { get; set; } = ""; //double
        public string VinWithoutChar = ""; //{ get; set; } = ""; //double
        public string AreaDesc { get; set; }
        public string ModuleName { get; set; }
        public string DefectDesc { get; set; }

        public string AuditDateFa { get; set; }
        public string StrenghtDesc { get; set; }
        public string Auditor2 { get; set; }
        public string CreatedByDesc { get; set; }
        public double CreatedBy { get; set; }

        public string Msg = "";



    }
}