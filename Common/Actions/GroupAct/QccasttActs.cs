﻿using Common.db;
using Common.Models;
using Common.Models.Audit;
using Common.Utility;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace Common.Actions
{
    public static class QccasttActs
    {
 
        public static object GetSaipaCitroenPDIData(string _Vin)
        {
            try
            {
                // create Archive commande
                string commandtext = string.Format(@"select q.CREATEDDAY_FA,
                                                       TO_char(q.createddate,
                                                               'YYYY/MM/DD HH24:MI:SS',
                                                               'nls_calendar=persian') as CREATEDDAte_FA,
                                                       u.fname || ' ' || u.lname as CREATEDBYDesc,
                                                       'NAS'||q.vin as vin,
                                                       q.AREACODE,
                                                       q.AREADESC,
                                                       q.modulecode,
                                                       q.modulename,
                                                       q.defectcode,
                                                       q.defectdesc,
                                                       q.cabdtitle,
                                                       q.DefectTypeTitle,
                                                       s.strenghtdesc,
                                                       q.aliasname,
                                                       fn_getweekofyearsaipa2(createddate) as YEARWEEKNO,
                                                       q.YEARMONTH,
                                                       pt.FNI_GetAsmProdCompanyByVin(q.vin) as companyName
                                                  from qc_v_qccastt3 q
                                                  left join qcstrgt s
                                                    on s.srl = q.qcstrgt_srl
                                                  left join qcusert u
                                                    on u.srl = q.createdby
                                                 where q.QCAREAT_SRL = 441
                                                   and q.vin='{0}'
                                                 ", VinUtility.GetVinWithoutChar(_Vin));
                //List<QCDataMining> lst = new List<QCDataMining>();
                return DBHelper.GetDBObjectByObj2(new QCDataMining(), null, commandtext, "ins");
                //return lst;
            }
            catch (Exception ex)
            {
                LogManager.MethodCallLog("GetSaipaCitroenPDIData: " + ex.Message.ToString());
                return null;
            }
        }



    }
}