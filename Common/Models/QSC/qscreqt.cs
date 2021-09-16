using Common.db;
using Common.Models.General;
using System;
using System.Collections.Generic;

namespace Common.Models.QSC
{
    public class qscreqt
    {
        public int Srl { get; set; }
        public string Vin { get; set; }
        public int Mileage { get; set; }
        public int CreatedBy { get; set; }
        public int QSCapunt_Srl   { get; set; }//واحد درخواست کننده
        public int QSCReft_Srl { get; set; } //ارجاع به مديريت
        public int QSCDtstt_Ssrl { get; set; } //مرحله شناسايي
        public int Qscscpt_srl { get; set; } //درخواست مرتبط با حوزه
        public int Qscrqtyt_srl { get; set; } //درخواست مرتبط با حوزه
        public string ReqDesc { get; set; }
        public int ReqBy { get; set; } 
        public int ReqBossBy { get; set; } 
        public int ReqMngBy { get; set; } 
        public int ReqConfirmBy { get; set; }
        public int Qscdebat_Srl { get; set; }
        public int Qscrqfistt_Srl { get; set; }
        public int Qsccocet_Srl { get; set; }

        //-------
        public string bdmdlAliasName { get; set; }
        public string EngineNo { get; set; }
        public string Prodno { get; set; }
        public int FinQCCode { get; set; }

        public int InsBy { get; set; }
        public int InsBossBy { get; set; }
        public int InsMngBy { get; set; }
        public int InsConfirmBy { get; set; }

        public List<qscapunt> LstQscapunt { get; set; }
        public List<qscreft> LstQscreft { get; set; }
        public List<qscdtstt> LstQSCDtstt { get; set; }
        public List<qscrsnt> LstQscrqrsnt { get; set; }
        public List<qscscpt> LstQscscpt { get; set; }
        public List<qscrqtyt> LstQscrqtyt { get; set; }
        public List<Users> LstQscUsers { get; set; }
        public List<qscordet> LstQscordet { get; set; }
        public List<qscdebat> LstQscdebat { get; set; }
        public List<qscbdmdt> LstQscbdmdt { get; set; }
        public List<qscrqfistt> LstQscrqfistt { get; set; }
        public List<qsccocet> LstQsccocet { get; set; }
        
        //---
        public IEnumerable<int> Sel_qscrqrsnt_Srl { get; set; }
        public IEnumerable<int> Sel_Qscordet_Srl { get; set; }
        public IEnumerable<int> Sel_Qscbdmdt_Srl { get; set; }


        //select srl, scopedesc, inuse from qscscpt; --حوزه ی درخواست


    }
}