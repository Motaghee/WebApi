using System;
//using System.Collections.Generic;
//using System.Configuration;
using System.Linq;
using System.Data;
using System.Drawing;
using System.IO;
//using System.Text;
using System.Security.Cryptography;
using Oracle.ManagedDataAccess.Client;
using System.Text;

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

        [Obsolete]
        public static int ExecuteQueryScalar(string commandText, bool BlnDispose)
        {
            //OracleDataAdapter da = new OracleDataAdapter();
            //DataSet ds = new DataSet();

            try
            {

                if (DBConnection.State == ConnectionState.Closed)
                {
                    DBConnection.ConnectionString = CnStr;
                    DBConnection.Open();
                }
                OracleCommand command = new OracleCommand(commandText, DBConnection);
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
                            catch (Exception e)
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

    }



    }