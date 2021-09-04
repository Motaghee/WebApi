namespace Common.Models.QccasttModels
{
    public class Qccastt
    {
        public string Id { get; set; }
        public double Srl { get; set; } = 0; //double
        public bool ValidFormat = false;// { get; set; } = false;
        public string Vin { get; set; } = ""; //double
        public string VinWithoutChar { get; set; }  //double
        public int AreaCode { get; set; }
        public string AreaDesc { get; set; }
        public string ModuleName { get; set; }
        public int QCMdult_Srl { get; set; }
        public int QCBadft_Srl { get; set; }
        public int QCSTRGT_SRL { get; set; }
        public string DefectDesc { get; set; }
        public string CreatedDateFa { get; set; }
        public string DeletedDateFa { get; set; }
        public string CreatedDayFa { get; set; }
        public string StrenghtDesc { get; set; }
        public int IsRepaired { get; set; } 
        public int CreatedBy { get; set; } = 0;
        public int RepairedBy { get; set; }
        public string CreatedByDesc { get; set; }
        public string RepairedByDesc { get; set; }
        public string RepairedDateFa { get; set; }
        public int GrpCode { get; set; }
        public int BdMdlCode { get; set; }
        public int AreaType { get; set; }
        public int ShopCode { get; set; }
        public int QCAreat_Srl { get; set; }
        public int RecordOwner { get; set; }
        public int CHECKLISTAREA_SRL { get; set; }
        public int IsDefected { get; set; }
        public int InUse { get; set; }
        public int ActAreaSrl { get; set; }
        public int ActBy { get; set; }
        public string InListQCSTRGTSRL;
        
        public string DateType;
        public int deletedby { get; set; }
        public string DeletedByDesc { get; set; }
        public int HaveAsmExitPer { get; set; }
        
    }
}