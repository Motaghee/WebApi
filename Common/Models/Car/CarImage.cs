using System;

namespace Common.Models.Car
{
    public class CarImage
    {
        public long Srl { get; set; }
        public string Vin { get; set; }
        public string Image { get; set; }
        public string Thumbnail { get; set; }
        public string Title { get; set; }
        public string ImageDesc { get; set; }
        public string CreatedDateFa { get; set; }
        public string CreatedByDesc { get; set; }
        // public DateTime CreatedDate { get; set; } //--
        public double CreatedBy { get; set; }     //--
        public double QCAreatSrl { get; set; }     //--
        public string AreaDesc { get; set; }     //--
        public double Inuse { get; set; }
        public double UpdatedBy { get; set; }
        public Boolean Updated;
        //public double QCAreat_Srl;
        //public double QCMdult_srl;
        //public string Msg  = "";



    }
}