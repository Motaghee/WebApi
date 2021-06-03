using Common.Actions;
using Common.CacheManager;
using Common.db;
using Common.Models;
using Common.Models.General;
using LiteDB;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Globalization;
using System.Text;
using WebApi2.Models;

namespace WebApi2.Controllers.Utility
{
    public class GeneralUtility
    {
        public static ResultMsg OTPSend(string _UserName, string _Password)
        {
            ResultMsg rm = new ResultMsg();
            try
            {

                byte[] userByte = Encoding.UTF8.GetBytes(_UserName);
                string strHashPSW = DBHelper.Cryptographer.CreateHash(_Password, "MD5", userByte);
                // ---
                if (DBHelper.DBConnectionIns.State == ConnectionState.Closed)
                {
                    DBHelper.DBConnectionIns.ConnectionString = DBHelper.CnStrInsLive;
                    DBHelper.DBConnectionIns.Open();
                }
                OracleCommand cmd = new OracleCommand();
                OracleDataAdapter da = new OracleDataAdapter();
                cmd.Connection = DBHelper.DBConnectionIns;
                da.SelectCommand = cmd;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_SendOTP";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("pUserName", OracleDbType.Varchar2).Value = _UserName;
                cmd.Parameters.Add("pPassword", OracleDbType.Varchar2).Value = strHashPSW;
                cmd.Parameters.Add("pMsgResult", OracleDbType.Varchar2, 2048);
                cmd.Parameters["pMsgResult"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                string result = cmd.Parameters["pMsgResult"].Value.ToString();
                rm.title = rm.Message = result;
                return rm;

            }
            catch (Exception ex)
            {
                rm.title = "1error";
                rm.Message = ex.Message.ToString();
                return rm;
            }

        }

        public static void UpdateUserData(User _user, NowDateTime _ndt, int _UserDataType)
        {
            OnlineUsers ud = new OnlineUsers();
            LiteDatabase db = null;
            try
            {
                if (_ndt == null)
                {
                    _ndt = new NowDateTime();
                }
                DateTime dt = new DateTime();
                dt = DateTime.Now;
                //_ndt.NowDateTimeFa.Replace(" ", "").Replace("/", "").Replace(":", "").Trim() + DateTime.Now.Millisecond.ToString() + Guid.NewGuid().ToString();
                ud.DateFa = _ndt.NowDateFa;
                ud.DateTime = _ndt.Now;
                ud.DateTimeFa = _ndt.NowDateTimeFa;
                ud.Time = _ndt.NowTime;
                // get instanse of ldb
                ConnectionString cn = ldbConfig.ldbOnlineUsersConnectionString;
                db = new LiteDatabase(cn);
                // get old ldb ps lst
                LiteCollection<OnlineUsers> dbUD = db.GetCollection<OnlineUsers>("OnlineUsers");
                OnlineUsers old = dbUD.FindById(_user.USERID);
                if (old == null)
                {
                    //ud.Id = _ndt.NowDateTimeFa.Replace(" ", "").Replace("/", "").Replace(":", "").Trim() + DateTime.Now.Millisecond.ToString() + Guid.NewGuid().ToString();
                    ud.UserId = ud.Id = _user.USERID;
                    ud.UserSRL = _user.SRL;
                    ud.DataType = _UserDataType;
                    ud.ClientVersion = _user.ClientVersion;
                    if (_UserDataType == 0)
                    {
                        ud.LoginDateTimeFa = ud.DateTimeFa;
                        ud.AreaCode = _user.AREACODE;
                        ud.UserDesc = _user.FNAME + " " + _user.LNAME;
                        dbUD.Insert(ud);
                    }
                }
                else
                {
                    if (_UserDataType == 0)
                    {
                        old.LoginDateTimeFa = ud.DateTimeFa;
                        old.AreaCode = _user.AREACODE;
                        old.UserDesc = _user.FNAME + " " + _user.LNAME;
                    }
                    old.ClientVersion = _user.ClientVersion;
                    old.DataType = _UserDataType;
                    old.DateFa = _ndt.NowDateFa;
                    old.DateTimeFa = _ndt.NowDateTimeFa;
                    old.Time = _ndt.NowTime;
                    old.DateTime = _ndt.Now;
                    dbUD.Update(old);
                }
                
            }
            catch (Exception e)
            {
                //LogManager.SetCommonLog(String.Format(@"Duplicate time:{0} UserId:{1} DuplicateId:{2} ", _ndt.NowDateFa, _user.USERID, ud.Id));
                DBHelper.LogFile(e);
            }
            finally
            {
                db.Dispose();
            }
        }
    }
}