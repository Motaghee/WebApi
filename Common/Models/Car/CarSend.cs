namespace Common.Models.Car
{
    public class CarSend
    {
        public string Vin { get; set; }
        public int FromAreaSrl { get; set; }
        public int ToAreaSrl { get; set; }
        public int QCUsertSrl { get; set; }
        public int UserId { get; set; }
        public int AreaType { get; set; }
    }
}