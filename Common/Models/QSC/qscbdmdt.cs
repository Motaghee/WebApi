using Common.db;
using System;

namespace Common.Models.QSC
{
    public class qscbdmdt
    {
        public int Srl { get; set; }
        public string ModuleName { get; set; }
        public int Inuse { get; set; }

        //select srl, modulename, inuse from qscbdmdt for update; -- لیست قطعات بدنه و قطعات جانبی
        public static object[] Get()
        {
            try
            {
                string commandtext = string.Format(@"select srl, modulename, inuse from qscbdmdt where inuse=1");
                return DBHelper.GetDBObjectByObj2_OnLive(new qscbdmdt(), null, commandtext, "qsc");
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}