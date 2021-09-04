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
                cmd.CommandText = string.Format(@"insert into qcprot (vin, createdby, createddate,LocIsValid) VALUES  ('{0}',{1},sysdate,{2})", CarUtility.GetVinWithoutChar(_QCProT.Vin), _QCProT.CreatedBy, _QCProT.LocIsValid);
                if (DBHelper.LiveDBConnectionIns.State == ConnectionState.Closed)
                {
                    DBHelper.LiveDBConnectionIns.ConnectionString = DBHelper.CnStrInsLive;
                    DBHelper.LiveDBConnectionIns.Open();
                }
                cmd.Connection = DBHelper.LiveDBConnectionIns;
                cmd.ExecuteNonQuery();
                //---
                string commandtext = string.Format(@"select TO_char(p.createddate,'YYYY/MM/DD HH24:MI:SS','nls_calendar=persian') as ProCreatedDateFa
                                                                ,u.UserId,u.fname ||' '|| u.lname as ProCreatedByDesc
                                                                ,p.srl,p.vin,p.createdby,p.LocIsValid
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


        public static List<QCProT> GetQCProT(QCProT _QCProT)
        {
            try
            {
                //---
                string commandtext = string.Format(@"select q.srl,TO_char(q.createddate,'YYYY/MM/DD HH24:MI:SS','nls_calendar=persian') as ProCreatedDateFa,u.userid,u.fname ||'_'|| u.lname as ProCreatedByDesc,c.nasvin as vin,q.createdby,q.Locisvalid  
                                                    from QCProT q join qccariddt c on q.vin=c.vin  join qcusert u on u.srl = q.createdby                 
                                                    order by srl desc"); //where q.createddate like sysdate
                List<QCProT> lstP = new List<QCProT>();
                Object[] obj = DBHelper.GetDBObjectByObj2_OnLive(new QCProT(), null, commandtext, "inspector");
                lstP = obj.Cast<QCProT>().ToList();
                //---
                if (lstP.Count > 0)
                {
                    return lstP;
                }
                else
                    return null;
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