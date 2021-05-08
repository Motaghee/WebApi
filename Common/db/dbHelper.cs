using Common.Utility;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
//using System.Collections.Generic;
//using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
//using System.Text;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.db
{

    public class DBHelper
    {

        public static string CnStrIns, CnStrStp, CnStrPT;
        //Guard cnstring
        public static string CnStrInsGuard = @"Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = 172.20.5.169)(PORT = 1521))   (CONNECT_DATA =
      (SERVER = DEDICATED)
      (SERVICE_NAME = prctlb.saipacorp.com)
    )  ) 
            ;User ID=inspector; Password =fpvhk92";



        public static string CnStrStpGuard = @"Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = 172.20.5.169)(PORT = 1521))   (CONNECT_DATA =
      (SERVER = DEDICATED)
      (SERVICE_NAME = prctlb.saipacorp.com)
    )  ) 
            ;User ID=stopage; Password =dodkesh92";

        public static string CnStrPTGuard = @"Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = 172.20.5.169)(PORT = 1521))   (CONNECT_DATA =
      (SERVER = DEDICATED)
      (SERVICE_NAME = prctlb.saipacorp.com)
    )  ) 
            ;User ID=pt; Password =laygi94";

        // Live cnstring
        public static string CnStrInsLive = @"Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = prctlxdbscan.saipacorp.com)(PORT = 1521))   (CONNECT_DATA =
      (SERVER = DEDICATED)
      (SERVICE_NAME = prctla.saipacorp.com)
    )  ) 
            ;User ID=inspector; Password =fpvhk92";

        public static string CnStrStpLive = @"Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = prctlxdbscan.saipacorp.com)(PORT = 1521))   (CONNECT_DATA =
      (SERVER = DEDICATED)
      (SERVICE_NAME = prctla.saipacorp.com)
    )  ) 
            ;User ID=stopage; Password =dodkesh92";

        public static string CnStrPTLive = @"Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = prctlxdbscan.saipacorp.com)(PORT = 1521))   (CONNECT_DATA =
      (SERVER = DEDICATED)
      (SERVICE_NAME = prctla.saipacorp.com)
    )  ) 
            ;User ID=pt; Password =laygi94";

        //"Data Source = PRIPRCTL.SAIPACORP.COM; Persist Security Info = True; User ID = inspector; Password = fpvhk92";

        static DBHelper()
        {
            bool Live = false;
            //===
            if (Live)
            { CnStrIns = CnStrInsLive; CnStrStp = CnStrStpLive; CnStrPT = CnStrPTLive; }
            else
            { CnStrIns = CnStrInsGuard; CnStrStp = CnStrStpGuard; CnStrPT = CnStrPTGuard; }

            DBConnectionIns = new OracleConnection(CnStrIns);
            DBConnectionStp = new OracleConnection(CnStrStp);
            DBConnectionPT = new OracleConnection(CnStrPT);
            //--
            LiveDBConnectionIns = new OracleConnection(CnStrInsLive);
            LiveDBConnectionStp = new OracleConnection(CnStrStpLive);
            LiveDBConnectionPT = new OracleConnection(CnStrPTLive);
        }


        public static OracleConnection DBConnectionIns;
        public static OracleConnection DBConnectionPT;
        public static OracleConnection DBConnectionStp;

        public static OracleConnection LiveDBConnectionIns;
        public static OracleConnection LiveDBConnectionPT;
        public static OracleConnection LiveDBConnectionStp;

        public static DataSet ExecuteMyQueryIns(string _CommandText)   // Execute Cmd
        {
            try
            {
                if (DBConnectionIns.State == ConnectionState.Closed)
                {
                    DBConnectionIns.ConnectionString = CnStrIns;
                    DBConnectionIns.Open();
                }
                OracleCommand cmd = new OracleCommand();
                OracleDataAdapter da = new OracleDataAdapter();
                cmd.Connection = DBConnectionIns;
                cmd.CommandText = _CommandText;
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                return ds;
                //int i=ds.Tables[0].Rows.Count;
            }
            catch (Exception ex)
            {
                string strExceptMSG = ex.Message;
                return null;
            }
        }
        public static DataSet ExecuteMyQueryInsOnLive(string _CommandText)   // Execute Cmd
        {
            try
            {
                if (LiveDBConnectionIns.State == ConnectionState.Closed)
                {
                    LiveDBConnectionIns.ConnectionString = CnStrInsLive;
                    LiveDBConnectionIns.Open();
                }
                OracleCommand cmd = new OracleCommand();
                OracleDataAdapter da = new OracleDataAdapter();
                cmd.Connection = LiveDBConnectionIns;
                cmd.CommandText = _CommandText;
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                return ds;
                //int i=ds.Tables[0].Rows.Count;
            }
            catch (Exception ex)
            {
                string strExceptMSG = ex.Message;
                return null;
            }
        }

        public static DataSet ExecuteMyQueryStp(string _CommandText)   // Execute Cmd
        {
            try
            {

                if (DBConnectionStp.State == ConnectionState.Closed)
                {
                    DBConnectionStp.ConnectionString = CnStrStp;
                    DBConnectionStp.Open();
                }
                OracleCommand cmd = new OracleCommand();
                OracleDataAdapter da = new OracleDataAdapter();
                cmd.Connection = DBConnectionStp;
                cmd.CommandText = _CommandText;
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                return ds;
                //int i=ds.Tables[0].Rows.Count;
            }
            catch (Exception ex)
            {
                DBHelper.LogFile(ex);
                return null;
            }
        }
        public static DataSet ExecuteMyQueryStpOnLive(string _CommandText)   // Execute Cmd
        {
            try
            {

                if (LiveDBConnectionStp.State == ConnectionState.Closed)
                {
                    LiveDBConnectionStp.ConnectionString = CnStrStpLive;
                    LiveDBConnectionStp.Open();
                }
                OracleCommand cmd = new OracleCommand();
                OracleDataAdapter da = new OracleDataAdapter();
                cmd.Connection = LiveDBConnectionStp;
                cmd.CommandText = _CommandText;
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                return ds;
                //int i=ds.Tables[0].Rows.Count;
            }
            catch (Exception ex)
            {
                DBHelper.LogFile(ex);
                return null;
            }
        }

        public static DataSet ExecuteMyQueryPT(string _CommandText)   // Execute Cmd
        {
            try
            {
                if (DBConnectionPT.State == ConnectionState.Closed)
                {
                    DBConnectionPT.ConnectionString = CnStrPT;
                    DBConnectionPT.Open();
                }
                OracleCommand cmd = new OracleCommand();
                OracleDataAdapter da = new OracleDataAdapter();
                cmd.Connection = DBConnectionPT;
                cmd.CommandText = _CommandText;
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                return ds;
            }
            catch (Exception ex)
            {
                string strExceptMSG = ex.Message;
                return null;
            }
        }
        public static DataSet ExecuteMyQueryPTOnLive(string _CommandText)   // Execute Cmd
        {
            try
            {
                if (LiveDBConnectionPT.State == ConnectionState.Closed)
                {
                    LiveDBConnectionPT.ConnectionString = CnStrPTLive;
                    LiveDBConnectionPT.Open();
                }
                OracleCommand cmd = new OracleCommand();
                OracleDataAdapter da = new OracleDataAdapter();
                cmd.Connection = LiveDBConnectionPT;
                cmd.CommandText = _CommandText;
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                return ds;
            }
            catch (Exception ex)
            {
                string strExceptMSG = ex.Message;
                return null;
            }
        }

        [Obsolete]
        public static DataSet ExecuteMyQuery(string commandText, bool BlnDispose)
        {
            OracleDataAdapter da = new OracleDataAdapter();
            DataSet ds = new DataSet();
            using (OracleConnection connection = DBConnectionIns)
            {
                try
                {

                    if (DBConnectionIns.State == ConnectionState.Closed)
                    {
                        DBConnectionIns.ConnectionString = CnStrIns;
                        DBConnectionIns.Open();
                    }
                    using (OracleCommand command = new OracleCommand(commandText, connection))
                    {
                        command.CommandType = CommandType.Text;
                        da.SelectCommand = command;
                        da.Fill(ds);
                        return ds;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if ((BlnDispose) && (connection.State == ConnectionState.Open))
                    {
                        connection.Close();
                        //connection.Dispose();
                    }
                }
            }
        }

        [Obsolete]
        public static DataSet ExecuteMyQuery(CommandType commandType, string commandText, object[] parameters)
        {
            OracleDataAdapter da = new OracleDataAdapter();
            DataSet ds = new DataSet();
            using (OracleConnection connection = DBConnectionIns/*new OracleConnection(ConnectionString)*/)
            {
                try
                {
                    if (DBConnectionIns.State == ConnectionState.Closed)
                    {
                        DBConnectionIns.ConnectionString = CnStrIns;
                        DBConnectionIns.Open();
                    }

                    using (OracleCommand command = new OracleCommand(commandText, connection))
                    {
                        command.CommandType = commandType;
                        //---
                        if (parameters != null)
                        {
                            for (int i = 0; i < parameters.Length; i++)
                            {
                                command.Parameters.Add(parameters[i] as OracleParameter);
                            }
                        }
                        //---
                        da.SelectCommand = command;
                        da.Fill(ds);
                        return ds;
                    }
                }
                catch (Exception ex)
                {
                    LogFile(ex);
                    throw ex;
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
        }

        [Obsolete]
        public static int ExecuteQueryScalar(string commandText, bool BlnDispose)
        {
            //OracleDataAdapter da = new OracleDataAdapter();
            //DataSet ds = new DataSet();

            try
            {

                if (DBConnectionIns.State == ConnectionState.Closed)
                {
                    DBConnectionIns.ConnectionString = CnStrIns;
                    DBConnectionIns.Open();
                }
                OracleCommand command = new OracleCommand(commandText, DBConnectionIns);
                command.CommandType = CommandType.Text;
                int result = command.ExecuteNonQuery();
                return result;
                //da.SelectCommand = command;
                //da.Fill(ds);
                //return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //private void AddParameters(OracleCommand command, object[] parameters)
        //{

        //    if (command == null)
        //    {
        //        throw new ApplicationException("null Command");
        //    }

        //    if (parameters != null)
        //    {
        //        for (int i = 0; i < parameters.Length; i++)
        //        {
        //            command.Parameters.Add(parameters[i] as OracleParameter);
        //        }
        //    }

        //}


        public static DateTime GetServerDateTime()
        {
            try
            {
                return DateTime.Now;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet GetDBObjectByDataSet(object _Obj, string _CommandText)
        {
            try
            {
                DataSet _ds = DBHelper.ExecuteMyQueryIns(_CommandText);
                return _ds;
            }
            catch (Exception ex)
            {
                string strExceptMSG = ex.Message;
                return null;
            }
        }

        public static object[] GetDBObjectByObj(object _Obj, DataSet _ds, string _CommandText)
        {
            try
            {
                if (_ds == null)
                {
                    _ds = DBHelper.ExecuteMyQueryIns(_CommandText);
                }
                object[] lstObj = null;
                if (_ds != null)
                {
                    lstObj = new object[_ds.Tables[0].Rows.Count];

                    for (int i = 0; i < _ds.Tables[0].Rows.Count; i++)
                    {
                        _Obj = Activator.CreateInstance(_Obj.GetType());
                        string strFieldName = "";
                        foreach (DataColumn column in _ds.Tables[0].Columns)
                        {

                            strFieldName = column.ColumnName;
                            try
                            {
                                if (_ds.Tables[0].Rows[i][strFieldName] != DBNull.Value)
                                {

                                    if (column.DataType == Type.GetType("System.Byte[]"))
                                    {
                                        byte[] b = (byte[])_ds.Tables[0].Rows[i][strFieldName];
                                        MemoryStream mstream = new MemoryStream(b);
                                        _Obj.GetType().GetProperty(strFieldName).SetValue(_Obj, Image.FromStream(mstream), null);
                                    }
                                    else
                                    {
                                        if (column.DataType.ToString() == "System.Decimal")
                                            _Obj.GetType().GetProperty(strFieldName).SetValue(_Obj, Convert.ToDouble(_ds.Tables[0].Rows[i][strFieldName].ToString()), null);
                                        else
                                            _Obj.GetType().GetProperty(strFieldName).SetValue(_Obj, _ds.Tables[0].Rows[i][strFieldName], null);
                                    }
                                }
                                else
                                {
                                    _Obj.GetType().GetProperty(strFieldName).SetValue(_Obj, null, null);
                                }
                                lstObj[i] = _Obj;
                            }
                            catch (Exception e)
                            {
                                LogFile(e);
                            }
                        }

                    }
                }
                return lstObj;
            }
            catch (Exception ex)
            {
                LogFile(ex);
                throw ex;
            }
        }


        public static object[] GetDBObjectByObj2(object _Obj, DataSet _ds, string _CommandText, string strSchema)
        {

            string strFieldName = "";
            try
            {
                //clsDBHelper.LogtxtToFile("GetDBObjectByObj2_ds-cmd-txt_ " + _CommandText);
                // clsDBHelper.LogtxtToFile("1-GetDBObjectByObj2");
                if (_ds == null)
                {
                    if (strSchema.ToLower() == "stopage")
                        _ds = DBHelper.ExecuteMyQueryStp(_CommandText);
                    else if (strSchema.ToLower() == "pt")
                        _ds = DBHelper.ExecuteMyQueryPT(_CommandText);
                    else
                        _ds = DBHelper.ExecuteMyQueryIns(_CommandText);

                }
                object[] lstObj = null;
                if (_ds != null)
                {
                    //clsDBHelper.LogtxtToFile("2-GetDBObjectByObj2");
                    lstObj = new object[_ds.Tables[0].Rows.Count];

                    for (int i = 0; i < _ds.Tables[0].Rows.Count; i++)
                    {
                        _Obj = Activator.CreateInstance(_Obj.GetType());
                        strFieldName = "";
                        Type myType = _Obj.GetType();
                        IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                        foreach (PropertyInfo prop in props)
                        {
                            //clsDBHelper.LogtxtToFile("3-GetDBObjectByObj2"+ prop.Name);
                            strFieldName = prop.Name.ToString();

                            //DataColumn column = _ds.Tables[0].Rows[0][strFieldName];
                            try
                            {
                                if ((!strFieldName.ToLower().StartsWith("lst"))&&(_ds.Tables[0].Rows[i][strFieldName] != DBNull.Value))
                                {
                                    //clsDBHelper.LogtxtToFile("4-GetDBObjectByObj2_"+ strFieldName + _ds.Tables[0].Rows[0][strFieldName].GetType().ToString());
                                    if (_ds.Tables[0].Rows[i][strFieldName].GetType().ToString() == "System.Byte[]")
                                    {
                                        //clsDBHelper.LogtxtToFile("GetDBObjectByObj2____Byte Detected_" + strFieldName);
                                        //byte[] b = (byte[])_ds.Tables[0].Rows[i][strFieldName];
                                        //sbyte[] sb = (sbyte[])(Array)b;
                                        //int i = BitConverter.ToInt32(bytes, 0);
                                        //int[] ib = b.Select(x => (int)x).ToArray(); ;
                                        //string s=Convert.ToBase64String((byte[])_ds.Tables[0].Rows[i][strFieldName]);
                                        //MemoryStream mstream = new MemoryStream(b);
                                        _Obj.GetType().GetProperty(strFieldName).SetValue(_Obj, Convert.ToBase64String((byte[])_ds.Tables[0].Rows[i][strFieldName]), null);
                                    }
                                    else
                                    {
                                        if (_ds.Tables[0].Rows[i][strFieldName].GetType().ToString() == "System.Decimal")
                                        {
                                            if (!_ds.Tables[0].Rows[i][strFieldName].ToString().Contains("."))
                                                _Obj.GetType().GetProperty(strFieldName).SetValue(_Obj, Convert.ToInt32(_ds.Tables[0].Rows[i][strFieldName].ToString()), null);
                                            else //ToDouble
                                                _Obj.GetType().GetProperty(strFieldName).SetValue(_Obj, Convert.ToDouble(_ds.Tables[0].Rows[i][strFieldName].ToString()), null);
                                        }
                                        else
                                        {
                                            if ((_ds.Tables[0].Rows[i][strFieldName].ToString() == "FALSE") || (_ds.Tables[0].Rows[i][strFieldName].ToString() == "TRUE"))
                                            {
                                                _Obj.GetType().GetProperty(strFieldName).SetValue(_Obj, Convert.ToBoolean(_ds.Tables[0].Rows[i][strFieldName].ToString()), null);
                                            }
                                            else
                                                _Obj.GetType().GetProperty(strFieldName).SetValue(_Obj, _ds.Tables[0].Rows[i][strFieldName], null);
                                        }
                                    }
                                }
                                else
                                {
                                    //DBHelper.LogtxtToFile("Null value-GetDBObjectByObj2" + strFieldName+ "_is null"+ "Query: "+_CommandText );// Thumbnail
                                    _Obj.GetType().GetProperty(strFieldName).SetValue(_Obj, null, null);
                                }
                                lstObj[i] = _Obj;
                            }
                            catch (Exception e)
                            {
                                LogFile(e);
                                DBHelper.LogtxtToFile("err1-GetDBObjectByObj2" + strFieldName + e.ToString() + e.Message.ToString());
                            }
                        }

                    }
                    //DBHelper.LogtxtToFile("Succeed-GetDBObjectByObj2");
                }
                else
                {
                    return GetDBObjectByObj2_OnLive(_Obj, _ds, _CommandText, strSchema);
                }
                return lstObj;
            }
            catch (Exception ex)
            {
                LogFile(ex);
                //DBHelper.LogtxtToFile("err2-GetDBObjectByObj2_Err:"+ex);
                throw ex;
            }
        }

        public static object[] GetDBObjectByObj2_OnLive(object _Obj, DataSet _ds, string _CommandText, string strSchema)
        {

            string strFieldName = "";
            try
            {
                //clsDBHelper.LogtxtToFile("GetDBObjectByObj2_ds-cmd-txt_ " + _CommandText);
                // clsDBHelper.LogtxtToFile("1-GetDBObjectByObj2");
                if (_ds == null)
                {
                    if (strSchema.ToLower() == "stopage")
                        _ds = DBHelper.ExecuteMyQueryStpOnLive(_CommandText);
                    else if (strSchema.ToLower() == "pt")
                        _ds = DBHelper.ExecuteMyQueryPTOnLive(_CommandText);
                    else
                        _ds = DBHelper.ExecuteMyQueryInsOnLive(_CommandText);

                }
                object[] lstObj = null;
                if (_ds != null)
                {
                    //clsDBHelper.LogtxtToFile("2-GetDBObjectByObj2");
                    lstObj = new object[_ds.Tables[0].Rows.Count];

                    for (int i = 0; i < _ds.Tables[0].Rows.Count; i++)
                    {
                        _Obj = Activator.CreateInstance(_Obj.GetType());
                        strFieldName = "";
                        Type myType = _Obj.GetType();
                        IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                        foreach (PropertyInfo prop in props)
                        {
                            //clsDBHelper.LogtxtToFile("3-GetDBObjectByObj2"+ prop.Name);
                            strFieldName = prop.Name.ToString();

                            //DataColumn column = _ds.Tables[0].Rows[0][strFieldName];
                            try
                            {
                                if ((!strFieldName.ToLower().StartsWith("lst")) && (_ds.Tables[0].Rows[i][strFieldName] != DBNull.Value))
                                {
                                    //clsDBHelper.LogtxtToFile("4-GetDBObjectByObj2_"+ strFieldName + _ds.Tables[0].Rows[0][strFieldName].GetType().ToString());
                                    if (_ds.Tables[0].Rows[i][strFieldName].GetType().ToString() == "System.Byte[]")
                                    {
                                        //clsDBHelper.LogtxtToFile("GetDBObjectByObj2____Byte Detected_" + strFieldName);
                                        //byte[] b = (byte[])_ds.Tables[0].Rows[i][strFieldName];
                                        //sbyte[] sb = (sbyte[])(Array)b;
                                        //int i = BitConverter.ToInt32(bytes, 0);
                                        //int[] ib = b.Select(x => (int)x).ToArray(); ;
                                        //string s=Convert.ToBase64String((byte[])_ds.Tables[0].Rows[i][strFieldName]);
                                        //MemoryStream mstream = new MemoryStream(b);
                                        _Obj.GetType().GetProperty(strFieldName).SetValue(_Obj, Convert.ToBase64String((byte[])_ds.Tables[0].Rows[i][strFieldName]), null);
                                    }
                                    else
                                    {
                                        if (_ds.Tables[0].Rows[i][strFieldName].GetType().ToString() == "System.Decimal")
                                            _Obj.GetType().GetProperty(strFieldName).SetValue(_Obj, Convert.ToInt32(_ds.Tables[0].Rows[i][strFieldName].ToString()), null);
                                        else if (_ds.Tables[0].Rows[i][strFieldName].GetType().ToString() == "System.Int32")
                                            _Obj.GetType().GetProperty(strFieldName).SetValue(_Obj, Convert.ToInt32(_ds.Tables[0].Rows[i][strFieldName].ToString()), null);
                                        else if (_ds.Tables[0].Rows[i][strFieldName].GetType().ToString() == "System.Int64")
                                            _Obj.GetType().GetProperty(strFieldName).SetValue(_Obj, Convert.ToInt32(_ds.Tables[0].Rows[i][strFieldName].ToString()), null);
                                        else
                                        {
                                            if ((_ds.Tables[0].Rows[i][strFieldName].ToString() == "FALSE") || (_ds.Tables[0].Rows[i][strFieldName].ToString() == "TRUE"))
                                            {
                                                _Obj.GetType().GetProperty(strFieldName).SetValue(_Obj, Convert.ToBoolean(_ds.Tables[0].Rows[i][strFieldName].ToString()), null);
                                            }
                                            else
                                                _Obj.GetType().GetProperty(strFieldName).SetValue(_Obj, _ds.Tables[0].Rows[i][strFieldName], null);
                                        }
                                    }
                                }
                                else
                                {
                                    //clsDBHelper.LogtxtToFile("Null value-GetDBObjectByObj2" + strFieldName+ "_is null");// Thumbnail
                                    _Obj.GetType().GetProperty(strFieldName).SetValue(_Obj, null, null);
                                }
                                lstObj[i] = _Obj;
                            }
                            catch (Exception e)
                            {
                                //LogFile(e);
                            }
                        }

                    }
                    //DBHelper.LogtxtToFile("Succeed-GetDBObjectByObj2");
                }
                //else
                //{
                //    Thread.Sleep(10000);
                //    return GetDBObjectByObj22(_Obj, _ds, _CommandText, strSchema);
                //}
                return lstObj;
            }
            catch (Exception ex)
            {
                LogFile(ex);
                DBHelper.LogtxtToFile("err2-GetDBObjectByObj2_Err:"+ex);
                throw ex;
            }
        }
        public static object[] GetDBObjectByObj22(object _Obj, DataSet _ds, string _CommandText, string strSchema)
        {

            string strFieldName = "";
            try
            {
                //clsDBHelper.LogtxtToFile("GetDBObjectByObj2_ds-cmd-txt_ " + _CommandText);
                // clsDBHelper.LogtxtToFile("1-GetDBObjectByObj2");
                if (_ds == null)
                {
                    if (strSchema.ToLower() == "stopage")
                        _ds = DBHelper.ExecuteMyQueryStp(_CommandText);
                    else if (strSchema.ToLower() == "pt")
                        _ds = DBHelper.ExecuteMyQueryPT(_CommandText);
                    else
                        _ds = DBHelper.ExecuteMyQueryIns(_CommandText);

                }
                object[] lstObj = null;
                if (_ds != null)
                {
                    //clsDBHelper.LogtxtToFile("2-GetDBObjectByObj2");
                    lstObj = new object[_ds.Tables[0].Rows.Count];

                    for (int i = 0; i < _ds.Tables[0].Rows.Count; i++)
                    {
                        _Obj = Activator.CreateInstance(_Obj.GetType());
                        strFieldName = "";
                        Type myType = _Obj.GetType();
                        IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                        foreach (PropertyInfo prop in props)
                        {
                            //clsDBHelper.LogtxtToFile("3-GetDBObjectByObj2"+ prop.Name);
                            strFieldName = prop.Name.ToString();

                            //DataColumn column = _ds.Tables[0].Rows[0][strFieldName];
                            try
                            {
                                if (_ds.Tables[0].Rows[i][strFieldName] != DBNull.Value)
                                {
                                    //clsDBHelper.LogtxtToFile("4-GetDBObjectByObj2_"+ strFieldName + _ds.Tables[0].Rows[0][strFieldName].GetType().ToString());
                                    if (_ds.Tables[0].Rows[i][strFieldName].GetType().ToString() == "System.Byte[]")
                                    {
                                        //clsDBHelper.LogtxtToFile("GetDBObjectByObj2____Byte Detected_" + strFieldName);
                                        //byte[] b = (byte[])_ds.Tables[0].Rows[i][strFieldName];
                                        //sbyte[] sb = (sbyte[])(Array)b;
                                        //int i = BitConverter.ToInt32(bytes, 0);
                                        //int[] ib = b.Select(x => (int)x).ToArray(); ;
                                        //string s=Convert.ToBase64String((byte[])_ds.Tables[0].Rows[i][strFieldName]);
                                        //MemoryStream mstream = new MemoryStream(b);
                                        _Obj.GetType().GetProperty(strFieldName).SetValue(_Obj, Convert.ToBase64String((byte[])_ds.Tables[0].Rows[i][strFieldName]), null);
                                    }
                                    else
                                    {
                                        if (_ds.Tables[0].Rows[i][strFieldName].GetType().ToString() == "System.Decimal")
                                            _Obj.GetType().GetProperty(strFieldName).SetValue(_Obj, Convert.ToInt32(_ds.Tables[0].Rows[i][strFieldName].ToString()), null);
                                        else
                                            _Obj.GetType().GetProperty(strFieldName).SetValue(_Obj, _ds.Tables[0].Rows[i][strFieldName], null);
                                    }
                                }
                                else
                                {
                                    //clsDBHelper.LogtxtToFile("Null value-GetDBObjectByObj2" + strFieldName+ "_is null");// Thumbnail
                                    _Obj.GetType().GetProperty(strFieldName).SetValue(_Obj, null, null);
                                }
                                lstObj[i] = _Obj;
                            }
                            catch (Exception e)
                            {
                                LogFile(e);
                                DBHelper.LogtxtToFile("err1-GetDBObjectByObj22_SecondTry_ => " + strFieldName + e.ToString() + e.InnerException.Message + e.Message.ToString());
                            }
                        }

                    }
                    //DBHelper.LogtxtToFile("Succeed-GetDBObjectByObj2");
                }
                else
                {
                    DBHelper.LogtxtToFile("*ds is Null- GetDBObjectByObj22_SecondTry_ => " + _CommandText);
                }
                return lstObj;
            }
            catch (Exception ex)
            {
                LogFile(ex);
                DBHelper.LogtxtToFile("err1-GetDBObjectByObj22_SecondTry_ => " + strFieldName + ex.ToString() + ex.InnerException.Message + ex.Message.ToString());
                //DBHelper.LogtxtToFile("err2-GetDBObjectByObj2_Err:"+ex);
                throw ex;
            }
        }

        public static object[] GetDBObjectByObj3(object _Obj, DataSet _ds, string _CommandText, string strSchema)
        {

            string strFieldName = "";
            try
            {
                //clsDBHelper.LogtxtToFile("GetDBObjectByObj2_ds-cmd-txt_ " + _CommandText);
                // clsDBHelper.LogtxtToFile("1-GetDBObjectByObj2");
                if (_ds == null)
                {
                    if (strSchema.ToLower() == "stopage")
                        _ds = DBHelper.ExecuteMyQueryStp(_CommandText);
                    else if (strSchema.ToLower() == "pt")
                        _ds = DBHelper.ExecuteMyQueryPT(_CommandText);
                    else
                        _ds = DBHelper.ExecuteMyQueryIns(_CommandText);

                }
                object[] lstObj = null;
                if (_ds != null)
                {
                    //clsDBHelper.LogtxtToFile("2-GetDBObjectByObj2");
                    lstObj = new object[_ds.Tables[0].Rows.Count];

                    for (int i = 0; i < _ds.Tables[0].Rows.Count; i++)
                    {
                        _Obj = Activator.CreateInstance(_Obj.GetType());
                        strFieldName = "";
                        Type myType = _Obj.GetType();
                        IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                        foreach (PropertyInfo prop in props)
                        {
                            //clsDBHelper.LogtxtToFile("3-GetDBObjectByObj2"+ prop.Name);
                            strFieldName = prop.Name.ToString();

                            //DataColumn column = _ds.Tables[0].Rows[0][strFieldName];
                            try
                            {
                                if (_ds.Tables[0].Rows[i][strFieldName] != DBNull.Value)
                                {
                                    //clsDBHelper.LogtxtToFile("4-GetDBObjectByObj2_"+ strFieldName + _ds.Tables[0].Rows[0][strFieldName].GetType().ToString());
                                    if (_ds.Tables[0].Rows[i][strFieldName].GetType().ToString() == "System.Byte[]")
                                    {
                                        //clsDBHelper.LogtxtToFile("GetDBObjectByObj2____Byte Detected_" + strFieldName);
                                        //byte[] b = (byte[])_ds.Tables[0].Rows[i][strFieldName];
                                        //sbyte[] sb = (sbyte[])(Array)b;
                                        //int i = BitConverter.ToInt32(bytes, 0);
                                        //int[] ib = b.Select(x => (int)x).ToArray(); ;
                                        //string s=Convert.ToBase64String((byte[])_ds.Tables[0].Rows[i][strFieldName]);
                                        //MemoryStream mstream = new MemoryStream(b);
                                        _Obj.GetType().GetProperty(strFieldName).SetValue(_Obj, Convert.ToBase64String((byte[])_ds.Tables[0].Rows[i][strFieldName]), null);
                                    }
                                    else
                                    {
                                        if (_ds.Tables[0].Rows[i][strFieldName].GetType().ToString() == "System.Decimal")
                                            _Obj.GetType().GetProperty(strFieldName).SetValue(_Obj, Convert.ToDouble(_ds.Tables[0].Rows[i][strFieldName].ToString()), null);
                                        else
                                            _Obj.GetType().GetProperty(strFieldName).SetValue(_Obj, _ds.Tables[0].Rows[i][strFieldName], null);
                                    }
                                }
                                else
                                {
                                    //clsDBHelper.LogtxtToFile("Null value-GetDBObjectByObj2" + strFieldName+ "_is null");// Thumbnail
                                    _Obj.GetType().GetProperty(strFieldName).SetValue(_Obj, null, null);
                                }
                                lstObj[i] = _Obj;
                            }
                            catch (Exception e)
                            {
                                LogFile(e);
                                DBHelper.LogtxtToFile("err1-GetDBObjectByObj3" + strFieldName + e.ToString() + e.InnerException.Message + e.Message.ToString());
                            }
                        }

                    }
                    //DBHelper.LogtxtToFile("Succeed-GetDBObjectByObj2");
                }
                else
                {
                    Thread.Sleep(10000);
                    return GetDBObjectByObj33(_Obj, _ds, _CommandText, strSchema);
                    //DBHelper.LogtxtToFile("ds is Null-GetDBObjectByObj3_" + _CommandText);
                }
                return lstObj;
            }
            catch (Exception ex)
            {
                LogFile(ex);
                //DBHelper.LogtxtToFile("err2-GetDBObjectByObj2_Err:"+ex);
                throw ex;
            }
        }
        public static object[] GetDBObjectByObj33(object _Obj, DataSet _ds, string _CommandText, string strSchema)
        {

            string strFieldName = "";
            try
            {
                //clsDBHelper.LogtxtToFile("GetDBObjectByObj2_ds-cmd-txt_ " + _CommandText);
                // clsDBHelper.LogtxtToFile("1-GetDBObjectByObj2");
                if (_ds == null)
                {
                    if (strSchema.ToLower() == "stopage")
                        _ds = DBHelper.ExecuteMyQueryStpOnLive(_CommandText);
                    else if (strSchema.ToLower() == "pt")
                        _ds = DBHelper.ExecuteMyQueryPTOnLive(_CommandText);
                    else
                        _ds = DBHelper.ExecuteMyQueryInsOnLive(_CommandText);

                }
                object[] lstObj = null;
                if (_ds != null)
                {
                    //clsDBHelper.LogtxtToFile("2-GetDBObjectByObj2");
                    lstObj = new object[_ds.Tables[0].Rows.Count];

                    for (int i = 0; i < _ds.Tables[0].Rows.Count; i++)
                    {
                        _Obj = Activator.CreateInstance(_Obj.GetType());
                        strFieldName = "";
                        Type myType = _Obj.GetType();
                        IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                        foreach (PropertyInfo prop in props)
                        {
                            //clsDBHelper.LogtxtToFile("3-GetDBObjectByObj2"+ prop.Name);
                            strFieldName = prop.Name.ToString();

                            //DataColumn column = _ds.Tables[0].Rows[0][strFieldName];
                            try
                            {
                                if (_ds.Tables[0].Rows[i][strFieldName] != DBNull.Value)
                                {
                                    //clsDBHelper.LogtxtToFile("4-GetDBObjectByObj2_"+ strFieldName + _ds.Tables[0].Rows[0][strFieldName].GetType().ToString());
                                    if (_ds.Tables[0].Rows[i][strFieldName].GetType().ToString() == "System.Byte[]")
                                    {
                                        //clsDBHelper.LogtxtToFile("GetDBObjectByObj2____Byte Detected_" + strFieldName);
                                        //byte[] b = (byte[])_ds.Tables[0].Rows[i][strFieldName];
                                        //sbyte[] sb = (sbyte[])(Array)b;
                                        //int i = BitConverter.ToInt32(bytes, 0);
                                        //int[] ib = b.Select(x => (int)x).ToArray(); ;
                                        //string s=Convert.ToBase64String((byte[])_ds.Tables[0].Rows[i][strFieldName]);
                                        //MemoryStream mstream = new MemoryStream(b);
                                        _Obj.GetType().GetProperty(strFieldName).SetValue(_Obj, Convert.ToBase64String((byte[])_ds.Tables[0].Rows[i][strFieldName]), null);
                                    }
                                    else
                                    {
                                        if (_ds.Tables[0].Rows[i][strFieldName].GetType().ToString() == "System.Decimal")
                                            _Obj.GetType().GetProperty(strFieldName).SetValue(_Obj, Convert.ToDouble(_ds.Tables[0].Rows[i][strFieldName].ToString()), null);
                                        else
                                            _Obj.GetType().GetProperty(strFieldName).SetValue(_Obj, _ds.Tables[0].Rows[i][strFieldName], null);
                                    }
                                }
                                else
                                {
                                    //clsDBHelper.LogtxtToFile("Null value-GetDBObjectByObj2" + strFieldName+ "_is null");// Thumbnail
                                    _Obj.GetType().GetProperty(strFieldName).SetValue(_Obj, null, null);
                                }
                                lstObj[i] = _Obj;
                            }
                            catch (Exception e)
                            {
                                LogFile(e);
                                DBHelper.LogtxtToFile("err1-GetDBObjectByObj3" + strFieldName + e.ToString() + e.InnerException.Message + e.Message.ToString());
                            }
                        }

                    }
                    //DBHelper.LogtxtToFile("Succeed-GetDBObjectByObj2");
                }
                else
                {
                    DBHelper.LogtxtToFile("ds is Null-GetDBObjectByObj33_Second Try" + _CommandText);
                }
                return lstObj;
            }
            catch (Exception ex)
            {
                LogFile(ex);
                //DBHelper.LogtxtToFile("err2-GetDBObjectByObj2_Err:"+ex);
                throw ex;
            }
        }
        public class Cryptographer
        {

            public static string CreateHash(string plainText,
                                             string hashAlgorithm,
                                             byte[] saltBytes)
            {
                // If salt is not specified, generate it on the fly.
                if (saltBytes == null)
                {
                    // Define min and max salt sizes.
                    int minSaltSize = 4;
                    int maxSaltSize = 8;

                    // Generate a random number for the size of the salt.
                    Random random = new Random();
                    int saltSize = random.Next(minSaltSize, maxSaltSize);

                    // Allocate a byte array, which will hold the salt.
                    saltBytes = new byte[saltSize];

                    // Initialize a random number generator.
                    RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

                    // Fill the salt with cryptographically strong byte values.
                    rng.GetNonZeroBytes(saltBytes);
                }

                // Convert plain text into a byte array.
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

                // Allocate array, which will hold plain text and salt.
                byte[] plainTextWithSaltBytes =
                        new byte[plainTextBytes.Length + saltBytes.Length];

                // Copy plain text bytes into resulting array.
                for (int i = 0; i < plainTextBytes.Length; i++)
                    plainTextWithSaltBytes[i] = plainTextBytes[i];

                // Append salt bytes to the resulting array.
                for (int i = 0; i < saltBytes.Length; i++)
                    plainTextWithSaltBytes[plainTextBytes.Length + i] = saltBytes[i];

                // Because we support multiple hashing algorithms, we must define
                // hash object as a common (abstract) base class. We will specify the
                // actual hashing algorithm class later during object creation.
                HashAlgorithm hash;

                // Make sure hashing algorithm name is specified.
                if (hashAlgorithm == null)
                    hashAlgorithm = "";

                // Initialize appropriate hashing algorithm class.
                switch (hashAlgorithm.ToUpper())
                {
                    case "SHA1":
                        hash = new SHA1Managed();
                        break;

                    case "SHA256":
                        hash = new SHA256Managed();
                        break;

                    case "SHA384":
                        hash = new SHA384Managed();
                        break;

                    case "SHA512":
                        hash = new SHA512Managed();
                        break;

                    default:
                        hash = new MD5CryptoServiceProvider();
                        break;
                }

                // Compute hash value of our plain text with appended salt.
                byte[] hashBytes = hash.ComputeHash(plainTextWithSaltBytes);

                // Create array which will hold hash and original salt bytes.
                byte[] hashWithSaltBytes = new byte[hashBytes.Length +
                                                    saltBytes.Length];

                // Copy hash bytes into resulting array.
                for (int i = 0; i < hashBytes.Length; i++)
                    hashWithSaltBytes[i] = hashBytes[i];

                // Append salt bytes to the result.
                for (int i = 0; i < saltBytes.Length; i++)
                    hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];

                // Convert result into a base64-encoded string.
                string hashValue = Convert.ToBase64String(hashWithSaltBytes);

                // Return the result.
                return hashValue;

            }



        }


        public static void LogFile(Exception ex)
        {
            try
            {
                string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
                message += Environment.NewLine;
                message += "-----------------------------------------------------------";
                message += Environment.NewLine;
                message += string.Format("Message: {0}", ex.Message);
                message += Environment.NewLine;
                message += string.Format("StackTrace: {0}", ex.StackTrace);
                message += Environment.NewLine;
                message += string.Format("Source: {0}", ex.Source);
                message += Environment.NewLine;
                message += string.Format("TargetSite: {0}", ex.TargetSite.ToString());
                message += Environment.NewLine;
                message += "-----------------------------------------------------------";
                message += Environment.NewLine;
                string path = @"D:\\WebApiLogs\\ErrorLog.txt";
                StreamWriter writer = new StreamWriter(path, true);
                writer.WriteLine(message);
                writer.Close();
            }
            catch
            {

            }
        }

        public static void LogtxtToFile(string txt)
        {
            try
            {
                string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
                message += Environment.NewLine;
                message += txt;
                string path = @"D:\\WebApiLogs\\TraceLog.txt";
                StreamWriter writer = new StreamWriter(path, true);
                writer.WriteLine(message);
                writer.Close();
            }
            catch { }
        }

        public static void LogtLoginUser(string txt)
        {
            try
            {
                string message = string.Format("Time: {0}", CommonUtility.GetNowDateTime().NowDateTimeFa);
                //message += Environment.NewLine;
                message += "_" + txt;
                string path = @"D:\\WebApiLogs\\UserLog.txt";
                StreamWriter writer = new StreamWriter(path, true);
                writer.WriteLine(message);
                writer.Close();
            }
            catch { }

        }

        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName].ToString(), null);
                    else
                        continue;
                }
            }
            return obj;
        }

        // private static Boolean 



        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
    }

}
