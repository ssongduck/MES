using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using SAMMI.Common;

namespace SAMMI.BM
{
    public class BM_Biz
    {
        DataSet rtnDsTemp = new DataSet(); // return DataSet 공통
        DataTable rtnDtTemp = new DataTable(); // return DataTable 공통

        public DataTable USP_BM0011_S1(string SFIND, ref string RS_CODE, ref string RS_MSG)
        {
            SqlDBHelper helper = new SqlDBHelper(false);
            SqlParameter[] param = new SqlParameter[3];            

            try
            {
                param[0] = helper.CreateParameter("SFIND",  SFIND, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("RS_CODE", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
                param[2] = helper.CreateParameter("RS_MSG",  SqlDbType.VarChar, ParameterDirection.Output, null, 1000);

                param[1].Direction = System.Data.ParameterDirection.Output;
                param[2].Direction = System.Data.ParameterDirection.Output;

                rtnDtTemp = helper.FillTable("USP_BM0011_S1", CommandType.StoredProcedure, param);

                RS_CODE = param[1].Value.ToString();
                RS_MSG = param[2].Value.ToString();

                return rtnDtTemp;
            }
            catch(Exception ex)
            {
                RS_CODE = "E";
                RS_MSG = ex.Message.ToString();
                return new DataTable();
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }                
            }
        }

        public DataTable USP_BM0011_S2(string SFIND, ref string RS_CODE, ref string RS_MSG)
        {
            SqlDBHelper helper = new SqlDBHelper(false);
            SqlParameter[] param = new SqlParameter[1];    
            StringBuilder query = null;

            try
            {
                query = new StringBuilder();
                query.Remove(0, query.Length);

                // 쿼리 구현 
                query.AppendLine("SELECT ITEMNAME AS COLS_A ");
                query.AppendLine("	  FROM TBM0100 ");
                query.AppendLine("	 WHERE ITEMNAME LIKE @SFIND + '%'   ");

                param[0] = helper.CreateParameter("@SFIND", SFIND, SqlDbType.VarChar, ParameterDirection.Input);

                rtnDtTemp = helper.FillTable(Convert.ToString(query), CommandType.Text, param);

                RS_CODE = "S";
                RS_MSG = "정상적으로 처리 되었습니다. ";
                return rtnDtTemp;
            }
            catch (Exception ex)
            {
                RS_CODE = "E";
                RS_MSG = ex.Message.ToString();
                return new DataTable();
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
                if (query != null) { query = null; }
            }
        }

        public DataTable USP_BM0400_S1(string sPlantCode, string sOPCode,string sOPName, string sUseFlag,ref string RS_CODE, ref string RS_MSG)
            {
            SqlDBHelper helper = new SqlDBHelper(false);
            SqlParameter[] param = new SqlParameter[4];

            try
            {
                param[0] = helper.CreateParameter("PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("OPCode", sOPCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("OPName", sOPName, SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("UseFlag", sUseFlag, SqlDbType.VarChar, ParameterDirection.Input);
       
              //  param[1] = helper.CreateParameter("RS_CODE", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
              //  param[2] = helper.CreateParameter("RS_MSG", SqlDbType.VarChar, ParameterDirection.Output, null, 1000);

              //  param[1].Direction = System.Data.ParameterDirection.Output;
              //  param[2].Direction = System.Data.ParameterDirection.Output;

                rtnDtTemp = helper.FillTable("USP_BM0400_S1", CommandType.StoredProcedure, param);

                RS_CODE = "OK";     // param[1].Value.ToString();
                RS_MSG = "정상처리"; //param[2].Value.ToString();

                return rtnDtTemp;
            }
            catch (Exception ex)
            {
                RS_CODE = "E";
                RS_MSG = ex.Message.ToString();
                return new DataTable();
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
        }

        public DataTable USP_TT1030_S1(string sStartDate, string sEndDate, string sItemCode, ref string RS_CODE, ref string RS_MSG)
        {
            SqlDBHelper helper = new SqlDBHelper(false);
            SqlParameter[] param = new SqlParameter[3];

            try
            {
                param[0] = helper.CreateParameter("StartDate", sStartDate, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("EndDate", sEndDate, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("ItemCode",sItemCode, SqlDbType.VarChar, ParameterDirection.Input);
         
     
                rtnDtTemp = helper.FillTable("USP_TT1030_S1", CommandType.StoredProcedure, param);

                RS_CODE = "OK";     // param[1].Value.ToString();
                RS_MSG = "정상처리"; //param[2].Value.ToString();

                return rtnDtTemp;
            }
            catch (Exception ex)
            {
                RS_CODE = "E";
                RS_MSG = ex.Message.ToString();
                return new DataTable();
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
        }

        public DataTable USP_TT1000_S1(string sStartDate, string sEndDate, string sItemCode, ref string RS_CODE, ref string RS_MSG)
        {
            SqlDBHelper helper = new SqlDBHelper(false);
            SqlParameter[] param = new SqlParameter[3];

            try
            {
                param[0] = helper.CreateParameter("StartDate", sStartDate, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("EndDate", sEndDate, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("ItemCode", sItemCode, SqlDbType.VarChar, ParameterDirection.Input);


                rtnDtTemp = helper.FillTable("USP_TT1000_S1", CommandType.StoredProcedure, param);

                RS_CODE = "OK";     // param[1].Value.ToString();
                RS_MSG = "정상처리"; //param[2].Value.ToString();

                return rtnDtTemp;
            }
            catch (Exception ex)
            {
                RS_CODE = "E";
                RS_MSG = ex.Message.ToString();
                return new DataTable();
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
        }

        public DataTable USP_TT1010_S1(string sStartDate, string sEndDate, string sItemCode, ref string RS_CODE, ref string RS_MSG)
        {
            SqlDBHelper helper = new SqlDBHelper(false);
            SqlParameter[] param = new SqlParameter[3];

            try
            {
                param[0] = helper.CreateParameter("StartDate", sStartDate, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("EndDate", sEndDate, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("ItemCode", sItemCode, SqlDbType.VarChar, ParameterDirection.Input);


                rtnDtTemp = helper.FillTable("USP_TT1010_S1", CommandType.StoredProcedure, param);

                RS_CODE = "OK";     // param[1].Value.ToString();
                RS_MSG = "정상처리"; //param[2].Value.ToString();

                return rtnDtTemp;
            }
            catch (Exception ex)
            {
                RS_CODE = "E";
                RS_MSG = ex.Message.ToString();
                return new DataTable();
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
        }

        public DataTable USP_TT1020_S1(string sStartDate, string sEndDate, string sItemCode, ref string RS_CODE, ref string RS_MSG)
        {
            SqlDBHelper helper = new SqlDBHelper(false);
            SqlParameter[] param = new SqlParameter[3];

            try
            {
                param[0] = helper.CreateParameter("StartDate", sStartDate, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("EndDate", sEndDate, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("ItemCode", sItemCode, SqlDbType.VarChar, ParameterDirection.Input);


                rtnDtTemp = helper.FillTable("USP_TT1020_S1", CommandType.StoredProcedure, param);

                RS_CODE = "OK";     // param[1].Value.ToString();
                RS_MSG = "정상처리"; //param[2].Value.ToString();

                return rtnDtTemp;
            }
            catch (Exception ex)
            {
                RS_CODE = "E";
                RS_MSG = ex.Message.ToString();
                return new DataTable();
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
        }
        /// <summary>
        /// insert or update
        /// </summary>
        /// <param name="sPlantCode"></param>
        /// <param name="sOPCode"></param>
        /// <param name="sOPName"></param>
        /// <param name="sUseFlag"></param>
        /// <param name="RS_CODE"></param>
        /// <param name="RS_MSG"></param>
        /// <returns></returns>
        public void USP_BM0400_U1(string sPlantCode, string sOPCode, string sOPName, string sUseFlag, ref string RS_CODE, ref string RS_MSG)
        {
            SqlDBHelper helper = new SqlDBHelper(false);
            SqlParameter[] param = new SqlParameter[4];

            try
            {
                param[0] = helper.CreateParameter("PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("OPCode", sOPCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("OPName", sOPName, SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("UseFlag", sUseFlag, SqlDbType.VarChar, ParameterDirection.Input);

                //  param[1] = helper.CreateParameter("RS_CODE", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
                //  param[2] = helper.CreateParameter("RS_MSG", SqlDbType.VarChar, ParameterDirection.Output, null, 1000);

                //  param[1].Direction = System.Data.ParameterDirection.Output;
                //  param[2].Direction = System.Data.ParameterDirection.Output;

                helper.ExecuteNoneQuery("USP_BM0400_U1", CommandType.StoredProcedure, param);

                RS_CODE = "OK";     // param[1].Value.ToString();
                RS_MSG = "정상처리"; //param[2].Value.ToString();
            }
            catch (Exception ex)
            {
                RS_CODE = "E";
                RS_MSG = ex.Message.ToString();                
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
        }

    }
}
