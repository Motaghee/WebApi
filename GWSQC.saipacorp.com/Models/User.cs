namespace GWSQC.saipacorp.com.Models
{
    public class User
    {
        public string USERNAME { get; set; }
        public string PSW { get; set; }
        public string Vin { get; set; }
        public string SDate { get; set; }
        public string EDate { get; set; }
        public User()
        {
            USERNAME = "0";
            PSW = "0";
            Vin = "0";
            SDate = "0";
            EDate = "0";
        }

    }
}