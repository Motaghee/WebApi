using Common.db;
using System;

namespace Common.Models.QSC
{
    public class qscrsnt
    {
        public int Srl { get; set; }
        public string ReqReasonDesc { get; set; }
        public int Inuse { get; set; }

        public static object[] Get()
        {
            try
            {
                string commandtext = string.Format(@"select srl, reqreasondesc, inuse from qscrsnt where inuse=1");
                return DBHelper.GetDBObjectByObj2_OnLive(new qscrsnt(), null, commandtext, "qsc");
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        //select srl, reqreasondesc, inuse from qscrqrsnt; --دلایل درخواست
    }
}