using Common.db;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Common.Models;
using Common.Models.Car;

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


    }
}