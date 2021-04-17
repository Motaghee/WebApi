using Common.Actions;
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
        public object GetSaipaCitroenSVAAuditData([FromBody] User _User)
        {
            try
            {
                if (!string.IsNullOrEmpty(_User.USERNAME) && _User.USERNAME != "0" && !string.IsNullOrEmpty(_User.PSW) && _User.PSW != "0"
                     && ((!string.IsNullOrEmpty(_User.Vin) && _User.Vin != "0") || (!string.IsNullOrEmpty(_User.SDate) && !string.IsNullOrEmpty(_User.EDate) && _User.SDate != "0" && _User.EDate != "0")))
                {
                    bool Login = Authentication.FindUser(_User.USERNAME, _User.PSW);
                    if (Login)
                    {
                        //LogManager.MethodCallLog("GetSaipaCitroenSVAAuditData _ RequestByUser: " + _User.USERNAME + " _RequestVin: " + _User.Vin);
                        return SVAActs.GetSaipaCitroenSVAAuditData(_User.Vin.ToUpper(), _User.SDate, _User.EDate);
                    }
                    else
                    {
                        //LogManager.MethodCallLog("GetSaipaCitroenSVAAuditData _ RequestByUser: " + _User.USERNAME + "_ AuthenticationFail");
                        return "Authentication Fail";
                    }
                }
                else
                    return "Check Your Parameters";
            }
            catch
            {
                return "Error Occurrence";
            }
        }

        [HttpPost]
        [Route("api/audit/GetSaipaCitroenIVAAuditData")]
        public object GetSaipaCitroenIVAAuditData([FromBody] User _User)
        {
            bool Login = Authentication.FindUser(_User.USERNAME, _User.PSW);
            if (Login)
            {
                LogManager.MethodCallLog("GetSaipaCitroenIVAAuditData _ RequestByUser: " + _User.USERNAME + " _RequestVin: " + _User.Vin);
                return SVAActs.GetSaipaCitroenIVAAuditData(_User.Vin.ToUpper());
            }
            else
            {
                LogManager.MethodCallLog("GetSaipaCitroenIVAAuditData _ RequestByUser: " + _User.USERNAME + "_ AuthenticationFail");
                return null;
            }
        }



    }
}
