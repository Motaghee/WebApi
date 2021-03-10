using System;
using System.ComponentModel.DataAnnotations;
namespace Common.Models.Qccastt
{
    public class Pcshopt
    {
        public int Srl { get; set; }
        public int ShopCode { get; set; }
        public string ShopName { get; set; }
        public int PtshopCode { get; set; }
        public bool Checked { get; set; }
    }
}

