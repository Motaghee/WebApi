#region Assembly System.Web.ApplicationServices, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6\System.Web.ApplicationServices.dll
#endregion

using Common.db;
using QCManagement.Models;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace System.Web.Security
{
    //
    // Summary:
    //     Exposes and updates membership user information in the membership data store.
    [TypeForwardedFrom("System.Web, Version=2.0.0.0, Culture=Neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    public class MyMembershipUser : MembershipUser
    {
        IEnumerable<QCManagement.Models.UserModels> Users;
        public MyMembershipUser(string UserName)
        {
            UserModels usermodel = new UserModels();
            string cmd = string.Format(@"select u.srl,u.userid,u.username,u.lname,u.fname,u.inuse,u.svadept_srl,u.svaacst_srl  from QCUSERT u 
                                Where USERName='{0}' and InUse=1", UserName);
            object[] lstUserModel = DBHelper.GetDBObjectByObj(usermodel, null, cmd);    // get data
            //result.Cast<ProductStatistics>()
            Users = lstUserModel.Cast<UserModels>();
            List<UserModels> lstUserModel2 = new List<UserModels>();
            lstUserModel2 = lstUserModel.Cast<UserModels>().ToList();
            USERID = lstUserModel2[0].USERID.ToString();
            UserName = lstUserModel2[0].USERNAME;
            FName = lstUserModel2[0].FNAME;
            LName = lstUserModel2[0].LNAME;

        }

        public string FName { get; }
        public string LName { get; }
        public string USERID { get; }
        public string INUSE { get; }
        public string SVADEPT_SRL { get; }
        public string SVAACST_SRL { get; }


    }
}