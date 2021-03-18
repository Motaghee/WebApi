using Common.Models.Qccastt;
using System.Collections.Generic;

namespace WebApi2.Models
{
    public class ResultMsg
    {
        public string title { get; set; } = ""; //double
        public string Message = ""; //{ get; set; } = ""; //double
        public string MessageFa = "";
        public IEnumerable<Qccastt> lstQccastt { get; set; }
    }
}