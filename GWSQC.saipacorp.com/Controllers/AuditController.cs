using Common.Actions;
using Common.Models.Audit;
using GWSQC.saipacorp.com.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace GWSQC.saipacorp.com.Controllers
{
    public class AuditController : ApiController
    {
        [HttpPost]
        [Route("api/audit/GetSaipaCitroenSVAAuditData")]
        public List<DataMining> GetSaipaCitroenSVAAuditData([FromBody] User _User)
        {
            try
            {
                if (!string.IsNullOrEmpty(_User.USERNAME) && _User.USERNAME != "0" && !string.IsNullOrEmpty(_User.PSW) && _User.PSW != "0"
                     && ((!string.IsNullOrEmpty(_User.Vin) && _User.Vin != "0") || (!string.IsNullOrEmpty(_User.SDate) && !string.IsNullOrEmpty(_User.EDate) && _User.SDate != "0" && _User.EDate != "0")))
                {
                    bool Login = Authentication.FindUser(_User.USERNAME, _User.PSW);
                    if (Login)
                    {
                        //LogManager.MethodCallLog("GetSaipaCitroenSVAAuditData _ RequestByUser: " + _User.USERNAME + " _RequestVin: " + _User.Vin + " _SDate: " + _User.SDate + " _EDate: " + _User.EDate);
                        return SVAActs.GetSaipaCitroenSVAAuditData(_User.Vin.ToUpper(), _User.SDate, _User.EDate);
                    }
                    else
                    {
                        //LogManager.MethodCallLog("GetSaipaCitroenSVAAuditData _ RequestByUser: " + _User.USERNAME + "_ AuthenticationFail");
                        return null;// "Authentication Fail";
                    }
                }
                else
                    return null;// "Check Your Parameters";
            }
            catch (Exception e)
            {
                throw e;// "Error Occurrence";
            }
        }

        [HttpGet]
        [Route("api/audit/GetSaipaCitroenSVAAuditData2")]
        public List<DataMining> GetSaipaCitroenSVAAuditData2()
        {
            try
            {
                User _User = new User();
                _User.Vin = "0";
                _User.SDate = "1399/10/01";
                _User.EDate = "1399/10/02";
                _User.USERNAME = "1000861";
                _User.PSW = "0082397171";
                if (!string.IsNullOrEmpty(_User.USERNAME) && _User.USERNAME != "0" && !string.IsNullOrEmpty(_User.PSW) && _User.PSW != "0"
                     && ((!string.IsNullOrEmpty(_User.Vin) && _User.Vin != "0") || (!string.IsNullOrEmpty(_User.SDate) && !string.IsNullOrEmpty(_User.EDate) && _User.SDate != "0" && _User.EDate != "0")))
                {
                    bool Login = Authentication.FindUser(_User.USERNAME, _User.PSW);
                    if (Login)
                    {
                        //LogManager.MethodCallLog("GetSaipaCitroenSVAAuditData _ RequestByUser: " + _User.USERNAME + " _RequestVin: " + _User.Vin + " _SDate: " + _User.SDate + " _EDate: " + _User.EDate);
                        List<DataMining> lst = SVAActs.GetSaipaCitroenIVAAuditData(_User.Vin.ToUpper(), _User.SDate, _User.EDate);
                        return lst;

                    }
                    else
                    {
                        //LogManager.MethodCallLog("GetSaipaCitroenSVAAuditData _ RequestByUser: " + _User.USERNAME + "_ AuthenticationFail");
                        return null; //"Authentication Fail";
                    }
                }
                else
                    return null; //"Check Your Parameters";
            }
            catch  (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        [Route("api/audit/GetSaipaCitroenIVAAuditData")]
        public List<DataMining> GetSaipaCitroenIVAAuditData([FromBody] User _User)
        {
            if (!string.IsNullOrEmpty(_User.USERNAME) && _User.USERNAME != "0" && !string.IsNullOrEmpty(_User.PSW) && _User.PSW != "0"
                     && ((!string.IsNullOrEmpty(_User.Vin) && _User.Vin != "0") || (!string.IsNullOrEmpty(_User.SDate) && !string.IsNullOrEmpty(_User.EDate) && _User.SDate != "0" && _User.EDate != "0")))
            {
                bool Login = Authentication.FindUser(_User.USERNAME, _User.PSW);
                if (Login)
                {
                    LogManager.MethodCallLog("GetSaipaCitroenIVAAuditData _ RequestByUser: " + _User.USERNAME + " _RequestVin: " + _User.Vin + " _SDate: " + _User.SDate + " _EDate: " + _User.EDate);
                    return SVAActs.GetSaipaCitroenIVAAuditData(_User.Vin.ToUpper(), _User.SDate, _User.EDate);
                }
                else
                {
                    LogManager.MethodCallLog("GetSaipaCitroenIVAAuditData _ RequestByUser: " + _User.USERNAME + "_ AuthenticationFail");
                    return null;
                }
            }
            else
                return null;// "Check Your Parameters";
        }



    }
}
