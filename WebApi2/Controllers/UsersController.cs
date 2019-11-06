using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using WebApi2.Controllers.Utility;
using WebApi2.Models;

namespace WebApi2.Controllers
{
    public class UsersController : ApiController
    {

        [AllowAnonymous]
        [HttpPost]
        [Route("api/users/SendOTP")]
        public ResultMsg SendOTP([FromBody] User user)
        {
            return GeneralUtility.OTPSend(user.USERNAME, user.PSW);
        }

        [HttpGet]
        [Authorize]
        [Route("api/users/UserInfo")]
        public User UserInfo()
        {
            User user = new User();
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            user.SRL = Convert.ToDouble(claims.FirstOrDefault(x => x.Type == "srl").Value.ToString());
            user.USERNAME = claims.FirstOrDefault(x => x.Type == "UserName").Value.ToString();
            user.FNAME = claims.FirstOrDefault(x => x.Type == "FName").Value.ToString();
            user.LNAME = claims.FirstOrDefault(x => x.Type == "LName").Value.ToString();
            user.QCAREATSRL = Convert.ToDouble(claims.FirstOrDefault(x => x.Type == "QCAreatSrl").Value.ToString());
            user.QCAREACODE = claims.FirstOrDefault(x => x.Type == "QCAreaCode").Value.ToString();
            user.QCAREATSRL = Convert.ToDouble(claims.FirstOrDefault(x => x.Type == "QCAreatSrl").Value.ToString());
            user.CHECKDEST = Convert.ToDouble(claims.FirstOrDefault(x => x.Type == "CheckDest").Value.ToString());
            user.MACISVALID = Convert.ToBoolean(claims.FirstOrDefault(x => x.Type == "MacIsValid").Value.ToString());
            user.CLIENTVERISVALID = Convert.ToBoolean(claims.FirstOrDefault(x => x.Type == "ClientVerIsValid").Value.ToString());
            return user;

        }



    }
}
