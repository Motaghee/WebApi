using Common.db;
using System;

namespace Common.Models.QSC
{
    public class qscreft
    {
        public int Srl { get; set; }
        public string RefName { get; set; }
        public int Inuse { get; set; }
        public static object[] Get()
        {
            try
            {
                string commandtext = string.Format(@"select srl, refname, inuse from qscreft where inuse=1");
                return DBHelper.GetDBObjectByObj2_OnLive(new qscreft(), null, commandtext, "qsc");
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        // select srl, refname, inuse from qscreft; -- ارجاع به مدیریت
    }
}