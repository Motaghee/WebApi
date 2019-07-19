using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi2.Models
{
    public class Car
    {
        public string VIN { get; set; }
        public string PRODNO { get; set; }
        public string VINWITHOUTCHAR { get; set; }
        public bool VALIDFORMAT { get; set; } = false;
        public bool AUDITEDITABLE { get; set; } = false;
        public string MSG { get; set; } = "";
        public string JOINERYDATE_FA { get; set; }
        public string JOINERYDATE  { get; set; }
        public string BDMDLCODE { get; set; }
        public string CARMDLCODE { get; set; }
        public string ALIASNAME { get; set; }
        public string FOREXPORT { get; set; }
        public string FITYPECODE  { get; set; }
        public string FITYPENAME { get; set; }
        public string GEARBOXTYPECODE { get; set; }
        public string GEARBOXTYPEDESC { get; set; }
        public string SHOPCODE { get; set; }
        public string SHOPNAME { get; set; }
        public string FINQCCODE { get; set; }
        public string CLRALIAS { get; set; }
        public string JOINARYTEAMDESC { get; set; } //--
        public string JOINARYTEAM { get; set; } //--
        public string PRODENDDATE_FA { get; set; } //--
        public string GRPCODE { get; set; }
        public string GRPNAME { get; set; }
        public string RECALLDESC { get; set; } //--
        public string LENDDESC { get; set; } // --
        public string ASMSHOPCODE { get; set; } //--
        public string PTCURRENTSHOPCODE { get; set; } //--
        public string COMPANYCODE { get; set; }
        public string TOAREAENTERNUM { get; set; }
        public string CARTYPEDESC { get; set; }
        public string PTTRACE { get; set; }
        public string QCTRACE { get; set; }


    }
}