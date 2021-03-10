using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utility
{
    public class VinUtility
    {
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
    }
}
