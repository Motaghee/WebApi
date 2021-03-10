using QCManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Security;
using Common.db;

namespace QCManagement
{
    public class MyRoleProvider : RoleProvider
    {
        private int _CacheTimeOutInMinute = 1;
        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return null;
            }
            //check cache
            var chacheKey = string.Format("{0}_role", username);
            if (HttpRuntime.Cache[chacheKey] != null)
            {
                return (string[])HttpRuntime.Cache[chacheKey];
            }
            string[] roles = new string[] { };
            //---get roles of user
            UserModels usermodel = new UserModels();
            DataSet dsUserRoles = DBHelper.GetDBObjectByDataSet(usermodel,
                string.Format(@"select RoleName from V_UserRolesName
                        where username='{0}'", username));    //get user roles
            if ((dsUserRoles != null) && (dsUserRoles.Tables[0] != null) && (dsUserRoles.Tables[0].Rows.Count > 0)) // user find
            {
                roles = new string[dsUserRoles.Tables[0].Rows.Count];
                for (int i = 0; i < dsUserRoles.Tables[0].Rows.Count; i++)
                {
                    roles[i] = dsUserRoles.Tables[0].Rows[i]["RoleName"].ToString();
                }
                HttpRuntime.Cache.Insert(chacheKey,roles,null,DateTime.Now.AddMinutes(_CacheTimeOutInMinute),Cache.NoSlidingExpiration);
            }
            return roles;
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            var userroles = GetRolesForUser(username);
            return userroles.Contains(roleName);
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}