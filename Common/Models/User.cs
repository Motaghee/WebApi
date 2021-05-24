using Common.Models.QccasttModels;
using System.Collections.Generic;

namespace Common.Models
{
    public class User
    {
        public double SRL { get; set; } //double
        public string FNAME { get; set; }
        public string LNAME { get; set; }
        public string USERNAME { get; set; }
        public int USERID { get; set; }
        public string PSW { get; set; }
        public string MACADDRESS; //{ get; set; }
        public double QCAREATSRL { get; set; }
        public double CHECKDEST { get; set; }
        public int AREACODE { get; set; }
        public string AREADESC { get; set; }
        public int AREATYPE { get; set; }
        public string SERVERREQVER;//{ get; set; }
        public string USERAPPVER;// { get; set; }
        public bool MACISVALID;// { get; set; } = false;
        public bool CLIENTVERISVALID;// { get; set; } = false;
        public string STRUSERPROFILEIMAGE;//{ get; set; }
        // -- MobAppPerrmission
        public string QCMOBAPPPER { get; set; }
        public string PTDASHPER { get; set; }
        public string QCDASHPER { get; set; }
        public string AUDITDASHPER { get; set; }
        public string AUDITUNLOCKPER { get; set; }
        public string QCREGDEFPER { get; set; }
        public string SMSQCPER { get; set; }
        public string SMSAUDITPER { get; set; }
        public string SMSSPPER { get; set; }
        public string QCCARDPER { get; set; }
        public string SMSPTPER { get; set; }
        public string AUDITCARDPER { get; set; }
        public string CARSTATUSPER { get; set; }
        public string AppName;// { get; set; }
        public string ClientVersion;// { get; set; }


        //public double SRL { get; set; } //double
        //public string FNAME { get; set; }
        //public string LNAME { get; set; }
        //public string USERNAME { get; set; }
        //public string PSW { get; set; }
        //public string MACADDRESS { get; set; }
        //public double QCAREATSRL { get; set; }
        //public double CHECKDEST { get; set; }
        //public double VALIDQCAREACODE { get; set; }
        //public string QCAREACODE { get; set; }
        //public string SERVERREQVER { get; set; }
        //public string USERAPPVER { get; set; }
        //public bool MACISVALID { get; set; } = false;
        //public bool CLIENTVERISVALID { get; set; } = false;



    }
}