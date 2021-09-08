using Common.db;
using System;

namespace Common.Models.QSC
{
    public class qscordet
    {
        public int Srl { get; set; }
        public string OriginDesc { get; set; }
        public int Inuse { get; set; }

        public static object[] Get()
        {
            try
            {
                string commandtext = string.Format(@"select srl, origindesc, inuse from qscordet where inuse=1");
                return DBHelper.GetDBObjectByObj2_OnLive(new qscordet(), null, commandtext, "qsc");
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        //select srl, origindesc, inuse from qscordet ;--جدول علل شناسايي در زمان بررسي کيفيت
    }
}