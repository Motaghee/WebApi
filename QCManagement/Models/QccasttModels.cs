using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Common.Models.Car;
using Common.Models.QccasttModels;

namespace QCManagement.Models
{
    public class QccasttModels
    {
        public Int32? srl { get; set; }
        public string Vin { get; set; }
        public int baseDefectSRL { get; set; }
        public int moduleSRL { get; set; }
        public int areaSRL { get; set; }
        public string StartCreatedDate { get; set; }
        public string EndCreatedDate { get; set; }
        public int checkListArea_SRL { get; set; }
        public int userSRL { get; set; }
        public string JoinShift { get; set; }
        public string ForExport { get; set; }
        public string DPU { get; set; }
        public int ReportTimeType { get; set; }
        public int qcstrgt_srl { get; set; }
        public int isDefected { get; set; }
        public int isRepaired { get; set; }
        public int isRecordOwner { get; set; }
        public int inused { get; set; }
        public string PCShopt_Srl { get; set; }
        public int QCAREAT_Srl { get; set; }
        public int QCMdult_Srl { get; set; }
        public int QCBadft_Srl { get; set; }
        public int GrpCode { get; set; }
        public int BdMdlCode { get; set; }
        
        public List<Pcshopt> Lstshop { get; set; }
        public List<Area> LstArea { get; set; }
        public List<CarGroup> LstCarGroup { get; set; }
        public IEnumerable<int> Sel_PCShopt_Srl { get; set; }
        public IEnumerable<int> Sel_QCAREAT_Srl { get; set; }
        public IEnumerable<int> Sel_GrpCode { get; set; }
        public IEnumerable<int> Sel_BdMdlCode { get; set; }
        public IEnumerable<int> Sel_Qcstrgt_srl { get; set; }
        public IEnumerable<string> Sel_JoinShift { get; set; }
        public IEnumerable<int> Sel_BdstlCode { get; set; }
        public List<Shift> LstShift { get; set; }
        public List<BodyModel> LstBodyModel { get; set; }
        public List<Strength> LstStrength { get; set; }
        public List<BodyStyle> LstBodyStyle { get; set; }


    }
}

