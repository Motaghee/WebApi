using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi2.Models
{
    public class User
    {
        public double SRL { get; set; } //double
        public string FNAME { get; set; }
        public string LNAME { get; set; }
        public string USERNAME { get; set; }
        public string PSW { get; set; }
        public string MACADDRESS { get; set; }
        public string QCAREATSRL { get; set; }
        public string SERVERREQVER { get; set; }
        public string USERAPPVER { get; set; } = "0.0.1";
        public bool USERATHENTICATION { get; set; } = false;
        public bool USERAUTHORIZATION { get; set; } = false;
        public bool MACISVALID { get; set; } = false;
        public bool CLIENTVERISVALID { get; set; } = false;
        


    }
}