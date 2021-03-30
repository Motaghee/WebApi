using Common.Models;
using Common.Models.Audit;
using Common.Models.Car;
using Common.Models.General;
using Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using WebApi2.Controllers.Utility;
using WebApi2.Models;

namespace WebApi2.Controllers
{
    public class AuditController : ApiController
    {

        [HttpPost]
        [Authorize]
        [Route("api/audit/AuditUnLockCar")]
        public Car UnLockCar([FromBody] Car car)
        {
            try
            {
                bool UserUnlockPermission = false;
                User user = new User();
                var identity = (ClaimsIdentity)User.Identity;
                IEnumerable<Claim> claims = identity.Claims;
                user.USERNAME = claims.FirstOrDefault(x => x.Type == "UserName").Value.ToString();
                if ((user.USERNAME != "1000861") || (user.USERNAME != "257923")||(user.USERNAME != "465992"))
                { UserUnlockPermission = true; }

                if ((car != null)&&(UserUnlockPermission)) // && (U.Macaddress == "48:13:7e:11:d7:1f"))
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
                    return null;
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
