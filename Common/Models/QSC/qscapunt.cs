using Common.db;
using System;
using System.Collections.Generic;

namespace Common.Models.QSC
{
    public class qscapunt
    {
        public int Srl { get; set; }
        public string AppunitName { get; set; }
        public int Inuse { get; set; }

        public static object[] Get()
        {
            try
            {
                string commandtext = string.Format(@"select srl, appunitname, inuse from qscapunt where inuse=1");
                return DBHelper.GetDBObjectByObj2_OnLive(new qscapunt(), null, commandtext, "qsc");
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
    
    // select srl, appunitname, inuse from qscapunt; --درخواست کننده
}

