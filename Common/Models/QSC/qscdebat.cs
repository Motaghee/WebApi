using Common.db;
using System;

namespace Common.Models.QSC
{
    public class qscdebat
    {
        public int Srl { get; set; }
        public string DecisionsDesc { get; set; }
        public int Inuse { get; set; }

        //select srl, decisionsdesc, inuse from qscdebat ; --تصمیم گیری وضعیت نهایی بر اساس
        public static object[] Get()
        {
            try
            {
                string commandtext = string.Format(@"select srl, decisionsdesc, inuse from qscdebat where inuse=1");
                return DBHelper.GetDBObjectByObj2_OnLive(new qscdebat(), null, commandtext, "qsc");
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}