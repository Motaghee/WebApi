using Common.db;
using System;
using System.Collections.Generic;

namespace Common.Models.QSC
{
    public class qscrqfistt
    {
        public int Srl { get; set; }
        public string finalstatusname { get; set; }
        public int Inuse { get; set; }
        //select srl, finalstatusname, inuse from QSCRQFIStT for update;-- وضعیت نهایی درجه 1 یا دو مزایده قابل فروش غیر قابل فروش

        public static object[] Get() 
        {
            try
            {
                string commandtext = string.Format(@"select srl, finalstatusname, inuse from QSCRQFIStT where inuse=1");
                return DBHelper.GetDBObjectByObj2_OnLive(new qscrqfistt(), null, commandtext, "qsc");
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }
    }

}