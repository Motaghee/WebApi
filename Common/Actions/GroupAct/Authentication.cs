using Common.db;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Common.Actions
{
    public class Authentication
    {
        public static bool FindUser(string _UserName, string _Password)
        {
            try
            {
                byte[] userByte = Encoding.UTF8.GetBytes(_UserName);
                string strHashPSW = DBHelper.Cryptographer.CreateHash(_Password, "MD5", userByte);
                string commandtext = string.Format(@"select srl,fname,lname,username,psw
                                                 from QCUSERT u Where USERName='{0}'
                                                 and PSW ='{1}' and (InUse=1)"
                                                     , _UserName, strHashPSW);
                object[] obj = DBHelper.GetDBObjectByObj(new User(), null, commandtext);
                if ((obj != null) && (obj.Length != 0))
                {
                    return true;
                }
                else
                    return false;
                //---
            }
            catch
            {
                return false;
            }

        }

    }
}