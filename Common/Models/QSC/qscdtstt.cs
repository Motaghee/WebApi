using Common.db;
using System;

namespace Common.Models.QSC
{
    public class qscdtstt
    {
        public int Srl { get; set; }
        public string DetectionStepName { get; set; }
        public int Inuse { get; set; }

        public static object[] Get()
        {
            try
            {
                string commandtext = string.Format(@"select srl, detectionstepname, inuse from qscdtstt where inuse=1");
                return DBHelper.GetDBObjectByObj2_OnLive(new qscdtstt(), null, commandtext, "qsc");
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        //select srl, detectionstepname, inuse from qscdtstt; --مراحل شناسایی
    }
}