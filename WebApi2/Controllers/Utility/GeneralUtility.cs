using Common.db;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
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
                string strHashPSW = clsDBHelper.Cryptographer.CreateHash(_Password, "MD5", userByte);
                // ---
                if (clsDBHelper.DBConnectionIns.State == ConnectionState.Closed)
                {
                    clsDBHelper.DBConnectionIns.ConnectionString = clsDBHelper.CnStrIns;
                    clsDBHelper.DBConnectionIns.Open();
                }
                OracleCommand cmd = new OracleCommand();
                OracleDataAdapter da = new OracleDataAdapter();
                cmd.Connection = clsDBHelper.DBConnectionIns;
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
    }
}