using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using WebApi2.Controllers.Utility;
using Common.db;
using System.Net;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using Common.Models;
using Common.Models.General;

namespace WebApi2.Controllers
{
    public class UsersController : ApiController
    {

        
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
            user.USERID = Convert.ToInt32(claims.FirstOrDefault(x => x.Type == "UserId").Value.ToString());
            user.FNAME = claims.FirstOrDefault(x => x.Type == "FName").Value.ToString();
            user.LNAME = claims.FirstOrDefault(x => x.Type == "LName").Value.ToString();
            user.QCAREATSRL = Convert.ToDouble(claims.FirstOrDefault(x => x.Type == "QCAreatSrl").Value.ToString());
            user.AREACODE= Convert.ToInt32(claims.FirstOrDefault(x => x.Type == "AreaCode").Value.ToString());
            user.AREADESC = claims.FirstOrDefault(x => x.Type == "AreaDesc").Value.ToString();
            user.AREATYPE = Convert.ToInt32(claims.FirstOrDefault(x => x.Type == "AreaType").Value.ToString());
            user.QCAREATSRL = Convert.ToDouble(claims.FirstOrDefault(x => x.Type == "QCAreatSrl").Value.ToString());
            user.CHECKDEST = Convert.ToDouble(claims.FirstOrDefault(x => x.Type == "CheckDest").Value.ToString());
            user.MACISVALID = Convert.ToBoolean(claims.FirstOrDefault(x => x.Type == "MacIsValid").Value.ToString());
            user.CLIENTVERISVALID = Convert.ToBoolean(claims.FirstOrDefault(x => x.Type == "ClientVerIsValid").Value.ToString());
            user.QCMOBAPPPER = claims.FirstOrDefault(x => x.Type == "QCMobAppPer").Value.ToString();
            user.PTDASHPER = claims.FirstOrDefault(x => x.Type == "PTDashPer").Value.ToString();
            user.QCDASHPER = claims.FirstOrDefault(x => x.Type == "QCDashPer").Value.ToString();
            user.AUDITDASHPER =claims.FirstOrDefault(x => x.Type == "AuditDashPer").Value.ToString();
            user.AUDITUNLOCKPER = claims.FirstOrDefault(x => x.Type == "AuditUnLockPer").Value.ToString();
            user.QCREGDEFPER = claims.FirstOrDefault(x => x.Type == "QCRegDefPer").Value.ToString();
            user.SMSQCPER = claims.FirstOrDefault(x => x.Type == "SMSQCPer").Value.ToString();
            user.SMSAUDITPER =claims.FirstOrDefault(x => x.Type == "SMSAuditPer").Value.ToString();
            user.SMSSPPER = claims.FirstOrDefault(x => x.Type == "SMSSPPer").Value.ToString();
            user.QCCARDPER = claims.FirstOrDefault(x => x.Type == "QCCardPer").Value.ToString();
            user.SMSPTPER =claims.FirstOrDefault(x => x.Type == "SMSPTPer").Value.ToString();
            user.AUDITCARDPER = claims.FirstOrDefault(x => x.Type == "AuditCardPer").Value.ToString();
            user.CARSTATUSPER = claims.FirstOrDefault(x => x.Type == "CarStatusPer").Value.ToString();
            user.AppName= claims.FirstOrDefault(x => x.Type == "AppName").Value.ToString();
            user.ClientVersion = claims.FirstOrDefault(x => x.Type == "ClientVersion").Value.ToString();
            user.STRUSERPROFILEIMAGE = GetUserProfileImage(user.USERNAME);
            if (user.USERNAME != "1000861")
                DBHelper.LogtLoginUser(user.USERNAME +" "+ user.LNAME+" AppName:"+ user.AppName + " ClientVersion:" + user.ClientVersion);
            return user;
        }


        [HttpGet]
        [Authorize]
        [Route("api/users/GetUserProfileImage")]
        public string GetUserProfileImage(string _UserId)
        {
            try
            {
                string url = "http://automation.saipa.net/PersonImage.aspx?pno=" + _UserId;
                string Image2BConverted;
                Bitmap b = GetUserProfileBitmap(url);
                using (MemoryStream ms = new MemoryStream())
                {
                    b.Save(ms, ImageFormat.Jpeg);
                    Image2BConverted = Convert.ToBase64String(ms.ToArray());
                    ms.Close();
                }
                return Image2BConverted; 
            }
            catch { return null; }
        }

        private Bitmap GetUserProfileBitmap(string _url)
        {
            WebClient client = new WebClient();
            Stream stream = client.OpenRead(_url);
            Bitmap bitmap; bitmap = new Bitmap(stream);
            stream.Flush();
            stream.Close();
            client.Dispose();
            if (bitmap != null)
                return bitmap;
            else
                return null;
        }


    }
}
