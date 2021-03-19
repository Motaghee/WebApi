using Common.db;
using Common.Models.Qccastt;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WebApi2.Models;

namespace WebApi2.Controllers.Utility
{
    public class CarUtility
    {

        public static bool CheckFormatVin(string vin)
        {
            bool value = false;
            vin = vin.Trim().ToUpper();
            if ((vin.StartsWith("NAS")) && (vin.Length == 17))
                value = true;
            return value;

        }
        public static string GetVinWithoutChar(string vin)
        {
            string value = vin;
            if (!string.IsNullOrEmpty(vin))
            {
                vin = vin.Trim().ToUpper();
                if (vin.StartsWith("S"))
                    value = vin.Replace("S", "");
                else
                    if (vin.StartsWith("NAS"))
                    value = vin.Replace("NAS", "");
            }
            return value;

        }

        public static Car GetCarInfo(Car car)
        {
            try
            {
                if ((car != null)) // && (U.Macaddress == "48:13:7e:11:d7:1f"))
                {
                    //Value cannot be null.
                    car.ValidFormat = CheckFormatVin(car.Vin);
                    car.VinWithoutChar = GetVinWithoutChar(car.Vin);
                    if (car.ValidFormat)
                    {
                        List<Car> carinfo = new List<Car>();
                        string commandtext = string.Format(@"select c.vin,c.prodno,c.joinerydate,c.bdmdlcode,c.bdstlcode,c.bdstlaliasname ,c.fitypecode,c.finqccode,c.clrcode, (select q.toareasrl from qcqctrt q where vin = '{0}' and passed=0) as CurAreaSrl,
                                                            c.JoinaryTeamDesc,c.nasvin,c.shopcode,c.shopname,c.joinaryteam,c.assmteamwork,c.assemblytypecode,c.gearboxtypecode,c.forexport,c.grpcode,c.bdmdlaliasname,
                                                            c.grpname,c.comanyname as companyname,c.companycode,c.fitypename,c.clralias,c.gearboxtypedesc,c.prodenddate,c.prodenddate_fa,c.joinerydate_fa,c.bodyshopproddate,c.paintshopproddate,asmshopproddate
                                                            from qccariddt c where c.vin ='{0}' "
                                                            , car.VinWithoutChar);
                        // 
                        carinfo = DBHelper.GetDBObjectByObj2_OnLive(new Car(), null, commandtext, "inspector").Cast<Car>().ToList();
                        if (carinfo.Count == 1)
                        {
                            carinfo[0].ValidFormat = car.ValidFormat;
                            carinfo[0].Vin = car.Vin;
                            carinfo[0].VinWithoutChar = car.VinWithoutChar;
                            commandtext = string.Format(@"select rownum as RowIndex,z.* from (select tr.srl,case when length(tr.vin) = 14 then 'NAS' || tr.vin else 'S' || tr.vin end as VIN,
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
                                                     order by tr.seq desc)z", car.VinWithoutChar);
                            List<Qcqctrt> lst = new List<Qcqctrt>();
                            lst = DBHelper.GetDBObjectByObj2_OnLive(new Qcqctrt(), null, commandtext, "ins").Cast<Qcqctrt>().ToList();
                            carinfo[0].lstQcqctrt = lst;
                            Qccastt q = new Qccastt();
                            q.Vin = car.Vin;
                            carinfo[0].lstQccastt= QccasttUtility.GetCarDefect(q);
                            return carinfo[0];
                            //
                            //commandtext = string.Format(@"select q.*,
                            //               TO_char(q.u_date,'YYYY/MM/DD HH24:MI','nls_calendar=persian') ||'_' || a1.areacode || '_' || a1.areadesc  as FromAreaCodeDesc,
                            //               TO_char(q.u_date,'YYYY/MM/DD HH24:MI','nls_calendar=persian') ||'_' || a2.areacode || ' ' || a2.areadesc as ToAreaCodeDesc
                            //          from qcqctrt q
                            //          join qcareat a1
                            //            on a1.srl = q.fromareasrl
                            //          join qcareat a2
                            //            on a2.srl = q.toareasrl
                            //         where q.vin = '{0}'
                            //         order by seq
                            //         ", car.VinWithoutChar);
                            //string strQCTrace = "";

                            //--
                            //string JSONresult;
                            //JSONresult = JsonConvert.SerializeObject(dt);
                            //Response.Write(JSONresult);

                            //--
                            //for (int i = 0; i < dsQCTrace.Tables[0].Rows.Count; i++)
                            //{
                            //    if (!strQCTrace.Equals(""))
                            //        strQCTrace += System.Environment.NewLine;
                            //    strQCTrace = strQCTrace + dsQCTrace.Tables[0].Rows[i]["FromAreaCodeDesc"].ToString() + "";
                            //    if (i == dsQCTrace.Tables[0].Rows.Count - 1)
                            //    {
                            //        if (!strQCTrace.Equals(""))
                            //            strQCTrace += System.Environment.NewLine;
                            //        strQCTrace = strQCTrace + dsQCTrace.Tables[0].Rows[i]["ToAreaCodeDesc"].ToString() + "";
                            //    }

                            //}
                        }
                        else
                        {
                            car.msg = "car not found" + commandtext;
                            return car;
                        }

                    }
                    else
                    {
                        car.msg = "vin is not valid format";
                        return car;
                    }
                }
                else
                {
                    car.msg = "Car Is Null";
                    return car;
                }
            }
            catch (Exception ex)
            {
                car.msg = ex.Message.ToString();
                //string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
                //message += Environment.NewLine;
                //message += "-----------------------------------------------------------";
                //message += Environment.NewLine;
                //message += string.Format("Message: {0}", ex.Message);
                //message += Environment.NewLine;
                //message += string.Format("StackTrace: {0}", ex.StackTrace);
                //message += Environment.NewLine;
                //message += string.Format("Source: {0}", ex.Source);
                //message += Environment.NewLine;
                //message += string.Format("TargetSite: {0}", ex.TargetSite.ToString());
                //message += Environment.NewLine;
                //message += "-----------------------------------------------------------";
                //message += Environment.NewLine;
                //string path = @"C:/ErrorLog/ErrorLog.txt";
                //StreamWriter writer = new StreamWriter(path, true);
                //writer.WriteLine(message);
                //writer.Close();
                return car;
            }

        }
    }
}