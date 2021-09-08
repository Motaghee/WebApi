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
        public int QSCapunt_Srl   { get; set; }//واحد درخواست کننده
        public int QSCReft_Srl { get; set; } //ارجاع به مديريت
        public int QSCDtstt_Ssrl { get; set; } //مرحله شناسايي
        public int Qscscpt_srl { get; set; } //درخواست مرتبط با حوزه
        public int Qscrqtyt_srl { get; set; } //درخواست مرتبط با حوزه
        public int ReqBy { get; set; } 
        public int ReqBossBy { get; set; } 
        public int ReqMngBy { get; set; } 
        public int ReqConfirmBy { get; set; }
        public int Qscdebat_Srl { get; set; }

        //-------
        public string bdmdlAliasName { get; set; }
        public string EngineNo { get; set; }
        public string Prodno { get; set; }

        public List<qscapunt> LstQscapunt { get; set; }
        public List<qscreft> LstQscreft { get; set; }
        public List<qscdtstt> LstQSCDtstt { get; set; }
        public List<qscrqrsnt> LstQscrqrsnt { get; set; }
        public List<qscscpt> LstQscscpt { get; set; }
        public List<qscrqtyt> LstQscrqtyt { get; set; }
        public List<Users> LstQscUsers { get; set; }
        public List<qscordet> LstQscordet { get; set; }
        public List<qscdebat> LstQscdebat { get; set; }
        public List<qscbdmdt> LstQscbdmdt { get; set; }
        
        //---
        public IEnumerable<int> Sel_qscrqrsnt_Srl { get; set; }
        public IEnumerable<int> Sel_Qscordet_Srl { get; set; }
        public IEnumerable<int> Sel_Qscbdmdt_Srl { get; set; }


        //select srl, scopedesc, inuse from qscscpt; --حوزه ی درخواست


    }
}