using Common.Models.QccasttModels;
using System.Collections.Generic;

namespace Common.Models.General
{
    public class ResultMsg
    {
        public string title { get; set; } = ""; //double
        public string Message = ""; //{ get; set; } = ""; //double
        public string MessageFa = "";
        public bool Successful { get; set; } = false;
        public IEnumerable<Qccastt> lstQccastt { get; set; }
        public IEnumerable<Qcqctrt> lstQcqctrt { get; set; }
    }
}