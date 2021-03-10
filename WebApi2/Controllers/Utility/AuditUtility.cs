using Common.db;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WebApi2.Models;

namespace WebApi2.Controllers.Utility
{
    public class AuditUtility
    {
        public static ResultMsg UnLockCar(string _Vin)
        {
            ResultMsg rm = new ResultMsg();
            try
            {
                if (DBHelper.DBConnectionIns.State == ConnectionState.Open)
                    DBHelper.DBConnectionIns.Close();
                if (DBHelper.DBConnectionIns.State == ConnectionState.Closed)
                {
                    DBHelper.DBConnectionIns.ConnectionString = DBHelper.CnStrInsLive;
                    DBHelper.DBConnectionIns.Open();
                }
                OracleCommand cmd = new OracleCommand();
                OracleDataAdapter da = new OracleDataAdapter();
                cmd.Connection = DBHelper.DBConnectionIns;
                da.SelectCommand = cmd;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_AuditUnlockCar";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("pVin", OracleDbType.Varchar2).Value = _Vin.ToUpper();
                cmd.Parameters.Add("pMsgResult", OracleDbType.Varchar2, 2048);
                cmd.Parameters["pMsgResult"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                string result = cmd.Parameters["pMsgResult"].Value.ToString();
                rm.title = rm.Message = result;
                return rm;

            }
            catch (Exception ex)
            {
                rm.title = "error";
                rm.Message = ex.Message.ToString();
                return rm;
            }
            finally 
            {
                DBHelper.DBConnectionIns.Close();
            }

        }


        public static List<AuditCarDetail> GetAuditCarCheckList(AuditCarDetail auditcardetails)
        {
            try
            {

                if ((auditcardetails != null)) // && (U.Macaddress == "48:13:7e:11:d7:1f"))
                {
                    auditcardetails.ValidFormat = CarUtility.CheckFormatVin(auditcardetails.Vin);
                    if (auditcardetails.ValidFormat)
                    {
                        auditcardetails.VinWithoutChar = CarUtility.GetVinWithoutChar(auditcardetails.Vin);
                        string commandtext = string.Format(@"select d.srl,
                                                                   a.vin,
                                                                   a.areacode,
                                                                   d.strenghtdesc,
                                                                   d.modulecode,
                                                                   d.defectcode,
                                                                   d.modulename,
                                                                   d.defectdesc,
                                                                   d.title,
                                                                   a.areacode || a.areadesc as AreaDesc,a.Auditor2,
                                                                   a.Auditor2 as CreatedByDesc,
                                                                   a.CreatedBy,
                                                                   a.AUDITDATE_fa ||' '|| a.CREATEDTIME as AuditDateFa
                                                              from sva_v_auditcardetail d 
                                                                   left join sva_v_auditcar a on a.srl = d.svaauditcar_srl
                                                              where a.vin = '{0}'
                                                            ", auditcardetails.Vin);
                        DataSet ds = DBHelper.ExecuteMyQueryIns(commandtext);
                        // --
                        //string jsonString = string.Empty;
                        //jsonString = JsonConvert.SerializeObject(ds.Tables[0]);
                        //return jsonString;
                        // --
                        List<AuditCarDetail> FoundDefects = new List<AuditCarDetail>();
                        FoundDefects = DBHelper.GetDBObjectByObj2(new AuditCarDetail(), null, commandtext, "inspector").Cast<AuditCarDetail>().ToList();
                        //---
                        if (FoundDefects.Count > 0)
                        {
                            FoundDefects[0].ValidFormat = auditcardetails.ValidFormat;
                            FoundDefects[0].VinWithoutChar = auditcardetails.VinWithoutChar;
                            FoundDefects[0].Msg = auditcardetails.Msg = "";
                            return FoundDefects;
                        }
                        else
                        {
                            List<AuditCarDetail> q = new List<AuditCarDetail>();
                            auditcardetails.Msg = "اطلاعاتی یافت نشد";
                            q.Add(auditcardetails);
                            return null;
                        }
                    }
                    else
                    {
                        List<AuditCarDetail> q = new List<AuditCarDetail>();
                        auditcardetails.Msg = "شاسی غیر مجاز است";
                        q.Add(auditcardetails);
                        return q;
                    }

                }
                else
                {
                    DBHelper.LogtxtToFile("z null");
                    return null;
                }
            }
            catch (Exception e)
            {
                //string err = e.ToString() + e.InnerException.Message + e.Message.ToString();
                DBHelper.LogFile(e);
                List<AuditCarDetail> q = new List<AuditCarDetail>();
                auditcardetails.Msg = e.Message;
                q.Add(auditcardetails);
                return q;
            }

        }

       

    }
}