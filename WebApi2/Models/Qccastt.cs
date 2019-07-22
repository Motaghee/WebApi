﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi2.Models
{
    public class Qccastt
    {
        public double Srl { get; set; } = 0; //double
        public bool ValidFormat { get; set; } = false;
        public string Vin { get; set; } = ""; //double
        public string VinWithoutChar { get; set; } = ""; //double
        public string AreaDesc { get; set; }
        public string Inspector { get; set; }
        public string ModuleName { get; set; }
        public string DefectDesc { get; set; }
        public string CreatedDateFa { get; set; }
        public string StrenghtDesc { get; set; }
        public string Msg { get; set; } = "";
        


    }
}