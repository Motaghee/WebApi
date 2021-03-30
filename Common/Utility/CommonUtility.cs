//using AutomationService.PostItemService;
using ClosedXML.Excel;
using Common.Automation;
using Common.db;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web.Mvc;

namespace Common.Utility
{
    public static class CommonUtility
    {
        public static string GetNowDateTimeFa()
        {
            //ndt.Now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            PersianCalendar pc = new PersianCalendar();
            DateTime dtN = DateTime.Now;
            string NowDateFa = pc.GetYear(dtN).ToString() + "/" + pc.GetMonth(dtN).ToString().PadLeft(2, '0') + "/" + pc.GetDayOfMonth(dtN).ToString().PadLeft(2, '0');
            string NowTime = dtN.ToString("HH:mm:ss");
            string NowDateTimeFa= NowDateFa + " " + NowTime;
            return NowDateTimeFa;
        }

        public static string GetNowDateFaNum()
        {
            //ndt.Now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            PersianCalendar pc = new PersianCalendar();
            DateTime dtN = DateTime.Now;
            string NowDateFa = pc.GetYear(dtN).ToString() +  pc.GetMonth(dtN).ToString().PadLeft(2, '0') +  pc.GetDayOfMonth(dtN).ToString().PadLeft(2, '0');
            string NowDateTimeFa = NowDateFa ;
            return NowDateTimeFa;
        }

        public static string GetNowTime()
        {
            DateTime dtN = DateTime.Now;
            string NowTime = dtN.ToString("HH:mm:ss");
            return NowTime;
        }

        public static string GetLastDateFa(int FromLastDay)
        {
            string Y, M, D;
            DateTime dtOldDay = DateTime.Now.AddDays(-FromLastDay);
            PersianCalendar pc = new PersianCalendar();
            Y = pc.GetYear(dtOldDay).ToString();
            M = pc.GetMonth(dtOldDay).ToString().PadLeft(2, '0');
            D = pc.GetDayOfMonth(dtOldDay).ToString().PadLeft(2, '0');
            string strFromLastDayCondition = Y + "/" + M + "/" + D;
            return strFromLastDayCondition;
        }

        public static double GetLastDateFaNum(int FromLastDay)
        {
            string Y, M, D;
            DateTime dtOldDay = DateTime.Now.AddDays(-FromLastDay);
            PersianCalendar pc = new PersianCalendar();
            Y = pc.GetYear(dtOldDay).ToString();
            M = pc.GetMonth(dtOldDay).ToString().PadLeft(2, '0');
            D = pc.GetDayOfMonth(dtOldDay).ToString().PadLeft(2, '0');
            string strFromLastDayCondition = Y + M +  D;
            return Convert.ToDouble(strFromLastDayCondition);
        }

        public static bool GenerateExcel2(DataTable dataTable, string path)
        {
            try
            {
                XLWorkbook wb = new XLWorkbook();
                wb.Worksheets.Add(dataTable, "MyDt");
                wb.SaveAs(path);
                return true;
            }
            catch (Exception ex)
            {
                DBHelper.LogtxtToFile("err "+ex.Message.ToString());
                return false;
            }
        }
            public static int SendAutomationAtachExcel(string _Subject,string _ContentMsg,string[] _MainPersonalId, string[] _CopyPersonalId,DataTable dt,string _FileName)
        {
            try
            {
                string filename = _FileName+"_" + DateTime.Now.Ticks;
                string fileExtention = ".xlsx";
                string path = @"D:\tmpFiles\" + filename + fileExtention;
                bool SaveResult = GenerateExcel2(dt, path);
                if (SaveResult)
                {
                    byte[] b = File.ReadAllBytes(path);
                    File.Delete(path);
                    return AutomationUtility.SendAutomationMessageWithAttachment(_Subject, _ContentMsg, _MainPersonalId, _CopyPersonalId, filename, fileExtention, b);
                }
                else
                    return 0;

            }
            catch (Exception ex)
            {
                DBHelper.LogFile(ex);
                return 0;
            }
            finally {
                
            }
        }


        private static byte[] ConvertDataSetToByteArray(DataTable dataTable)
        {
            byte[] binaryDataResult = null;
            using (MemoryStream memStream = new MemoryStream())
            {
                BinaryFormatter brFormatter = new BinaryFormatter();
                dataTable.RemotingFormat = SerializationFormat.Binary;
                brFormatter.Serialize(memStream, dataTable);
                binaryDataResult = memStream.ToArray();
            }
            return binaryDataResult;
        }

        public static SelectList ToSelectList(this DataTable table, string valueField, string textField, bool blnAddNull)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            if (blnAddNull)
            {
                list.Add(new SelectListItem()
                {
                    Text = "همه",
                    Value = null
                });
            }

            foreach (DataRow row in table.Rows)
            {
                list.Add(new SelectListItem()
                {
                    Text = row[textField].ToString(),
                    Value = row[valueField].ToString()
                });
            }

            return new SelectList(list, "Value", "Text");
        }

        public static bool QCInsertUserAct2(string _QcusertSrl,
                            string _Action,
                            string _vin,
                            string _QcareatSrl,
                            string _REPORTMETHODCODE,
                            string _ClientIP,
                            string _CLIENTMACADDRESS,
                            string _COMPUTERNAME,
                            string _WINDOWSUSERNAME,
                            string _Device,
                            string _QCUsAcTytSrl)
        {
            try
            {
                //==
                if (string.IsNullOrEmpty(_QcusertSrl))
                    _QcusertSrl = "null";
                else if (_QcusertSrl.Substring(0, 2) == "UI")
                {
                    _QcusertSrl = _QcusertSrl.Remove(0, 2);
                    _QcusertSrl = string.Format(@"(select srl from qcusert u where userid={0})", _QcusertSrl);
                }
                //==
                if (string.IsNullOrEmpty(_Action))
                    _Action = "null";
                else
                    _Action = "'" + _Action + "'";
                //--
                if (string.IsNullOrEmpty(_vin))
                    _vin = "null";
                else
                    _vin = "'" + _vin + "'";
                //--
                if (string.IsNullOrEmpty(_QcareatSrl))
                    _QcareatSrl = "null";
                else if (_QcareatSrl.Substring(0, 2) == "AC")
                {
                    _QcareatSrl = _QcareatSrl.Remove(0, 2);
                    _QcareatSrl = string.Format(@"(select srl from qcareat a where AreaCode={0})", _QcareatSrl);
                }
                //--
                if (string.IsNullOrEmpty(_REPORTMETHODCODE))
                    _REPORTMETHODCODE = "null";
                else
                    _REPORTMETHODCODE = "'" + _REPORTMETHODCODE + "'";
                //--
                if (string.IsNullOrEmpty(_ClientIP))
                    _ClientIP = "null";
                else
                    _ClientIP = "'" + _ClientIP + "'";
                //--
                if (string.IsNullOrEmpty(_CLIENTMACADDRESS))
                    _CLIENTMACADDRESS = "null";
                else
                    _CLIENTMACADDRESS = "'" + _CLIENTMACADDRESS + "'";
                //---
                if (string.IsNullOrEmpty(_COMPUTERNAME))
                    _COMPUTERNAME = "null";
                else
                    _COMPUTERNAME = "'" + _COMPUTERNAME + "'";
                //---
                if (string.IsNullOrEmpty(_WINDOWSUSERNAME))
                    _WINDOWSUSERNAME = "null";
                else
                    _WINDOWSUSERNAME = "'" + _WINDOWSUSERNAME + "'";
                //---
                if (string.IsNullOrEmpty(_Device))
                    _Device = "null";
                //--
                if (DBHelper.LiveDBConnectionIns.State == ConnectionState.Closed)
                {
                    DBHelper.LiveDBConnectionIns.ConnectionString = DBHelper.CnStrInsLive;
                    DBHelper.LiveDBConnectionIns.Open();
                }
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = DBHelper.LiveDBConnectionIns;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = string.Format(@"insert into QCUSACT
                                                  (QCUSERT_SRL,
                                                   Action,
                                                   vin,
                                                   qcareat_Srl,
                                                   REPORTMETHODCODE,
                                                   CLIENTIP,
                                                   CLIENTMACADDRESS,
                                                   COMPUTERNAME,
                                                   WINDOWSUSERNAME,
                                                   Device,
                                                   CREATEDDATE,
                                                   QCUsAcTyt_Srl)
                                                   values
                                                  ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},sysdate,{10})
                                                       ",
                                              _QcusertSrl,
                                              _Action,
                                              _vin,
                                              _QcareatSrl,
                                              _REPORTMETHODCODE,
                                              _ClientIP,
                                              _CLIENTMACADDRESS,
                                              _COMPUTERNAME,
                                              _WINDOWSUSERNAME, _Device, _QCUsAcTytSrl
                                              );
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                DBHelper.LiveDBConnectionIns.Close();
                return false;
                //throw;
            }
        }

        }
}
