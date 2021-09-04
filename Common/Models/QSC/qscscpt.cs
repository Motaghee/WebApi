using Common.db;
using System;

namespace Common.Models.QSC
{
    public class qscscpt
    {
        public int Srl { get; set; }
        public string ScopeDesc { get; set; }
        public int Inuse { get; set; }

        public static object[] Get()
        {
            try
            {
                string commandtext = string.Format(@"select srl, scopedesc, inuse from qscscpt where inuse=1");
                return DBHelper.GetDBObjectByObj2_OnLive(new qscscpt(), null, commandtext, "qsc");
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        //select srl, scopedesc, inuse from qscscpt; --حوزه ی درخواست
    }
}