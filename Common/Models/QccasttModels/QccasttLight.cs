namespace Common.Models.QccasttModels
{
    public class QccasttLight
    {
        public string Id { get; set; }
        public double Srl { get; set; } = 0; //double
        public string Vin { get; set; } = ""; //double
        public string AreaDesc { get; set; }
        public string ModuleName { get; set; }
        public string DefectDesc { get; set; }
        public string CreatedDateFa { get; set; }
        public string StrenghtDesc { get; set; }
        public string CreatedByDesc { get; set; } 
    }
}