using Common.Actions;
using Common.db;
using Common.Models;
using Common.Models.Car;
using Common.Models.General;
using Common.Models.PT;
using Common.Models.QccasttModels;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;

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
                        string commandtext = string.Format(@"select null as Id,q.srl,q.vin,q.qcmdult_srl,q.qcbadft_srl,
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

        public static List<Qccastt> GetCarDefect(Qccastt qccastt)
        {
            try
            {
                if ((qccastt != null)) // && (U.Macaddress == "48:13:7e:11:d7:1f"))
                {
                    #region Condition
                    PersianCalendar pc = new PersianCalendar();
                    DateTime dtN = DateTime.Now;
                    string Y = pc.GetYear(dtN).ToString();
                    string M = pc.GetMonth(dtN).ToString().PadLeft(2, '0');
                    string D = pc.GetDayOfMonth(dtN).ToString().PadLeft(2, '0');
                    string strDateCondition = "";
                    string Condition = " q.recordowner=1 and q.isdefected=1 ";
                    //---
                    if (qccastt.DateType == "M")
                        strDateCondition = string.Format(@" createddate >= to_date('{0}/{1}/01','yyyy/mm/dd','nls_calendar=persian') ", Y, M);
                    else if (qccastt.DateType == "D")
                        strDateCondition = string.Format(@" createddate >= to_date('{0}/{1}/{2}','yyyy/mm/dd','nls_calendar=persian') ", Y, M, D);

                    if (!string.IsNullOrEmpty(qccastt.InListQCSTRGTSRL))
                        Condition += string.Format(@" And  q.qcstrgt_srl in ({0})", qccastt.InListQCSTRGTSRL);

                    if (qccastt.deletedby == 0)
                        Condition += string.Format(@" And q.inuse=1 ");
                    else if (qccastt.deletedby == -1)
                    {
                        Condition += string.Format(@" And q.deletedby is not null", qccastt.deletedby);
                        Condition += string.Format(@" And q.qcareat_srl in ('{0}')", qccastt.ActAreaSrl);
                    }
                    else
                    {
                        Condition += string.Format(@" And q.deletedby in (select srl from qcusert where userid in ({0})) ", qccastt.deletedby);
                        Condition += string.Format(@" And q.qcareat_srl in ('{0}')", qccastt.ActAreaSrl);
                    }
                    if (!string.IsNullOrEmpty(qccastt.Vin))
                    {
                        qccastt.VinWithoutChar = CarUtility.GetVinWithoutChar(qccastt.Vin);
                        Condition += string.Format(@" And q.vin = '{0}'", qccastt.VinWithoutChar);
                    }
                    if (!string.IsNullOrEmpty(strDateCondition))
                    {
                        Condition = strDateCondition + " And " + Condition;
                    }

                    //if ((qccastt.ActAreaSrl != 0))
                    //{
                    //    Condition += string.Format(@" And q.qcareat_srl in ('{0}')", qccastt.ActAreaSrl);
                    //}
                    #endregion


                    // SYS_GUID() as Id,
                    string commandtext = string.Format(@"select null as Id,q.srl,'NAS'||q.vin as vin,q.vin as VinWithoutChar,q.qcmdult_srl,q.qcbadft_srl,
                                                               q.qcareat_srl,a.areacode,a.areatype,p.shopcode,
                                                               s.strenghtdesc,
                                                               m.modulecode,
                                                               d.defectcode,
                                                               m.modulename,
                                                               d.defectdesc,c.bdmdlcode,
                                                               bm.grpcode,
                                                               cg.grpname,
                                                               t.title,q.RecordOwner,q.CHECKLISTAREA_SRL,q.QCSTRGT_SRL,q.IsDefected,q.InUse,
                                                               a.areacode||' '|| a.areadesc as AreaDesc,
                                                               u.lname as CreatedByDesc,q.CreatedBy,
                                                               ur.lname as RepairedByDesc,q.RepairedBy,
                                                               ud.lname as DeletedByDesc,q.DeletedBy,
                                                               TO_char(q.createddate,'YYYY/MM/DD HH24:MI:SS','nls_calendar=persian') as createddateFa,
                                                               TO_char(q.repaireddate,'YYYY/MM/DD HH24:MI:SS','nls_calendar=persian') as repaireddateFa,
                                                               TO_char(q.deleteddate,'YYYY/MM/DD HH24:MI:SS','nls_calendar=persian') as DeleteddateFa,
                                                               to_char(q.createddate,'yyyy/mm/dd','nls_calendar=persian') as CreatedDayFa,
                                                               q.isrepaired,
                                                               {0} as ActAreaSrl,{1} as ActBy
                                                          from qccastt q
                                                          join qcusert u on u.srl = q.createdby
                                                          left join qcusert ur on ur.srl = q.RepairedBy
                                                          left join qcusert ud on ud.srl = q.deletedby
                                                          join qccariddt c on c.vin = q.vin 
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
                                                         where {2} 
                                                          order by q.createddate desc",
                                                      qccastt.ActAreaSrl.ToString(), qccastt.ActBy.ToString(), Condition);
                    //DataSet ds = clsDBHelper.ExecuteMyQueryIns(commandtext);
                    // --
                    //string jsonString = string.Empty;
                    //jsonString = JsonConvert.SerializeObject(ds.Tables[0]);
                    //return jsonString;
                    // --
                    List<Qccastt> FoundDefects = new List<Qccastt>();
                    Object[] obj = DBHelper.GetDBObjectByObj2_OnLive(new Qccastt(), null, commandtext, "inspector");
                    FoundDefects = obj.Cast<Qccastt>().ToList();
                    //---
                    if (FoundDefects.Count > 0)
                    {
                        //for (int i = 0; i < FoundDefects.Count; i++)
                        //    FoundDefects[i].Id = "";
                        // -
                        FoundDefects[0].ValidFormat = qccastt.ValidFormat;
                        //FoundDefects[0].VinWithoutChar = qccastt.VinWithoutChar;
                        return FoundDefects;
                    }
                    else
                    {
                        List<Qccastt> q = new List<Qccastt>();
                        q.Add(qccastt);
                        return q;
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
                //string err = e.ToString() + e.InnerException.Message + e.Message.ToString();
                //clsDBHelper.LogFile(e);
                DBHelper.LogFile(ex);
                List<Qccastt> q = new List<Qccastt>();
                q.Add(qccastt);
                return q;
            }
        }

        public static List<Qcqctrt> GetCarTrace(Qccastt qccastt)
        {
            try
            {
                if ((qccastt != null)) // && (U.Macaddress == "48:13:7e:11:d7:1f"))
                {
                    qccastt.ValidFormat = CarUtility.CheckFormatVin(qccastt.Vin);
                    if (qccastt.ValidFormat)
                    {
                        qccastt.VinWithoutChar = CarUtility.GetVinWithoutChar(qccastt.Vin);
                        string commandtext = string.Format(@"select rownum as RowIndex,z.* from (select tr.srl,case when length(tr.vin) = 14 then 'NAS' || tr.vin else 'S' || tr.vin end as VIN,
                                                           a1.areacode as FromAreaCode,
                                                           a1.areadesc as FromAreaDesc,
                                                           to_char(a1.areacode) || '_' || a1.areadesc as FromAreaCodeDesc,
                                                           a2.areaCode as ToAreaCode,
                                                           a2.areadesc as ToAreaDesc,
                                                           to_char(a2.areacode) || '_' || a2.areadesc as ToAreaCodeDesc,
                                                           tr.passed,
                                                           TO_char(tr.u_date,'YYYY/MM/DD HH24:MI','nls_calendar=persian') as u_dateFA,
                                                           u.userid,u.fname,u.lname,u.fname || '_' || u.lname as USERFAMILYNAME
                                                      from qcqctrt tr join qcareat a1 on tr.fromareasrl = a1.srl
                                                      join qcareat a2 on tr.toareasrl = a2.srl
                                                      join qcusert u on tr.uauser_srl = u.srl where tr.vin = '{0}'
                                                     order by tr.seq desc)z", qccastt.VinWithoutChar);
                        // --
                        List<Qcqctrt> qcTrace = new List<Qcqctrt>();
                        qcTrace = DBHelper.GetDBObjectByObj2_OnLive(new Qcqctrt(), null, commandtext, "inspector").Cast<Qcqctrt>().ToList();
                        //---

                        return qcTrace;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static int GetLastStopTime(string vin)
        {
            try
            {
                string commandtext = string.Format(@"select trunc((sysdate-q.u_date)*24) as LastStopTime  from qcqctrt q where vin = '{0}'
                                                       and seq = (select max(seq)
                                                                    from qcqctrt
                                                                    where vin = '{0}'
                                                                  group by vin) 
                                                       and passed = 0", vin);
                DataSet ds = DBHelper.ExecuteMyQueryInsOnLive(commandtext);
                if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    return Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
                }
                else
                    return -1;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public static int GettCurrentAreaDefectCoun(string vin, string AreaSrl)
        {
            try
            {
                string commandtext = string.Format(@"select count(srl) from qccastt q where q.vin = '{0}' 
                                                        and q.qcareat_srl={1} and q.isdefected=1 
                                                        and q.inuse=1 and q.recordowner=1 "
                                                        , vin, AreaSrl);
                DataSet ds = DBHelper.ExecuteMyQueryInsOnLive(commandtext);
                if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    return Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
                }
                else
                    return -1;
            }
            catch (Exception ex)
            {
                return -1;
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

        public static Area GetAreaBySrl(string _AreaSrl)
        {
            try
            {
                string commandtext = string.Format(@"select  a.srl,a.areacode,a.areadesc,a.CheckDest,p.ptshopcode,a.IsAuditArea,
                                        decode(a.areatype,35,(decode(p.shopcode,14,30,17,40)),a.areatype) as areatype,PCShopt_Srl
                                        from qcareat a join pcshopt p on p.srl = a.pcshopt_srl where a.srl in ({0})", _AreaSrl);
                DataSet ds = DBHelper.ExecuteMyQueryIns(commandtext);
                List<Area> AreaList = new List<Area>();
                AreaList = DBHelper.GetDBObjectByObj2(new Area(), null, commandtext, "inspector").Cast<Area>().ToList();
                //---
                if (AreaList.Count > 0)
                {
                    return AreaList[0];
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                DBHelper.LogFile(ex);
                return null;
            }
        }

        public static Area GetVinCurrentArea(string _vin)
        {
            try
            {
                string commandtext = string.Format(@"select  a.srl,a.areacode,a.areadesc,a.CheckDest,p.ptshopcode,a.IsAuditArea,
                                        decode(a.areatype,35,(decode(p.shopcode,14,30,17,40)),a.areatype) as areatype,PCShopt_Srl
                                        from qcareat a join pcshopt p on p.srl = a.pcshopt_srl
                                        where a.srl in (select toareasrl from qcqctrt q where q.vin = '{0}' and q.passed=0)"
                                           , _vin);
                DataSet ds = DBHelper.ExecuteMyQueryIns(commandtext);
                List<Area> AreaList = new List<Area>();
                AreaList = DBHelper.GetDBObjectByObj2(new Area(), null, commandtext, "inspector").Cast<Area>().ToList();
                //---
                if (AreaList.Count > 0)
                {
                    return AreaList[0];
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                DBHelper.LogFile(ex);
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

        public static QCUsert GetQCUserT(string _QcUsertSrl)
        {
            try
            {
                string commandtext = string.Format(@"select u.srl,u.Fname,u.lname,u.username from qcusert u where srl = {0}"
                                           , _QcUsertSrl);
                //DataSet ds = DBHelper.ExecuteMyQueryIns(commandtext);
                List<QCUsert> List = new List<QCUsert>();
                List = DBHelper.GetDBObjectByObj2(new QCUsert(), null, commandtext, "inspector").Cast<QCUsert>().ToList();
                //---
                if (List.Count > 0)
                {
                    return List[0];
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                DBHelper.LogFile(ex);
                return null;
            }
        }

        public static Qcdsart GetQcdsartByPath(int _FromAreaSrl, int _ToAreaSrl)
        {
            try
            {
                string commandtext = string.Format(@"select  d.fromareasrl,d.toareasrl,d.IsDefault,d.grpcode,d.isreject,d.inuse from Qcdsart d where d.fromareasrl ={0} and d.toareasrl = {1}"
                                           , _FromAreaSrl.ToString(), _ToAreaSrl.ToString());
                //DataSet ds = DBHelper.ExecuteMyQueryIns(commandtext);
                List<Qcdsart> List = new List<Qcdsart>();
                List = DBHelper.GetDBObjectByObj2(new Qcdsart(), null, commandtext, "inspector").Cast<Qcdsart>().ToList();
                //---
                if (List.Count > 0)
                {
                    return List[0];
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                DBHelper.LogFile(ex);
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


        public static List<CarImage> GetCarImages(CarImage carImage)
        {
            try
            {
                //clsDBHelper.LogtxtToFile("00000 GetCarImages" + car.Vin);
                string commandtext = "";
                if ((!carImage.Srl.Equals(null)) && (carImage.Srl != 0))
                {
                    commandtext = string.Format(@"select i.Srl,
                                                                    'NAS'||i.Vin as Vin,
                                                                    i.Title,
                                                                    i.ImageDesc,
                                                                    i.CreatedBy,
                                                                    i.QCAreatSrl,
                                                                    i.Image,i.Thumbnail,
                                                                    i.inuse,i.updatedby,
                                                                    u.fname || ' ' || u.lname as createdbydesc,
                                                                    i.qcareatsrl,
                                                                    a.areacode || ' ' || a.areadesc as areadesc,
                                                                    TO_char(i.createddate,
                                                                            'YYYY/MM/DD HH24:MI:SS',
                                                                            'nls_calendar=persian') as createddateFa
                                                                from qccarimgt i
                                                                left join qcusert u
                                                                on u.srl = i.createdby
                                                                left join qcareat a
                                                                on a.srl = i.qcareatsrl
                                                                where i.inuse = 1
                                                                and i.Srl = {0}
                                                            ", carImage.Srl);
                }
                else
                {
                    commandtext = string.Format(@"select i.Srl,
                                                                    'NAS'||i.Vin as Vin,
                                                                    i.Title,
                                                                    i.ImageDesc,
                                                                    i.CreatedBy,
                                                                    i.QCAreatSrl,
                                                                    null as Image,i.Thumbnail,
                                                                    i.inuse,i.updatedby,
                                                                    u.fname || ' ' || u.lname as createdbydesc,
                                                                    i.qcareatsrl,
                                                                    a.areacode || ' ' || a.areadesc as areadesc,
                                                                    TO_char(i.createddate,
                                                                            'YYYY/MM/DD HH24:MI:SS',
                                                                            'nls_calendar=persian') as createddateFa
                                                                from qccarimgt i
                                                                left join qcusert u
                                                                on u.srl = i.createdby
                                                                left join qcareat a
                                                                on a.srl = i.qcareatsrl
                                                                where i.inuse = 1
                                                                and i.vin = '{0}'
                                                            ", CarUtility.GetVinWithoutChar(carImage.Vin));
                }


                // --
                List<CarImage> FoundCarImages = new List<CarImage>();
                FoundCarImages = DBHelper.GetDBObjectByObj2(new CarImage(), null, commandtext, "inspector").Cast<CarImage>().ToList();
                //---
                if (FoundCarImages.Count > 0)
                {
                    return FoundCarImages;
                }
                else
                {
                    DBHelper.LogtxtToFile("count 0");
                    return null;
                }
            }
            catch (Exception e)
            {
                //string err = e.ToString() + e.InnerException.Message + e.Message.ToString();
                DBHelper.LogFile(e);
                return null;
            }
        }

        public static CarImage UpdateCarImage(CarImage carImage)
        {
            string errTrc = carImage.Vin + "_1_";
            try
            {

                OracleCommand cmd = new OracleCommand();
                if (carImage.Inuse == 1)
                {
                    if (carImage.Updated == false) //insert new image
                    {
                        if (carImage.Image == null)
                        {
                            errTrc += "null image_";
                        }


                        cmd.CommandText = string.Format(@"INSERT INTO qccarimgt (Srl,Vin,Title,ImageDesc,CreatedBy,CreatedDate,QCAreatSrl, Image,Thumbnail)
                        VALUES ({0},'{1}','{2}','{3}',{4},sysdate,{5},:blobImage,:blobThumbnail)"
                       , carImage.Srl, CarUtility.GetVinWithoutChar(carImage.Vin), carImage.Title, carImage.ImageDesc, carImage.CreatedBy,
                       carImage.QCAreatSrl);
                        OracleParameter blobImage = new OracleParameter();
                        blobImage.OracleDbType = OracleDbType.Blob;
                        blobImage.ParameterName = ":blobImage";
                        blobImage.Direction = ParameterDirection.Input;
                        byte[] bImage = Convert.FromBase64String(carImage.Image);
                        blobImage.Value = bImage;
                        cmd.Parameters.Add(blobImage);
                        //--
                        OracleParameter blobThumbnail = new OracleParameter();
                        blobThumbnail.OracleDbType = OracleDbType.Blob;
                        blobThumbnail.ParameterName = ":blobThumbnail";
                        blobThumbnail.Direction = ParameterDirection.Input;
                        byte[] bThumbnail = Convert.FromBase64String(carImage.Thumbnail);
                        blobThumbnail.Value = bThumbnail;
                        cmd.Parameters.Add(blobThumbnail);


                    }
                    else         //update desc of image
                    {
                        cmd.CommandText = string.Format(@"update qccarimgt i
                                                   set i.Title = {1},
                                                       i.ImageDesc={2}
                                                       i.updatedby = {3},
                                                       i.u_date= sysdate
                                                   where srl={0}
                                                ", carImage.Srl, carImage.Title, carImage.ImageDesc, carImage.UpdatedBy);

                    }
                }
                else
                {
                    cmd.CommandText = string.Format(@"update qccarimgt i
                                                   set i.inuse = 0,
                                                       i.updatedby = {1},
                                                       i.u_date= sysdate
                                                   where srl={0}
                                                ", carImage.Srl, carImage.UpdatedBy);
                }
                if (DBHelper.DBConnectionIns.State == ConnectionState.Closed)
                {
                    DBHelper.DBConnectionIns.ConnectionString = DBHelper.CnStrIns;
                    DBHelper.DBConnectionIns.Open();
                }
                cmd.Connection = DBHelper.DBConnectionIns;
                cmd.ExecuteNonQuery();
                // ---
                //cmd.Parameters.Clear();
                string commandtext = "";
                if (carImage.Inuse == 1)
                {
                    commandtext = string.Format(@"select i.Srl,
                                                            'NAS' || i.Vin as Vin,
                                                            i.Title,
                                                            i.ImageDesc,
                                                            i.CreatedBy,
                                                            i.QCAreatSrl,Thumbnail,
                                                            null as Image,i.Inuse,i.updatedby,
                                                            u.fname ||' '|| u.lname as createdbydesc,
                                                            a.areacode || ' ' || a.areadesc as areadesc,
                                                            TO_char(i.createddate,'YYYY/MM/DD HH24:MI:SS','nls_calendar=persian') as createddateFa
                                                        from qccarimgt i
                                                        left join qcusert u
                                                        on u.srl = i.createdby
                                                        left join qcareat a
                                                        on a.srl = i.qcareatsrl
                                                        Where i.Srl ={0}", carImage.Srl);
                }
                else
                {
                    commandtext = string.Format(@"select i.Srl,
                                                                'NAS' || i.Vin as Vin,
                                                                i.Title,
                                                                i.ImageDesc,
                                                                i.CreatedBy,
                                                                i.QCAreatSrl,null as Thumbnail,
                                                                null as Image,i.Inuse,i.updatedby,
                                                                u.fname ||' '|| u.lname as createdbydesc,
                                                                a.areacode || ' ' || a.areadesc as areadesc,
                                                                TO_char(i.createddate,'YYYY/MM/DD HH24:MI:SS','nls_calendar=persian') as createddateFa
                                                            from qccarimgt i
                                                            left join qcusert u
                                                            on u.srl = i.createdby
                                                            left join qcareat a
                                                            on a.srl = i.qcareatsrl
                                                            Where i.Srl ={0}", carImage.Srl);
                }
                CarImage InsertedCarImage = DBHelper.GetDBObjectByObj2(new CarImage(), null, commandtext, "inspector").Cast<CarImage>().ToList()[0];
                return InsertedCarImage;
            }
            catch (Exception e)
            {
                DBHelper.LogtxtToFile("err_UpdateCarImage_" + errTrc);
                DBHelper.LogFile(e);


                return null;
            }
        }

        //GetModuleList
        public static List<Module> GetBaseModuleList()
        {
            try
            {
                string commandtext = string.Format(@"select srl,modulename,modulecode from qcmdult m ");
                DataSet ds = DBHelper.ExecuteMyQueryIns(commandtext);
                List<Module> ModuleList = new List<Module>();
                ModuleList = DBHelper.GetDBObjectByObj2(new Module(), null, commandtext, "inspector").Cast<Module>().ToList();
                //---
                if (ModuleList.Count > 0)
                {
                    return ModuleList;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                DBHelper.LogFile(e);
                return null;
            }
        }


        public static List<Defect> GetBaseDefectList()
        {
            try
            {
                string commandtext = string.Format(@"select srl,defectcode,defectdesc from qcbadft d");
                DataSet ds = DBHelper.ExecuteMyQueryIns(commandtext);
                List<Defect> DefectList = new List<Defect>();
                DefectList = DBHelper.GetDBObjectByObj2(new Defect(), null, commandtext, "inspector").Cast<Defect>().ToList();
                //---
                if (DefectList.Count > 0)
                {
                    return DefectList;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {

                DBHelper.LogFile(ex);
                return null;
            }
        }

        public static List<Strength> GetBaseStrengthList()
        {
            try
            {
                string commandtext = string.Format(@"select  s.srl,s.strenghtcode,s.strenghtdesc from qcstrgt s");
                DataSet ds = DBHelper.ExecuteMyQueryIns(commandtext);
                List<Strength> StrengthList = new List<Strength>();
                StrengthList = DBHelper.GetDBObjectByObj2(new Strength(), null, commandtext, "inspector").Cast<Strength>().ToList();
                //---
                if (StrengthList.Count > 0)
                {
                    return StrengthList;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                DBHelper.LogFile(ex);
                return null;
            }
        }


        public static List<Qcdfctt> GetAreaCheckList(Area _area)
        {
            try
            {
                string commandtext = string.Format(@"select distinct q.srl ,q.qcmdult_srl,
                                                        q.qcbadft_srl,q.qcstrgt_srl,q.grpcode  from qcdfctt q 
				                                        where q.qcareat_srl = {0}",
                                                    _area.Srl);
                DataSet ds = DBHelper.ExecuteMyQueryIns(commandtext);
                List<Qcdfctt> CheckList = new List<Qcdfctt>();
                CheckList = DBHelper.GetDBObjectByObj2(new Qcdfctt(), null, commandtext, "inspector").Cast<Qcdfctt>().ToList();
                //---
                if (CheckList.Count > 0)
                {
                    return CheckList;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                DBHelper.LogFile(e);
                return null;
            }
        }



        public static List<Area> GetBaseAreaList()
        {
            try
            {
                string commandtext = string.Format(@"select  a.srl,a.areacode,a.areadesc,a.CheckDest,p.ptshopcode,a.IsAuditArea,
                                        decode(a.areatype,35,(decode(p.shopcode,14,30,17,40)),a.areatype) as areatype,PCShopt_Srl
                                        from qcareat a join pcshopt p on p.srl = a.pcshopt_srl ");
                DataSet ds = DBHelper.ExecuteMyQueryIns(commandtext);
                List<Area> AreaList = new List<Area>();
                AreaList = DBHelper.GetDBObjectByObj2(new Area(), null, commandtext, "inspector").Cast<Area>().ToList();
                //---
                if (AreaList.Count > 0)
                {
                    return AreaList;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                DBHelper.LogFile(ex);
                return null;
            }
        }

        public static List<Area> GetGetUserAreaPermision(User _user)
        {
            try
            {
                string commandtext = string.Format(@"select distinct ar.srl,ar.pcshopt_srl,ar.areacode,ar.areadesc,ar.areacode,ar.checkdest ,ar.areatype,p.ptshopcode,ar.areatype,ar.IsAuditArea
                                                  from qcusert ut join qcussft us  on ut.srl = us.qcusert_srl 
                                                  join qcareat ar on us.parameter_srl = ar.srl
                                                  join pcshopt p on p.srl = ar.pcshopt_srl 
                                                  where ut.username = '{0}' and us.inuse = 1 and ut.inuse = 1 and ar.IsAuditArea <> 1
                                                 order by ar.areacode desc ", _user.USERID);
                //DataSet ds = DBHelper.ExecuteMyQueryIns(commandtext);
                List<Area> AreaList = new List<Area>();
                Object[] obj = DBHelper.GetDBObjectByObj2_OnLive(new Area(), null, commandtext, "inspector");
                if (obj != null)
                {
                    AreaList = obj.Cast<Area>().ToList();
                    return AreaList;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                DBHelper.LogFile(ex);
                return null;
            }
        }


        public static List<Shop> GetBaseShopList()
        {
            try
            {
                string commandtext = string.Format(@"select p.shopcode,p.shopname from pcshopt p join pt.shop s on s.shopcode=p.ptshopcode where companycode=82");
                DataSet ds = DBHelper.ExecuteMyQueryIns(commandtext);
                List<Shop> List = new List<Shop>();
                List = DBHelper.GetDBObjectByObj2(new Shop(), null, commandtext, "inspector").Cast<Shop>().ToList();
                //---
                if (List.Count > 0)
                {
                    return List;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                DBHelper.LogFile(ex);
                return null;
            }
        }

        public static List<CarGroup> GetBaseCarGroupList()
        {
            try
            {
                string commandtext = string.Format(@"select GrpCode,GrpName,SmsTitle from pt.cargroup cg");
                DataSet ds = DBHelper.ExecuteMyQueryIns(commandtext);
                List<CarGroup> List = new List<CarGroup>();
                List = DBHelper.GetDBObjectByObj2(new CarGroup(), null, commandtext, "inspector").Cast<CarGroup>().ToList();
                //---
                if (List.Count > 0)
                {
                    return List;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                DBHelper.LogFile(ex);
                return null;
            }
        }

        public static List<BodyModel> GetBaseBodyModelList()
        {
            try
            {
                string commandtext = string.Format(@"select bdmdlcode,grpcode,aliasname,case when bm.aliasname='تيبا 211' then 'تیبا2' when bm.aliasname='تيبا 212'  or bm.aliasname='تيبا 232'  then gs.name else bm.aliasname end  as CommonBodyModelName  from pt.bodymodel bm JOIN pt.groupsproductsmsgroup gs on gs.smsgrpid=bm.smsgrpid");
                DataSet ds = DBHelper.ExecuteMyQueryIns(commandtext);
                List<BodyModel> List = new List<BodyModel>();
                List = DBHelper.GetDBObjectByObj2(new BodyModel(), null, commandtext, "inspector").Cast<BodyModel>().ToList();
                //---
                if (List.Count > 0)
                {
                    return List;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                DBHelper.LogFile(ex);
                return null;
            }
        }

        public static List<SaleStatus> GetBaseSaleStatusList()
        {
            try
            {
                string commandtext = string.Format(@"select * from sale.car_status@saleguard_priprctl s where s.Status_Code in (select distinct Status_Code from sale.car_id@saleguard_priprctl p where p.prod_date> 980101)");
                DataSet ds = DBHelper.ExecuteMyQueryIns(commandtext);
                List<SaleStatus> List = new List<SaleStatus>();
                List = DBHelper.GetDBObjectByObj2(new SaleStatus(), null, commandtext, "inspector").Cast<SaleStatus>().ToList();
                //---
                if (List.Count > 0)
                {
                    return List;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                DBHelper.LogFile(ex);
                return null;
            }
        }

        public static List<FinalQC> GetBaseFinalQCList()
        {
            try
            {
                string commandtext = string.Format(@"select finqccode,finqcname from finalqc f where f.isactive=1");
                DataSet ds = DBHelper.ExecuteMyQueryIns(commandtext);
                List<FinalQC> List = new List<FinalQC>();
                List = DBHelper.GetDBObjectByObj2(new FinalQC(), null, commandtext, "inspector").Cast<FinalQC>().ToList();
                //---
                if (List.Count > 0)
                {
                    return List;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                DBHelper.LogFile(ex);
                return null;
            }
        }

        public static List<Qcdsart> GetQcdsart()
        {
            try
            {
                string commandtext = string.Format(@"select  d.fromareasrl,d.toareasrl,d.IsDefault,d.grpcode,d.isreject,d.inuse from Qcdsart d where d.inuse=1");
                DataSet ds = DBHelper.ExecuteMyQueryIns(commandtext);
                List<Qcdsart> PathList = new List<Qcdsart>();
                PathList = DBHelper.GetDBObjectByObj2(new Qcdsart(), null, commandtext, "inspector").Cast<Qcdsart>().ToList();
                //---
                if (PathList.Count > 0)
                {
                    return PathList;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                DBHelper.LogFile(ex);
                return null;
            }
        }


        public static ResultMsg InsertQcqctrt(CarSend _CarSend)
        {
            ResultMsg rm = new ResultMsg();
            ResultMsg PDIResultMsg = new ResultMsg();
            Qcdsart qcdsart = null;
            try
            {
                bool NeedSetPdi = false;
                // 
                if (_CarSend.AreaType == 50)
                {
                    qcdsart = GetQcdsartByPath(_CarSend.FromAreaSrl, _CarSend.ToAreaSrl);
                    if (qcdsart.IsDefault == 1)
                    {
                        NeedSetPdi = true;
                        Qccastt q = new Qccastt();
                        q.Vin = _CarSend.Vin;
                        q.ActBy = _CarSend.QCUsertSrl;
                        q.ActAreaSrl = _CarSend.FromAreaSrl;
                        PDIResultMsg = PDIConfirm(q);

                    }
                }
                //---
                if ((!NeedSetPdi) || (NeedSetPdi && PDIResultMsg.Successful))
                {
                    if (DBHelper.LiveDBConnectionIns.State != ConnectionState.Open)
                        DBHelper.LiveDBConnectionIns.Open();
                    OracleCommand cmd = new OracleCommand();
                    OracleDataAdapter da = new OracleDataAdapter();
                    cmd.Connection = DBHelper.LiveDBConnectionIns;
                    da.SelectCommand = cmd;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "qcinsertqcqctrt";
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("pvin", OracleDbType.Varchar2).Value = CarUtility.GetVinWithoutChar(_CarSend.Vin);
                    cmd.Parameters.Add("pfromareasrl", OracleDbType.Int32).Value = _CarSend.FromAreaSrl;
                    cmd.Parameters.Add("ptoareasrl", OracleDbType.Int32).Value = _CarSend.ToAreaSrl;
                    cmd.Parameters.Add("pcurteamwork", OracleDbType.Varchar2).Value = "";
                    cmd.Parameters.Add("pstatuscode", OracleDbType.Int32).Value = 1;
                    cmd.Parameters.Add("puauser_srl", OracleDbType.Int32).Value = _CarSend.QCUsertSrl;
                    cmd.Parameters.Add("pErrorMessages", OracleDbType.Varchar2, 2048);
                    cmd.Parameters["pErrorMessages"].Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    string result = cmd.Parameters["pErrorMessages"].Value.ToString();
                    rm.title = rm.Message = result;
                    if (String.IsNullOrEmpty(result) || result.Length == 0 || result == "null")
                    {
                        rm.title = rm.Message = "SUCCESSFUL";
                        rm.MessageFa = "خودرو ارسال گردید";
                        if (NeedSetPdi)
                        {
                            rm.MessageFa += " و " + PDIResultMsg.MessageFa;
                        }
                        //-- disable pdiok if reject from 710 area
                        if (_CarSend.FromAreaSrl == 461)
                        {
                            if (qcdsart == null)
                                qcdsart = GetQcdsartByPath(_CarSend.FromAreaSrl, _CarSend.ToAreaSrl);
                            if (qcdsart.IsReject == 1)
                            {
                                DisablePDIConfirm(_CarSend);
                            }
                        }
                        //--
                        Qccastt q = new Qccastt();
                        q.Vin = _CarSend.Vin;
                        rm.lstQcqctrt = GetCarTrace(q);
                    }
                    else if (result.Equals("HaveSPDefect"))
                        rm.MessageFa = "عدم ارسال خودرو به دلیل محدودیت منع ارسال با عیب ایمنی";
                    else if (result.Equals("CarNotInYourArea"))
                    {
                        rm.MessageFa = "خودرو در ناحیه ی شما نمی باشد";
                    }
                    else if (result.Equals("HaveSecurityConfirm"))
                    {
                        rm.MessageFa = "عدم ارسال خودرو به دلیل ثبت مجوز تایید حراست";
                    }
                    else
                        rm.MessageFa = "بروز خطا در ارسال خودرو";
                    // ---
                    return rm;
                }
                else
                    return PDIResultMsg;


            }
            catch (Exception ex)
            {
                rm.title = "error";
                rm.Message = ex.Message.ToString();
                return rm;
            }

        }

        public static ResultMsg Delete_QCCASTT(Qccastt qccastt)
        {
            //int QCCASTTSRL = Convert.ToInt32(qccastt.Srl);
            //int areaSRL = Convert.ToInt32(qccastt.ActAreaSrl);
            //int userSRL = Convert.ToInt32(qccastt.ActBy);
            //LogManager.SetCommonLog("Delete_QCCASTT" + qccastt.Srl.ToString() + "_" + qccastt.ActAreaSrl.ToString() + "_" + qccastt.ActBy.ToString());
            ResultMsg rm = new ResultMsg();
            try
            {
                //LogManager.SetCommonLog("Delete_QCCASTT:" + qccastt.IsRepaired.ToString() + "_actby:" + qccastt.ActBy.ToString() + "_actareasrl:" + qccastt.ActAreaSrl.ToString());
                OracleCommand cmd = new OracleCommand();
                OracleDataAdapter da = new OracleDataAdapter();
                cmd.Connection = DBHelper.LiveDBConnectionIns;
                if (DBHelper.LiveDBConnectionIns.State != ConnectionState.Open)
                    DBHelper.LiveDBConnectionIns.Open();
                da.SelectCommand = cmd;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "QCP_QCCASTT_Delete";
                cmd.Parameters.Add("pQCCASTT_SRL", OracleDbType.Int32).Value = qccastt.Srl;
                cmd.Parameters.Add("pQCUSERT_SRL", OracleDbType.Int32).Value = qccastt.ActBy;
                cmd.Parameters.Add("pQCAREAT_SRL", OracleDbType.Int32).Value = qccastt.ActAreaSrl;
                cmd.Parameters.Add("pMessage", OracleDbType.Varchar2, 1000);
                cmd.Parameters["pMessage"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                string result = cmd.Parameters["pMessage"].Value.ToString();
                rm.title = rm.Message = result;
                //-- translate result  msg --
                if (result.Contains("SUCCESSFUL"))
                    rm.MessageFa = "عیب مورد نظر با موفقیت حذف گردید";
                else if (result.Contains("NO_DATA_FOUND"))
                    rm.MessageFa = "عیب مورد نظر در سیستم یافت نشد";
                else
                    rm.MessageFa = "بروز خطا در حذف عیب";
                //LogManager.SetCommonLog("Delete_QCCASTT:err" + result);
                //System.Threading.Thread.Sleep(1000);
                rm.lstQccastt = GetCarDefect(qccastt);


                // --
                return rm;
            }
            catch (Exception ex)
            {
                rm.title = "error";
                rm.Message = ex.Message.ToString();
                return rm;
            }

        }

        public static ResultMsg PDIConfirm(Qccastt qccastt)
        {
            //LogManager.SetCommonLog("Delete_QCCASTT" + qccastt.Srl.ToString() + "_" + qccastt.ActAreaSrl.ToString() + "_" + qccastt.ActBy.ToString());
            ResultMsg rm = new ResultMsg();
            try
            {
                Area CarLastArea = null;
                Area UserArea = QccasttUtility.GetAreaBySrl(qccastt.ActAreaSrl.ToString());
                if (UserArea.CheckDest != 2)
                {
                    CarLastArea = QccasttUtility.GetVinCurrentArea(CarUtility.GetVinWithoutChar(qccastt.Vin));
                }
                QCUsert u = GetQCUserT(qccastt.ActBy.ToString());
                //---
                if ((UserArea.CheckDest == 2) || (CarLastArea.Srl == UserArea.Srl))
                {
                    //LogManager.SetCommonLog("Delete_QCCASTT:" + qccastt.IsRepaired.ToString() + "_actby:" + qccastt.ActBy.ToString() + "_actareasrl:" + qccastt.ActAreaSrl.ToString());
                    OracleCommand cmd = new OracleCommand();
                    OracleDataAdapter da = new OracleDataAdapter();
                    cmd.Connection = DBHelper.LiveDBConnectionIns;
                    if (DBHelper.LiveDBConnectionIns.State != ConnectionState.Open)
                        DBHelper.LiveDBConnectionIns.Open();
                    da.SelectCommand = cmd;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "QCPDIUpdateOKStatus";
                    cmd.Parameters.Add("strVIN", OracleDbType.Varchar2).Value = CarUtility.GetVinWithoutChar(qccastt.Vin);
                    cmd.Parameters.Add("strUserID", OracleDbType.Varchar2).Value = u.USERNAME;
                    cmd.Parameters.Add("strQCAreaCD", OracleDbType.Varchar2).Value = UserArea.AreaCode;
                    cmd.Parameters.Add("ErrorMessages", OracleDbType.Varchar2, 500);
                    cmd.Parameters["ErrorMessages"].Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    string result = cmd.Parameters["ErrorMessages"].Value.ToString();
                    rm.title = rm.Message = result;
                    //--translate result msg --
                    if (string.IsNullOrEmpty(result) || result == "null")
                    {
                        rm.Successful = true;
                        rm.title = rm.Message = "SUCCESSFUL";
                        rm.MessageFa = "عمليات تایید جهت بارگیری با موفقيت انجام شد";
                        string strActDesc = string.Format(@"QCPDIConfirm_InAreaCode{0}_LogByWS", UserArea.AreaCode);
                        bool blnLogResult = CommonUtility.QCInsertUserAct2("UI" + u.USERNAME, strActDesc, CarUtility.GetVinWithoutChar(qccastt.Vin), "AC" + UserArea.AreaCode, "", "WS", "WS", "WS", "WS", "0", "3");
                    }
                    else if (result.Contains("Fractional Export"))
                        rm.MessageFa = "این خودرو صادراتي كسري دار بوده و مجاز به تائيد نمي باشد";
                    else if (result.Contains("CarNotInYourArea"))
                        rm.MessageFa = "محل خودرو این ناحیه نیست";
                    else if (result.Contains("Record Was Not Found"))
                    {
                        rm.Successful = true;
                        rm.MessageFa = "این خودرو نیازی به تایید بارگیری ندارد";
                    }
                    else if (result.Contains("CarHaveASPDefect"))
                        rm.MessageFa = "این خودرو دارای عیب ایمنی است";
                    else if (result.Contains("ApplyRequestCarNoAudit"))
                        rm.MessageFa = "این خودرو آدیت نشده است";
                    else if (result.Contains("LoadRecall"))
                        rm.MessageFa = "این خودرو محدودیت بارگیری / فراخوان دارد";
                    else if (result.Contains("LendingInvalid"))
                        rm.MessageFa = "این خودرو محدودیت بارگیری در سیستم خودروهای امانی دارد";
                    else if (result.Contains("ex_InvalidVin"))
                        rm.MessageFa = "شاسی ارسالی اشتباه می باشد";
                    else if (result.Contains("ex_InvalidCartrce") || result.Contains("ex_InvalidCartraceRec"))
                        rm.MessageFa = "اشکال در سیستم ردیابی خودرو";
                    else if (result.Contains("Is99YearFraq"))
                        rm.MessageFa = "محدودیت سال99کسری دار7570";

                    else
                        rm.MessageFa = "عمليات تایید با خطا مواجه شده است";
                    //LogManager.SetCommonLog("Delete_QCCASTT:err" + result);
                    //System.Threading.Thread.Sleep(1000);
                    rm.lstQccastt = QccasttUtility.GetCarDefect(qccastt);


                    // --
                }
                else
                {
                    rm.Message = rm.title = "CarNotInYourArea";
                    rm.MessageFa = "محل خودرو این ناحیه نیست";
                }
                return rm;
            }
            catch (Exception ex)
            {
                rm.title = "error";
                rm.Message = ex.Message.ToString();
                return rm;
            }

        }

        public static bool CheckConsistencyBetweenCheckListAndCarGroupCode(Qccastt qccastt)
        {
            try
            {
                string commandtext = string.Format(@"Select * from qcdfctt d where (d.grpcode={0} or  d.qcareat_srl=841 )
                                                        And d.qcbadft_srl={1} 
                                                        And d.qcareat_srl={2}
                                                        And d.qcmdult_srl={3}
                                                        ", qccastt.GrpCode, qccastt.QCBadft_Srl, qccastt.QCAreat_Srl, qccastt.QCMdult_Srl);
                DataSet ds = DBHelper.ExecuteMyQueryInsOnLive(commandtext);
                if ((ds.Tables[0] != null) && (ds.Tables[0].Rows.Count != 0))
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static ResultMsg QCCASTT_DefectDetect(Qccastt qccastt)
        {
            ResultMsg rm = new ResultMsg();

            try
            {
                bool NeedSetPdi = false;

                //LogManager.SetCommonLog("QCCASTT_DefectDetect_isrep:" + qccastt.IsRepaired.ToString() + "_actby:" + qccastt.ActBy.ToString() + "_actareasrl:" + qccastt.ActAreaSrl.ToString() + "_QCSTRGT_SRL:" + qccastt.QCSTRGT_SRL.ToString() + "_IsDefected:" + qccastt.IsDefected.ToString() + "_QcmdultSrl:" + qccastt.QCMdult_Srl.ToString() + "_Qcbadft:" + qccastt.QCBadft_Srl.ToString() + "_Vin:" + qccastt.Vin.ToString());//
                bool blnConsistency = false;
                if (qccastt.IsDefected == 1)
                {
                    blnConsistency =
                        CheckConsistencyBetweenCheckListAndCarGroupCode(qccastt);
                }
                else if (qccastt.IsDefected == 0 && qccastt.AreaType == 50)
                {
                    NeedSetPdi = true;
                }

                if ((blnConsistency) || (qccastt.IsDefected == 0))
                {
                    OracleCommand cmd = new OracleCommand();
                    OracleDataAdapter da = new OracleDataAdapter();
                    cmd.Connection = DBHelper.LiveDBConnectionIns;
                    if (DBHelper.LiveDBConnectionIns.State != ConnectionState.Open)
                        DBHelper.LiveDBConnectionIns.Open();
                    da.SelectCommand = cmd;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "QCP_QCCASTT_Detect";
                    if (!qccastt.QCBadft_Srl.Equals(null))
                        cmd.Parameters.Add("PQCBADFT_SRL", OracleDbType.Int32).Value = qccastt.QCBadft_Srl;
                    else
                        cmd.Parameters.Add("PQCBADFT_SRL", OracleDbType.Int32).Value = DBNull.Value;
                    //-
                    if (!qccastt.QCMdult_Srl.Equals(null))
                        cmd.Parameters.Add("PQCMDULT_SRL", OracleDbType.Int32).Value = qccastt.QCMdult_Srl;
                    else
                        cmd.Parameters.Add("PQCMDULT_SRL", OracleDbType.Int32).Value = DBNull.Value;
                    //--
                    cmd.Parameters.Add("PVIN", OracleDbType.Varchar2).Value = qccastt.VinWithoutChar;
                    cmd.Parameters.Add("PQCAREAT_SRL", OracleDbType.Int32).Value = qccastt.ActAreaSrl;
                    // --
                    if (!qccastt.IsRepaired.Equals(null))
                        cmd.Parameters.Add("PISREPAIRED", OracleDbType.Int32).Value = qccastt.IsRepaired;
                    else
                        cmd.Parameters.Add("PISREPAIRED", OracleDbType.Int32).Value = DBNull.Value;

                    cmd.Parameters.Add("PQCUSERT_SRL", OracleDbType.Int32).Value = qccastt.ActBy;
                    //-
                    if (!qccastt.QCSTRGT_SRL.Equals(null))
                        cmd.Parameters.Add("PQCSTRGT_SRL", OracleDbType.Int32).Value = qccastt.QCSTRGT_SRL;
                    else
                        cmd.Parameters.Add("PQCSTRGT_SRL", OracleDbType.Int32).Value = DBNull.Value;
                    // --
                    if (!qccastt.CHECKLISTAREA_SRL.Equals(null))
                        cmd.Parameters.Add("pCheckListArea_SRL", OracleDbType.Int32).Value = qccastt.CHECKLISTAREA_SRL;
                    else
                        cmd.Parameters.Add("pCheckListArea_SRL", OracleDbType.Int32).Value = DBNull.Value;
                    // --
                    cmd.Parameters.Add("PINUSE", OracleDbType.Int32).Value = qccastt.InUse;
                    cmd.Parameters.Add("PISDEFECTED", OracleDbType.Int32).Value = qccastt.IsDefected;
                    // --
                    if (!qccastt.RecordOwner.Equals(null))
                        cmd.Parameters.Add("pRecordOwner", OracleDbType.Int32).Value = qccastt.RecordOwner;
                    else
                        cmd.Parameters.Add("pRecordOwner", OracleDbType.Int32).Value = DBNull.Value;
                    // --
                    cmd.Parameters.Add("pGrpCode", OracleDbType.Int32).Value = qccastt.GrpCode;
                    // --
                    cmd.Parameters.Add("pMessage", OracleDbType.Varchar2, 1000);
                    cmd.Parameters["PMESSAGE"].Direction = ParameterDirection.Output;
                    // --
                    cmd.ExecuteNonQuery();
                    string result = cmd.Parameters["PMESSAGE"].Value.ToString();
                    rm.title = rm.Message = result;
                    //-- translate result  msg --
                    if (result.Contains("SUCCESSFUL"))
                    {
                        rm.Successful = true;
                        if (qccastt.IsDefected == 1)
                        {
                            rm.MessageFa = "عیب با موفقیت ثبت گردید";
                        }
                        else if (qccastt.IsDefected == 0)
                        {
                            rm.MessageFa = "عبور مستقیم خودرو با موفقیت ثبت گردید";
                        }
                    }
                    else if (result.ToUpper().Trim().Equals("REPAIRED CHANGE") || result.ToUpper().Trim().Equals("EDIT_REPAIRED"))
                    {
                        if (qccastt.IsRepaired == 1)
                            rm.MessageFa = "عیب رفع شده ثبت گردید";
                        else
                            rm.MessageFa = "عیب رفع نشده ثبت گردید";
                    }
                    else if (result.Contains("EDIT DEFECT"))
                        rm.MessageFa = "عیب ویرایش گردید";

                    else if (result.Contains("REPEATED DEFECT"))
                        rm.MessageFa = "این عیب قبلا در این ناحیه ثبت گردیده است";
                    else if ((qccastt.IsDefected == 0) && (result.Contains("REPEATED STRAIGHT")))
                    {
                        rm.MessageFa = "براي اين خودرو ،قبلاً عبور مستقيم ثبت شده است";
                        rm.Successful = true;
                    }
                    else if ((qccastt.IsDefected == 0) && (result.Contains("IS DEFECTED")))
                        rm.MessageFa = "برای این خودرو در این ناحیه قبلاً عیب ثبت شده است";
                    else
                        rm.MessageFa = ":خطایی رخ داده است" + result;
                    // --
                    rm.lstQccastt = QccasttUtility.GetCarDefect(qccastt);
                    //---
                    if (qccastt.IsDefected == 0 && qccastt.AreaType == 50 && rm.Successful && NeedSetPdi)
                    {
                        Qccastt q = new Qccastt();
                        q.Vin = qccastt.Vin;
                        q.ActBy = qccastt.ActBy;
                        q.ActAreaSrl = qccastt.ActAreaSrl;
                        rm = PDIConfirm(q);
                    }
                    return rm;
                }
                else
                {
                    rm.title = rm.Message = rm.MessageFa = "عدم همخوانی عیب باخودرو";
                    return rm;
                }
            }
            catch (Exception ex)
            {
                rm.title = "error";
                rm.Message = ex.Message.ToString();
                //LogManager.SetCommonLog("QCCASTT_DefectDetect_Err:" + ex.Message.ToString());
                return rm;
            }
        }

        public static ResultMsg QCCASTT_MultiDefectRepair3(List<Qccastt> LstQCcastt)
        {
            ResultMsg rm = new ResultMsg();
            try
            {
                for (int i = 0; i < LstQCcastt.Count; i++)
                {
                    bool blnConsistency = false;
                    if (LstQCcastt[i].IsDefected == 1)
                    {
                        blnConsistency =
                            CheckConsistencyBetweenCheckListAndCarGroupCode(LstQCcastt[i]);
                    }
                    if ((blnConsistency) || (LstQCcastt[i].IsDefected == 0))
                    {
                        OracleCommand cmd = new OracleCommand();
                        OracleDataAdapter da = new OracleDataAdapter();
                        cmd.Connection = DBHelper.LiveDBConnectionIns;
                        if (DBHelper.LiveDBConnectionIns.State != ConnectionState.Open)
                            DBHelper.LiveDBConnectionIns.Open();
                        da.SelectCommand = cmd;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "QCP_QCCASTT_Detect";
                        if (!LstQCcastt[i].QCBadft_Srl.Equals(null))
                            cmd.Parameters.Add("PQCBADFT_SRL", OracleDbType.Int32).Value = LstQCcastt[i].QCBadft_Srl;
                        else
                            cmd.Parameters.Add("PQCBADFT_SRL", OracleDbType.Int32).Value = DBNull.Value;
                        //-
                        if (!LstQCcastt[i].QCMdult_Srl.Equals(null))
                            cmd.Parameters.Add("PQCMDULT_SRL", OracleDbType.Int32).Value = LstQCcastt[i].QCMdult_Srl;
                        else
                            cmd.Parameters.Add("PQCMDULT_SRL", OracleDbType.Int32).Value = DBNull.Value;
                        //--
                        cmd.Parameters.Add("PVIN", OracleDbType.Varchar2).Value = LstQCcastt[i].VinWithoutChar;
                        cmd.Parameters.Add("PQCAREAT_SRL", OracleDbType.Int32).Value = LstQCcastt[i].ActAreaSrl;
                        // --
                        if (!LstQCcastt[i].IsRepaired.Equals(null))
                            cmd.Parameters.Add("PISREPAIRED", OracleDbType.Int32).Value = LstQCcastt[i].IsRepaired;
                        else
                            cmd.Parameters.Add("PISREPAIRED", OracleDbType.Int32).Value = DBNull.Value;

                        cmd.Parameters.Add("PQCUSERT_SRL", OracleDbType.Int32).Value = LstQCcastt[i].ActBy;
                        //-
                        if (!LstQCcastt[i].QCSTRGT_SRL.Equals(null))
                            cmd.Parameters.Add("PQCSTRGT_SRL", OracleDbType.Int32).Value = LstQCcastt[i].QCSTRGT_SRL;
                        else
                            cmd.Parameters.Add("PQCSTRGT_SRL", OracleDbType.Int32).Value = DBNull.Value;
                        // --
                        if (!LstQCcastt[i].CHECKLISTAREA_SRL.Equals(null))
                            cmd.Parameters.Add("pCheckListArea_SRL", OracleDbType.Int32).Value = LstQCcastt[i].CHECKLISTAREA_SRL;
                        else
                            cmd.Parameters.Add("pCheckListArea_SRL", OracleDbType.Int32).Value = DBNull.Value;
                        // --
                        cmd.Parameters.Add("PINUSE", OracleDbType.Int32).Value = LstQCcastt[i].InUse;
                        cmd.Parameters.Add("PISDEFECTED", OracleDbType.Int32).Value = LstQCcastt[i].IsDefected;
                        // --
                        if (!LstQCcastt[i].RecordOwner.Equals(null))
                            cmd.Parameters.Add("pRecordOwner", OracleDbType.Int32).Value = LstQCcastt[i].RecordOwner;
                        else
                            cmd.Parameters.Add("pRecordOwner", OracleDbType.Int32).Value = DBNull.Value;
                        // --
                        cmd.Parameters.Add("pGrpCode", OracleDbType.Int32).Value = LstQCcastt[i].GrpCode;
                        // --
                        cmd.Parameters.Add("pMessage", OracleDbType.Varchar2, 1000);
                        cmd.Parameters["PMESSAGE"].Direction = ParameterDirection.Output;
                        // --
                        cmd.ExecuteNonQuery();
                        string result = cmd.Parameters["PMESSAGE"].Value.ToString();
                        rm.Message += result;
                        //-- translate result  msg --
                        if (result.Contains("SUCCESSFUL"))
                            rm.MessageFa = "عیب با موفقیت ثبت گردید";
                        else if (result.ToUpper().Trim().Equals("REPAIRED CHANGE") || result.ToUpper().Trim().Equals("EDIT_REPAIRED"))
                        {
                            if (LstQCcastt[i].IsRepaired == 1)
                                rm.MessageFa += "عیب رفع شده ثبت گردید";
                            else
                                rm.MessageFa += "عیب رفع نشده ثبت گردید";
                        }
                        else if (result.Contains("EDIT DEFECT"))
                            rm.MessageFa += "عیب ویرایش گردید";

                        else if (result.Contains("REPEATED DEFECT"))
                            rm.MessageFa += "این عیب قبلا در این ناحیه ثبت گردیده است";
                        else
                            rm.MessageFa += "خطایی رخ داده است";
                        // --
                    }
                    else
                    {
                        //rm.title = rm.Message =
                        rm.MessageFa += "عدم همخوانی عیب باخودرو";

                    }
                }
                rm.lstQccastt = QccasttUtility.GetCarDefect(LstQCcastt[0]);
                return rm;
            }
            catch (Exception ex)
            {
                rm.lstQccastt = null;
                rm.title = "error";
                rm.Message = ex.Message.ToString();
                return rm;
            }
        }

        public static ResultMsg DisablePDIConfirm(CarSend _CarSend)
        {
            //LogManager.SetCommonLog("Delete_QCCASTT" + qccastt.Srl.ToString() + "_" + qccastt.ActAreaSrl.ToString() + "_" + qccastt.ActBy.ToString());
            ResultMsg rm = new ResultMsg();
            try
            {
                Area UserArea = QccasttUtility.GetAreaBySrl(_CarSend.FromAreaSrl.ToString());
                //LogManager.SetCommonLog("Delete_QCCASTT:" + qccastt.IsRepaired.ToString() + "_actby:" + qccastt.ActBy.ToString() + "_actareasrl:" + qccastt.ActAreaSrl.ToString());
                string strComment = string.Format(@"QCLock_IsReject; _qcusertSrl={0}; _FromAreaSrl={1}; _ToAreaSrl={2}", _CarSend.QCUsertSrl.ToString(), _CarSend.FromAreaSrl, _CarSend.ToAreaSrl);
                OracleCommand cmd = new OracleCommand();
                OracleDataAdapter da = new OracleDataAdapter();
                cmd.Connection = DBHelper.LiveDBConnectionIns;
                if (DBHelper.LiveDBConnectionIns.State != ConnectionState.Open)
                    DBHelper.LiveDBConnectionIns.Open();
                da.SelectCommand = cmd;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "QCUpdatePDITracStatus";
                cmd.Parameters.Add("strVIN", OracleDbType.Varchar2).Value = CarUtility.GetVinWithoutChar(_CarSend.Vin);
                cmd.Parameters.Add("intStatus", OracleDbType.Int32).Value = 2;
                cmd.Parameters.Add("StatusComment", OracleDbType.Varchar2).Value = strComment;
                cmd.Parameters.Add("ErrorMessages", OracleDbType.Varchar2, 1000);
                cmd.Parameters["ErrorMessages"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                string result = cmd.Parameters["ErrorMessages"].Value.ToString();
                rm.title = rm.Message = result;
                //--translate result msg --
                if (string.IsNullOrEmpty(result) || result == "null")
                {
                    rm.title = rm.Message = "SUCCESSFUL";
                    rm.MessageFa = "PDI_Disable";
                    string strActDesc = string.Format(@"QCPDIConfirm_InAreaCode{0}_LogByWS", UserArea.AreaCode);
                    bool LogResult = CommonUtility.QCInsertUserAct2("UI" + _CarSend.UserId, strActDesc, CarUtility.GetVinWithoutChar(_CarSend.Vin), "AC" + UserArea.AreaCode, "", "WS", "WS", "WS", "WS", "0", "3");
                }
                return rm;
            }
            catch (Exception ex)
            {
                rm.title = "error";
                rm.Message = ex.Message.ToString();
                return rm;
            }

        }

        public static User FindUser(string _UserName, string _Password,
                              string _SecondPassword,
                              string _AreaCode, string _Mac)
        {
            byte[] userByte = Encoding.UTF8.GetBytes(_UserName);
            string strHashPSW = DBHelper.Cryptographer.CreateHash(_Password, "MD5", userByte);
            string commandtext = string.Format(@"select srl,fname,lname,username,psw,userid,
                                                (select a.AreaCode from qcareat a where areacode={2}) as AreaCode,
                                                (select a.CheckDest from qcareat a where areacode={2}) as CheckDest,
                                                (select a.AreaType from qcareat a where areacode={2}) as AreaType,
                                                (select a.AreaDesc from qcareat a where areacode={2}) as AreaDesc,
                                                (select a.isauditarea from qcareat a where areacode=500) as IsAuditArea,
                                                (select distinct  q.parameter_srl from qcussft q where
                                                    q.qcusert_srl in (select srl from qcusert u2 where u2.srl = u.srl)
                                                        and q.inuse=1
                                                    and q.parameter_srl in 
                                                        (select a.srl from qcareat a
                                                            where areacode={2})) as QCAreatSrl
                                                 ,(select decode(count(distinct q.qcsyfot_srl),0,'false','true') from qcussft q  join qcsyfot sf on q.qcsyfot_srl=sf.srl join qcformt f on f.srl = sf.qcformt_srl where q.inuse=1 and q.qcusert_srl = (select srl from qcusert u where u.username = '{0}')  and sf.qcsystt_srl = 121 and f.url='QCMobAppPer') As QCMOBAPPPER
                                                 ,(select decode(count(distinct q.qcsyfot_srl),0,'false','true') from qcussft q  join qcsyfot sf on q.qcsyfot_srl=sf.srl join qcformt f on f.srl = sf.qcformt_srl where q.inuse=1 and q.qcusert_srl = (select srl from qcusert u where u.username = '{0}')  and sf.qcsystt_srl = 121 and f.url='PTDashPer') As PTDASHPER
                                                 ,(select decode(count(distinct q.qcsyfot_srl),0,'false','true') from qcussft q  join qcsyfot sf on q.qcsyfot_srl=sf.srl join qcformt f on f.srl = sf.qcformt_srl where q.inuse=1 and q.qcusert_srl = (select srl from qcusert u where u.username = '{0}')  and sf.qcsystt_srl = 121 and f.url='QCDashPer') As QCDASHPER
                                                 ,(select decode(count(distinct q.qcsyfot_srl),0,'false','true') from qcussft q  join qcsyfot sf on q.qcsyfot_srl=sf.srl join qcformt f on f.srl = sf.qcformt_srl where q.inuse=1 and q.qcusert_srl = (select srl from qcusert u where u.username = '{0}')  and sf.qcsystt_srl = 121 and f.url='AuditDashPer') As AUDITDASHPER
                                                 ,(select decode(count(distinct q.qcsyfot_srl),0,'false','true') from qcussft q  join qcsyfot sf on q.qcsyfot_srl=sf.srl join qcformt f on f.srl = sf.qcformt_srl where q.inuse=1 and q.qcusert_srl = (select srl from qcusert u where u.username = '{0}')  and sf.qcsystt_srl = 121 and f.url='AuditUnLockPer') As AUDITUNLOCKPER
                                                 ,(select decode(count(distinct q.qcsyfot_srl),0,'false','true') from qcussft q  join qcsyfot sf on q.qcsyfot_srl=sf.srl join qcformt f on f.srl = sf.qcformt_srl where q.inuse=1 and q.qcusert_srl = (select srl from qcusert u where u.username = '{0}')  and sf.qcsystt_srl = 121 and f.url='QCRegDefPer') As QCREGDEFPER
                                                 ,(select decode(count(distinct q.qcsyfot_srl),0,'false','true') from qcussft q  join qcsyfot sf on q.qcsyfot_srl=sf.srl join qcformt f on f.srl = sf.qcformt_srl where q.inuse=1 and q.qcusert_srl = (select srl from qcusert u where u.username = '{0}')  and sf.qcsystt_srl = 121 and f.url='SMSQCPer') As SMSQCPER
                                                 ,(select decode(count(distinct q.qcsyfot_srl),0,'false','true') from qcussft q  join qcsyfot sf on q.qcsyfot_srl=sf.srl join qcformt f on f.srl = sf.qcformt_srl where q.inuse=1 and q.qcusert_srl = (select srl from qcusert u where u.username = '{0}')  and sf.qcsystt_srl = 121 and f.url='SMSAuditPer') As SMSAUDITPER
                                                 ,(select decode(count(distinct q.qcsyfot_srl),0,'false','true') from qcussft q  join qcsyfot sf on q.qcsyfot_srl=sf.srl join qcformt f on f.srl = sf.qcformt_srl where q.inuse=1 and q.qcusert_srl = (select srl from qcusert u where u.username = '{0}')  and sf.qcsystt_srl = 121 and f.url='SMSSPPer') As SMSSPPER
                                                 ,(select decode(count(distinct q.qcsyfot_srl),0,'false','true') from qcussft q  join qcsyfot sf on q.qcsyfot_srl=sf.srl join qcformt f on f.srl = sf.qcformt_srl where q.inuse=1 and q.qcusert_srl = (select srl from qcusert u where u.username = '{0}')  and sf.qcsystt_srl = 121 and f.url='QCCardPer') As QCCARDPER
                                                 ,(select decode(count(distinct q.qcsyfot_srl),0,'false','true') from qcussft q  join qcsyfot sf on q.qcsyfot_srl=sf.srl join qcformt f on f.srl = sf.qcformt_srl where q.inuse=1 and q.qcusert_srl = (select srl from qcusert u where u.username = '{0}')  and sf.qcsystt_srl = 121 and f.url='SMSPTPer') As SMSPTPER
                                                 ,(select decode(count(distinct q.qcsyfot_srl),0,'false','true') from qcussft q  join qcsyfot sf on q.qcsyfot_srl=sf.srl join qcformt f on f.srl = sf.qcformt_srl where q.inuse=1 and q.qcusert_srl = (select srl from qcusert u where u.username = '{0}')  and sf.qcsystt_srl = 121 and f.url='AuditCardPer') As AUDITCARDPER
                                                 ,(select decode(count(distinct q.qcsyfot_srl),0,'false','true') from qcussft q  join qcsyfot sf on q.qcsyfot_srl=sf.srl join qcformt f on f.srl = sf.qcformt_srl where q.inuse=1 and q.qcusert_srl = (select srl from qcusert u where u.username = '{0}')  and sf.qcsystt_srl = 121 and f.url='CarStatusPer') As CARSTATUSPER
                                                 from QCUSERT u Where USERName='{0}'
                                                 and PSW ='{1}' and (InUse=1) 
                                                 and otp = '{3}'  and u.otpexpire > sysdate "
                                                 , _UserName, strHashPSW, _AreaCode, _SecondPassword);
            object[] obj = DBHelper.GetDBObjectByObj2_OnLive(new User(), null, commandtext, "inspector");
            if ((obj != null) && (obj.Length != 0))
            {
                return obj.Cast<User>().ToList()[0];
            }
            else
                return null;
            //---

        }


        public static List<Summary> GetUserSammary(int _QcAreatSrl, int _QCUsertSrl, int _FromDayAgo, string _Type)
        {
            try
            {
                PersianCalendar pc = new PersianCalendar();
                DateTime dtN = DateTime.Now;
                string Y = pc.GetYear(dtN).ToString();
                string M = pc.GetMonth(dtN).ToString().PadLeft(2, '0');
                string D = pc.GetDayOfMonth(dtN).ToString().PadLeft(2, '0');
                string strDateCondition = "";
                if (_Type == "M")
                    strDateCondition = string.Format(@"createddate >= to_date('{0}/{1}/01','yyyy/mm/dd','nls_calendar=persian')", Y, M);
                else if (_Type == "D")
                    strDateCondition = string.Format(@"createddate >= to_date('{0}/{1}/{2}','yyyy/mm/dd','nls_calendar=persian')", Y, M, D);
                List<Summary> lstSummary = new List<Summary>();
                if (_QcAreatSrl != 1483) //herasat code 110
                {
                    string commandtext = string.Format(@"select count(distinct q.vin) as TotalDetectCarCount,     
                                              count(distinct (decode(cd.grpcode,21,q.vin,null))) as TotalSP100DetectCarCount,
                                              count(distinct (decode(cd.midcat,'Q200',q.vin,null))) as TotalQ200DetectCarCount,
                                              count(distinct (decode(cd.midcat,'X200',q.vin,null))) as TotalTibaDetectCarCount,
                                              count(distinct (decode(q.isdefected,0,q.vin,null))) as TotalStrCarCount,
                                              count(distinct (decode(q.isdefected,1,q.vin,null))) as TotalDefCarCount,
                                              count((decode(q.isdefected,1,q.vin,null))) as TotalRegDefCount,
                                              count((decode(q.isdefected,0,q.vin,null))) as TotalStrCount,
                                              count(distinct (decode(q.qcstrgt_srl,62,q.vin,42,q.vin,101,q.vin,null))) as TotalASPCarCount,
                                              count((decode(q.qcstrgt_srl,62,q.vin,42,q.vin,101,q.vin,null))) as TotalASPRegDefCount,
                                              count(distinct (decode(q.qcstrgt_srl,42,q.vin,101,q.vin,null))) as TotalSPCarCount,
                                              count((decode(q.qcstrgt_srl,42,q.vin,101,q.vin,null))) as TotalSPRegDefCount,
                                              count(distinct (decode(q.qcstrgt_srl,62,q.vin,null))) as TotalACarCount,
                                              count((decode(q.qcstrgt_srl,62,q.vin,null))) as TotalARegDefCount,
                                              count(distinct (decode(q.qcstrgt_srl,63,q.vin,null))) as TotalBCarCount,
                                              count((decode(q.qcstrgt_srl,63,q.vin,null))) as TotalBRegDefCount,
                                              count(distinct decode(q.qcstrgt_srl,62,q.vin,42,q.vin,101,q.vin,63,q.vin,null)) as TotalBPlusCarCount,
                                              count((decode(q.qcstrgt_srl,62,q.vin,42,q.vin,101,q.vin,63,q.vin,null))) as TotalBPlusRegDefCount,
                                              (select count(c.vin) from qccastt c where  {2} and c.isdefected=1 and c.qcareat_srl={0} and c.deletedby is not null ) as DefectDeletedCount 
                                             from qccastt q join qccariddt cd on cd.vin = q.vin
                                             where {2} and q.recordowner=1 and q.qcareat_srl={0} 
                                             group by qcareat_srl
                                                ", _QcAreatSrl, _QCUsertSrl, strDateCondition);
                    // -- having q.createdby ={1}
                    //List<Qcqctrt> qcTrace = new List<Qcqctrt>();
                    DataSet ds = DBHelper.GetDBObjectByDataSet(null, commandtext);

                    Summary s = new Summary();
                    if ((ds != null) && (ds.Tables[0] != null) && (ds.Tables[0].Rows.Count > 0)) // user find
                    {
                        s = new Summary();
                        s.UserId = 0;
                        s.SummaryTitle = "تعداد بازرسی خودرو در ناحیه";
                        s.SummaryValue = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalDetectCarCount"].ToString());
                        lstSummary.Add(s);
                        //---
                        s = new Summary();
                        s.UserId = 0;
                        s.SummaryTitle = "تعداد شاهین بازرسی شده";
                        s.SummaryValue = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalSP100DetectCarCount"].ToString());
                        lstSummary.Add(s);
                        //---
                        s = new Summary();
                        s.UserId = 0;
                        s.SummaryTitle = "تعداد کوییک بازرسی شده";
                        s.SummaryValue = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalQ200DetectCarCount"].ToString());
                        lstSummary.Add(s);
                        //---
                        s = new Summary();
                        s.UserId = 0;
                        s.SummaryTitle = "تعداد تیبا بازرسی شده";
                        s.SummaryValue = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalTibaDetectCarCount"].ToString());
                        lstSummary.Add(s);
                        //---
                        s = new Summary();
                        s.UserId = 0;
                        s.SummaryTitle = "تعداد عبور مستقیم خودرو در ناحیه";
                        s.SummaryValue = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalStrCarCount"].ToString());
                        lstSummary.Add(s);
                        //---
                        s = new Summary();
                        s.UserId = 0;
                        s.SummaryTitle = "تعداد خودروی معیوب در ناحیه";
                        s.SummaryValue = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalDefCarCount"].ToString());
                        lstSummary.Add(s);
                        //
                        s = new Summary();
                        s.UserId = 0;
                        s.SummaryTitle = "تعداد عیب ثبتی در ناحیه";
                        s.SummaryValue = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalRegDefCount"].ToString());
                        lstSummary.Add(s);
                        // 
                        s = new Summary();
                        s.UserId = 0;
                        s.SummaryType = "D";
                        s.SummaryTitle = "تعداد عیب حذف شده در ناحیه";
                        s.SummaryValue = Convert.ToInt32(ds.Tables[0].Rows[0]["DefectDeletedCount"].ToString());
                        lstSummary.Add(s);
                        //
                        s = new Summary();
                        s.UserId = 0;
                        s.SummaryTitle = "تعداد خودرو با عیب ایمنی در ناحیه";
                        s.SummaryValue = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalASPCarCount"].ToString());
                        lstSummary.Add(s);
                        //
                        s = new Summary();
                        s.UserId = 0;
                        s.SummaryTitle = "تعداد ثبت عیب ایمنی در ناحیه";
                        s.SummaryValue = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalASPRegDefCount"].ToString());
                        lstSummary.Add(s);
                        //
                        s = new Summary();
                        s.UserId = 0;
                        s.SummaryTitle = "تعداد خودرو با عیب SP در ناحیه ";
                        s.SummaryValue = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalSPCarCount"].ToString());
                        lstSummary.Add(s);
                        //
                        s = new Summary();
                        s.UserId = 0;
                        s.SummaryTitle = "تعداد ثبت عیب SP در ناحیه ";
                        s.SummaryValue = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalSPRegDefCount"].ToString());
                        lstSummary.Add(s);
                        //
                        s = new Summary();
                        s.UserId = 0;
                        s.SummaryTitle = "تعداد خودرو با عیب A در ناحیه ";
                        s.SummaryValue = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalACarCount"].ToString());
                        lstSummary.Add(s);
                        //
                        s = new Summary();
                        s.UserId = 0;
                        s.SummaryTitle = "تعداد ثبت عیب A در ناحیه ";
                        s.SummaryValue = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalARegDefCount"].ToString());
                        lstSummary.Add(s);
                        //
                        s = new Summary();
                        s.UserId = 0;
                        s.SummaryTitle = "تعداد خودرو با عیب B در ناحیه ";
                        s.SummaryValue = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalBCarCount"].ToString());
                        lstSummary.Add(s);
                        //
                        s = new Summary();
                        s.UserId = 0;
                        s.SummaryTitle = "تعداد ثبت عیب B در ناحیه ";
                        s.SummaryValue = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalBRegDefCount"].ToString());
                        lstSummary.Add(s);
                        //
                        s = new Summary();
                        s.UserId = 0;
                        s.SummaryTitle = "تعداد خودرو با عیب B پلاس در ناحیه ";
                        s.SummaryValue = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalBPlusCarCount"].ToString());
                        lstSummary.Add(s);
                        //
                        s = new Summary();
                        s.UserId = 0;
                        s.SummaryTitle = "تعداد ثبت عیب B پلاس در ناحیه ";
                        s.SummaryValue = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalBPlusRegDefCount"].ToString());
                        lstSummary.Add(s);


                        // by usert statistic
                        commandtext = string.Format(@"select q.createdby,u.userid,u.fname,u.lname,   
                                              count(distinct q.vin) as DetectCarCount,
                                              count(distinct (decode(cd.grpcode,21,q.vin,null))) as SP100DetectCarCount,
                                              count(distinct (decode(cd.midcat,'Q200',q.vin,null))) as Q200DetectCarCount,
                                              count(distinct (decode(cd.midcat,'X200',q.vin,null))) as TibaDetectCarCount,
                                              count(distinct (decode(q.isdefected,1,q.vin,null))) as DefCarCount,
                                              count(distinct (decode(q.isdefected,0,q.vin,null))) as StrCarCount,
                                              count((decode(q.isdefected,1,q.vin,null))) as RegDefCount,
                                              count((decode(q.isdefected,0,q.vin,null))) as StrCount,
                                              count(distinct(decode(q.qcstrgt_srl,62,q.vin,42,q.vin,101,q.vin,null))) as ASPCarCount,
                                              count((decode(q.qcstrgt_srl,62,q.vin,42,q.vin,101,q.vin,null))) as ASPRegDefCount,
                                              count(distinct (decode(q.qcstrgt_srl,42,q.vin,101,q.vin,null))) as TotalSPCarCount,
                                              count((decode(q.qcstrgt_srl,42,q.vin,101,q.vin,null))) as TotalSPRegDefCount,
                                              count(distinct (decode(q.qcstrgt_srl,62,q.vin,null))) as TotalACarCount,
                                              count((decode(q.qcstrgt_srl,62,q.vin,null))) as TotalARegDefCount,
                                              count(distinct (decode(q.qcstrgt_srl,63,q.vin,null))) as TotalBCarCount,
                                              count((decode(q.qcstrgt_srl,63,q.vin,null))) as TotalBRegDefCount,
                                              count(distinct (decode(q.qcstrgt_srl,62,q.vin,42,q.vin,101,q.vin,63,q.vin,null))) as BPlusCarCount,
                                              count((decode(q.qcstrgt_srl,62,q.vin,42,q.vin,101,q.vin,63,q.vin,null))) as BPlusRegDefCount,
                                             (select count(c.vin) from qccastt c where  {1} and c.isdefected=1 and c.qcareat_srl=q.qcareat_srl and c.deletedby = q.createdby) as DefectDeletedCount
                                             from qccastt q join qccariddt cd on cd.vin = q.vin 
                                                            join qcusert u on u.srl = q.createdby
                                             where {1} and q.recordowner=1 and q.qcareat_srl={0} 
                                             group by q.qcareat_srl,q.createdby,u.userid,u.fname,u.lname 
                                             order by DetectCarCount desc
                                                ", _QcAreatSrl, strDateCondition);
                        ds = DBHelper.GetDBObjectByDataSet(null, commandtext);
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            //---
                            s = new Summary();
                            s.UserId = Convert.ToInt32(ds.Tables[0].Rows[i]["UserId"].ToString());
                            s.FName = ds.Tables[0].Rows[i]["FName"].ToString();
                            s.LName = ds.Tables[0].Rows[i]["LName"].ToString();
                            s.SummaryTitle = "تعداد شاسی بازرسی شده";
                            s.SummaryValue = Convert.ToInt32(ds.Tables[0].Rows[i]["DetectCarCount"].ToString());
                            lstSummary.Add(s);
                            //---
                            s = new Summary();
                            s.UserId = Convert.ToInt32(ds.Tables[0].Rows[i]["UserId"].ToString());
                            s.FName = ds.Tables[0].Rows[i]["FName"].ToString();
                            s.LName = ds.Tables[0].Rows[i]["LName"].ToString();
                            s.SummaryTitle = "تعداد شاهین بازرسی شده";
                            s.SummaryValue = Convert.ToInt32(ds.Tables[0].Rows[i]["SP100DetectCarCount"].ToString());
                            lstSummary.Add(s);
                            //---
                            s = new Summary();
                            s.UserId = Convert.ToInt32(ds.Tables[0].Rows[i]["UserId"].ToString());
                            s.FName = ds.Tables[0].Rows[i]["FName"].ToString();
                            s.LName = ds.Tables[0].Rows[i]["LName"].ToString();
                            s.SummaryTitle = "تعداد شاسی کوییک بازرسی شده";
                            s.SummaryValue = Convert.ToInt32(ds.Tables[0].Rows[i]["Q200DetectCarCount"].ToString());
                            lstSummary.Add(s);
                            //---
                            s = new Summary();
                            s.UserId = Convert.ToInt32(ds.Tables[0].Rows[i]["UserId"].ToString());
                            s.FName = ds.Tables[0].Rows[i]["FName"].ToString();
                            s.LName = ds.Tables[0].Rows[i]["LName"].ToString();
                            s.SummaryTitle = "تعداد شاسی تیبا بازرسی شده";
                            s.SummaryValue = Convert.ToInt32(ds.Tables[0].Rows[i]["TibaDetectCarCount"].ToString());
                            lstSummary.Add(s);
                            //---
                            s = new Summary();
                            s.UserId = Convert.ToInt32(ds.Tables[0].Rows[i]["UserId"].ToString());
                            s.FName = ds.Tables[0].Rows[i]["FName"].ToString();
                            s.LName = ds.Tables[0].Rows[i]["LName"].ToString();
                            s.SummaryTitle = "تعداد شاسی معیوب بازرسی شده ";
                            s.SummaryValue = Convert.ToInt32(ds.Tables[0].Rows[i]["DefCarCount"].ToString());
                            lstSummary.Add(s);
                            //---
                            s = new Summary();
                            s.UserId = Convert.ToInt32(ds.Tables[0].Rows[i]["UserId"].ToString());
                            s.FName = ds.Tables[0].Rows[i]["FName"].ToString();
                            s.LName = ds.Tables[0].Rows[i]["LName"].ToString();
                            s.SummaryTitle = "تعداد شاسی عبور مستقیم ";
                            s.SummaryValue = Convert.ToInt32(ds.Tables[0].Rows[i]["StrCarCount"].ToString());
                            lstSummary.Add(s);
                            //---
                            s = new Summary();
                            s.UserId = Convert.ToInt32(ds.Tables[0].Rows[i]["UserId"].ToString());
                            s.FName = ds.Tables[0].Rows[i]["FName"].ToString();
                            s.LName = ds.Tables[0].Rows[i]["LName"].ToString();
                            s.SummaryTitle = "تعداد ثبت عیب ";
                            s.SummaryValue = Convert.ToInt32(ds.Tables[0].Rows[i]["RegDefCount"].ToString());
                            lstSummary.Add(s);
                            //---
                            s = new Summary();
                            s.UserId = Convert.ToInt32(ds.Tables[0].Rows[i]["UserId"].ToString());
                            s.FName = ds.Tables[0].Rows[i]["FName"].ToString();
                            s.LName = ds.Tables[0].Rows[i]["LName"].ToString();
                            s.SummaryTitle = "تعداد ثبت عیب ایمنی ";
                            s.SummaryValue = Convert.ToInt32(ds.Tables[0].Rows[i]["ASPRegDefCount"].ToString());
                            lstSummary.Add(s);
                            //
                            //
                            s = new Summary();
                            s.UserId = Convert.ToInt32(ds.Tables[0].Rows[i]["UserId"].ToString());
                            s.FName = ds.Tables[0].Rows[i]["FName"].ToString();
                            s.LName = ds.Tables[0].Rows[i]["LName"].ToString();
                            s.SummaryTitle = "تعداد خودرو با عیب SP در ناحیه ";
                            s.SummaryValue = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalSPCarCount"].ToString());
                            lstSummary.Add(s);
                            //
                            s = new Summary();
                            s.UserId = Convert.ToInt32(ds.Tables[0].Rows[i]["UserId"].ToString());
                            s.FName = ds.Tables[0].Rows[i]["FName"].ToString();
                            s.LName = ds.Tables[0].Rows[i]["LName"].ToString();
                            s.SummaryTitle = "تعداد ثبت عیب SP در ناحیه ";
                            s.SummaryValue = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalSPRegDefCount"].ToString());
                            lstSummary.Add(s);
                            //
                            s = new Summary();
                            s.UserId = Convert.ToInt32(ds.Tables[0].Rows[i]["UserId"].ToString());
                            s.FName = ds.Tables[0].Rows[i]["FName"].ToString();
                            s.LName = ds.Tables[0].Rows[i]["LName"].ToString();
                            s.SummaryTitle = "تعداد خودرو با عیب A در ناحیه ";
                            s.SummaryValue = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalACarCount"].ToString());
                            lstSummary.Add(s);
                            //
                            s = new Summary();
                            s.UserId = Convert.ToInt32(ds.Tables[0].Rows[i]["UserId"].ToString());
                            s.FName = ds.Tables[0].Rows[i]["FName"].ToString();
                            s.LName = ds.Tables[0].Rows[i]["LName"].ToString();
                            s.SummaryTitle = "تعداد ثبت عیب A در ناحیه ";
                            s.SummaryValue = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalARegDefCount"].ToString());
                            lstSummary.Add(s);
                            //
                            s = new Summary();
                            s.UserId = Convert.ToInt32(ds.Tables[0].Rows[i]["UserId"].ToString());
                            s.FName = ds.Tables[0].Rows[i]["FName"].ToString();
                            s.LName = ds.Tables[0].Rows[i]["LName"].ToString();
                            s.SummaryTitle = "تعداد خودرو با عیب B در ناحیه ";
                            s.SummaryValue = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalBCarCount"].ToString());
                            lstSummary.Add(s);
                            //
                            s = new Summary();
                            s.UserId = Convert.ToInt32(ds.Tables[0].Rows[i]["UserId"].ToString());
                            s.FName = ds.Tables[0].Rows[i]["FName"].ToString();
                            s.LName = ds.Tables[0].Rows[i]["LName"].ToString();
                            s.SummaryTitle = "تعداد ثبت عیب B در ناحیه ";
                            s.SummaryValue = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalBRegDefCount"].ToString());
                            lstSummary.Add(s);
                            //
                            //---
                            s = new Summary();
                            s.UserId = Convert.ToInt32(ds.Tables[0].Rows[i]["UserId"].ToString());
                            s.FName = ds.Tables[0].Rows[i]["FName"].ToString();
                            s.LName = ds.Tables[0].Rows[i]["LName"].ToString();
                            s.SummaryTitle = "تعداد ثبت عیب B پلاس ";
                            s.SummaryValue = Convert.ToInt32(ds.Tables[0].Rows[i]["BPlusRegDefCount"].ToString());
                            lstSummary.Add(s);
                            //---
                            s = new Summary();
                            s.UserId = Convert.ToInt32(ds.Tables[0].Rows[i]["UserId"].ToString());
                            s.FName = ds.Tables[0].Rows[i]["FName"].ToString();
                            s.LName = ds.Tables[0].Rows[i]["LName"].ToString();
                            s.SummaryType = "D";
                            s.SummaryTitle = "تعداد حذف عیب";
                            s.SummaryValue = Convert.ToInt32(ds.Tables[0].Rows[i]["DefectDeletedCount"].ToString());
                            lstSummary.Add(s);
                        }
                        return lstSummary;
                    }
                    else return null;
                }
                else // Herasat Report
                {
                    string commandtext = string.Format(@"select count(distinct q.vin) as TotalDetectCarCount
                                             from QCProT q  where {0} ", strDateCondition);
                    DataSet ds = DBHelper.GetDBObjectByDataSet(null, commandtext);
                    Summary s = new Summary();
                    if ((ds != null) && (ds.Tables[0] != null) && (ds.Tables[0].Rows.Count > 0)) // user find
                    {
                        s = new Summary();
                        s.UserId = 0;
                        s.SummaryType = "P";
                        s.SummaryTitle = "تعداد بازرسی حراست";
                        s.SummaryValue = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalDetectCarCount"].ToString());
                        lstSummary.Add(s);

                        // by usert statistic
                        commandtext = string.Format(@"select q.createdby,u.userid,u.fname,u.lname,
                                                         count(distinct q.vin) as DetectCarCount
                                                   from QCProT q
                                                   join qcusert u on u.srl = q.createdby                 
                                                   where {0}
                                                   group by q.createdby,u.userid,u.fname,u.lname 
                                                   order by DetectCarCount desc
                                                ", strDateCondition);
                        ds = DBHelper.GetDBObjectByDataSet(null, commandtext);
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            //---
                            s = new Summary();
                            s.UserId = Convert.ToInt32(ds.Tables[0].Rows[i]["UserId"].ToString());
                            s.FName = ds.Tables[0].Rows[i]["FName"].ToString();
                            s.LName = ds.Tables[0].Rows[i]["LName"].ToString();
                            s.SummaryType = "P";
                            s.SummaryTitle = "تعداد شاسی بازرسی شده";
                            s.SummaryValue = Convert.ToInt32(ds.Tables[0].Rows[i]["DetectCarCount"].ToString());
                            lstSummary.Add(s);
                            //---
                        }
                        return lstSummary;
                    }
                    else
                        return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}