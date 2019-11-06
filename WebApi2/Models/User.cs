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
        public double QCAREATSRL { get; set; }
        public double CHECKDEST { get; set; }
        public double VALIDQCAREACODE { get; set; }
        public string QCAREACODE { get; set; }
        public string SERVERREQVER { get; set; }
        public string USERAPPVER { get; set; }
        public bool MACISVALID { get; set; } = false;
        public bool CLIENTVERISVALID { get; set; } = false;



    }
}