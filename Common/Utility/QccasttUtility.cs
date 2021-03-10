using Common.db;
using Common.Models;
using Common.Models.Qccastt;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Common.Models;

namespace Common.Utility
{
    public static class QccasttUtility
    {

        public static DataSet GetCarDefect(Qccastt qccastt, out object[] lstObj)
        {
            try
            {
                if ((qccastt != null)) // && (U.Macaddress == "48:13:7e:11:d7:1f"))
                {
                    qccastt.ValidFormat = CarUtility.CheckFormatVin(qccastt.Vin);
                    if (qccastt.ValidFormat)
                    {
                        qccastt.VinWithoutChar = CarUtility.GetVinWithoutChar(qccastt.Vin);
                        string commandtext = string.Format(@"select SYS_GUID() as Id,q.srl,q.vin,q.qcmdult_srl,q.qcbadft_srl,
                                                               q.qcareat_srl,a.areacode,a.areatype,p.shopcode,
                                                               s.strenghtdesc,
                                                               m.modulecode,
                                                               d.defectcode,
                                                               m.modulename,
                                                               d.defectdesc,c.bdmdlcode,
                                                               bm.grpcode,
                                                               cg.grpname,
                                                               t.title,
                                                               q.inuse,
                                                               a.areacode||a.areadesc as AreaDesc,
                                                               u.lname as CreatedByDesc,q.CreatedBy,
                                                               ur.lname as RepairedByDesc,q.RepairedBy,
                                                               TO_char(q.createddate,'YYYY/MM/DD HH24:MI:SS','nls_calendar=persian') as createddateFa,
                                                               to_char(q.createddate,'yyyy/mm/dd','nls_calendar=persian') as CreatedDayFa,
                                                               q.isrepaired
                                                          from qccastt q
                                                          join qcusert u on u.srl = q.createdby
                                                          left join qcusert ur on ur.srl = q.RepairedBy
                                                          join carid c on c.vin = q.vin 
                                                          join bodymodel bm on bm.bdmdlcode=c.bdmdlcode
                                                          join qcareat a
                                                            on q.qcareat_srl = a.srl
                                                     left join pcshopt p on p.srl = a.pcshopt_srl
                                                          join qcmdult m
                                                            on q.qcmdult_srl = m.srl
                                                          join qcbadft d
                                                            on q.qcbadft_srl = d.srl
                                                          join qcstrgt s
                                                            on q.qcstrgt_srl = s.srl
                                                          join cargroup cg on cg.grpcode=bm.grpcode
                                                          join qccabdt t on t.srl = d.qccabdt_srl
                                                         where q.inuse=1 and q.recordowner=1 and q.isdefected=1  
                                                         And q.vin= '{0}' order by q.createddate desc", qccastt.VinWithoutChar);

                        lstObj = DBHelper.GetDBObjectByObj2(new Qccastt(), null, commandtext, "ins");
                        return DBHelper.ExecuteMyQueryIns(commandtext);

                        // --
                        //string jsonString = string.Empty;
                        //jsonString = JsonConvert.SerializeObject(ds.Tables[0]);
                        //return jsonString;
                        // --
                        //List<Qccastt> FoundDefects = new List<Qccastt>();
                        //FoundDefects = DBHelper.GetDBObjectByDataSet(new Qccastt(), null, commandtext, "inspector").Cast<Qccastt>().ToList();
                        //---
                        //if (FoundDefects.Count > 0)
                        //{
                        //    FoundDefects[0].ValidFormat = qccastt.ValidFormat;
                        //    FoundDefects[0].VinWithoutChar = qccastt.VinWithoutChar;
                        //    return FoundDefects;
                        //}
                        //else
                        //{
                        //    List<Qccastt> q = new List<Qccastt>();
                        //    q.Add(qccastt);
                        //    return q;
                        //}
                    }
                    else
                    {
                        //List<Qccastt> q = new List<Qccastt>();
                        //q.Add(qccastt);
                        lstObj = null;
                        return null;
                    }

                }
                else
                {
                    DBHelper.LogtxtToFile("z null");
                    lstObj = null;
                    return null;
                }
            }
            catch (Exception ex)
            {
                //string err = e.ToString() + e.InnerException.Message + e.Message.ToString();
                //clsDBHelper.LogFile(e);
                DBHelper.LogFile(ex);
                //List<Qccastt> q = new List<Qccastt>();
                //q.Add(qccastt);
                lstObj = null;
                return null;
            }
        }

        public static object[] GetShop()
        {
            try
            {
                string commandtext = string.Format(@"select srl,p.shopcode,p.shopname,inuse,PtshopCode,'FALSE' as Checked from pcshopt p join pt.shop s on s.shopcode=p.ptshopcode where p.inuse=1 and s.companycode=82 ");
                return DBHelper.GetDBObjectByObj2(new Pcshopt(), null, commandtext, "ins");
                //return DBHelper.ExecuteMyQueryIns(commandtext);

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static object[] GetArea(string _AreaCodes)
        {
            try
            {
                if (!string.IsNullOrEmpty(_AreaCodes))
                {
                    _AreaCodes = string.Format(" And a.pcshopt_srl in ({0})", _AreaCodes);
                }
                string commandtext = string.Format(@"select srl,areacode,areadesc,areatype,pcshopt_srl,CheckDest from qcareat a  where a.inuse=1  {0} order by pcshopt_srl,areadesc", _AreaCodes);
                return DBHelper.GetDBObjectByObj2(new Area(), null, commandtext, "ins");
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static DataSet GetdsArea(string _AreaCodes)
        {
            try
            {
                if (!string.IsNullOrEmpty(_AreaCodes))
                {
                    _AreaCodes = string.Format(" And a.pcshopt_srl in ({0})", _AreaCodes);

                }
                string commandtext = string.Format(@"select srl,areacode,areadesc,areatype,pcshopt_srl,CheckDest from qcareat a  where a.inuse=1  {0} order by pcshopt_srl,areadesc", _AreaCodes);
                //return DBHelper.GetDBObjectByObj2(new Area(), null, commandtext, "ins");
                return DBHelper.ExecuteMyQueryIns(commandtext);

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static object[] GetBaseStrength()
        {
            try
            {
                string commandtext = string.Format(@"select  s.srl,s.strenghtcode,s.strenghtdesc from qcstrgt s order by strenghtdesc");
                return DBHelper.GetDBObjectByObj2(new Strength(), null, commandtext, "ins");
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        public static SelectList ToSelectList(this DataTable table, string valueField, string textField, bool blnAddNull)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            if (blnAddNull)
            {
                list.Add(new SelectListItem()
                {
                    Text = "همه",
                    Value = null
                });
            }
            foreach (DataRow row in table.Rows)
            {
                list.Add(new SelectListItem()
                {
                    Text = row[textField].ToString(),
                    Value = row[valueField].ToString()
                });
            }

            return new SelectList(list, "Value", "Text");
        }


        public static List<QccasttLight> GetCarDefectLight(string _Vin)
        {
            try
            {
                if ((_Vin != null)) // && (U.Macaddress == "48:13:7e:11:d7:1f"))
                {
                    bool VinValidFormat = CarUtility.CheckFormatVin(_Vin);
                    if (VinValidFormat)
                    {
                        string VinWithoutChar = CarUtility.GetVinWithoutChar(_Vin);
                        string commandtext = string.Format(@"select SYS_GUID() as Id,q.srl,'NAS' || q.vin as vin,
                                                               a.areacode||a.areadesc as AreaDesc,
                                                               m.modulename,
                                                               d.defectdesc,
                                                               TO_char(q.createddate,'YYYY/MM/DD HH24:MI:SS','nls_calendar=persian') as createddateFa,                                                               
                                                               s.strenghtdesc,
                                                               u.lname as CreatedByDesc                                                                               

                                                          from qccastt q
                                                          join qcusert u on u.srl = q.createdby
                                                          left join qcusert ur on ur.srl = q.RepairedBy
                                                          join carid c on c.vin = q.vin 
                                                          join bodymodel bm on bm.bdmdlcode=c.bdmdlcode
                                                          join qcareat a
                                                            on q.qcareat_srl = a.srl
                                                     left join pcshopt p on p.srl = a.pcshopt_srl
                                                          join qcmdult m
                                                            on q.qcmdult_srl = m.srl
                                                          join qcbadft d
                                                            on q.qcbadft_srl = d.srl
                                                          join qcstrgt s
                                                            on q.qcstrgt_srl = s.srl
                                                          join cargroup cg on cg.grpcode=bm.grpcode
                                                          join qccabdt t on t.srl = d.qccabdt_srl
                                                         where q.inuse=1 and q.recordowner=1 and q.isdefected=1  
                                                         And q.vin= '{0}' order by q.createddate desc", VinWithoutChar);

                        List<QccasttLight> lst = new List<QccasttLight>();
                        lst = DBHelper.GetDBObjectByObj2(new QccasttLight(), null, commandtext, "inspector").Cast<QccasttLight>().ToList();
                        return lst;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    DBHelper.LogtxtToFile("z null");
                    return null;
                }
            }
            catch (Exception ex)
            {
                DBHelper.LogFile(ex);
                return null;
            }
        }
    }
}