using Common.db;
using System;
using System.Data;

namespace Common.Models.General
{
    public class Users
    {
        
        public string Srl { get; set; }
        //public int UserId { get; set; }
        //public string LName { get; set; }
        //public string FName { get; set; }
        public string UserDesc { get; set; }
        //public int QCUsertSrl { get; set; }
        //public int AreaCode { get; set; }


        public static bool CheckAccessByqcsyfotSrl(string _strQcusertSrl, string _StrQcareatSrl, int _qcsyfotSrl)
        {
            try
            {
                string commandtext = string.Format(@"select *
                                                          from qcussft q
                                                         where q.qcusert_srl = {0}
                                                           and q.qcsyfot_srl = {2}
                                                           and parameter_srl = {1}
                                                           and q.inuse=1
                                                        ", _strQcusertSrl, _StrQcareatSrl, _qcsyfotSrl.ToString());
                DataSet ds = DBHelper.ExecuteMyQueryQSCOnLive(commandtext); ;
                if (ds != null && ds.Tables != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    return true;
                else
                    return false;

            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public static object[] GetQSCUsers()
        {
            try
            {                                                        // u.userid||' '||
                string commandtext = string.Format(@"select distinct u.fname||' '|| u.lname as UserDesc,q.qcusert_srl as srl from qcussft q join qcusert u on u.srl = q.qcusert_srl where q.qcsyfot_srl = 638 and parameter_srl = 1486 and q.inuse=1");
                return DBHelper.GetDBObjectByObj2_OnLive(new Users(), null, commandtext, "Ins");
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}