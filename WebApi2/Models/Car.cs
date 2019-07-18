using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi2.Models
{
    public class Car
    {
        public string VIN { get; set; }
        public bool VALIDFORMAT { get; set; } = false;
        public bool AUDITEDITABLE { get; set; } = false;
        public static bool CheckFormatVin(string vin)
        {
            bool value = false;
            vin = vin.Trim().ToUpper();
            if ((vin.StartsWith("NAS")) && (vin.Length ==17))
                value = true;
            return value;

        }
        public static string GetVinWithoutChar(string vin)
        {
            string value = vin;
            if (!string.IsNullOrEmpty(vin))
            {
                vin = vin.Trim().ToUpper();
                if (vin.StartsWith("S"))
                    value = vin.Replace("S", "");
                else
                    if (vin.StartsWith("NAS"))
                    value = vin.Replace("NAS", "");
            }
            return value;

        }
        public string MSG { get; set; } = "";
    }
}