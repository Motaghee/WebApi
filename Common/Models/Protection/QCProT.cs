using LiteDB;
using System.Collections.Generic;

namespace Common.Models.Protection
{
    public class QCProT
    {
        //public string U_DateTime { get; set; }
        public int Srl { get; set; }
        public string Vin { get; set; }
        public int UserId { get; set; }
        public string ProCreatedDateFa { get; set; }
        public string ProCreatedByDesc { get; set; }
        public int CreatedBy { get; set; }
        public int LocIsValid { get; set; }
    }

}
