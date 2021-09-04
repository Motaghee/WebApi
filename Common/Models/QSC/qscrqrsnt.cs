using Common.db;
using System;

namespace Common.Models.QSC
{
    public class qscrqrsnt
    {
        public int Srl { get; set; }
        public string ReqReasonDesc { get; set; }
        public int Inuse { get; set; }

        public static object[] Get()
        {
            try
            {
                string commandtext = string.Format(@"select srl, reqreasondesc, inuse from qscrqrsnt where inuse=1");
                return DBHelper.GetDBObjectByObj2_OnLive(new qscrqrsnt(), null, commandtext, "qsc");
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        //select srl, reqreasondesc, inuse from qscrqrsnt; --دلایل درخواست
    }
}