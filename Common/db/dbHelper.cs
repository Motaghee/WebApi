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

namespace Common.db
{
    public class clsDBHelper
    {
        public static string CnStrIns = @"Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = prctlxdbscan.saipacorp.com)(PORT = 1521))   (CONNECT_DATA =
      (SERVER = DEDICATED)
      (SERVICE_NAME = prctla.saipacorp.com)
    )  ) 
            ;User ID=inspector; Password =fpvhk92";

        public static string CnStrStp = @"Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = prctlxdbscan.saipacorp.com)(PORT = 1521))   (CONNECT_DATA =
      (SERVER = DEDICATED)
      (SERVICE_NAME = prctla.saipacorp.com)
    )  ) 
            ;User ID=stopage; Password =dodkesh92";

        public static string CnStrPT = @"Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = prctlxdbscan.saipacorp.com)(PORT = 1521))   (CONNECT_DATA =
      (SERVER = DEDICATED)
      (SERVICE_NAME = prctla.saipacorp.com)
    )  ) 
            ;User ID=pt; Password =laygi94";

        //"Data Source = PRIPRCTL.SAIPACORP.COM; Persist Security Info = True; User ID = inspector; Password = fpvhk92";
        static clsDBHelper()
        {
            DBConnectionIns = new OracleConnection(CnStrIns);
            DBConnectionStp = new OracleConnection(CnStrStp);
            DBConnectionPT = new OracleConnection(CnStrPT);
        }


        public static OracleConnection DBConnectionIns;
        public static OracleConnection DBConnectionPT;
        public static OracleConnection DBConnectionStp;

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
                DataSet _ds = clsDBHelper.ExecuteMyQueryIns(_CommandText);
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
                    _ds = clsDBHelper.ExecuteMyQueryIns(_CommandText);
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
                        _ds = clsDBHelper.ExecuteMyQueryStp(_CommandText);
                    else if (strSchema.ToLower() == "pt")
                        _ds = clsDBHelper.ExecuteMyQueryPT(_CommandText);
                    else
                        _ds = clsDBHelper.ExecuteMyQueryIns(_CommandText);

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
                                            _Obj.GetType().GetProperty(strFieldName).SetValue(_Obj, Convert.ToInt64(_ds.Tables[0].Rows[i][strFieldName].ToString()), null);
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
                                clsDBHelper.LogtxtToFile("err1-GetDBObjectByObj2" + strFieldName + e.ToString() + e.InnerException.Message + e.Message.ToString());
                            }
                        }

                    }
                    clsDBHelper.LogtxtToFile("Succeed-GetDBObjectByObj2");
                }
                else
                {
                    //clsDBHelper.LogtxtToFile("ds is Null-GetDBObjectByObj2_"+ _CommandText);
                }
                return lstObj;
            }
            catch (Exception ex)
            {
                LogFile(ex);
                clsDBHelper.LogtxtToFile("err2-GetDBObjectByObj2");
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
                string path = @"C:/ErrorLog/ErrorLog.txt";
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
                string path = @"C:/ErrorLog/TraceLog.txt";
                StreamWriter writer = new StreamWriter(path, true);
                writer.WriteLine(message);
                writer.Close();
            }
            catch { }
        }

        public static void LogtLoginUser(string txt)
        {
            string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
            //message += Environment.NewLine;
            message += "__" + txt;
            string path = @"C:/ErrorLog/UserLog.txt";
            StreamWriter writer = new StreamWriter(path, true);
            writer.WriteLine(message);
            writer.Close();
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



    }
}
