using Common.db;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApi2.Models;
using WebApi2.Models.utility;

namespace WebApi2.Security
{

    public class MyAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            //return base.ValidateClientAuthentication(context); 
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            // authenticat from database
            string[] strScope = context.Scope[0].ToString().Split(',');
            string strSecondPassword = strScope[0];
            string strAreaCode = strScope[1];
            int intClientVersion = Convert.ToInt32(strScope[2].Replace(".", ""));
            int intClientForceVersion = Convert.ToInt32(clsCommon.ClientForceVersion.Replace(".", ""));
            //string strMac = strScope[3];
            User FoundUser = FindUser(context.UserName, context.Password, strSecondPassword, strAreaCode, "");
            if (FoundUser != null)
            {
                string QCAreatSrl = FoundUser.QCAREATSRL.ToString();
                string ValidQCAreaCode = FoundUser.VALIDQCAREACODE.ToString();
                if (!string.IsNullOrEmpty(ValidQCAreaCode))
                {
                    if (!string.IsNullOrEmpty(QCAreatSrl))
                    {
                        identity.AddClaim(new Claim("UserName", context.UserName));
                        identity.AddClaim(new Claim("FName", FoundUser.FNAME));
                        identity.AddClaim(new Claim("LName", FoundUser.LNAME));
                        identity.AddClaim(new Claim("srl", FoundUser.SRL.ToString()));
                        identity.AddClaim(new Claim("QCAreatSrl", FoundUser.QCAREATSRL.ToString()));
                        identity.AddClaim(new Claim("QCAreaCode", FoundUser.VALIDQCAREACODE.ToString()));
                        identity.AddClaim(new Claim("CheckDest", FoundUser.CHECKDEST.ToString()));
                        identity.AddClaim(new Claim("MacIsValid", "true"));
                        if (intClientVersion >= intClientForceVersion)
                            identity.AddClaim(new Claim("ClientVerIsValid", "true"));
                        else
                            identity.AddClaim(new Claim("ClientVerIsValid", "false"));
                        identity.AddClaim(new Claim("UserAuthorization", "true"));
                        context.Validated(identity);
                    }
                    else
                    {
                        context.SetError("invalid_grant_areacode", "Area access is not allowed for the user");
                        return;
                    }
                }
                else
                {
                    context.SetError("invalid_areacode", "Provided areacode is incorrect");
                    return;
                }
            }
            else
            {
                context.SetError("invalid_grant", "Provided username and password is incorrect");
                return;
            }



        }
        private User FindUser(string _UserName, string _Password,
                              string _SecondPassword,
                              string _AreaCode, string _Mac)
        {
            byte[] userByte = Encoding.UTF8.GetBytes(_UserName);
            string strHashPSW = clsDBHelper.Cryptographer.CreateHash(_Password, "MD5", userByte);
            string commandtext = string.Format(@"select srl,fname,lname,username,psw,
                                                (select a.AreaCode from qcareat a where areacode={2}) as ValidQCAreaCode,
                                                (select a.CheckDest from qcareat a where areacode={2}) as CheckDest,
                                                (select distinct  q.parameter_srl from qcussft q where
                                                    q.qcusert_srl in (select srl from qcusert u2 where u2.srl = u.srl)
                                                        and q.inuse=1
                                                    and q.parameter_srl in 
                                                        (select a.srl from qcareat a
                                                            where areacode={2})) as QCAreatSrl 
                                                 from QCUSERT u Where USERName='{0}'
                                                 and PSW ='{1}' and (InUse=1) 
                                                 and otp = '{3}'  and u.otpexpire > sysdate "
                                                 , _UserName, strHashPSW, _AreaCode, _SecondPassword);
            object[] obj = clsDBHelper.GetDBObjectByObj(new User(), null, commandtext);
            if ((obj != null) && (obj.Length != 0))
            {
                return obj.Cast<User>().ToList()[0];
            }
            else
                return null;
            //---

        }



    }

}