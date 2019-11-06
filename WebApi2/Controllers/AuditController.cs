using System;
using System.Collections.Generic;
using System.Web.Http;
using WebApi2.Controllers.Utility;
using WebApi2.Models;

namespace WebApi2.Controllers
{
    public class AuditController : ApiController
    {

        [HttpPost]
        [Route("api/audit/AuditUnLockCar")]
        public Car UnLockCar([FromBody] Car car)
        {
            try
            {
                if ((car != null)) // && (U.Macaddress == "48:13:7e:11:d7:1f"))
                {
                    car.ValidFormat = CarUtility.CheckFormatVin(car.Vin);

                    car.AuditEditable = false;
                    if (car.ValidFormat)
                    {
                        car.VinWithoutChar = CarUtility.GetVinWithoutChar(car.Vin);
                        ResultMsg rm = AuditUtility.UnLockCar(car.VinWithoutChar);
                        if (rm.title != "error")
                        {
                            int intResult = Convert.ToInt32(rm.Message);
                            if (intResult > 0)
                            {
                                car.AuditEditable = true;
                                return car;
                            }
                            else
                            {
                                return car;
                            }
                        }
                        else
                        {
                            car.msg = rm.Message;
                            return car;
                        }
                    }
                    else
                        return car;
                }
                else
                {
                    return car;
                }
            }
            catch (Exception e)
            {
                car.msg = "exc1:" + e.Message;
                return car;
            }

        }


        [HttpPost]
        [Route("api/audit/GetAuditCarCheckList")]
        public List<AuditCarDetail> AuditCarCheckList([FromBody] AuditCarDetail auditcardetails)
        {
            return AuditUtility.GetAuditCarCheckList(auditcardetails);
        }

    }
}
