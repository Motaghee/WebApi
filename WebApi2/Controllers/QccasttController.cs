using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi2.Models;
using WebApi2.Models.utility;

namespace WebApi2.Controllers
{
    public class QccasttController : ApiController
    {
        // GET: api/Qccastt
        public List<Qccastt> Get()
        {
            Qccastt qccastt = new Qccastt();
            qccastt.Vin = "NAS411100K1146021";
            clsDBHelper.LogtxtToFile("Get");
            clsDBHelper.LogtxtToFile("1");
            try
            {
                clsDBHelper.LogtxtToFile("2");
                if ((qccastt != null)) // && (U.Macaddress == "48:13:7e:11:d7:1f"))
                {
                    clsDBHelper.LogtxtToFile("3");
                    qccastt.ValidFormat = CarUtility.CheckFormatVin(qccastt.Vin);
                    if (qccastt.ValidFormat)
                    {
                        qccastt.VinWithoutChar = CarUtility.GetVinWithoutChar(qccastt.Vin);
                        clsDBHelper.LogtxtToFile("4");
                        string commandtext = string.Format(@"select q.srl,q.vin,
                                                               a.areacode,
                                                               s.strenghtdesc,
                                                               m.modulecode,
                                                               d.defectcode,
                                                               m.modulename,
                                                               d.defectdesc,
                                                               bm.grpcode,
                                                               cg.grpname,
                                                               t.title,
                                                               q.inuse,
                                                               a.areacode||a.areadesc as AreaDesc,
                                                               u.lname as inspector,
                                                               TO_char(q.createddate,'YYYY/MM/DD HH24:MI:SS','nls_calendar=persian') as createddateFa
       
                                                          from qccastt q
                                                          join qcusert u on u.srl = q.createdby
                                                          join carid c on c.vin = q.vin 
                                                          join bodymodel bm on bm.bdmdlcode=c.bdmdlcode
                                                          join qcareat a
                                                            on q.qcareat_srl = a.srl
                                                          join qcmdult m
                                                            on q.qcmdult_srl = m.srl
                                                          join qcbadft d
                                                            on q.qcbadft_srl = d.srl
                                                          join qcstrgt s
                                                            on q.qcstrgt_srl = s.srl
                                                          join cargroup cg on cg.grpcode=bm.grpcode
                                                          join qccabdt t on t.srl = d.qccabdt_srl
                                                         where q.vin= '411100K1146021'", qccastt.VinWithoutChar);
                        //DataSet ds = clsDBHelper.ExecuteMyQuery(commandtext);
                        //List<Qccastt> lst = new List<Qccastt>();
                        //DataTable dt = new DataTable();
                        //clsDBHelper.ConvertDataTable(ds);
                        List<Qccastt> FoundDefects = new List<Qccastt>();
                        clsDBHelper.LogtxtToFile("5" + commandtext);
                        FoundDefects = clsDBHelper.GetDBObjectByObj2(new Qccastt(), null, commandtext).Cast<Qccastt>().ToList();
                        clsDBHelper.LogtxtToFile("6");
                        //---
                        clsDBHelper.LogtxtToFile("6 count=" + FoundDefects.Count);
                        if (FoundDefects.Count > 0)
                        {
                            return FoundDefects;
                        }
                        else
                        {
                            List<Qccastt> q = new List<Qccastt>();
                            qccastt.Msg = "اطلاعاتی یافت نشد";
                            q.Add(qccastt);
                            return q;
                        }
                    }
                    else
                    {
                        List<Qccastt> q = new List<Qccastt>();
                        qccastt.Msg = "شاسی غیر مجاز است";
                        q.Add(qccastt);
                        return q;
                    }

                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                clsDBHelper.LogFile(e);
                List<Qccastt> q = new List<Qccastt>();
                qccastt.Msg = e.Message;
                q.Add(qccastt);
                return q;
            }

            //return new string[] { "value1", "value2" };

        }

        // GET: api/Qccastt/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Qccastt
        public void Post([FromBody]string value)
        {
        }

        // POST: api/qccastt
        [HttpPost]
        public List<Qccastt> Post([FromBody] Qccastt qccastt)
        {
            try
            {
                if ((qccastt != null)) // && (U.Macaddress == "48:13:7e:11:d7:1f"))
                {
                    qccastt.ValidFormat = CarUtility.CheckFormatVin(qccastt.Vin);
                    if (qccastt.ValidFormat)
                    {
                        qccastt.VinWithoutChar = CarUtility.GetVinWithoutChar(qccastt.Vin);
                        string commandtext = string.Format(@"select q.srl,q.vin,
                                                               a.areacode,
                                                               s.strenghtdesc,
                                                               m.modulecode,
                                                               d.defectcode,
                                                               m.modulename,
                                                               d.defectdesc,
                                                               bm.grpcode,
                                                               cg.grpname,
                                                               t.title,
                                                               q.inuse,
                                                               a.areacode||a.areadesc as AreaDesc,
                                                               u.lname as inspector,
                                                               TO_char(q.createddate,'YYYY/MM/DD HH24:MI:SS','nls_calendar=persian') as createddateFa
       
                                                          from qccastt q
                                                          join qcusert u on u.srl = q.createdby
                                                          join carid c on c.vin = q.vin 
                                                          join bodymodel bm on bm.bdmdlcode=c.bdmdlcode
                                                          join qcareat a
                                                            on q.qcareat_srl = a.srl
                                                          join qcmdult m
                                                            on q.qcmdult_srl = m.srl
                                                          join qcbadft d
                                                            on q.qcbadft_srl = d.srl
                                                          join qcstrgt s
                                                            on q.qcstrgt_srl = s.srl
                                                          join cargroup cg on cg.grpcode=bm.grpcode
                                                          join qccabdt t on t.srl = d.qccabdt_srl
                                                         where q.vin= '411100K1146021'", qccastt.VinWithoutChar);
                        //DataSet ds = clsDBHelper.ExecuteMyQuery(commandtext);
                        List<Qccastt> FoundDefects = new List<Qccastt>();
                        FoundDefects = clsDBHelper.GetDBObjectByObj2(new Qccastt(), null, commandtext).Cast<Qccastt>().ToList();
                        //---
                        if (FoundDefects.Count > 0)
                        {
                            return FoundDefects;
                        }
                        else
                        {
                            List<Qccastt> q = new List<Qccastt>();
                            qccastt.Msg = "اطلاعاتی یافت نشد";
                            q.Add(qccastt);
                            return q;
                        }
                    }
                    else
                    {
                        List<Qccastt> q = new List<Qccastt>();
                        qccastt.Msg = "شاسی غیر مجاز است";
                        q.Add(qccastt);
                        return q;
                    }

                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                clsDBHelper.LogFile(e);
                List<Qccastt> q = new List<Qccastt>();
                qccastt.Msg = e.Message;
                q.Add(qccastt);
                return q;
            }

        }
    }
}
