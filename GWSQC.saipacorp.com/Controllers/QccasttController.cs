using Common.Actions;
using GWSQC.saipacorp.com.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace GWSQC.saipacorp.com.Controllers
{
    public class QccasttController : ApiController
    {
        [HttpPost]
        [Route("api/Qccastt/GetSaipaCitroenPDIData")]
        public object GetSaipaCitroenPDIData([FromBody] User _User)
        {
            bool Login = Authentication.FindUser(_User.USERNAME, _User.PSW);
            if (Login)
            {
                LogManager.MethodCallLog("GetSaipaCitroenPDIData _ RequestByUser: " + _User.USERNAME + " _RequestVin: " + _User.Vin);
                return QccasttActs.GetSaipaCitroenPDIData(_User.Vin.ToUpper());
            }
            else
            {
                LogManager.MethodCallLog("GetSaipaCitroenPDIData _ RequestByUser: " + _User.USERNAME + "_ AuthenticationFail");
                return null;
            }
        }





    }
}
