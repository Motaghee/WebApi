using Common.Actions;
using Common.db;
using Common.Models;
using Common.Utility;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApi2.Controllers.Utility;
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
            string LoginFromAppName = strScope[3];
            User FoundUser = QccasttUtility.FindUser(context.UserName, context.Password, strSecondPassword, strAreaCode, "");
            FoundUser.ClientVersion = strScope[2].ToString();
            if (FoundUser != null)
            {
                string QCAreatSrl = FoundUser.QCAREATSRL.ToString();
                string ValidQCAreaCode = FoundUser.AREACODE.ToString();
                if (!string.IsNullOrEmpty(ValidQCAreaCode))
                {
                    if (!string.IsNullOrEmpty(QCAreatSrl))
                    {
                        identity.AddClaim(new Claim("UserName", context.UserName));
                        identity.AddClaim(new Claim("UserId", FoundUser.USERID.ToString()));
                        identity.AddClaim(new Claim("FName", FoundUser.FNAME));
                        identity.AddClaim(new Claim("LName", FoundUser.LNAME));
                        identity.AddClaim(new Claim("srl", FoundUser.SRL.ToString()));
                        identity.AddClaim(new Claim("QCAreatSrl", FoundUser.QCAREATSRL.ToString()));
                        identity.AddClaim(new Claim("AreaCode", FoundUser.AREACODE.ToString()));
                        identity.AddClaim(new Claim("AreaDesc", FoundUser.AREADESC.ToString()));
                        identity.AddClaim(new Claim("CheckDest", FoundUser.CHECKDEST.ToString()));
                        identity.AddClaim(new Claim("AreaType", FoundUser.AREATYPE.ToString()));
                        identity.AddClaim(new Claim("MacIsValid", "true"));
                        //--- per
                        identity.AddClaim(new Claim("QCMobAppPer", FoundUser.QCMOBAPPPER.ToString()));
                        identity.AddClaim(new Claim("PTDashPer", FoundUser.PTDASHPER.ToString()));
                        identity.AddClaim(new Claim("QCDashPer", FoundUser.QCDASHPER.ToString()));
                        identity.AddClaim(new Claim("AuditDashPer", FoundUser.AUDITDASHPER.ToString()));
                        identity.AddClaim(new Claim("AuditUnLockPer", FoundUser.AUDITUNLOCKPER.ToString()));
                        identity.AddClaim(new Claim("QCRegDefPer", FoundUser.QCREGDEFPER.ToString()));
                        identity.AddClaim(new Claim("SMSQCPer", FoundUser.SMSQCPER.ToString()));
                        identity.AddClaim(new Claim("SMSAuditPer", FoundUser.SMSAUDITPER.ToString()));
                        identity.AddClaim(new Claim("SMSSPPer", FoundUser.SMSSPPER.ToString()));
                        identity.AddClaim(new Claim("QCCardPer", FoundUser.QCCARDPER.ToString()));
                        identity.AddClaim(new Claim("SMSPTPer", FoundUser.SMSPTPER.ToString()));
                        identity.AddClaim(new Claim("AuditCardPer", FoundUser.AUDITCARDPER.ToString()));
                        identity.AddClaim(new Claim("CarStatusPer", FoundUser.CARSTATUSPER.ToString()));
                        identity.AddClaim(new Claim("AppName", LoginFromAppName));
                        identity.AddClaim(new Claim("ClientVersion", strScope[2]));
                        // ---
                        if (LoginFromAppName == "qcm")
                        {
                            int ClientForceVersion = Convert.ToInt32(clsCommon.ClientForceVersion_qcmobapp.Replace(".", ""));
                            if (intClientVersion >= ClientForceVersion)
                                identity.AddClaim(new Claim("ClientVerIsValid", "true"));
                            else
                                identity.AddClaim(new Claim("ClientVerIsValid", "false"));
                            identity.AddClaim(new Claim("UserAuthorization", "true"));
                            context.Validated(identity);
                        }
                        else if (LoginFromAppName == "ins")
                        {
                            int InspectorClientForceVersion = Convert.ToInt32(clsCommon.InspectorClientForceVersion_inspector.Replace(".", ""));
                            if (intClientVersion >= InspectorClientForceVersion)
                                identity.AddClaim(new Claim("ClientVerIsValid", "true"));
                            else
                                identity.AddClaim(new Claim("ClientVerIsValid", "false"));
                            identity.AddClaim(new Claim("UserAuthorization", "true"));
                            context.Validated(identity);

                        }
                        GeneralUtility.UpdateUserData(FoundUser, null, 0);
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

    }

}