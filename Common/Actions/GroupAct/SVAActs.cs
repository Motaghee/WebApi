using Common.db;
using Common.Models;
using Common.Models.Audit;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Common.Actions
{
    public static class SVAActs
    {
 
        public static List<DataMining> GetSaipaCitroenSVAAuditData(string _Vin,string _SDate,string _EDate)
        {
            try
            {
                // create Archive commande
                string commandtext = string.Format(@"select a.auditdate,
                                                        a.auditor2 as auditor,
                                                        a.vin,
                                                        a.areacode,
                                                        d.modulecode,
                                                        d.modulename,
                                                        d.defectcode,
                                                        d.defectdesc,
                                                        d.title,
                                                        d.defecttypedesc,
                                                        d.strenghtdesc,
                                                        d.negativescorevalue,
                                                        a.auditdate_fa,
                                                        ad.defectorigindesc,
                                                        ad.effectpercent,
                                                        a.comanyname,
                                                        a.aliasname,
                                                        a.YEARWEEKNO,
                                                        a.YEARMONTH
                                                    from sva_v_auditcardetaildefector ad
                                                    join Sva_v_Auditcardetail d
                                                     on ad.svaauditcardetail_srl = d.srl
                                                    join sva_v_auditcar_offline a
                                                    on d.svaauditcar_srl = a.srl
                                                    where (a.areacode =1002) 
                                                          And ('{0}'='0' or a.vin = '{0}')
                                                          And ('{1}'='0' or a.AUDITDATE >= TO_date('{1}','YYYY/MM/DD','nls_calendar=persian'))
                                                          And ('{2}'='0' or a.AUDITDATE <= TO_date('{2}','YYYY/MM/DD','nls_calendar=persian'))
                                                          ", _Vin.ToUpper(),_SDate,_EDate);
                // or ((a.areacode = 1000) And (a.svaauditvart_srl =2) ))
                List<DataMining> lst = new List<DataMining>();
                Object[] obj = DBHelper.GetDBObjectByObj2(new DataMining(), null, commandtext, "inspector");
                lst = obj.Cast<DataMining>().ToList();
                return lst;
                //object[] obj = DBHelper.GetDBObjectByObj2(new DataMining(), null, commandtext, "ins");
                //return obj;

            }
            catch (Exception ex)
            {
                LogManager.MethodCallLog("GetSaipaCitroenSVAAuditData: " + ex.Message.ToString());
                return null;
            }
        }

        public static List<DataMining> GetSaipaCitroenIVAAuditData(string _Vin, string _SDate, string _EDate)
        {
            try
            {
                // create Archive commande
                string commandtext = string.Format(@"select a.auditdate,
                                                            a.auditor,
                                                            a.vin,
                                                            a.areacode,
                                                            d.modulecode,
                                                            d.modulename,
                                                            d.defectcode,
                                                            d.defectdesc,
                                                            d.title,
                                                            d.defecttypedesc,
                                                            d.strenghtdesc,
                                                            d.negativescorevalue,
                                                            a.auditdate_fa,
                                                            ad.defectorigindesc,
                                                            ad.effectpercent,
                                                            a.comanyname,
                                                            a.aliasname,
                                                            a.YEARWEEKNO,
                                                            a.YEARMONTH
                                                        from sva_v_auditcardetaildefector ad
                                                        join iva_v_Auditcardetail d
                                                         on ad.svaauditcardetail_srl = d.srl
                                                        join iva_v_auditcar a
                                                        on d.svaauditcar_srl = a.srl
                                                        where a.areacode in (1102) 
                                                          And ('{0}'='0' or a.vin = '{0}')
                                                          And ('{1}'='0' or a.AUDITDATE >= TO_date('{1}','YYYY/MM/DD','nls_calendar=persian'))
                                                          And ('{2}'='0' or a.AUDITDATE <= TO_date('{2}','YYYY/MM/DD','nls_calendar=persian'))
                                                        ", _Vin, _SDate, _EDate);
                List<DataMining> lst = new List<DataMining>();

                Object[] obj = DBHelper.GetDBObjectByObj2(new DataMining(), null, commandtext, "inspector");
                lst = obj.Cast<DataMining>().ToList();
                return lst;


//                return DBHelper.GetDBObjectByObj2(new DataMining(), null, commandtext, "ins");
                //return lst;
            }
            catch (Exception ex)
            {
                LogManager.MethodCallLog("GetSaipaCitroenIVAAuditData: " + ex.Message.ToString());
                return null;


            }
        }

    }
}