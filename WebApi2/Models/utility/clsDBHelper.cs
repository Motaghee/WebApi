using System;
//using System.Collections.Generic;
//using System.Configuration;
using System.Linq;
using System.Data;
using System.Drawing;
using System.IO;
using Oracle.ManagedDataAccess.Client;

namespace WebApi2.Models.utility
{
    public class clsDBHelper
    {
        public static string CnStr = @"Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = prctlxdbscan.saipacorp.com)(PORT = 1521))   (CONNECT_DATA =
      (SERVER = DEDICATED)
      (SERVICE_NAME = prctla.saipacorp.com)
    )  ) 
            ;User ID=inspector; Password =fpvhk92";

        //"Data Source = PRIPRCTL.SAIPACORP.COM; Persist Security Info = True; User ID = inspector; Password = fpvhk92";
        static clsDBHelper()
            {
                DBConnection = new OracleConnection(CnStr);
                //cntMain.ConnectionString = 
                ////string Username = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
                //string computerName = System.Net.Dns.GetHostName();
                //if (computerName == "1000861T1071799")
                //    DBConnection = new OracleConnection("Data Source = PRIPRCTL.SAIPACORP.COM; Persist Security Info = True; User ID = inspector; Password = fpvhk92");
                //else
                //    DBConnection = new OracleConnection("Data Source = DESKTOP-A6QLHCE;Initial Catalog = GHMPasargad; Integrated Security = True");

            }

        //------------------------
        // Saipa cs
        //public static OracleConnection DBConnection = new OracleConnection("Data Source = 1000861T1071799;Initial Catalog = GHMPasargad; Integrated Security = True");
        // Home cs
        
        public static OracleConnection DBConnection;// = new OracleConnection("Data Source = DESKTOP-A6QLHCE;Initial Catalog = GHMPasargad; Integrated Security = True");
                                                    //public static OracleConnection DBConnection= new OracleConnection("Data Source=1000861T1071799\\MSOracleSERVER2;Initial Catalog=EquipmentManagement;Integrated Security=True");
                                                    // Hamed cs
                                                    //public static OracleConnection DBConnection = new OracleConnection("Data Source = SERVER\\MSOracleSERVER3;Initial Catalog = EquipmentManagement; Integrated Security = True");
                                                    // OracleExpress
                                                    //public static OracleConnection DBConnection = new OracleConnection("Data Source = OracleEXPRESS;Initial Catalog = EquipmentManagement; Integrated Security = True");
                                                    //---

        public static DataSet ExecuteMyQuery(string _CommandText)   // Execute Cmd
            {
                try
                {

                    if (DBConnection.State == ConnectionState.Closed)
                    {
                        DBConnection.ConnectionString = CnStr;
                        DBConnection.Open();
                    }
                    OracleCommand cmd = new OracleCommand();
                    OracleDataAdapter da = new OracleDataAdapter();
                    cmd.Connection = DBConnection;
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
                    return null;
                }
            }

        [Obsolete]
        public static DataSet ExecuteMyQuery(string commandText, bool BlnDispose)
            {
                OracleDataAdapter da = new OracleDataAdapter();
                DataSet ds = new DataSet();
                using (OracleConnection connection = DBConnection)
                {
                    try
                    {

                        if (DBConnection.State == ConnectionState.Closed)
                        {
                            DBConnection.ConnectionString = CnStr;
                            DBConnection.Open();
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
                using (OracleConnection connection = DBConnection/*new OracleConnection(ConnectionString)*/)
                {
                    try
                    {
                        if (DBConnection.State == ConnectionState.Closed)
                        {
                            DBConnection.ConnectionString = CnStr;
                            DBConnection.Open();
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
                    DataSet _ds = clsDBHelper.ExecuteMyQuery(_CommandText);
                    return _ds;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            public static object[] GetDBObjectByObj(object _Obj, DataSet _ds, string _CommandText)
            {
                try
                {
                    if (_ds == null)
                    {
                        _ds = clsDBHelper.ExecuteMyQuery(_CommandText);
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
                                            _Obj.GetType().GetProperty(strFieldName).SetValue(_Obj, _ds.Tables[0].Rows[i][strFieldName], null);
                                    }
                                    else
                                    {
                                        _Obj.GetType().GetProperty(strFieldName).SetValue(_Obj, null, null);
                                    }
                                }
                                catch
                                {
                                }
                            }
                            lstObj[i] = _Obj;
                        }
                    }
                    return lstObj;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }


        }



    }