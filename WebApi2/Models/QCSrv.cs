using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

using WebApi2.Models.utility;
using Oracle.ManagedDataAccess.Client;

namespace WebApi2.Models
{
    public class QCSrv
    {
        #region Variables
        
        OracleConnection cntMain = clsDBHelper.DBConnection;
        OracleCommand cmdMain = new OracleCommand();
        OracleDataAdapter daMain = new OracleDataAdapter();
        string[] msg = new string[1];
        DataTable dt = new DataTable();
        DataSet dsMain = new DataSet();
        string commandtext = string.Empty;

        #endregion


    }
}