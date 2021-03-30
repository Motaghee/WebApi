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
            LogManager.SetCommonLog("GrantResourceOwnerCredentials1:" );
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            // authenticat from database
            string[] strScope = context.Scope[0].ToString().Split(',');
            string strSecondPassword = strScope[0];
            string strAreaCode = strScope[1];
            int intClientVersion = Convert.ToInt32(strScope[2].Replace(".", ""));
            string LoginFromAppName = strScope[3];
            //string strMac = strScope[3];
            LogManager.SetCommonLog("GrantResourceOwnerCredentials1:"+ LoginFromAppName);
            User FoundUser = QccasttUtility.FindUser(context.UserName, context.Password, strSecondPassword, strAreaCode, "");
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
                        // ---
                        if (LoginFromAppName != "Inspector")
                        {
                            int ClientForceVersion = Convert.ToInt32(clsCommon.ClientForceVersion.Replace(".", ""));
                            if (intClientVersion >= ClientForceVersion)
                                identity.AddClaim(new Claim("ClientVerIsValid", "true"));
                            else
                                identity.AddClaim(new Claim("ClientVerIsValid", "false"));
                            identity.AddClaim(new Claim("UserAuthorization", "true"));
                            context.Validated(identity);
                        }
                        else
                        {
                            int InspectorClientForceVersion = Convert.ToInt32(clsCommon.InspectorClientForceVersion.Replace(".", ""));
                            if (intClientVersion >= InspectorClientForceVersion)
                                identity.AddClaim(new Claim("ClientVerIsValid", "true"));
                            else
                                identity.AddClaim(new Claim("ClientVerIsValid", "false"));
                            identity.AddClaim(new Claim("UserAuthorization", "true"));
                            context.Validated(identity);

                        }
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
        //private User FindUser(string _UserName, string _Password,
        //                      string _SecondPassword,
        //                      string _AreaCode, string _Mac)
        //{
        //    LogManager.SetCommonLog("FindUser:" + _UserName);
        //    byte[] userByte = Encoding.UTF8.GetBytes(_UserName);
        //    string strHashPSW = DBHelper.Cryptographer.CreateHash(_Password, "MD5", userByte);
        //    string commandtext = string.Format(@"select srl,fname,lname,username,psw,userid,
        //                                        (select a.AreaCode from qcareat a where areacode={2}) as AreaCode,
        //                                        (select a.CheckDest from qcareat a where areacode={2}) as CheckDest,
        //                                        (select a.AreaType from qcareat a where areacode={2}) as AreaType,
        //                                        (select distinct  q.parameter_srl from qcussft q where
        //                                            q.qcusert_srl in (select srl from qcusert u2 where u2.srl = u.srl)
        //                                                and q.inuse=1
        //                                            and q.parameter_srl in 
        //                                                (select a.srl from qcareat a
        //                                                    where areacode={2})) as QCAreatSrl
        //                                         ,(select decode(count(distinct q.qcsyfot_srl),0,'false','true') from qcussft q  join qcsyfot sf on q.qcsyfot_srl=sf.srl join qcformt f on f.srl = sf.qcformt_srl where q.inuse=1 and q.qcusert_srl = (select srl from qcusert u where u.username = '{0}')  and sf.qcsystt_srl = 121 and f.url='QCMobAppPer') As QCMOBAPPPER
        //                                         ,(select decode(count(distinct q.qcsyfot_srl),0,'false','true') from qcussft q  join qcsyfot sf on q.qcsyfot_srl=sf.srl join qcformt f on f.srl = sf.qcformt_srl where q.inuse=1 and q.qcusert_srl = (select srl from qcusert u where u.username = '{0}')  and sf.qcsystt_srl = 121 and f.url='PTDashPer') As PTDASHPER
        //                                         ,(select decode(count(distinct q.qcsyfot_srl),0,'false','true') from qcussft q  join qcsyfot sf on q.qcsyfot_srl=sf.srl join qcformt f on f.srl = sf.qcformt_srl where q.inuse=1 and q.qcusert_srl = (select srl from qcusert u where u.username = '{0}')  and sf.qcsystt_srl = 121 and f.url='QCDashPer') As QCDASHPER
        //                                         ,(select decode(count(distinct q.qcsyfot_srl),0,'false','true') from qcussft q  join qcsyfot sf on q.qcsyfot_srl=sf.srl join qcformt f on f.srl = sf.qcformt_srl where q.inuse=1 and q.qcusert_srl = (select srl from qcusert u where u.username = '{0}')  and sf.qcsystt_srl = 121 and f.url='AuditDashPer') As AUDITDASHPER
        //                                         ,(select decode(count(distinct q.qcsyfot_srl),0,'false','true') from qcussft q  join qcsyfot sf on q.qcsyfot_srl=sf.srl join qcformt f on f.srl = sf.qcformt_srl where q.inuse=1 and q.qcusert_srl = (select srl from qcusert u where u.username = '{0}')  and sf.qcsystt_srl = 121 and f.url='AuditUnLockPer') As AUDITUNLOCKPER
        //                                         ,(select decode(count(distinct q.qcsyfot_srl),0,'false','true') from qcussft q  join qcsyfot sf on q.qcsyfot_srl=sf.srl join qcformt f on f.srl = sf.qcformt_srl where q.inuse=1 and q.qcusert_srl = (select srl from qcusert u where u.username = '{0}')  and sf.qcsystt_srl = 121 and f.url='QCRegDefPer') As QCREGDEFPER
        //                                         ,(select decode(count(distinct q.qcsyfot_srl),0,'false','true') from qcussft q  join qcsyfot sf on q.qcsyfot_srl=sf.srl join qcformt f on f.srl = sf.qcformt_srl where q.inuse=1 and q.qcusert_srl = (select srl from qcusert u where u.username = '{0}')  and sf.qcsystt_srl = 121 and f.url='SMSQCPer') As SMSQCPER
        //                                         ,(select decode(count(distinct q.qcsyfot_srl),0,'false','true') from qcussft q  join qcsyfot sf on q.qcsyfot_srl=sf.srl join qcformt f on f.srl = sf.qcformt_srl where q.inuse=1 and q.qcusert_srl = (select srl from qcusert u where u.username = '{0}')  and sf.qcsystt_srl = 121 and f.url='SMSAuditPer') As SMSAUDITPER
        //                                         ,(select decode(count(distinct q.qcsyfot_srl),0,'false','true') from qcussft q  join qcsyfot sf on q.qcsyfot_srl=sf.srl join qcformt f on f.srl = sf.qcformt_srl where q.inuse=1 and q.qcusert_srl = (select srl from qcusert u where u.username = '{0}')  and sf.qcsystt_srl = 121 and f.url='SMSSPPer') As SMSSPPER
        //                                         ,(select decode(count(distinct q.qcsyfot_srl),0,'false','true') from qcussft q  join qcsyfot sf on q.qcsyfot_srl=sf.srl join qcformt f on f.srl = sf.qcformt_srl where q.inuse=1 and q.qcusert_srl = (select srl from qcusert u where u.username = '{0}')  and sf.qcsystt_srl = 121 and f.url='QCCardPer') As QCCARDPER
        //                                         ,(select decode(count(distinct q.qcsyfot_srl),0,'false','true') from qcussft q  join qcsyfot sf on q.qcsyfot_srl=sf.srl join qcformt f on f.srl = sf.qcformt_srl where q.inuse=1 and q.qcusert_srl = (select srl from qcusert u where u.username = '{0}')  and sf.qcsystt_srl = 121 and f.url='SMSPTPer') As SMSPTPER
        //                                         ,(select decode(count(distinct q.qcsyfot_srl),0,'false','true') from qcussft q  join qcsyfot sf on q.qcsyfot_srl=sf.srl join qcformt f on f.srl = sf.qcformt_srl where q.inuse=1 and q.qcusert_srl = (select srl from qcusert u where u.username = '{0}')  and sf.qcsystt_srl = 121 and f.url='AuditCardPer') As AUDITCARDPER
        //                                         ,(select decode(count(distinct q.qcsyfot_srl),0,'false','true') from qcussft q  join qcsyfot sf on q.qcsyfot_srl=sf.srl join qcformt f on f.srl = sf.qcformt_srl where q.inuse=1 and q.qcusert_srl = (select srl from qcusert u where u.username = '{0}')  and sf.qcsystt_srl = 121 and f.url='CarStatusPer') As CARSTATUSPER
        //                                         from QCUSERT u Where USERName='{0}'
        //                                         and PSW ='{1}' and (InUse=1) 
        //                                         and otp = '{3}'  and u.otpexpire > sysdate "
        //                                         , _UserName, strHashPSW, _AreaCode, _SecondPassword);
        //    object[] obj = DBHelper.GetDBObjectByObj2_OnLive(new User(), null, commandtext, "inspector");
        //    LogManager.SetCommonLog("FindUser2:" + _AreaCode);
        //    if ((obj != null) && (obj.Length != 0))
        //    {
        //        LogManager.SetCommonLog("FindUser3" );
        //        return obj.Cast<User>().ToList()[0];
        //    }
        //    else
        //        return null;
        //    //---

        //}



    }

}