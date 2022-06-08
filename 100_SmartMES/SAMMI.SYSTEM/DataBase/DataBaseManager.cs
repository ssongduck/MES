using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;

using SAMMI.Common;

namespace SAMMI.SY
{
    /// <summary>
    /// Database manager class
    /// </summary>
    public class DataBaseManager
    {
        #region Variables

        /// <summary>
        /// Sql db helper
        /// </summary>
        private static SqlDBHelper _SqlDBHelper;

        #endregion

        #region Methods

        /// <summary>
        /// Get data info
        /// </summary>
        /// <param name="sProcedureName"></param>
        /// <param name="sqlParameters"></param>
        /// <returns></returns>
        public static DataTable GetDataInfo(string sProcedureName, SqlParameter[] sqlParameters)
        {
            _SqlDBHelper = new SqlDBHelper(true, false);

            DataTable dt = _SqlDBHelper.FillTable(sProcedureName, CommandType.StoredProcedure, sqlParameters);

            return dt;
        }

        /// <summary>
        /// Createparameters
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="SqlType"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static System.Data.SqlClient.SqlParameter CreateParameters(string name, object value, System.Data.SqlDbType SqlType, System.Data.ParameterDirection direction)
        {
            System.Data.SqlClient.SqlParameter sqlparameter1 = new System.Data.SqlClient.SqlParameter();

            sqlparameter1.ParameterName = name;
            sqlparameter1.Value = value;
            sqlparameter1.SqlDbType = SqlType;
            sqlparameter1.Direction = direction;

            return sqlparameter1;
        }
        
        #endregion
    }
}
