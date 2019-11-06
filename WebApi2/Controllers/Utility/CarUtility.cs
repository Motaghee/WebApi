using Common.db;
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
                        string commandtext = string.Format(@"select z.*,(select count(srl) from qccarimgt q where vin ='{0}' and inuse=1) as CarImageCount
                                                                       ,(select Listagg(q.srl,',') from qccarimgt q where vin ='{0}' and inuse=1) as CarImageSrls
                                                                       ,(select q.toareasrl from qcqctrt q where vin = '{0}' and passed=0) as CurAreaSrl
                                                            ,case 
                                                             when (z.joinaryTeam is null) then
                                                              'عدم تجاري'
                                                             when (z.joinaryTeam ='Z') then
                                                               'Other'
                                                             when (z.shopcode = 14) then
                                                              JoinaryTeam
                                                             when (z.shopcode = 15) then
                                                              JoinaryTeam || '1'
                                                             when (z.shopcode = 23) then  
                                                               z.JoinaryTeam
                                                             else
                                                              'Other'
                                                             end as JoinaryTeamDesc,
                                                             'NAS'||vin as nasvin,
                                                                              (select distinct fn_getvinshopproddates(vin) from (select distinct sh.shopname,shopgrpcode
                                          from cartrace c
                                          join station s
                                            on s.stncode = c.stncode
                                          join shop sh on sh.shopcode = s.shopcode
                                         where vin = z.vin and c.statecode=1
                                            )) as pttrace                                                            

                                               from 
                                            (select c.vin,c.prodno,c.joinerydate,c.bdmdlcode,c.bdstlcode ,c.fitypecode,c.finqccode,c.clrcode,
                                                   pt.FNI_GetAsmProdShopCodeByVin (c.vin) as shopCode
                                                   ,pt.FNI_GetAsmProdShopByVin (c.vin) as shopname,
                                                   pt.FNI_GetStationTeamWorkByVin(c.vin, 'T',8) as JoinaryTeam,pt.FNI_GetStationTeamWorkByVin(c.vin, 'A',1) as assmteamwork,
                                                   c.assemblytypecode,bds.gearboxtypecode,f.forexport,bm.grpcode,bm.aliasname,cg.grpname,
                                                   pt.FNI_GetAsmProdCompanyByVin(c.vin) as companyName,pt.FNI_GetAsmProdCompanyCodeByVin(c.vin) as companycode
                                                   ,ft.fitypename,co.clralias,gbt.gearboxtypedesc,
                                                     c.prodenddate,
                                                     TO_char(prodenddate,'YYYY/MM/DD HH24:MI:SS','nls_calendar=persian') as prodenddate_Fa,
                                                     TO_char(joinerydate,'YYYY/MM/DD HH24:MI:SS','nls_calendar=persian') as joinerydate_fa
                                            from
                                                   carid c 
                                                   left join bodymodel bm on c.bdmdlcode = bm.bdmdlcode
                                                   left join pt.bodystyle bds on bds.bdstlcode = c.bdstlcode
                                                   left join finalqc f  on c.finqccode = f.finqccode
                                                   left join cargroup cg on cg.grpcode=bm.GRPCODE
                                                   left join pt.fueltype ft on c.fitypecode = ft.fitypecode
                                                   left join color co on co.clrcode = c.clrcode
                                                   join pt.gearboxtype gbt on gbt.gearboxtypecode=bds.gearboxtypecode
                                                   where c.vin ='{0}') z", car.VinWithoutChar);
                        // 
                        carinfo = clsDBHelper.GetDBObjectByObj2(new Car(), null, commandtext, "inspector").Cast<Car>().ToList();

                        if (carinfo.Count == 1)
                        {
                            carinfo[0].ValidFormat = car.ValidFormat;
                            carinfo[0].Vin = car.Vin;
                            car.VinWithoutChar = car.VinWithoutChar;
                            //
                            commandtext = string.Format(@"select q.*,
                                           TO_char(q.u_date,'YYYY/MM/DD HH24:MI','nls_calendar=persian') ||'_' || a1.areacode || '_' || a1.areadesc  as FromAreaCodeDesc,
                                           TO_char(q.u_date,'YYYY/MM/DD HH24:MI','nls_calendar=persian') ||'_' || a2.areacode || ' ' || a2.areadesc as ToAreaCodeDesc
                                      from qcqctrt q
                                      join qcareat a1
                                        on a1.srl = q.fromareasrl
                                      join qcareat a2
                                        on a2.srl = q.toareasrl
                                     where q.vin = '{0}'
                                     order by seq
                                     ", car.VinWithoutChar);
                            string strQCTrace = "";
                            DataSet dsQCTrace = clsDBHelper.GetDBObjectByDataSet(car, commandtext);
                            //--
                            //string JSONresult;
                            //JSONresult = JsonConvert.SerializeObject(dt);
                            //Response.Write(JSONresult);
                            //--
                            for (int i = 0; i < dsQCTrace.Tables[0].Rows.Count; i++)
                            {
                                if (!strQCTrace.Equals(""))
                                    strQCTrace += System.Environment.NewLine;
                                strQCTrace = strQCTrace + dsQCTrace.Tables[0].Rows[i]["FromAreaCodeDesc"].ToString() + "";
                                if (i == dsQCTrace.Tables[0].Rows.Count - 1)
                                {
                                    if (!strQCTrace.Equals(""))
                                        strQCTrace += System.Environment.NewLine;
                                    strQCTrace = strQCTrace + dsQCTrace.Tables[0].Rows[i]["ToAreaCodeDesc"].ToString() + "";
                                }

                            }
                            //
                            carinfo[0].QCTrace = strQCTrace;
                            carinfo[0].VinWithoutChar = car.VinWithoutChar;
                            return carinfo[0];
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