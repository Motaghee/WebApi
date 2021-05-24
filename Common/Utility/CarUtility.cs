using Common.db;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Common.Models.Car;
using Common.Models.QccasttModels;

namespace Common.Utility
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

        public static object[] GetBaseCarGroupList()
        {
            try
            {
                string commandtext = string.Format(@"select GrpCode,GrpName,SmsTitle from pt.cargroup cg where inprod=1");
                return DBHelper.GetDBObjectByObj2(new CarGroup(), null, commandtext, "ins");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static object[] GetBaseBodyModelList()
        {
            try
            {
                string commandtext = string.Format(@"select bdmdlcode,grpcode,aliasname,case when bm.aliasname='تيبا 211' then 'تیبا2' when bm.aliasname='تيبا 212'  or bm.aliasname='تيبا 232'  then gs.name else bm.aliasname end  as CommonBodyModelName  from pt.bodymodel bm JOIN pt.groupsproductsmsgroup gs on gs.smsgrpid=bm.smsgrpid where bm.inprod =1");
                return DBHelper.GetDBObjectByObj2(new BodyModel(), null, commandtext, "ins");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static DataSet GetdsBaseBodyModelList(string _GrpCode)
        {
            try
            {
                if (!string.IsNullOrEmpty(_GrpCode))
                {
                    _GrpCode = string.Format(" and bm.grpcode in ({0})", _GrpCode);

                }
                string commandtext = string.Format(@"select bdmdlcode,grpcode,aliasname,case when bm.aliasname='تيبا 211' then 'تیبا2' when bm.aliasname='تيبا 212'  or bm.aliasname='تيبا 232'  then gs.name else bm.aliasname end  as CommonBodyModelName  from pt.bodymodel bm JOIN pt.groupsproductsmsgroup gs on gs.smsgrpid=bm.smsgrpid where bm.inprod =1 {0}", _GrpCode);
                return DBHelper.ExecuteMyQueryIns(commandtext);

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static object[] GetBodyStyle(string _GrpCode, string _BdmdlCode)
        {
            try
            {
                if (!string.IsNullOrEmpty(_GrpCode))
                {
                    _GrpCode = string.Format(" And bm.grpcode in ({0})", _GrpCode);
                }
                if (!string.IsNullOrEmpty(_BdmdlCode))
                {
                    _BdmdlCode = string.Format(" And s.bdmdlcode in ({0})", _BdmdlCode);
                }
                string commandtext = string.Format(@"select s.bdstlcode,s.bdmdlcode,bm.grpcode,s.aliasname from pt.bodystyle s
                                                                                                            join bodymodel bm on bm.bdmdlcode=s.bdmdlcode 
                                                    Where s.ISACTIVE=1 {0} {1} order by s.bdmdlcode desc", _GrpCode, _BdmdlCode);
                return DBHelper.GetDBObjectByObj2(new BodyStyle(), null, commandtext, "ins");
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static DataSet GetdsBodyStyle(string _GrpCode, string _BdmdlCode)
        {
            try
            {
                if (!string.IsNullOrEmpty(_GrpCode))
                {
                    _GrpCode = string.Format(" And bm.grpcode in ({0})", _GrpCode);
                }
                if (!string.IsNullOrEmpty(_BdmdlCode))
                {
                    _BdmdlCode = string.Format(" And s.bdmdlcode in ({0})", _BdmdlCode);
                }
                string commandtext = string.Format(@"select s.bdstlcode,s.bdmdlcode,bm.grpcode,s.aliasname from pt.bodystyle s
                                                                                                            join bodymodel bm on bm.bdmdlcode=s.bdmdlcode 
                                                    Where s.ISACTIVE=1 {0} {1} order by s.bdmdlcode desc", _GrpCode, _BdmdlCode);
                return DBHelper.ExecuteMyQueryIns(commandtext);
            }
            catch (Exception ex)
            {
                return null;
            }
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
                        string commandtext = string.Format(@"select c.vin,c.prodno,c.joinerydate,c.bdmdlcode,c.bdstlcode,c.bdstlaliasname ,c.fitypecode,c.finqccode,c.clrcode, (select q.toareasrl from qcqctrt q where vin =c.vin and passed=0) as CurAreaSrl,
                                                            c.JoinaryTeamDesc,c.nasvin,c.shopcode,c.shopname,c.joinaryteam,c.assmteamwork,c.assemblytypecode,c.gearboxtypecode,c.forexport,c.grpcode,c.bdmdlaliasname,
                                                            c.grpname,c.comanyname as companyname,c.companycode,c.fitypename,c.clralias,c.gearboxtypedesc,c.prodenddate,substr(c.prodenddate_fa,0,16) as prodenddate_fa,substr(c.joinerydate_fa,0,16) as joinerydate_fa,substr(c.bodyshopproddate,0,16)as bodyshopproddate,substr(c.paintshopproddate,0,16) as paintshopproddate,substr(asmshopproddate,0,16) as asmshopproddate,
                                                            (select distinct (s.shopcode) from pt.cartrace ct join pt.station s on ct.stncode = s.stncode where ct.vin = c.vin and ct.passed=0) as PTCurrentShopCode,
                                                            {1} as ActAreaSrl,{2} as ActBy,
                                                            (select count(srl) from qccastt t where t.qcareat_srl ={1} and t.vin = c.vin and t.isdefected=1 and t.inuse=1 and t.deletedby is null and t.recordowner=1 ) CurrentAreaDefCount
                                                            from qccariddt c where c.vin ='{0}' "
                                                            , car.VinWithoutChar, car.ActAreaSrl, car.ActBy);
                        // 
                        carinfo = DBHelper.GetDBObjectByObj2_OnLive(new Car(), null, commandtext, "inspector").Cast<Car>().ToList();
                        if (carinfo.Count == 1)
                        {
                            carinfo[0].ValidFormat = car.ValidFormat;
                            carinfo[0].Vin = car.Vin;
                            carinfo[0].VinWithoutChar = car.VinWithoutChar;
                            Qccastt q = new Qccastt();
                            q.Vin = car.Vin;
                            q.ActAreaSrl = car.ActAreaSrl;
                            q.ActBy = car.ActBy;
                            q.VinWithoutChar = car.VinWithoutChar;
                            carinfo[0].lstQcqctrt = QccasttUtility.GetCarTrace(q);
                            carinfo[0].lstQccastt = QccasttUtility.GetCarDefect(q);
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