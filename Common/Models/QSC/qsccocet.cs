using Common.db;
using System;

namespace Common.Models.QSC
{
    public class qsccocet
    {
        public int Srl { get; set; }
        public string CostCenterName { get; set; }
        public int Inuse { get; set; }

        //select * from QSCCoCet ; -- مرکز هزینه و مسئول

        public static object[] Get()
        {
            try
            {
                string commandtext = string.Format(@"select srl, costcentername, inuse from qsccocet where inuse=1");
                return DBHelper.GetDBObjectByObj2_OnLive(new qsccocet(), null, commandtext, "qsc");
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}