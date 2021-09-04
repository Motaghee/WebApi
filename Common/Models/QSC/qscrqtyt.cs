using Common.db;
using System;

namespace Common.Models.QSC
{
    public class qscrqtyt
    {
        public int Srl { get; set; }
        public string RequestTypeName { get; set; }
        public int Inuse { get; set; }

        public static object[] Get()
        {
            try
            {
                string commandtext = string.Format(@"select srl, requesttypename, inuse from qscrqtyt where inuse=1");
                return DBHelper.GetDBObjectByObj2_OnLive(new qscrqtyt(), null, commandtext, "qsc");
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        //select srl, requesttype, inuse from qscrqtyt; -- نوع درخواست درجه دو قابل فروش یا غیر قابل فروش
    }
}