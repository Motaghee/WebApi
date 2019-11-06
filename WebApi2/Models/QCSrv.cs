using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace WebApi2.Models
{
    public class QCSrv
    {
        #region Variables

        //OracleConnection cntMain = clsDBHelper.DBConnectionIns;
        //OracleConnection cntMain = clsDBHelper.DBConnectionPT;
        //OracleConnection cntMain = clsDBHelper.DBConnectionIns;
        OracleCommand cmdMain = new OracleCommand();
        OracleDataAdapter daMain = new OracleDataAdapter();
        string[] msg = new string[1];
        DataTable dt = new DataTable();
        DataSet dsMain = new DataSet();
        string commandtext = string.Empty;

        #endregion


    }
}