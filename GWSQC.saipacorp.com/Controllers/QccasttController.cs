using Common.Actions;
using Common.Models.QccasttModels;
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
        public List<QCDataMining> GetSaipaCitroenPDIData([FromBody] User _User)
        {
            try
            {
                if (!string.IsNullOrEmpty(_User.USERNAME) && _User.USERNAME != "0" && !string.IsNullOrEmpty(_User.PSW) && _User.PSW != "0"
                     && ((!string.IsNullOrEmpty(_User.Vin) && _User.Vin != "0") || (!string.IsNullOrEmpty(_User.SDate) && !string.IsNullOrEmpty(_User.EDate) && _User.SDate != "0" && _User.EDate != "0")))
                {
                    bool Login = Authentication.FindUser(_User.USERNAME, _User.PSW);
                    if (Login)
                    {
                        LogManager.MethodCallLog("GetSaipaCitroenPDIData _ RequestByUser: " + _User.USERNAME + " _RequestVin: " + _User.Vin);
                        return QccasttActs.GetSaipaCitroenPDIData(_User.Vin.ToUpper(), _User.SDate, _User.EDate);
                    }
                    else
                    {
                        LogManager.MethodCallLog("GetSaipaCitroenPDIData _ RequestByUser: " + _User.USERNAME + "_ AuthenticationFail");
                        return null;
                    }
                }
                else
                    return null; // "Check Your Parameters";
            }
            catch
            {
                return null;// "Error Occurrence";
            }
        }






    }
}
