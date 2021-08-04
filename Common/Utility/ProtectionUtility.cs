using Common.db;
using Common.Models.General;
using Common.Models.Protection;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Common.Utility
{
    public static class ProtectionUtility
    {
        public static QCProT InsertQCProT(QCProT _QCProT)
        {
            try
            {
                OracleCommand cmd = new OracleCommand();
                cmd.CommandText = string.Format(@"delete qcprot where vin=  '{0}'", CarUtility.GetVinWithoutChar(_QCProT.Vin), _QCProT.CreatedBy);
                if (DBHelper.LiveDBConnectionIns.State == ConnectionState.Closed)
                {
                    DBHelper.LiveDBConnectionIns.ConnectionString = DBHelper.CnStrInsLive;
                    DBHelper.LiveDBConnectionIns.Open();
                }
                cmd.Connection = DBHelper.LiveDBConnectionIns;
                cmd.ExecuteNonQuery();
                //----
                cmd.CommandText = string.Format(@"insert into qcprot (vin, createdby, createddate) VALUES  ('{0}',{1},sysdate)", CarUtility.GetVinWithoutChar(_QCProT.Vin),_QCProT.CreatedBy);
                if (DBHelper.LiveDBConnectionIns.State == ConnectionState.Closed)
                {
                    DBHelper.LiveDBConnectionIns.ConnectionString = DBHelper.CnStrInsLive;
                    DBHelper.LiveDBConnectionIns.Open();
                }
                cmd.Connection = DBHelper.LiveDBConnectionIns;
                cmd.ExecuteNonQuery();
                //---
                string commandtext = string.Format(@"select TO_char(p.createddate,'YYYY/MM/DD HH24:MI:SS','nls_calendar=persian') as ProCreatedDateFa
                                                                ,u.fname ||' '|| u.lname as ProCreatedByDesc
                                                                ,p.srl,p.vin,p.createdby
                                                                from qcprot p left join qcusert u on p.createdby = u.srl
                                                                where vin ='{0}'", CarUtility.GetVinWithoutChar(_QCProT.Vin));
                QCProT p = DBHelper.GetDBObjectByObj2_OnLive(new QCProT(), null, commandtext, "inspector").Cast<QCProT>().ToList()[0];
                return p;
            }
            catch (Exception e)
            {
                DBHelper.LogtxtToFile("err_InsertQCProT_" + e.Message.ToString());
                DBHelper.LogFile(e);
                return null;
            }

        }
    }
}