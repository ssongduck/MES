using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using SAMMI.Common;
using System.Windows.Forms;
using SAMMI.PopManager;
using Infragistics.Win.UltraWinGrid;

using Infragistics.Win.UltraWinEditors;

namespace SAMMI.PopUp
{
    public class PopUp_Biz
    {
        DataSet rtnDsTemp = new DataSet();   // return DataSet 공통
        DataTable rtnDtTemp = new DataTable(); // return DataTable 공통
        DataTable DtTemp = new DataTable();

        public PopUp_Biz()
        {

        }
        #region 품목 팝업
        /// <summary>
        /// 품목정보 팝업
        /// </summary>
        /// <param name="sPlantCD">공장</param>
        /// <param name="sItemCD">품목</param>
        /// <param name="sItemNM">품목명</param>
        /// <param name="sItemType">품목 유형</param>
        /// <param name="RS_CODE">리턴 코드</param>
        /// <param name="RS_MSG">리턴 메시지</param>
        /// <returns></returns>
        public DataTable SEL_TBM0100(string sPlantCD, string sItemCD, string sItemNM, string sItemType)
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[4];

            try
            {
                param[0] = helper.CreateParameter("@PLANTCODE", sPlantCD,  SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@ITEMCODE",  sItemCD,   SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@ITEMNAME",  sItemNM,   SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("@ITEMTYPE",  sItemType, SqlDbType.VarChar, ParameterDirection.Input);

               // param[4].Direction = System.Data.ParameterDirection.Output;
                //param[5].Direction = System.Data.ParameterDirection.Output;

                //rtnDtTemp = helper.FillTable("USP_BM0100_POP", CommandType.StoredProcedure, param);
                rtnDtTemp = helper.FillTable("USP_BM0100_POP_UNION", CommandType.StoredProcedure, param);

                return rtnDtTemp;
            }
            catch (Exception)
            {
            
                return new DataTable();
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
        }
        #endregion 품목 팝업
        #region 품목
        /// <summary>
        /// 품목 팝업 데이타 가져오기
        /// </summary>
        /// <param name="ITEM_CD">품목</param>
        /// <param name="ITEM_NAME">품목명</param>
        /// <param name="PLANT_CD">사업장</param>
        /// <param name="ITEM_TYPE">품목 타입</param>
        /// <param name="code_id">Return TextBox 컨트롤 이름(CODE_ID)</param>
        /// <param name="code_nm">Return TextBox 컨트롤 이름(CODE_NAME)</param>
        public void TBM0100_POP(string ITEM_CD, string ITEM_NAME, string PLANT_CD, string ITEM_TYPE, TextBox code_id, TextBox code_nm)
        {

            try
            {

                DtTemp = SEL_TBM0100(PLANT_CD, ITEM_CD, ITEM_NAME, ITEM_TYPE);

                if (DtTemp.Rows.Count == 1)
                {
                    code_id.Text = Convert.ToString(DtTemp.Rows[0]["ItemCode"]);
                    code_nm.Text = Convert.ToString(DtTemp.Rows[0]["itemname"]);
                }
                else
                {
                    if (DtTemp.Rows.Count == 0)
                    {
                        if (ITEM_CD.Trim() != "" || ITEM_NAME != "")
                        {
                            code_id.Text = string.Empty;
                            code_nm.Text = string.Empty;
                           // MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                        }
                    }

                    // 품목 POP-UP 창 처리
                    PopUpManagerEX pu = new PopUpManagerEX();
                    DtTemp = pu.OpenPopUp("Item", new string[] { PLANT_CD, ITEM_TYPE, ITEM_CD, ITEM_NAME }); // 품목 조회 POP-UP창 Parameter(비가동코드, 비가동명, 비가동그룹)

                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        code_id.Text = Convert.ToString(DtTemp.Rows[0]["ItemCode"]);
                        code_nm.Text = Convert.ToString(DtTemp.Rows[0]["itemname"]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
        #endregion
        #region 품목 Grid
        /// <summary>
        /// 품목 팝업 데이타 가져오기
        /// </summary>
        /// <param name="ITEM_CD">품목</param>
        /// <param name="ITEM_NAME">품목명</param>
        /// <param name="PLANT_CD">사업장</param>
        /// <param name="ITEM_TYPE">품목 타입</param>
        /// <param name="Grid">대상 그리드</param>
        /// <param name="Column1">리턴 품목 코드(그리드 해당 컬럼 명)</param>
        /// <param name="Column2">리턴 품목 명(그리드 해당 컬럼 명)</param>
        public void TBM0100_POP_Grid(string ITEM_CD, string ITEM_NAME, string PLANT_CD, string ITEM_TYPE, UltraGrid Grid, string Column1, string Column2 )
        {
            try
            {
                DtTemp = SEL_TBM0100(PLANT_CD, ITEM_CD, ITEM_NAME, ITEM_TYPE);

                if (DtTemp.Rows.Count == 1)
                {
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = Convert.ToString(DtTemp.Rows[0]["ItemCode"]);
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = Convert.ToString(DtTemp.Rows[0]["itemname"]);
                }
                else
                {
                    if (DtTemp.Rows.Count == 0)
                    {
                        if (ITEM_CD.Trim() != "" || ITEM_NAME != "")
                        {
                            Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = string.Empty;
                            Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = string.Empty;
                            //MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                        }
                    }
                    // 품목 POP-UP 창 처리
                    PopUpManagerEX pu = new PopUpManagerEX();
                    DtTemp = pu.OpenPopUp("Item", new string[] { PLANT_CD, ITEM_TYPE, ITEM_CD, ITEM_NAME }); // 품목 조회 POP-UP창 Parameter(비가동코드, 비가동명, 비가동그룹)

                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = Convert.ToString(DtTemp.Rows[0]["ItemCode"]);
                        Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = Convert.ToString(DtTemp.Rows[0]["itemname"]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
        #endregion

        #region 작업장별 품목 팝업
        /// <summary>
        /// 품목정보 팝업
        /// </summary>
        /// <param name="sPlantCD">공장</param>
        /// <param name="sItemCD">품목</param>
        /// <param name="sItemNM">품목명</param>
        /// <param name="sItemType">품목 유형</param>
        /// <param name="RS_CODE">리턴 코드</param>
        /// <param name="RS_MSG">리턴 메시지</param>
        /// <returns></returns>
        public DataTable SEL_TBM0101(string sPlantCD, string sItemCD, string sItemNM, string sWorkCenterCode, string sWorkCenterName)
        {
            return SEL_TBM0101(sPlantCD, sItemCD, sItemNM, sWorkCenterCode, sWorkCenterName, "");
       
        }
        public DataTable SEL_TBM0101(string sPlantCD, string sItemCD, string sItemNM, string sWorkCenterCode, string sWorkCenterName, string sItemType)
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[6];

            try
            {
                param[0] = helper.CreateParameter("@PLANTCODE", sPlantCD, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@ITEMCODE", sItemCD, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@ITEMNAME", sItemNM, SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("@WorkCenterCode", sWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);

                if (sWorkCenterName.Contains("["))
                {
                    sWorkCenterName = string.Empty;
                }

                param[4] = helper.CreateParameter("@WorkCenterName", sWorkCenterName, SqlDbType.VarChar, ParameterDirection.Input);
                param[5] = helper.CreateParameter("@ItemType", sItemType, SqlDbType.VarChar, ParameterDirection.Input);
               
                //rtnDtTemp = helper.FillTable("USP_BM0101_POP", CommandType.StoredProcedure, param);
                rtnDtTemp = helper.FillTable("USP_BM0101_POP_UNION", CommandType.StoredProcedure, param);

                return rtnDtTemp;
            }
            catch (Exception ex)
            {

                return new DataTable();
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
        }
        #endregion 품목 팝업
        #region 작업장별 품목
        /// <summary>
        /// 품목 팝업 데이타 가져오기
        /// </summary>
        /// <param name="ITEM_CD">품목</param>
        /// <param name="ITEM_NAME">품목명</param>
        /// <param name="PLANT_CD">사업장</param>
        /// <param name="ITEM_TYPE">품목 타입</param>
        /// <param name="code_id">Return TextBox 컨트롤 이름(CODE_ID)</param>
        /// <param name="code_nm">Return TextBox 컨트롤 이름(CODE_NAME)</param>
        public void TBM0101_POP(string ITEM_CD, string ITEM_NAME, string PLANT_CD, string WorkCenterCode, string WorkCenterName, string ItemType, TextBox code_id, TextBox code_nm)
        {
            try
            {
                DtTemp = SEL_TBM0101(PLANT_CD, ITEM_CD, ITEM_NAME, WorkCenterCode, WorkCenterName, ItemType);

                if (DtTemp.Rows.Count == 1)
                {
                    code_id.Text = Convert.ToString(DtTemp.Rows[0]["ItemCode"]);
                    code_nm.Text = Convert.ToString(DtTemp.Rows[0]["itemname"]);
                }
                else
                {
                    if (DtTemp.Rows.Count == 0)
                    {
                        if (ITEM_CD.Trim() != "" || ITEM_NAME != "")
                        {
                            code_id.Text = string.Empty;
                            code_nm.Text = string.Empty;
                          //  MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                        }
                    }

                    // 품목 POP-UP 창 처리
                    PopUpManagerEX pu = new PopUpManagerEX();
                    DtTemp = pu.OpenPopUp("TBM0101", new string[] { PLANT_CD, WorkCenterCode, WorkCenterName, ITEM_CD, ITEM_NAME, ItemType }); // 품목 조회 POP-UP창 Parameter(비가동코드, 비가동명, 비가동그룹)

                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        code_id.Text = Convert.ToString(DtTemp.Rows[0]["ItemCode"]);
                        code_nm.Text = Convert.ToString(DtTemp.Rows[0]["itemname"]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
        #endregion

        #region 작업장별 품목 Grid
        /// <summary>
        /// 품목 팝업 데이타 가져오기
        /// </summary>
        /// <param name="ITEM_CD">품목</param>
        /// <param name="ITEM_NAME">품목명</param>
        /// <param name="PLANT_CD">사업장</param>
        /// <param name="ITEM_TYPE">품목 타입</param>
        /// <param name="Grid">대상 그리드</param>
        /// <param name="Column1">리턴 품목 코드(그리드 해당 컬럼 명)</param>
        /// <param name="Column2">리턴 품목 명(그리드 해당 컬럼 명)</param>
        public void TBM0101_POP_Grid(string ITEM_CD, string ITEM_NAME, string PLANT_CD, string WorkCenterCode, string WorkCenterName, UltraGrid Grid, string Column1, string Column2)
        {
            try
            {
                DtTemp = SEL_TBM0101(PLANT_CD, ITEM_CD, ITEM_NAME, WorkCenterCode, WorkCenterName);

                if (DtTemp.Rows.Count == 1)
                {
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = Convert.ToString(DtTemp.Rows[0]["ItemCode"]);
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = Convert.ToString(DtTemp.Rows[0]["itemname"]);
                }
                else
                {
                    if (DtTemp.Rows.Count == 0)
                    {
                        if (ITEM_CD.Trim() != "" || ITEM_NAME != "")
                        {
                            Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = string.Empty;
                            Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = string.Empty;
                        //   MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                        }
                    }
                    // 품목 POP-UP 창 처리
                    PopUpManagerEX pu = new PopUpManagerEX();
                    DtTemp = pu.OpenPopUp("TBM0101", new string[] { PLANT_CD, WorkCenterCode, WorkCenterName, ITEM_CD, ITEM_NAME }); // 품목 조회 POP-UP창 Parameter(비가동코드, 비가동명, 비가동그룹)

                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = Convert.ToString(DtTemp.Rows[0]["ItemCode"]);
                        Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = Convert.ToString(DtTemp.Rows[0]["itemname"]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
        #endregion


        #region 작업자((WorkerID) 팝업
        /// <summary>
        /// 작업자 정보 팝업
        /// </summary>
        /// <param name="sPlantCode">공장(사업장)</param>
        /// <param name="sOPCode">공정코드</param>
        /// <param name="sLineCode">라인코드</param>
        /// <param name="sWorkCenterCode">작업장코드</param>
        /// <param name="sWorkerID">작업자 ID</param>
        /// <param name="sWorkerName">작업자명</param>
        /// <param name="sUseFlag">사용여부</param>
        /// <param name="RS_CODE">리턴 코드</param>
        /// <param name="RS_MSG">리턴 메시지</param>
        /// <returns></returns>
        /// 
        public DataTable SEL_TBM0200(string sPlantCode, string sOPCode, string sLineCode, string sWorkCenterCode, string sWorkerID, string sWorkerName, string sUseFlag)
        {
            return SEL_TBM0200(sPlantCode, sOPCode, sLineCode, sWorkCenterCode, sWorkerID, sWorkerName, sUseFlag, "0");
        }
        public DataTable SEL_TBM0200(string sPlantCode, string sOPCode, string sLineCode, string sWorkCenterCode, string sWorkerID, string sWorkerName, string sUseFlag,string div)
        {
            SqlDBHelper helper = new SqlDBHelper(false);
            SqlParameter[] param = new SqlParameter[8];

            try
            {
                param[0] = helper.CreateParameter("@PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@OPCode", sOPCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@LineCode", sLineCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("@WorkCenterCode", sWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[4] = helper.CreateParameter("@WorkerID", sWorkerID, SqlDbType.VarChar, ParameterDirection.Input);
                param[5] = helper.CreateParameter("@WorkerName", sWorkerName, SqlDbType.VarChar, ParameterDirection.Input);
                param[6] = helper.CreateParameter("@UseFlag", sUseFlag, SqlDbType.VarChar, ParameterDirection.Input);
                param[7] = helper.CreateParameter("@Div", div, SqlDbType.VarChar, ParameterDirection.Input);

                rtnDtTemp = helper.FillTable("USP_BM0200_POP", CommandType.StoredProcedure, param);


                return rtnDtTemp;
            }
            catch (Exception)
            {
                return new DataTable();
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
        }
        #endregion 작업자  팝업
        #region 작업자
        /// <summary>
        /// 작업자 팝업 데이타 가져오기
        /// </summary>
        /// <param name="PlantCode">사업장</param>
        /// <param name="OPCode">공정코드</param>
        /// <param name="LineCode">라인코드</param>
        /// <param name="WorkCenterCode">작업장코드</param>
        /// <param name="WorkerID">작업자</param>
        /// <param name="WorkerName">작업자명</param>
        /// <param name="UseFlag">사용여부</param>
        /// <param name="Code_ID">Return TextBox 컨트롤 이름(CODE_ID)</param>
        /// <param name="Code_Name">Return TextBox 컨트롤 이름(CODE_NAME)</param>
         public void TBM0200_POP(string PlantCode, string OPCode, string LineCode, string WorkCenterCode, string WorkerID, string WorkerName, string UseFlag, string div,
                                TextBox Code_ID, TextBox Code_Name)
        {
            try
            {
                DtTemp = SEL_TBM0200(PlantCode, OPCode, LineCode, WorkCenterCode, WorkerID, WorkerName, UseFlag,div);
                
                    if (DtTemp.Rows.Count == 1)
                    {
                        Code_ID.Text = Convert.ToString(DtTemp.Rows[0]["WorkerID"]);
                        Code_Name.Text = Convert.ToString(DtTemp.Rows[0]["WorkerName"]);
                    }
                    else
                    {
                        if (DtTemp.Rows.Count == 0)
                        {
                            Code_ID.Text = string.Empty;
                            Code_Name.Text = string.Empty;
                            try
                            {
                                WorkerID = "";
                                WorkerName = "";
                            }
                            catch { }
                            //  MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                        }

                        
                        PopUpManagerEX pu = new PopUpManagerEX();
                        DtTemp = pu.OpenPopUp("TBM200", new string[] { PlantCode, OPCode, LineCode, WorkCenterCode, WorkerID, WorkerName, UseFlag, div }); // 작업  POP-UP창 Parameter(작업자 ID, 작업자명)

                        if (DtTemp != null && DtTemp.Rows.Count > 0)
                        {
                            Code_ID.Text = Convert.ToString(DtTemp.Rows[0]["WorkerID"]);
                            Code_Name.Text = Convert.ToString(DtTemp.Rows[0]["WorkerName"]);
                        }
                    }
             
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }

        }
        #endregion
        #region 작업자  Grid
        /// <summary>
        /// 작업자 가져오기
        /// </summary>
        /// <param name="sPlantCode">공장(사업장)</param>
        /// <param name="sOPCode">공정코드</param>
        /// <param name="sLineCode">라인코드</param>
        /// <param name="sWorkCenterCode">작업장코드</param>
        /// <param name="sWorkerID">작업자 ID</param>
        /// <param name="sWorkerName">작업자명</param>
        /// <param name="sUseFlag">사용여부</param>
        /// <param name="Grid">대상 그리드</param>
        /// <param name="Column1">리턴 품목 코드(그리드 해당 컬럼 명)</param>
        /// <param name="Column2">리턴 품목 명(그리드 해당 컬럼 명)</param>

        public void TBM0200_POP_Grid(string sPlantCode, string sOPCode, string sLineCode, string sWorkCenterCode, string sWorkerID, string sWorkerName, string sUseFlag, UltraGrid Grid, string Column1, string Column2)
        {
            try
            {
                DtTemp = SEL_TBM0200(sPlantCode, sOPCode, sLineCode, sWorkCenterCode, sWorkerID, sWorkerName, sUseFlag);

                if (DtTemp.Rows.Count == 1)
                {
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = Convert.ToString(DtTemp.Rows[0]["WorkerID"]);
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = Convert.ToString(DtTemp.Rows[0]["WorkerName"]);
                }
                else
                {
                    if (DtTemp.Rows.Count == 0)
                    {
                        Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = string.Empty;
                        Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = string.Empty;
                        //MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                    }
                    // 품목 POP-UP 창 처리
                    PopUpManagerEX pu = new PopUpManagerEX();
                    DtTemp = pu.OpenPopUp("TBM200", new string[] { sPlantCode, sOPCode, sLineCode, sWorkCenterCode, sWorkerID, sWorkerName, sUseFlag }); // 작업자  POP-UP창 Parameter(작업자ID, 작업자명)

                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = Convert.ToString(DtTemp.Rows[0]["WorkerID"]);
                        Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = Convert.ToString(DtTemp.Rows[0]["WorkerName"]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
        #endregion

        #region 거래선-업체  (CustCode) 팝업
        /// <summary>
        /// 거래선(업체) 정보 팝업
        /// </summary>
        /// <param name="sCustType">거래처구분</param>
        /// <param name="sCustCode">거래처코드</param>
        /// <param name="sCustName">거래처명</param>
        /// <param name="sUseFlag">사용여부</param>
        /// <param name="RS_CODE">리턴 코드</param>
        /// <param name="RS_MSG">리턴 메시지</param>
        /// <returns></returns>
        /// 
        public DataTable SEL_TBM0300(string sCustCode, string sCustName, string sCustType, string sUseFlag)
        {
            SqlDBHelper helper = new SqlDBHelper(false);
            SqlParameter[] param = new SqlParameter[4];

            try
            {
                param[0] = helper.CreateParameter("@CustCode", sCustCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@CustName", sCustName, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@CustType", sCustType, SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("@UseFlag", sUseFlag, SqlDbType.VarChar, ParameterDirection.Input);

                rtnDtTemp = helper.FillTable("USP_BM0300_POP", CommandType.StoredProcedure, param);


                return rtnDtTemp;
            }
            catch (Exception)
            {

                return new DataTable();
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
        }
        #endregion 거래선  팝업       
        #region 거래선(업체)
        /// <summary>
        /// 거래선(업체) 팝업 데이타 가져오기
        /// </summary>
        /// <param name="CustCode">거래처 코드</param>
        /// <param name="CustName">거래처 명</param>
        /// <param name="CustType">거래처 구분</param>
        /// <param name="UseFlag">사용여부</param>
        /// <param name="Code_ID">Return TextBox 컨트롤 이름(CODE_ID)</param>
        /// <param name="Code_Name">Return TextBox 컨트롤 이름(CODE_ID)</param>
        public void TBM0300_POP(string CustCode, string CustName, string sPlantcode, string CustType, string UseFlag
                               , TextBox Code_ID, TextBox Code_Name )
        {
            try
            {
                DtTemp = SEL_TBM0300(CustCode, CustName, CustType, UseFlag);

                if (DtTemp.Rows.Count == 1)
                {
                    Code_ID.Text = Convert.ToString(DtTemp.Rows[0]["CustCode"]);
                    Code_Name.Text = Convert.ToString(DtTemp.Rows[0]["CustName"]);
                }
                else
                {
                    if (DtTemp.Rows.Count == 0)
                    {
                        Code_ID.Text = string.Empty;
                        Code_Name.Text = string.Empty;
                        //MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                    }
                    // 거래선 POP-UP 창 처리
                    PopUpManagerEX pu = new PopUpManagerEX();
                    DtTemp = pu.OpenPopUp("TBM300", new string[] { CustCode, CustName, CustType, UseFlag }); // 거래처  POP-UP창 Parameter(거래처코드, 거래처명)

                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        Code_ID.Text = Convert.ToString(DtTemp.Rows[0]["CustCode"]);
                        Code_Name.Text = Convert.ToString(DtTemp.Rows[0]["CustName"]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }

        /// <summary>
        /// 거래선(업체) 팝업 데이타 가져오기
        /// </summary>
        /// <param name="CustCode">거래처 코드</param>
        /// <param name="CustName">거래처 명</param>
        /// <param name="CustType">거래처 구분</param>
        /// <param name="UseFlag">사용여부</param>
        /// <param name="Code_ID">Return TextBox 컨트롤 이름(CODE_ID)</param>
        /// <param name="Code_Name">Return TextBox 컨트롤 이름(CODE_ID)</param>
        public void TBM0300_POP(string CustCode, string CustName, string CustType, string UseFlag
                               , TextBox Code_ID, TextBox Code_Name)
        {
            try
            {
                DtTemp = SEL_TBM0300(CustCode, CustName, CustType, UseFlag);

                if (DtTemp.Rows.Count == 1)
                {
                    Code_ID.Text = Convert.ToString(DtTemp.Rows[0]["CustCode"]);
                    Code_Name.Text = Convert.ToString(DtTemp.Rows[0]["CustName"]);
                }
                else
                {
                    if (DtTemp.Rows.Count == 0)
                    {
                        Code_ID.Text = string.Empty;
                        Code_Name.Text = string.Empty;
                        //MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                    }
                    // 거래선 POP-UP 창 처리
                    PopUpManagerEX pu = new PopUpManagerEX();
                    DtTemp = pu.OpenPopUp("TBM300", new string[] { CustCode, CustName, CustType, UseFlag }); // 거래처  POP-UP창 Parameter(거래처코드, 거래처명)

                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        Code_ID.Text = Convert.ToString(DtTemp.Rows[0]["CustCode"]);
                        Code_Name.Text = Convert.ToString(DtTemp.Rows[0]["CustName"]);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }

        /// <summary>
        /// 거래선(업체) 팝업 데이타 가져오기
        /// </summary>
        /// <param name="CustCode">거래처 코드</param>
        /// <param name="CustName">거래처 명</param>
        /// <param name="CustType">거래처 구분</param>
        /// <param name="UseFlag">사용여부</param>
        /// <param name="Grid">적용할 Grid</param>
        /// <param name="Column1">Return 컬럼 ( Code )</param>
        /// <param name="Column2">Return 컬럼 ( Name )</param>
        public void TBM0300_POP_Grid(string CustCode, string CustName, string sPlantCode, string CustType, string UseFlag
                                    , UltraGrid Grid, string Column1, string Column2)
        {
            try
            {
                DtTemp = SEL_TBM0300(CustCode, CustName, CustType, UseFlag);

                if (DtTemp.Rows.Count == 1)
                {
                    Grid.ActiveRow.Cells[Column1].Value = Convert.ToString(DtTemp.Rows[0]["CustCode"]);
                    Grid.ActiveRow.Cells[Column2].Value = Convert.ToString(DtTemp.Rows[0]["CustName"]);
                }
                else
                {
                    if (DtTemp.Rows.Count == 0)
                    {
                        Grid.ActiveRow.Cells[Column1].Value = string.Empty;
                        Grid.ActiveRow.Cells[Column2].Value = string.Empty;
                    }
                    // 거래선 POP-UP 창 처리
                    PopUpManagerEX pu = new PopUpManagerEX();
                    DtTemp = pu.OpenPopUp("TBM300", new string[] { CustCode, CustName, CustType, UseFlag }); // 거래처  POP-UP창 Parameter(거래처코드, 거래처명)

                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        Grid.ActiveRow.Cells[Column1].Value = Convert.ToString(DtTemp.Rows[0]["CustCode"]);
                        Grid.ActiveRow.Cells[Column2].Value = Convert.ToString(DtTemp.Rows[0]["CustName"]);
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
        /// <summary>
        /// 거래선(업체) 팝업 데이타 가져오기
        /// </summary>
        /// <param name="CustCode">거래처 코드</param>
        /// <param name="CustName">거래처 명</param>
        /// <param name="CustType">거래처 구분</param>
        /// <param name="UseFlag">사용여부</param>
        /// <param name="Grid">적용할 Grid</param>
        /// <param name="Column1">Return 컬럼 ( Code )</param>
        /// <param name="Column2">Return 컬럼 ( Name )</param>
        public void TBM0300_POP_Grid(string CustCode, string CustName, string CustType, string UseFlag
                                    , UltraGrid Grid, string Column1, string Column2)
        {
            try
            {
                DtTemp = SEL_TBM0300(CustCode, CustName, CustType, UseFlag);

                if (DtTemp.Rows.Count == 1)
                {
                    Grid.ActiveRow.Cells[Column1].Value = Convert.ToString(DtTemp.Rows[0]["CustCode"]);
                    Grid.ActiveRow.Cells[Column2].Value = Convert.ToString(DtTemp.Rows[0]["CustName"]);
                }
                else
                {
                    if (DtTemp.Rows.Count == 0)
                    {
                        Grid.ActiveRow.Cells[Column1].Value = string.Empty;
                        Grid.ActiveRow.Cells[Column2].Value = string.Empty;
                        // MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                    }
                    // 거래선 POP-UP 창 처리
                    PopUpManagerEX pu = new PopUpManagerEX();
                    DtTemp = pu.OpenPopUp("TBM300", new string[] { CustCode, CustName, CustType, UseFlag }); // 거래처  POP-UP창 Parameter(거래처코드, 거래처명)

                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        Grid.ActiveRow.Cells[Column1].Value = Convert.ToString(DtTemp.Rows[0]["CustCode"]);
                        Grid.ActiveRow.Cells[Column2].Value = Convert.ToString(DtTemp.Rows[0]["CustName"]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
        #endregion

        #region 거래선-업체  ( CustCode) 팝업 - PlantCode 포함
        /// <summary>
        /// 거래선(업체) 정보 팝업
        /// </summary>
        /// <param name="sCustType">거래처구분</param>
        /// <param name="sCustCode">거래처코드</param>
        /// <param name="sCustName">거래처명</param>
        /// <param name="sUseFlag">사용여부</param>
        /// <param name="RS_CODE">리턴 코드</param>
        /// <param name="RS_MSG">리턴 메시지</param>
        /// <returns></returns>
        /// 
        public DataTable SEL_TBM0301(string sCustCode, string sCustName, string sPlantCode, string sCustType, string sUseFlag)
        {
            SqlDBHelper helper = new SqlDBHelper(false);
            SqlParameter[] param = new SqlParameter[5];

            try
            {
                param[0] = helper.CreateParameter("@PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@CustCode", sCustCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@CustName", sCustName, SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("@CustType", sCustType, SqlDbType.VarChar, ParameterDirection.Input);
                param[4] = helper.CreateParameter("@UseFlag", sUseFlag, SqlDbType.VarChar, ParameterDirection.Input);

                rtnDtTemp = helper.FillTable("USP_BM0301_POP", CommandType.StoredProcedure, param);


                return rtnDtTemp;
            }
            catch (Exception)
            {

                return new DataTable();
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
        }
        #endregion 거래선  팝업
        #region 거래선(업체)
        /// <summary>
        /// 거래선(업체) 팝업 데이타 가져오기
        /// </summary>
        /// <param name="CustCode">거래처 코드</param>
        /// <param name="CustName">거래처 명</param>
        /// <param name="CustType">거래처 구분</param>
        /// <param name="UseFlag">사용여부</param>
        /// <param name="Code_ID">Return TextBox 컨트롤 이름(CODE_ID)</param>
        /// <param name="Code_Name">Return TextBox 컨트롤 이름(CODE_ID)</param>
        public void TBM0301_POP(string CustCode, string CustName, string PlantCode, string CustType, string UseFlag
                               , TextBox Code_ID, TextBox Code_Name)
        {
            try
            {
                DtTemp = SEL_TBM0301(CustCode, CustName, PlantCode, CustType, UseFlag);
                if (DtTemp.Rows.Count == 1)
                {
                    Code_ID.Text = Convert.ToString(DtTemp.Rows[0]["CustCode"]);
                    Code_Name.Text = Convert.ToString(DtTemp.Rows[0]["CustName"]);
                }
                else
                {
                    if (DtTemp.Rows.Count == 0)
                    {
                        Code_ID.Text = string.Empty;
                        Code_Name.Text = string.Empty;
                        //MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                    }
                    // 거래선 POP-UP 창 처리
                    PopUpManagerEX pu = new PopUpManagerEX();
                    DtTemp = pu.OpenPopUp("TBM301", new string[] { CustCode, CustName, PlantCode, CustType, UseFlag }); // 거래처  POP-UP창 Parameter(거래처코드, 거래처명)

                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        Code_ID.Text = Convert.ToString(DtTemp.Rows[0]["CustCode"]);
                        Code_Name.Text = Convert.ToString(DtTemp.Rows[0]["CustName"]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }

        /// <summary>
        /// 거래선(업체) 팝업 데이타 가져오기
        /// </summary>
        /// <param name="CustCode">거래처 코드</param>
        /// <param name="CustName">거래처 명</param>
        /// <param name="CustType">거래처 구분</param>
        /// <param name="UseFlag">사용여부</param>
        /// <param name="Grid">적용할 Grid</param>
        /// <param name="Column1">Return 컬럼 ( Code )</param>
        /// <param name="Column2">Return 컬럼 ( Name )</param>
        public void TBM0301_POP_Grid(string CustCode, string CustName, string PlantCode, string CustType, string UseFlag
                                    , UltraGrid Grid, string Column1, string Column2)
        {
            try
            {
                DtTemp = SEL_TBM0301(CustCode, CustName, PlantCode, CustType, UseFlag);

                if (DtTemp.Rows.Count == 1)
                {
                    Grid.ActiveRow.Cells[Column1].Value = Convert.ToString(DtTemp.Rows[0]["CustCode"]);
                    Grid.ActiveRow.Cells[Column2].Value = Convert.ToString(DtTemp.Rows[0]["CustName"]);
                }
                else
                {
                    Grid.ActiveRow.Cells[Column1].Value = string.Empty;
                    Grid.ActiveRow.Cells[Column2].Value = string.Empty;
                    // MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                    // 거래선 POP-UP 창 처리
                    PopUpManagerEX pu = new PopUpManagerEX();
                    DtTemp = pu.OpenPopUp("TBM301", new string[] { CustCode, CustName, PlantCode, CustType, UseFlag }); // 거래처  POP-UP창 Parameter(거래처코드, 거래처명)

                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        Grid.ActiveRow.Cells[Column1].Value = Convert.ToString(DtTemp.Rows[0]["CustCode"]);
                        Grid.ActiveRow.Cells[Column2].Value = Convert.ToString(DtTemp.Rows[0]["CustName"]);
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
        #endregion

        #region 작업장(OPCODE) 공정 팝업
        /// <summary>
        /// 품목정보 팝업
        /// </summary>
        /// <param name="sPlantCD">공장</param>
        /// <param name="sOpCode">작업장코드</param>
        /// <param name="sOpName">작업장명</param>
        /// <param name="sUseFlag">사용여부</param>
        /// <param name="RS_CODE">리턴 코드</param>
        /// <param name="RS_MSG">리턴 메시지</param>
        /// <returns></returns>
        public DataTable SEL_TBM0400(string sPlantCode, string sOpCode, string sOpName, string sUseFlag)
        {
            SqlDBHelper helper = new SqlDBHelper(true,false);
            SqlParameter[] param = new SqlParameter[4];

            try
            {
                param[0] = helper.CreateParameter("@PLANTCODE", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@OPCODE", sOpCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@OPNAME", sOpName, SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("@USEFLAG", sUseFlag, SqlDbType.VarChar, ParameterDirection.Input);

                // param[4].Direction = System.Data.ParameterDirection.Output;
                //param[5].Direction = System.Data.ParameterDirection.Output;

                //rtnDtTemp = helper.FillTable("USP_BM0400_POP", CommandType.StoredProcedure, param);
                rtnDtTemp = helper.FillTable("USP_BM0400_POP_UNION", CommandType.StoredProcedure, param);

                return rtnDtTemp;
            }
            catch (Exception)
            {

                return new DataTable();
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
        }
        #endregion 작업장 팝업
        #region 공정(작업장)
        /// <summary>
        /// 공정 작업장 팝업 데이타 가져오기
        /// </summary>
        /// <param name="PlantCode">사업장</param>
        /// <param name="OPCode">공정코드</param>
        /// <param name="OPName">공정명</param>
        /// <param name="UseFlag">사용여부</param>
        /// <param name="Code_ID">Return TextBox 컨트롤 이름(CODE_ID)</param>
        /// <param name="Code_Name">Return TextBox 컨트롤 이름(CODE_NAME)</param>
        public void TBM0400_POP(string PlantCode, string OPCode, string OPName, string UseFlag,
                                TextBox Code_ID, TextBox Code_Name)
        {
            try
            {
                DtTemp = SEL_TBM0400(PlantCode, OPCode, OPName, UseFlag);


                if (DtTemp.Rows.Count == 1)
                {
                    Code_ID.Text = Convert.ToString(DtTemp.Rows[0]["OPCode"]);
                    Code_Name.Text = Convert.ToString(DtTemp.Rows[0]["OPName"]);
                }
                else
                {
                    Code_ID.Text = string.Empty;
                    Code_Name.Text = string.Empty;
                    //MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                    // 작업자 POP-UP 창 처리
                    PopUpManagerEX pu = new PopUpManagerEX();
                    DtTemp = pu.OpenPopUp("TBM400", new string[] { PlantCode, OPCode, OPName, UseFlag }); // 작업  POP-UP창 Parameter(공정, 공정명)

                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        Code_ID.Text = Convert.ToString(DtTemp.Rows[0]["OPCode"]);
                        Code_Name.Text = Convert.ToString(DtTemp.Rows[0]["OPName"]);
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }

        }
        #endregion 공정(작업장) 
        #region 공정(작업장) Grid
        /// <summary>
        /// 공정(작업장) 가져오기
        /// </summary>
        /// <param name="PlantCode">사업장</param>
        /// <param name="OPCode">공정코드</param>
        /// <param name="OPName">공정명</param>
        /// <param name="UseFlag">사용여부</param>
        /// <param name="Grid">대상 그리드</param>
        /// <param name="Column1">리턴 품목 코드(그리드 해당 컬럼 명)</param>
        /// <param name="Column2">리턴 품목 명(그리드 해당 컬럼 명)</param>
        public void TBM0400_POP_Grid(string PlantCode, string OPCode, string OPName, string UseFlag, UltraGrid Grid, string Column1, string Column2)
        {

            try
            {

                DtTemp = SEL_TBM0400(PlantCode, OPCode, OPName, UseFlag);

                if (DtTemp.Rows.Count == 1)
                {
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = Convert.ToString(DtTemp.Rows[0]["OPCode"]);
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = Convert.ToString(DtTemp.Rows[0]["OPName"]);
                }
                else
                {
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = string.Empty;
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = string.Empty;
                    // MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                    // 품목 POP-UP 창 처리
                    PopUpManagerEX pu = new PopUpManagerEX();
                    DtTemp = pu.OpenPopUp("TBM400", new string[] { PlantCode, OPCode, OPName, UseFlag }); // 작업  POP-UP창 Parameter(공정, 공정명)

                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = Convert.ToString(DtTemp.Rows[0]["OPCode"]);
                        Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = Convert.ToString(DtTemp.Rows[0]["OPName"]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
        #endregion

        #region 작업장(OPCODE) 공정 팝업
        /// <summary>
        /// 품목정보 팝업
        /// </summary>
        /// <param name="sPlantCD">공장</param>
        /// <param name="sOpCode">작업장코드</param>
        /// <param name="sOpName">작업장명</param>
        /// <param name="sUseFlag">사용여부</param>
        /// <param name="RS_CODE">리턴 코드</param>
        /// <param name="RS_MSG">리턴 메시지</param>
        /// <returns></returns>
        public DataTable SEL_TBM0401(string sPlantCode, string sOpCode, string sOpName, string ItemCode, string ItemName, string sUseFlag)
        {
            SqlDBHelper helper = new SqlDBHelper(false);
            SqlParameter[] param = new SqlParameter[6];

            try
            {
                param[0] = helper.CreateParameter("@PLANTCODE", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@OPCODE", sOpCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@OPNAME", sOpName, SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("@ITEMCODE", ItemCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[4] = helper.CreateParameter("@ITEMNAME", ItemName, SqlDbType.VarChar, ParameterDirection.Input);
                param[5] = helper.CreateParameter("@USEFLAG", sUseFlag, SqlDbType.VarChar, ParameterDirection.Input);

                // param[4].Direction = System.Data.ParameterDirection.Output;
                //param[5].Direction = System.Data.ParameterDirection.Output;

                rtnDtTemp = helper.FillTable("USP_BM0401_POP", CommandType.StoredProcedure, param);

                return rtnDtTemp;
            }
            catch (Exception)
            {

                return new DataTable();
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
        }
        #endregion 작업장 팝업
        #region 공정(작업장)
        /// <summary>
        /// 공정 작업장 팝업 데이타 가져오기
        /// </summary>
        /// <param name="PlantCode">사업장</param>
        /// <param name="OPCode">공정코드</param>
        /// <param name="OPName">공정명</param>
        /// <param name="UseFlag">사용여부</param>
        /// <param name="Code_ID">Return TextBox 컨트롤 이름(CODE_ID)</param>
        /// <param name="Code_Name">Return TextBox 컨트롤 이름(CODE_NAME)</param>
        public void TBM0401_POP(string PlantCode, string OPCode, string OPName, string UseFlag,
                                string ItemCode, string ItemName,
                                TextBox Code_ID, TextBox Code_Name)
        {
            try
            {
                DtTemp = SEL_TBM0401(PlantCode, OPCode, OPName, ItemCode, ItemName, UseFlag);

                if (DtTemp.Rows.Count == 0)
                {
                    ItemCode = "";
                    ItemName = "";
                    DtTemp = SEL_TBM0401(PlantCode, OPCode, OPName, ItemCode, ItemName, UseFlag);
                }


                if (DtTemp.Rows.Count == 1)
                {
                    Code_ID.Text = Convert.ToString(DtTemp.Rows[0]["OPCode"]);
                    Code_Name.Text = Convert.ToString(DtTemp.Rows[0]["OPName"]);
                }
                else
                {
                    Code_ID.Text = string.Empty;
                    Code_Name.Text = string.Empty;
                    // MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                    // 작업자 POP-UP 창 처리
                    PopUpManagerEX pu = new PopUpManagerEX();
                    DtTemp = pu.OpenPopUp("TBM401", new string[] { PlantCode, OPCode, OPName, ItemCode, ItemName, UseFlag }); // 작업  POP-UP창 Parameter(공정, 공정명)

                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        Code_ID.Text = Convert.ToString(DtTemp.Rows[0]["OPCode"]);
                        Code_Name.Text = Convert.ToString(DtTemp.Rows[0]["OPName"]);
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }

        }
        #endregion 공정(작업장)
        #region 공정(작업장) Grid
        /// <summary>
        /// 공정(작업장) 가져오기
        /// </summary>
        /// <param name="PlantCode">사업장</param>
        /// <param name="OPCode">공정코드</param>
        /// <param name="OPName">공정명</param>
        /// <param name="UseFlag">사용여부</param>
        /// <param name="Grid">대상 그리드</param>
        /// <param name="Column1">리턴 품목 코드(그리드 해당 컬럼 명)</param>
        /// <param name="Column2">리턴 품목 명(그리드 해당 컬럼 명)</param>
        public void TBM0400_POP_Grid(string PlantCode, string OPCode, string OPName, string UseFlag
                        , string ItemCode, string ItemName, UltraGrid Grid, string Column1, string Column2)
        {

            try
            {

                DtTemp = SEL_TBM0401(PlantCode, OPCode, OPName, ItemCode, ItemName, UseFlag);

                if (DtTemp.Rows.Count == 0)
                {
                    ItemCode = "";
                    ItemName = "";
                    DtTemp = SEL_TBM0401(PlantCode, OPCode, OPName, ItemCode, ItemName, UseFlag);
                }



                if (DtTemp.Rows.Count == 1)
                {
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = Convert.ToString(DtTemp.Rows[0]["OPCode"]);
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = Convert.ToString(DtTemp.Rows[0]["OPName"]);
                }
                else
                {
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = string.Empty;
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = string.Empty;
                   // MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                    // 품목 POP-UP 창 처리
                    PopUpManagerEX pu = new PopUpManagerEX();
                    DtTemp = pu.OpenPopUp("TBM401", new string[] { PlantCode, OPCode, OPName, ItemCode, ItemName, UseFlag }); // 작업  POP-UP창 Parameter(공정, 공정명)

                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = Convert.ToString(DtTemp.Rows[0]["OPCode"]);
                        Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = Convert.ToString(DtTemp.Rows[0]["OPName"]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
        #endregion

        #region 라인(LINECODE) 팝업
        /// <summary>
        /// 라인 정보  팝업
        /// </summary>
        /// <param name="sPlantCode">사업장코드</param> 
        /// <param name="sLineCode">라인코드</param> 
        /// <param name="sLineName">라인명</param>  
        /// <param name="sUseFlag">사용여부</param>   
        /// <param name="RS_CODE">리턴 코드</param>
        /// <param name="RS_MSG">리턴 메시지</param>
        /// <returns></returns>

        public DataTable SEL_TBM0500(string sPlantCode, string OPCode, string sLineCode, string sUseFlag)
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[4];

            try
            {
                param[0] = helper.CreateParameter("@PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@OPCode", OPCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@LineCode", sLineCode, SqlDbType.VarChar, ParameterDirection.Input);
                //param[3] = helper.CreateParameter("@LineName", sLineName, SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("@UseFlag", sUseFlag, SqlDbType.VarChar, ParameterDirection.Input);

                //rtnDtTemp = helper.FillTable("USP_BM0500_POP", CommandType.StoredProcedure, param);
                rtnDtTemp = helper.FillTable("USP_BM0500_POP_UNION", CommandType.StoredProcedure, param);

                return rtnDtTemp;
            }
            catch (Exception)
            {

                return new DataTable();
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
        }
        #endregion 라인  팝업
        #region 라인
        /// <summary>
        /// 라인 팝업 데이타 가져오기
        /// </summary>
        /// <param name="PlantCode">사업장 코드</param>
        /// <param name="LineCode">라인 코드</param>
        /// <param name="LineName">라인 명</param>
        /// <param name="UseFlag">사용여부</param>
        /// <param name="Code_ID">Return TextBox 컨트롤 이름(CODE_ID)</param>
        /// <param name="Code_Name">Return TextBox 컨트롤 이름(CODE_NAME)</param>
        public void TBM0500_POP(string PlantCode, string OPCode, string LineCode, string LineName, string UseFlag,
                                 TextBox Code_ID, TextBox Code_Name)
        {
            try
            {
                DtTemp = SEL_TBM0500(PlantCode, OPCode, LineCode, UseFlag);

                if (DtTemp.Rows.Count == 1)
                {
                    Code_ID.Text = Convert.ToString(DtTemp.Rows[0]["LineCode"]);
                    Code_Name.Text = Convert.ToString(DtTemp.Rows[0]["LineName"]);
                }
                else
                {
                    Code_ID.Text = string.Empty;
                    Code_Name.Text = string.Empty;
                    // MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                    // 라인  POP-UP 창 처리
                    PopUpManagerEX pu = new PopUpManagerEX();
                    DtTemp = pu.OpenPopUp("TBM500", new string[] { PlantCode, OPCode, LineCode, LineName, UseFlag }); // 라인  POP-UP창 Parameter(라인코드, 라인명)

                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        Code_ID.Text = Convert.ToString(DtTemp.Rows[0]["LineCode"]);
                        Code_Name.Text = Convert.ToString(DtTemp.Rows[0]["LineName"]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }

        }
        #endregion
        #region 라인팝업2
        public void TBM0501_POP(string PlantCode, string OPCode, string LineCode, string LineName, string UseFlag,
                                 TextBox Code_ID, TextBox Code_Name, string[] sList = null, object[] objList = null)
        {
            try
            {
                DtTemp = SEL_TBM0500(PlantCode, OPCode, LineCode, UseFlag);

                //if (DtTemp.Rows.Count > 1)
                //{
                //    // 라인  POP-UP 창 처리
                //    PopUpManagerEX pu = new PopUpManagerEX();
                //    DtTemp = pu.OpenPopUp("TBM500", new string[] { PlantCode, OPCode, LineCode, LineName, UseFlag }); // 라인  POP-UP창 Parameter(라인코드, 라인명)

                //    if (DtTemp != null && DtTemp.Rows.Count > 0)
                //    {
                //        Code_ID.Text = Convert.ToString(DtTemp.Rows[0]["LineCode"]);
                //        Code_Name.Text = Convert.ToString(DtTemp.Rows[0]["LineName"]);
                //    }
                //}
                //else
                //{
                //    if (DtTemp.Rows.Count == 1)
                //    {
                //        Code_ID.Text = Convert.ToString(DtTemp.Rows[0]["LineCode"]);
                //        Code_Name.Text = Convert.ToString(DtTemp.Rows[0]["LineName"]);
                //    }
                //    else
                //    {
                //        Code_ID.Text = string.Empty;
                //        Code_Name.Text = string.Empty;
                //        MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                //    }
                //}

                if (DtTemp.Rows.Count == 1)
                {
                    Code_ID.Text = Convert.ToString(DtTemp.Rows[0]["LineCode"]);
                    Code_Name.Text = Convert.ToString(DtTemp.Rows[0]["LineName"]);
                }
                else
                {
                    Code_ID.Text = string.Empty;
                    Code_Name.Text = string.Empty;
                    //MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                    // 작업장  POP-UP 창 처리
                    PopUpManagerEX pu = new PopUpManagerEX();
                    DtTemp = pu.OpenPopUp("TBM500", new string[] { PlantCode, OPCode, LineCode, LineName, UseFlag }); // 라인  POP-UP창 Parameter(라인코드, 라인명)

                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        Code_ID.Text = Convert.ToString(DtTemp.Rows[0]["LineCode"]);
                        Code_Name.Text = Convert.ToString(DtTemp.Rows[0]["LineName"]);

                        if (sList != null && objList != null)
                        {
                            if (sList.Count() == objList.Count())
                            {
                                for (int i = 0; i < sList.Count(); i++)
                                {
                                    TextBox t = (TextBox)objList[i];
                                    t.Text = SqlDBHelper.nvlString(DtTemp.Rows[0][sList[i]]);
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }

        }
        #endregion
        #region 라인 Grid
        /// <summary>
        /// 공정(작업장) 가져오기
        /// </summary>
        /// <param name="PlantCode">사업장</param>
        /// <param name="OPCode">공정코드</param>
        /// <param name="OPName">공정명</param>
        /// <param name="UseFlag">사용여부</param>
        /// <param name="Grid">대상 그리드</param>
        /// <param name="Column1">리턴 품목 코드(그리드 해당 컬럼 명)</param>
        /// <param name="Column2">리턴 품목 명(그리드 해당 컬럼 명)</param>
        public void TBM0500_POP_Grid(string PlantCode, string OPCode, string LineCode, string UseFlag, UltraGrid Grid, string Column1, string Column2)
        {

            try
            {

                DtTemp = SEL_TBM0500(PlantCode, OPCode, LineCode, UseFlag);
                if (DtTemp.Rows.Count == 1)
                {
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = Convert.ToString(DtTemp.Rows[0]["LineCode"]);
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = Convert.ToString(DtTemp.Rows[0]["LineName"]);
                }
                else
                {
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = string.Empty;
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = string.Empty;
                    //MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                    // 품목 POP-UP 창 처리
                    PopUpManagerEX pu = new PopUpManagerEX();
                    DtTemp = pu.OpenPopUp("TBM500", new string[] { PlantCode, OPCode, LineCode, UseFlag }); // 작업  POP-UP창 Parameter(공정, 공정명)

                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = Convert.ToString(DtTemp.Rows[0]["LineCode"]);
                        Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = Convert.ToString(DtTemp.Rows[0]["LineName"]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
        #endregion

        #region WorkCenter(WorkCenterCode) 팝업
        /// <summary>
        /// WorkCenter 정보  팝업
        /// </summary>
        /// <param name="sPlantCode">공장코드</param>   
        /// <param name="sWorkCenterCode">WorkCenter코드</param> 
        /// <param name="sWorkCenterName">WorkCenter명</param> 
        /// <param name="sOPCode">공정코드</param> 
        /// <param name="sLineCode">라인코드</param>   
        /// <param name="sUseFlag">사용여부</param> 
        /// <param name="RS_CODE">리턴 코드</param>
        /// <param name="RS_MSG">리턴 메시지</param>
        /// <returns></returns>

        public DataTable SEL_TBM0600(string sPlantCode, string sWorkCenterCode, string sWorkCenterName, string sOPCode, string sLineCode, string sUseFlag)
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[6];

            try
            {

                param[0] = helper.CreateParameter("@PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);           // 공장코드  
                param[1] = helper.CreateParameter("@WorkCenterCode", sWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);      // WorkCenter코드
                param[2] = helper.CreateParameter("@WorkCenterName", sWorkCenterName, SqlDbType.VarChar, ParameterDirection.Input);      // WorkCenter명
                param[3] = helper.CreateParameter("@OPCode", sOPCode, SqlDbType.VarChar, ParameterDirection.Input);              // 공정코드
                param[4] = helper.CreateParameter("@LineCode", sLineCode, SqlDbType.VarChar, ParameterDirection.Input);            // 라인  
                param[5] = helper.CreateParameter("@UseFlag", sUseFlag, SqlDbType.VarChar, ParameterDirection.Input);             // 사용여부

                //rtnDtTemp = helper.FillTable("USP_BM0600_POP", CommandType.StoredProcedure, param);
                rtnDtTemp = helper.FillTable("USP_BM0600_POP_UNION", CommandType.StoredProcedure, param);

                return rtnDtTemp;
            }
            catch (Exception)
            {

                return new DataTable();
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
        }
        #endregion  WorkCenter(WorkCenterCode) 팝업
        #region  WorkCenter call
        public void TBM0600_POP (string sPlantCode,  string sWorkCenterCode,  string sWorkCenterName,  string sOpCode,  string sLineCode, string sUseFlag,
                                  TextBox Code_ID, TextBox Code_Name, string[] sList = null, object[] objList = null)
        {
            try
            {
                DtTemp = SEL_TBM0600(sPlantCode, sWorkCenterCode, sWorkCenterName, sOpCode, sLineCode, sUseFlag);

                if (DtTemp.Rows.Count == 1)
                {
                    Code_ID.Text = Convert.ToString(DtTemp.Rows[0]["WorkCenterCode"]);
                    Code_Name.Text = Convert.ToString(DtTemp.Rows[0]["WorkCenterName"]);
                }
                else
                {
                    Code_ID.Text = string.Empty;
                    Code_Name.Text = string.Empty;
                   // MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                    // 작업장  POP-UP 창 처리
                    PopUpManagerEX pu = new PopUpManagerEX();
                    DtTemp = pu.OpenPopUp("TBM600", new string[] { sPlantCode, sWorkCenterCode, sWorkCenterName, sOpCode, sLineCode, sUseFlag }); // WorkCenter 창 Parameter(WorkCenter코드, WorkCenter명)

                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        Code_ID.Text = Convert.ToString(DtTemp.Rows[0]["WorkCenterCode"]);
                        Code_Name.Text = Convert.ToString(DtTemp.Rows[0]["WorkCenterName"]);

                        if (sList != null && objList != null)
                        {
                            if (sList.Count() == objList.Count())
                            {
                                for (int i = 0; i < sList.Count(); i++)
                                {
                                    TextBox t = (TextBox)objList[i];
                                    t.Text = SqlDBHelper.nvlString(DtTemp.Rows[0][sList[i]]);
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }

        }
        #endregion WorkCenter call
        #region 작업라인(호기,장) Grid
        /// <summary>
        /// 작업장(호기,라인)  가져오기
        /// </summary>
        /// </summary>
        /// <param name="sPlantCode">사업장</param>
        /// <param name="sWorkCenterCod">작업호기코드</param>
        /// <param name="sWorkCenterName">작업호기명</param>       
        /// <param name="sOPCode">공정코드</param>
        /// <param name="sLineCode">라인코드</param>
        /// <param name="sUseFlag">사용여부</param>
        /// <param name="Grid">대상 그리드</param>
        /// <param name="Column1">리턴 품목 코드(그리드 해당 컬럼 명)</param>
        /// <param name="Column2">리턴 품목 명(그리드 해당 컬럼 명)</param>
        public void TBM0600_POP_Grid(string sPlantCode, string sWorkCenterCode, string sWorkCenterName, string sOPCode, string sLineCode, string sUseFlag, UltraGrid Grid, string Column1, string Column2)
        {

            try
            {

                DtTemp = SEL_TBM0600(sPlantCode, sWorkCenterCode, sWorkCenterName, sOPCode, sLineCode, sUseFlag);

                if (DtTemp.Rows.Count == 1)
                {
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = Convert.ToString(DtTemp.Rows[0]["WorkCenterCode"]);
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = Convert.ToString(DtTemp.Rows[0]["WorkCenterName"]);
                }
                else
                {
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = string.Empty;
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = string.Empty;
                    // MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                    // 품목 POP-UP 창 처리
                    PopUpManagerEX pu = new PopUpManagerEX();
                    DtTemp = pu.OpenPopUp("TBM600", new string[] { sPlantCode, sWorkCenterCode, sWorkCenterName, sOPCode, sLineCode, sUseFlag }); // WorkCenter 창 Parameter(WorkCenter코드, WorkCenter명)


                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = Convert.ToString(DtTemp.Rows[0]["WorkCenterCode"]);
                        Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = Convert.ToString(DtTemp.Rows[0]["WorkCenterName"]);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
        #endregion

        #region WorkCenterOperation Master 팝업
        public DataTable SEL_TBM0610(string sPlantCode, string sWorkCenterOPCode, string sWorkCenterOPName, string sWorkCenterCode, string sUseFlag)
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[4];

            try
            {

                param[0] = helper.CreateParameter("@PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);           // 공장코드  
                param[1] = helper.CreateParameter("@WorkCenterCode", sWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);           // 공장코드  
                param[2] = helper.CreateParameter("@WorkCenterOPCode", sWorkCenterOPCode, SqlDbType.VarChar, ParameterDirection.Input);      // WorkCenterOP 코드
                param[3] = helper.CreateParameter("@UseFlag", sUseFlag, SqlDbType.VarChar, ParameterDirection.Input);             // 사용여부

                //rtnDtTemp = helper.FillTable("USP_BM0610_POP", CommandType.StoredProcedure, param);
                rtnDtTemp = helper.FillTable("USP_BM0610_POP_UNION", CommandType.StoredProcedure, param);

                return rtnDtTemp;
            }
            catch (Exception)
            {

                return new DataTable();
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
        }
        #endregion
        #region WorkCenterOP call
        public void TBM0610_POP(string sPlantCode, string sWorkCenterOPCode, string sWorkCenterOPName, string sWorkCenterCode, string sUseFlag,
                                  TextBox Code_ID, TextBox Code_Name, string[] sList = null, object[] objList = null)
        //    public void TBM0610_POP(string sPlantCode, string sWorkCenterOPCode, string sWorkCenterOPName, string sUseFlag)
        {
            try
            {
                DtTemp = SEL_TBM0610(sPlantCode, sWorkCenterOPCode, sWorkCenterOPName, sWorkCenterCode, sUseFlag);
                if (DtTemp.Rows.Count == 1)
                {
                    Code_ID.Text = Convert.ToString(DtTemp.Rows[0]["WorkCenterOPCode"]);
                    Code_Name.Text = Convert.ToString(DtTemp.Rows[0]["WorkCenterOPName"]);
                }
                else
                {
                    Code_ID.Text = string.Empty;
                    Code_Name.Text = string.Empty;
                    // MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                    // 작업장  POP-UP 창 처리
                    PopUpManagerEX pu = new PopUpManagerEX();
                    DtTemp = pu.OpenPopUp("TBM610", new string[] { sPlantCode, sWorkCenterCode, sWorkCenterOPCode, sWorkCenterOPName, sUseFlag }); // WorkCenter 창 Parameter(WorkCenter코드, WorkCenter명)

                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        Code_ID.Text = Convert.ToString(DtTemp.Rows[0]["WorkCenterOPCode"]);
                        Code_Name.Text = Convert.ToString(DtTemp.Rows[0]["WorkCenterOPName"]);

                        if (sList != null && objList != null)
                        {
                            if (sList.Count() == objList.Count())
                            {
                                for (int i = 0; i < sList.Count(); i++)
                                {
                                    TextBox t = (TextBox)objList[i];
                                    t.Text = SqlDBHelper.nvlString(DtTemp.Rows[0][sList[i]]);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }

        }

        #endregion
        #region 작업라인(호기,장) Grid
        /// <summary>
        /// 작업장(호기,라인)  가져오기
        /// </summary>
        /// </summary>
        /// <param name="sPlantCode">사업장</param>
        /// <param name="sWorkCenterCod">작업호기코드</param>
        /// <param name="sWorkCenterName">작업호기명</param>       
        /// <param name="sOPCode">공정코드</param>
        /// <param name="sLineCode">라인코드</param>
        /// <param name="sUseFlag">사용여부</param>
        /// <param name="Grid">대상 그리드</param>
        /// <param name="Column1">리턴 품목 코드(그리드 해당 컬럼 명)</param>
        /// <param name="Column2">리턴 품목 명(그리드 해당 컬럼 명)</param>
        public void TBM0610_POP_Grid(string sPlantCode, string sWorkCenterOPCode, string sWorkCenterOPName, string sWorkCenterCode, string sUseFlag, UltraGrid Grid, string Column1, string Column2)
        {

            try
            {

                DtTemp = SEL_TBM0610(sPlantCode, sWorkCenterOPCode, sWorkCenterOPName, sWorkCenterCode, sUseFlag);

                if (DtTemp.Rows.Count == 1)
                {
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = Convert.ToString(DtTemp.Rows[0]["WorkCenterOPCode"]);
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = Convert.ToString(DtTemp.Rows[0]["WorkCenterOPName"]);
                }
                else
                {
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = string.Empty;
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = string.Empty;
                    // MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                    // 품목 POP-UP 창 처리
                    PopUpManagerEX pu = new PopUpManagerEX();
                    DtTemp = pu.OpenPopUp("TBM610", new string[] { sPlantCode, sWorkCenterCode, sWorkCenterOPCode, sWorkCenterOPName, sUseFlag }); // WorkCenter 창 Parameter(WorkCenter코드, WorkCenter명)


                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = Convert.ToString(DtTemp.Rows[0]["WorkCenterOPCode"]);
                        Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = Convert.ToString(DtTemp.Rows[0]["WorkCenterOPName"]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
        #endregion

        #region 가공라인 팝업
        public DataTable SEL_TBM5210(string sPlantCode, string sWorkCenterLineCode, string sWorkCenterLineName, string sWorkCenterCode, string sUseFlag)
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[4];

            try
            {

                param[0] = helper.CreateParameter("@PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);           // 공장코드  
                param[1] = helper.CreateParameter("@WorkCenterCode", sWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);           // 공장코드  
                param[2] = helper.CreateParameter("@WorkCenterLineCode", sWorkCenterLineCode, SqlDbType.VarChar, ParameterDirection.Input);      // WorkCenterOP 코드
                param[3] = helper.CreateParameter("@UseFlag", sUseFlag, SqlDbType.VarChar, ParameterDirection.Input);             // 사용여부

                //rtnDtTemp = helper.FillTable("USP_BM5210_POP", CommandType.StoredProcedure, param);
                rtnDtTemp = helper.FillTable("USP_BM5210_POP_UNION", CommandType.StoredProcedure, param);

                return rtnDtTemp;
            }
            catch (Exception ex)
            {

                return new DataTable();
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
        }
        #endregion
        #region 가공라인 call
        public void TBM5210_POP(string sPlantCode, string sWorkCenterLineCode, string sWorkCenterLineName, string sWorkCenterCode, string sUseFlag,
                                  TextBox Code_ID, TextBox Code_Name, string[] sList = null, object[] objList = null)
        //    public void TBM0610_POP(string sPlantCode, string sWorkCenterOPCode, string sWorkCenterOPName, string sUseFlag)
        {
            try
            {
                DtTemp = SEL_TBM5210(sPlantCode, sWorkCenterLineCode, sWorkCenterLineName, sWorkCenterCode, sUseFlag);

                if (DtTemp.Rows.Count == 1)
                {
                    Code_ID.Text = Convert.ToString(DtTemp.Rows[0]["WorkCenterLineCode"]);
                    Code_Name.Text = Convert.ToString(DtTemp.Rows[0]["WorkCenterLineName"]);
                }
                else
                {
                    Code_ID.Text = string.Empty;
                    Code_Name.Text = string.Empty;
                   // MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                    // 작업장  POP-UP 창 처리
                    PopUpManagerEX pu = new PopUpManagerEX();
                    DtTemp = pu.OpenPopUp("TBM5210", new string[] { sPlantCode, sWorkCenterCode, sWorkCenterLineCode, sWorkCenterLineName, sUseFlag }); // WorkCenter 창 Parameter(WorkCenter코드, WorkCenter명)

                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        Code_ID.Text = Convert.ToString(DtTemp.Rows[0]["WorkCenterLineCode"]);
                        Code_Name.Text = Convert.ToString(DtTemp.Rows[0]["WorkCenterLineName"]);

                        if (sList != null && objList != null)
                        {
                            if (sList.Count() == objList.Count())
                            {
                                for (int i = 0; i < sList.Count(); i++)
                                {
                                    TextBox t = (TextBox)objList[i];
                                    t.Text = SqlDBHelper.nvlString(DtTemp.Rows[0][sList[i]]);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }

        }

        #endregion
        #region 가공라인(호기,장) Grid
        /// <summary>
        /// 작업장(호기,라인)  가져오기
        /// </summary>
        /// </summary>
        /// <param name="sPlantCode">사업장</param>
        /// <param name="sWorkCenterCod">작업호기코드</param>
        /// <param name="sWorkCenterName">작업호기명</param>       
        /// <param name="sOPCode">공정코드</param>
        /// <param name="sLineCode">라인코드</param>
        /// <param name="sUseFlag">사용여부</param>
        /// <param name="Grid">대상 그리드</param>
        /// <param name="Column1">리턴 품목 코드(그리드 해당 컬럼 명)</param>
        /// <param name="Column2">리턴 품목 명(그리드 해당 컬럼 명)</param>
        public void TBM5210_POP_Grid(string sPlantCode, string sWorkCenterLineCode, string sWorkCenterLineName, string sWorkCenterCode, string sUseFlag, UltraGrid Grid, string Column1, string Column2)
        {

            try
            {

                DtTemp = SEL_TBM5210(sPlantCode, sWorkCenterLineCode, sWorkCenterLineName, sWorkCenterCode, sUseFlag);

                if (DtTemp.Rows.Count == 1)
                {
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = Convert.ToString(DtTemp.Rows[0]["WorkCenterLineCode"]);
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = Convert.ToString(DtTemp.Rows[0]["WorkCenterLineName"]);
                }
                else
                {
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = string.Empty;
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = string.Empty;
                   // MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                    // 품목 POP-UP 창 처리
                    PopUpManagerEX pu = new PopUpManagerEX();
                    DtTemp = pu.OpenPopUp("TBM5210", new string[] { sPlantCode, sWorkCenterCode, sWorkCenterLineCode, sWorkCenterLineName, sUseFlag }); // WorkCenter 창 Parameter(WorkCenter코드, WorkCenter명)


                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = Convert.ToString(DtTemp.Rows[0]["WorkCenterLineCode"]);
                        Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = Convert.ToString(DtTemp.Rows[0]["WorkCenterLineName"]);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
        #endregion

        #region 설비  팝업
        /// <summary>
        /// 설비 정보  팝업
        /// </summary>
        /// <param name="sMachCode">설비(장비)코드</param>  
        /// <param name="sMachname">설비명</param>  
        /// <param name="sMachType">설비타입</param>  
        /// <param name="sMachType1">분류1</param> 
        /// <param name="sMachType2">분류2</param> 
        /// <param name="sUseFlag">사용여부</param> 
        /// <param name="RS_CODE">리턴 코드</param>
        /// <param name="RS_MSG">리턴 메시지</param>
        /// <returns></returns>

        public DataTable SEL_TBM0700(string sMachCode, string sMachname, string sPlantCode, string sMachType, string sMachType1, string sMachType2, string sUseFlag)
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[7];

            try
            {
                param[0] = helper.CreateParameter("@PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input); 
                param[1] = helper.CreateParameter("@MachCode", sMachCode, SqlDbType.VarChar, ParameterDirection.Input);        // 설비(장비)코드 
                param[2] = helper.CreateParameter("@Machname", sMachname, SqlDbType.VarChar, ParameterDirection.Input);        // 설비명 
                param[3] = helper.CreateParameter("@MachType", sMachType, SqlDbType.VarChar, ParameterDirection.Input);        // 설비타입 
                param[4] = helper.CreateParameter("@MachType1", sMachType1, SqlDbType.VarChar, ParameterDirection.Input);       // 분류1
                param[5] = helper.CreateParameter("@MachType2", sMachType2, SqlDbType.VarChar, ParameterDirection.Input);       // 분류2
                param[6] = helper.CreateParameter("@UseFlag", sUseFlag, SqlDbType.VarChar, ParameterDirection.Input);         // 사용여부
  
                //rtnDtTemp = helper.FillTable("USP_BM0700_POP", CommandType.StoredProcedure, param);
                rtnDtTemp = helper.FillTable("USP_BM0700_POP_UNION", CommandType.StoredProcedure, param);

                return rtnDtTemp;
            }
            catch (Exception)
            {

                return new DataTable();
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
        }
        #endregion  설비정보  팝업
        #region  설비  call
        public void TBM0700_POP(string sMachCode, string sMachname, string sPlantCode, string sMachType, string sMachType1, string sMachType2, string sUseFlag,
                                  TextBox Code_ID, TextBox Code_Name, object Code_ID1 = null, object Code_Name1 = null)
        {
            try
            {
                DtTemp = SEL_TBM0700(sMachCode, sMachname, sPlantCode, sMachType, sMachType1, sMachType2, sUseFlag);


                if (DtTemp.Rows.Count == 1)
                {
                    Code_ID.Text = Convert.ToString(DtTemp.Rows[0]["MachCode"]);
                    Code_Name.Text = Convert.ToString(DtTemp.Rows[0]["MachName"]);
                    if (Code_ID1 != null)
                    {
                        ((TextBox)Code_ID1).Text = Convert.ToString(DtTemp.Rows[0]["WorkCenterCode"]);
                        ((TextBox)Code_Name1).Text = Convert.ToString(DtTemp.Rows[0]["WorkCenterName"]);
                    }
                }
                else
                {
                    Code_ID.Text = string.Empty;
                    Code_Name.Text = string.Empty;
                    //  MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                    // 작업장  POP-UP 창 처리
                    PopUpManagerEX pu = new PopUpManagerEX();
                    DtTemp = pu.OpenPopUp("TBM700", new string[] { sMachCode, sMachname, sPlantCode, sMachType, sMachType1, sMachType2, sUseFlag }); // 설비정보 창 Parameter(섧비코드, 설비명)

                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        Code_ID.Text = Convert.ToString(DtTemp.Rows[0]["MachCode"]);
                        Code_Name.Text = Convert.ToString(DtTemp.Rows[0]["MachName"]);
                        if (Code_ID1 != null)
                        {
                            ((TextBox)Code_ID1).Text = Convert.ToString(DtTemp.Rows[0]["WorkCenterCode"]);
                            ((TextBox)Code_Name1).Text = Convert.ToString(DtTemp.Rows[0]["WorkCenterName"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }

        }
        #endregion
        #region 설비(TBM0700) Grid
        /// <summary>
        /// 그리드에서 검사항목 팝업 데이타 가져오기
        /// </summary>
        /// <param name="sMachCode">설비코드</param> 
        /// <param name="sMachname">설비명</param> 
        /// <param name="sMachType">설비타입</param>   
        /// <param name="sMachType1">분류1</param>  
        /// <param name="sMachType2">분류2</param>  
        /// <param name="sUseFlag ">사용여부</param>
        /// <param name="Grid">대상 그리드</param>
        /// <param name="Column1">리턴 품목 코드(그리드 해당 컬럼 명)</param>
        /// <param name="Column2">리턴 품목 명(그리드 해당 컬럼 명)</param>
        public void TBM0700_POP_Grid(string sPlantCode, string sMachCode, string sMachname, string sMachType, string sMachType1, string sMachType2, string sUseFlag,
                                     UltraGrid Grid, string Column1, string Column2)
        {

            try
            {
                DtTemp = SEL_TBM0700(sPlantCode, sMachCode, sMachname, sMachType, sMachType1, sMachType2, sUseFlag);


                if (DtTemp.Rows.Count == 1)
                {
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = Convert.ToString(DtTemp.Rows[0]["MachCode"]);
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = Convert.ToString(DtTemp.Rows[0]["MachName"]);
                }
                else
                {
                    //// 설비  POP-UP 창 처
                    //PopUpManagerEX pu = new PopUpManagerEX();
                    //DtTemp = pu.OpenPopUp("TBM700", new string[] { sMachCode, sMachname, sMachType, sMachType1, sMachType2, sUseFlag }); //설비 조회 POP-UP창 Parameter(설비코드, 설비명, 비가동그룹)

                    //if (DtTemp != null && DtTemp.Rows.Count > 0)
                    //{
                    //    Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = Convert.ToString(DtTemp.Rows[0]["MachCode"]);
                    //    Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = Convert.ToString(DtTemp.Rows[0]["MachName"]);
                    //}
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = string.Empty;
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = string.Empty;
                    //MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                    PopUpManagerEX pu = new PopUpManagerEX();
                    DtTemp = pu.OpenPopUp("TBM700", new string[] { sPlantCode, sMachCode, sMachname, sMachType, sMachType1, sMachType2, sUseFlag }); //설비 조회 POP-UP창 Parameter(설비코드, 설비명, 비가동그룹)

                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = Convert.ToString(DtTemp.Rows[0]["MachCode"]);
                        Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = Convert.ToString(DtTemp.Rows[0]["MachName"]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
        #endregion 설비(TBM0700) Grid

        #region 창고  팝업
        /// <summary>
        /// 창고  정보  팝업
        /// </summary>
        /// <param name="sPlantCode">공장코드</param>
        /// <param name="sWHCode">창고코드</param>  
        /// <param name="sWHName">창고명</param>   
        /// <param name="sBaseWHFlag">기본창고여부</param>
        /// <param name="sProdWHFlag">제품창고여부</param>
        /// <param name="sMetWHFlag">자재창고여부</param>
        /// <param name="sUseFlag">사용여부</param> 
        /// <param name="RS_CODE">리턴 코드</param>
        /// <param name="RS_MSG">리턴 메시지</param>
        /// <returns></returns>
  
  
  
        public DataTable SEL_TBM0800(string sPlantCode ,string sWHCode, string sWHName, string sBaseWHFlag, string sProdWHFlag, string sMetWHFlag, string sUseFlag)
        {
            SqlDBHelper helper = new SqlDBHelper(false);
            SqlParameter[] param = new SqlParameter[7];

            try
            {
            
                param[0] = helper.CreateParameter("@PlantCode" , sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);           //공장코드
                param[1] = helper.CreateParameter("@WHCode"    , sWHCode, SqlDbType.VarChar, ParameterDirection.Input);              //창고코드  
                param[2] = helper.CreateParameter("@WHName"    , sWHName, SqlDbType.VarChar, ParameterDirection.Input);              //창고명   
                param[3] = helper.CreateParameter("@BaseWHFlag", sBaseWHFlag, SqlDbType.VarChar, ParameterDirection.Input);          //기본창고여부
                param[4] = helper.CreateParameter("@ProdWHFlag", sProdWHFlag, SqlDbType.VarChar, ParameterDirection.Input);          //제품창고여부
                param[5] = helper.CreateParameter("@MetWHFlag" , sMetWHFlag, SqlDbType.VarChar, ParameterDirection.Input);           //자재창고여부
                param[6] = helper.CreateParameter("@UseFlag "  , sUseFlag, SqlDbType.VarChar, ParameterDirection.Input);             //사용여부  
 
                rtnDtTemp = helper.FillTable("USP_BM0800_POP", CommandType.StoredProcedure, param);


                return rtnDtTemp;
            }
            catch (Exception)
            {

                return new DataTable();
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
        }
        #endregion  창고정보  팝업
        #region  창고  call
        public void TBM0800_POP(string sPlantCode ,string sWHCode, string sWHName, string sBaseWHFlag, string sProdWHFlag, string sMetWHFlag, string sUseFlag,
                                  TextBox Code_ID, TextBox Code_Name)
        {
            try
            {
                DtTemp = SEL_TBM0800(sPlantCode, sWHCode, sWHName, sBaseWHFlag, sProdWHFlag, sMetWHFlag, sUseFlag);


                if (DtTemp.Rows.Count == 1)
                {
                    Code_ID.Text = Convert.ToString(DtTemp.Rows[0]["WHCode"]);
                    Code_Name.Text = Convert.ToString(DtTemp.Rows[0]["WHName"]);
                }
                else
                {
                    Code_ID.Text = string.Empty;
                    Code_Name.Text = string.Empty;
                    // MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                    // 창고  POP-UP 창 처리
                    PopUpManagerEX pu = new PopUpManagerEX();
                    DtTemp = pu.OpenPopUp("TBM800", new string[] { sPlantCode, sWHCode, sWHName, sBaseWHFlag, sProdWHFlag, sMetWHFlag, sUseFlag }); // 창고정보 창 Parameter(창고코드, 창고명)

                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        Code_ID.Text = Convert.ToString(DtTemp.Rows[0]["WHCode"]);
                        Code_Name.Text = Convert.ToString(DtTemp.Rows[0]["WHName"]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }

        }
        #endregion   
    
        #region 저장위치  팝업
        /// <summary>
        /// 저장위치   정보  팝업
        /// </summary>
        /// <param name="sPlantCode">사업장(공장)</param>
        /// <param name="sWHCode">창고코드</param>       
        /// <param name="sStorageLocCode">저장위치</param>       
        /// <param name="sStorageLocName">저장위치명</param>     
        /// <param name="sStorageLocType">저장위치구분</param>  
        /// <param name="sUseFlag">사용여부      
        /// <param name="RS_CODE">리턴 코드</param>
        /// <param name="RS_MSG">리턴 메시지</param>
        /// <returns></returns>

  
        public DataTable SEL_TBM0900(string sPlantCode, string sWHCode, string sStorageLocCode, string sStorageLocName, string sStorageLocType, string sUseFlag) 
        {
            SqlDBHelper helper = new SqlDBHelper(false);
            SqlParameter[] param = new SqlParameter[6];

            try
            {
                param[0] = helper.CreateParameter("@PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);        // 사업장(공장)      
                param[1] = helper.CreateParameter("@WHCode", sWHCode, SqlDbType.VarChar, ParameterDirection.Input);           // 창고코드          
                param[2] = helper.CreateParameter("@StorageLocCode", sStorageLocCode, SqlDbType.VarChar, ParameterDirection.Input);   // 저장위치          
                param[3] = helper.CreateParameter("@StorageLocName", sStorageLocName, SqlDbType.VarChar, ParameterDirection.Input);   // 저장위치명        
                param[4] = helper.CreateParameter("@StorageLocType", sStorageLocType, SqlDbType.VarChar, ParameterDirection.Input);   // 저장위치구분      
                param[5] = helper.CreateParameter("@UseFlag ", sUseFlag, SqlDbType.VarChar, ParameterDirection.Input);             //사용여부  
 
                rtnDtTemp = helper.FillTable("USP_BM0900_POP", CommandType.StoredProcedure, param);


                return rtnDtTemp;
            }
            catch (Exception)
            {

                return new DataTable();
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
        }
        #endregion  저장위치정보  팝업
        #region  저장위치  call
        public void TBM0900_POP(string sPlantCode, string sWHCode, string sStorageLocCode, string sStorageLocName, string sStorageLocType, string sUseFlag,
                                  TextBox Code_ID, TextBox Code_Name)
        {
            try
            {
                DtTemp = SEL_TBM0900(sPlantCode, sWHCode, sStorageLocCode, sStorageLocName, sStorageLocType, sUseFlag);

                if (DtTemp.Rows.Count == 1)
                {
                    Code_ID.Text = Convert.ToString(DtTemp.Rows[0]["StorageLocCode"]);
                    Code_Name.Text = Convert.ToString(DtTemp.Rows[0]["StorageLocName"]);
                }
                else
                {
                    Code_ID.Text = string.Empty;
                    Code_Name.Text = string.Empty;
                    // MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                    // 저장위치 팝업 창 처리
                    PopUpManagerEX pu = new PopUpManagerEX();
                    DtTemp = pu.OpenPopUp("TBM900", new string[] { sPlantCode, sWHCode, sStorageLocCode, sStorageLocName, sStorageLocType, sUseFlag }); // 저장위치정보 창 Parameter(저장위치코드, 저장우치명)

                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        Code_ID.Text = Convert.ToString(DtTemp.Rows[0]["StorageLocCode"]);
                        Code_Name.Text = Convert.ToString(DtTemp.Rows[0]["StorageLocName"]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }

        }
        #endregion        


        #region 불량항목  팝업
        /// <summary>
        /// 불량항목  정보  팝업
        /// </summary>
        /// <param name="sErrorType">불량구분</param>
        /// <param name="sErrorClass">불량유형</param>
        /// <param name="sErrorCode">불량코드</param>  
        /// <param name="sErrorDesc">불량명</param> 
        /// <param name="sUseFlag">사용여부      
        /// <param name="RS_CODE">리턴 코드</param>
        /// <param name="RS_MSG">리턴 메시지</param>
        /// <returns></returns>


        public DataTable SEL_TBM1000(string sErrorType, string sErrorClass, string sErrorCode, string sErrorDesc, string sUseFlag)
        {
            SqlDBHelper helper = new SqlDBHelper(false);
            SqlParameter[] param = new SqlParameter[5];

            try
            {
                param[0] = helper.CreateParameter("@ErrorType", sErrorType, SqlDbType.VarChar, ParameterDirection.Input);      // 불량구분
                param[1] = helper.CreateParameter("@ErrorClass", sErrorClass, SqlDbType.VarChar, ParameterDirection.Input);     // 불량유형
                param[2] = helper.CreateParameter("@ErrorCode", sErrorCode, SqlDbType.VarChar, ParameterDirection.Input);      // 불량코드  
                param[3] = helper.CreateParameter("@ErrorDesc", sErrorDesc, SqlDbType.VarChar, ParameterDirection.Input);      // 불량명 
                param[4] = helper.CreateParameter("@UseFlag", sUseFlag, SqlDbType.VarChar, ParameterDirection.Input);        // 사용여부  

                rtnDtTemp = helper.FillTable("USP_BM1000_POP", CommandType.StoredProcedure, param);


                return rtnDtTemp;
            }
            catch (Exception)
            {

                return new DataTable();
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
        }
        #endregion  불량정보  팝업
        #region  불량항목  call
        public void TBM1000_POP(string sErrorType, string sErrorClass, string sErrorCode, string sErrorDesc, string sUseFlag,
                                TextBox Code_ID, TextBox Code_Name)
        {
            try
            {

                DtTemp = SEL_TBM1000(sErrorType, sErrorClass, sErrorCode, sErrorDesc, sUseFlag);


                if (DtTemp.Rows.Count == 1)
                {
                    Code_ID.Text = Convert.ToString(DtTemp.Rows[0]["ErrorCode"]);
                    Code_Name.Text = Convert.ToString(DtTemp.Rows[0]["ErrorDesc"]);
                }
                else
                {
                    Code_ID.Text = string.Empty;
                    Code_Name.Text = string.Empty;
                    //MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                    // 저장위치 팝업 창 처리
                    PopUpManagerEX pu = new PopUpManagerEX();
                    DtTemp = pu.OpenPopUp("TBM1000", new string[] { sErrorType, sErrorClass, sErrorCode, sErrorDesc, sUseFlag }); // 불량항목 정보 창 Parameter(불량코드, 불량명)

                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        Code_ID.Text = Convert.ToString(DtTemp.Rows[0]["ErrorCode"]);
                        Code_Name.Text = Convert.ToString(DtTemp.Rows[0]["ErrorDesc"]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }

        }

        /// <summary>
        /// 그리드에서 검사항목 팝업 데이타 가져오기
        /// </summary>
        /// <param name="sInspCase">검사구분</param> 
        /// <param name="sInspType">검사대상</param> 
        /// <param name="sInspCode">검사항목코드</param>   
        /// <param name="sInspName">검사항목</param>  
        /// <param name="sUseFlag ">사용여부</param>
        /// <param name="Grid">대상 그리드</param>
        /// <param name="Column1">리턴 품목 코드(그리드 해당 컬럼 명)</param>
        /// <param name="Column2">리턴 품목 명(그리드 해당 컬럼 명)</param>
        public void TBM1000_POP_Grid(string sErrorType, string sErrorClass, string sErrorCode, string sErrorDesc, string sUseFlag,
                                     UltraGrid Grid, string Column1, string Column2, string[] sParam)
        {

            try
            {

                DtTemp = SEL_TBM1000(sErrorType, sErrorClass, sErrorCode, sErrorDesc, sUseFlag);

                if (DtTemp.Rows.Count == 1)
                {
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = Convert.ToString(DtTemp.Rows[0][Column1]);
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = Convert.ToString(DtTemp.Rows[0][Column2]);
                }
                else
                {
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = "";
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = "";
                    // 품목 POP-UP 창 처리
                    PopUpManagerEX pu = new PopUpManagerEX();
                    DtTemp = pu.OpenPopUp("TBM1000", new string[] { sErrorType, sErrorClass, sErrorCode, sErrorDesc, sUseFlag }); // 조회 POP-UP창 Parameter

                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = Convert.ToString(DtTemp.Rows[0][Column1]);
                        Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = Convert.ToString(DtTemp.Rows[0][Column2]);

                        if (sParam != null)
                        {
                            foreach (string s in sParam)
                            {
                                string[] sA = s.Split('|');

                                if (sA.Length == 2)
                                {
                                    Grid.Rows[Grid.ActiveRow.Index].Cells[sA[0]].Value = SqlDBHelper.nvlString(DtTemp.Rows[0][sA[1]]);
                                }
                                else
                                {
                                    Grid.Rows[Grid.ActiveRow.Index].Cells[s].Value = SqlDBHelper.nvlString(DtTemp.Rows[0][s]);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
        /// <summary>
        /// 그리드에서 검사항목 팝업 데이타 가져오기
        /// </summary>
        /// <param name="sInspCase">검사구분</param> 
        /// <param name="sInspType">검사대상</param> 
        /// <param name="sInspCode">검사항목코드</param>   
        /// <param name="sInspName">검사항목</param>  
        /// <param name="sUseFlag ">사용여부</param>
        /// <param name="Grid">대상 그리드</param>
        /// <param name="Column1">리턴 품목 코드(그리드 해당 컬럼 명)</param>
        /// <param name="Column2">리턴 품목 명(그리드 해당 컬럼 명)</param>
        public void TBM1000_POP_Grid(string sErrorType, string sErrorClass, string sErrorCode, string sErrorDesc, string sUseFlag,
                                     UltraGrid Grid, string Column1, string Column2, string Column3)
        {

            try
            {

                DtTemp = SEL_TBM1000(sErrorType, sErrorClass, sErrorCode, sErrorDesc, sUseFlag);


                if (DtTemp.Rows.Count == 1)
                {
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = Convert.ToString(DtTemp.Rows[0][Column1]);
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = Convert.ToString(DtTemp.Rows[0][Column2]);
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column3].Value = Convert.ToString(DtTemp.Rows[0][Column3]);
                }
                else
                {
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = "";
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = "";
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column3].Value = "";
                    // 품목 POP-UP 창 처리
                    PopUpManagerEX pu = new PopUpManagerEX();
                    DtTemp = pu.OpenPopUp("TBM1000", new string[] { sErrorType, sErrorClass, sErrorCode, sErrorDesc, sUseFlag }); // 조회 POP-UP창 Parameter

                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = Convert.ToString(DtTemp.Rows[0][Column1]);
                        Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = Convert.ToString(DtTemp.Rows[0][Column2]);
                        Grid.Rows[Grid.ActiveRow.Index].Cells[Column3].Value = Convert.ToString(DtTemp.Rows[0][Column3]);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
        #endregion    

        
        #region 품목별 불량항목  팝업
        /// <summary>
        /// 품목별 불량항목 팝업
        /// </summary>
        /// <param name="sErrorType"></param>
        /// <param name="sErrorClass"></param>
        /// <param name="sErrorCode"></param>
        /// <param name="sErrorDesc"></param>
        /// <param name="sUseFlag"></param>
        /// <param name="sPlantCode"></param>
        /// <param name="sItemCode"></param>
        /// <param name="sComponent"></param>
        /// <returns></returns>
        public DataTable SEL_TBM2500(string sErrorType, string sErrorClass, string sErrorCode, string sErrorDesc, string sUseFlag, string sPlantCode, string sItemCode, string sComponent)
        {
            SqlDBHelper helper = new SqlDBHelper(false);
            SqlParameter[] param = new SqlParameter[11];

            try
            {
                param[0] = helper.CreateParameter("@PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);      // 
                param[1] = helper.CreateParameter("@ItemCode", sItemCode, SqlDbType.VarChar, ParameterDirection.Input);     // 
                param[2] = helper.CreateParameter("@Component", sComponent, SqlDbType.VarChar, ParameterDirection.Input);      // 
                
                param[3] = helper.CreateParameter("@ErrorType", sErrorType, SqlDbType.VarChar, ParameterDirection.Input);      // 불량구분
                param[4] = helper.CreateParameter("@ErrorClass", sErrorClass, SqlDbType.VarChar, ParameterDirection.Input);     // 불량유형
                param[5] = helper.CreateParameter("@ErrorCode", sErrorCode, SqlDbType.VarChar, ParameterDirection.Input);      // 불량코드  
                param[6] = helper.CreateParameter("@ErrorDesc", sErrorDesc, SqlDbType.VarChar, ParameterDirection.Input);      // 불량명 
                param[7] = helper.CreateParameter("@UseFlag", sUseFlag, SqlDbType.VarChar, ParameterDirection.Input);        // 사용여부  

                param[8] = helper.CreateParameter("@Param1", DBNull.Value, SqlDbType.VarChar, ParameterDirection.Input);      // 
                param[9] = helper.CreateParameter("@Param2", DBNull.Value, SqlDbType.VarChar, ParameterDirection.Input);     // 
                param[10] = helper.CreateParameter("@Param3", DBNull.Value, SqlDbType.VarChar, ParameterDirection.Input);      // 
                
                rtnDtTemp = helper.FillTable("USP_BM2500_POP", CommandType.StoredProcedure, param);


                return rtnDtTemp;
            }
            catch (Exception)
            {

                return new DataTable();
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
        }
        #endregion
        #region  품목별 불량항목  call
        public void TBM2500_POP(string sErrorType, string sErrorClass, string sErrorCode, string sErrorDesc, string sUseFlag, string sPlantCode, string sItemCode, string sComponent,
                                TextBox Code_ID, TextBox Code_Name)
        {
            try
            {

                DtTemp = SEL_TBM2500(sErrorType, sErrorClass, sErrorCode, sErrorDesc, sUseFlag, sPlantCode, sItemCode, sComponent);

                if (DtTemp.Rows.Count == 1)
                {
                    Code_ID.Text = Convert.ToString(DtTemp.Rows[0]["ErrorCode"]);
                    Code_Name.Text = Convert.ToString(DtTemp.Rows[0]["ErrorDesc"]);
                }
                else
                {
                    Code_ID.Text = string.Empty;
                    Code_Name.Text = string.Empty;
                    // MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                    // 저장위치 팝업 창 처리
                    PopUpManagerEX pu = new PopUpManagerEX();
                    DtTemp = pu.OpenPopUp("TBM2500", new string[] { sErrorType, sErrorClass, sErrorCode, sErrorDesc, sUseFlag, sPlantCode, sItemCode, sComponent }); // 불량항목 정보 창 Parameter(불량코드, 불량명)

                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        Code_ID.Text = Convert.ToString(DtTemp.Rows[0]["ErrorCode"]);
                        Code_Name.Text = Convert.ToString(DtTemp.Rows[0]["ErrorDesc"]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sErrorType"></param>
        /// <param name="sErrorClass"></param>
        /// <param name="sErrorCode"></param>
        /// <param name="sErrorDesc"></param>
        /// <param name="sUseFlag"></param>
        /// <param name="sPlantCode"></param>
        /// <param name="sItemCode"></param>
        /// <param name="sComponent"></param>
        /// <param name="Grid"></param>
        /// <param name="Column1"></param>
        /// <param name="Column2"></param>
        /// <param name="sParam"></param>
        public void TBM2500_POP_Grid(string sErrorType, string sErrorClass, string sErrorCode, string sErrorDesc, string sUseFlag, string sPlantCode, string sItemCode, string sComponent, string sPlantEnable,
                                     UltraGrid Grid, string Column1, string Column2, string[] sParam)
        {

            try
            {

                DtTemp = SEL_TBM2500(sErrorType, sErrorClass, sErrorCode, sErrorDesc, sUseFlag, sPlantCode, sItemCode, sComponent);

                if (DtTemp.Rows.Count == 1)
                {
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = Convert.ToString(DtTemp.Rows[0][Column1]);
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = Convert.ToString(DtTemp.Rows[0][Column2]);
                }
                else
                {
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = "";
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = "";
                    // POP-UP 창 처리
                    PopUpManagerEX pu = new PopUpManagerEX();
                    DtTemp = pu.OpenPopUp("TBM2500", new string[] { sErrorType, sErrorClass, sErrorCode, sErrorDesc, sUseFlag, sPlantCode, sItemCode, sComponent, sPlantEnable }); // 조회 POP-UP창 Parameter

                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = Convert.ToString(DtTemp.Rows[0][Column1]);
                        Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = Convert.ToString(DtTemp.Rows[0][Column2]);

                        if (sParam != null)
                        {
                            foreach (string s in sParam)
                            {
                                string[] sA = s.Split('|');

                                if (sA.Length == 2)
                                {
                                    Grid.Rows[Grid.ActiveRow.Index].Cells[sA[0]].Value = SqlDBHelper.nvlString(DtTemp.Rows[0][sA[1]]);
                                }
                                else
                                {
                                    Grid.Rows[Grid.ActiveRow.Index].Cells[s].Value = SqlDBHelper.nvlString(DtTemp.Rows[0][s]);
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
        /// <summary>
        /// 그리드에서 품목별 불량 항목 가져오기
        /// </summary>
        /// <param name="sErrorType"></param>
        /// <param name="sErrorClass"></param>
        /// <param name="sErrorCode"></param>
        /// <param name="sErrorDesc"></param>
        /// <param name="sUseFlag"></param>
        /// <param name="sPlantCode">사업장</param>
        /// <param name="sItemCode">제품번</param>
        /// <param name="sComponent">단품번</param>
        /// <param name="Grid"></param>
        /// <param name="Column1"></param>
        /// <param name="Column2"></param>
        /// <param name="Column3"></param>
        public void TBM2500_POP_Grid(string sErrorType, string sErrorClass, string sErrorCode, string sErrorDesc, string sUseFlag, string sPlantCode, string sItemCode, string sComponent, string sPlantEnable,
                                     UltraGrid Grid, string Column1, string Column2, string Column3)
        {

            try
            {

                DtTemp = SEL_TBM2500(sErrorType, sErrorClass, sErrorCode, sErrorDesc, sUseFlag, sPlantCode, sItemCode, sComponent);

                if (DtTemp.Rows.Count == 1)
                {
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = Convert.ToString(DtTemp.Rows[0][Column1]);
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = Convert.ToString(DtTemp.Rows[0][Column2]);
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column3].Value = Convert.ToString(DtTemp.Rows[0][Column3]);
                }
                else
                {
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value ="";
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = "";
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column3].Value ="";
                    // POP-UP 창 처리
                    PopUpManagerEX pu = new PopUpManagerEX();
                    DtTemp = pu.OpenPopUp("TBM2500", new string[] { sErrorType, sErrorClass, sErrorCode, sErrorDesc, sUseFlag, sPlantCode, sItemCode, sComponent, sPlantEnable }); // 조회 POP-UP창 Parameter

                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = Convert.ToString(DtTemp.Rows[0][Column1]);
                        Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = Convert.ToString(DtTemp.Rows[0][Column2]);
                        Grid.Rows[Grid.ActiveRow.Index].Cells[Column3].Value = Convert.ToString(DtTemp.Rows[0][Column3]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
        #endregion    
        
        #region 비가동항목  팝업
        /// <summary>
        /// 비가동항목  정보  팝업
        /// </summary>
        /// <param name="sStopType">비가동구분</param>
        /// <param name="sStopClass">비가동유형</param>
        /// <param name="sStopCode">비가동코드</param>  
        /// <param name="sStopDesc">비가동명</param> 
        /// <param name="sUseFlag">사용여부      
        /// <param name="RS_CODE">리턴 코드</param>
        /// <param name="RS_MSG">리턴 메시지</param>
        /// <returns></returns>

         public DataTable SEL_TBM1100(string sPlantCode,string sStopType, string sStopClass,  string sStopCode, string sStopDesc, string sUseFlag)
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[6];

            try
            {
                param[0] = helper.CreateParameter("@PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);      // 비가동구분
                param[1] = helper.CreateParameter("@StopType", sStopType, SqlDbType.VarChar, ParameterDirection.Input);      // 비가동구분
                param[2] = helper.CreateParameter("@StopClass", sStopClass, SqlDbType.VarChar, ParameterDirection.Input);    // 비가동유형
                param[3] = helper.CreateParameter("@StopCode", sStopCode, SqlDbType.VarChar, ParameterDirection.Input);      // 비가동코드  
                param[4] = helper.CreateParameter("@StopDesc", sStopDesc, SqlDbType.VarChar, ParameterDirection.Input);      // 비가동명 
                param[5] = helper.CreateParameter("@UseFlag", sUseFlag, SqlDbType.VarChar, ParameterDirection.Input);        // 사용여부  

                rtnDtTemp = helper.FillTable("USP_BM1100_POP", CommandType.StoredProcedure, param);


                return rtnDtTemp;
            }
            catch (Exception ex)
            {

                return new DataTable();
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
        }
        #endregion  비가동정보  팝업
        #region  비가동항목  call
        public void TBM1100_POP(string sStopCode, string sStopDesc, string sPlantCode, string sStopType, string sStopClass, string sUseFlag,
                                TextBox Code_ID, TextBox Code_Name)
        {
            try
            {

                DtTemp = SEL_TBM1100(sStopType, sStopClass, sPlantCode, sStopCode, sStopDesc, sUseFlag);

                if (DtTemp.Rows.Count == 1)
                {
                    Code_ID.Text = Convert.ToString(DtTemp.Rows[0]["StopCode"]);
                    Code_Name.Text = Convert.ToString(DtTemp.Rows[0]["StopDesc"]);
                }
                else
                {
                    Code_ID.Text = string.Empty;
                    Code_Name.Text = string.Empty;
                    // MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                    // 비가동  팝업 창 처리
                    PopUpManagerEX pu = new PopUpManagerEX();
                    DtTemp = pu.OpenPopUp("TBM1100", new string[] { sStopType, sStopClass, sPlantCode, sStopCode, sStopDesc, sUseFlag }); // 비가동항목 정보 창 Parameter(비가동코드, 비가동명)

                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        Code_ID.Text = Convert.ToString(DtTemp.Rows[0]["StopCode"]);
                        Code_Name.Text = Convert.ToString(DtTemp.Rows[0]["StopDesc"]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }

        }
        #endregion    
        #region 비가동 팝업 Grid
        /// <summary>
        /// 그리드에서 비가동 팝업 데이타 가져오기
        /// </summary>

        public void TBM1100_POP_Grid(string sPlantCode, string sStopCode, string sStopDesc, string sStopType, string sStopClass, string sUseFlag,
                                UltraGrid Grid, string Column1, string Column2)
        {

            try
            {
                DtTemp = SEL_TBM1100(sPlantCode, sStopType, sStopClass, sStopCode, sStopDesc, sUseFlag);

                if (DtTemp.Rows.Count == 1)
                {
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].SetValue(SqlDBHelper.nvlString(DtTemp.Rows[0][Column1]), true);
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].SetValue(SqlDBHelper.nvlString(DtTemp.Rows[0][Column2]), true);
                }
                else
                {
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = string.Empty;
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = string.Empty;
                    //MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                    // 비가동 POP-UP 창 처리
                    PopUpManagerEX pu = new PopUpManagerEX();
                    DtTemp = pu.OpenPopUp("TBM1100", new string[] { sStopType, sStopClass, sStopCode, sStopDesc, sUseFlag }); // 비가동항목 정보 창 Parameter(비가동코드, 비가동명)

                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].SetValue(SqlDBHelper.nvlString(DtTemp.Rows[0][Column1]), true);
                        Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].SetValue(SqlDBHelper.nvlString(DtTemp.Rows[0][Column2]), true);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
        /// <summary>
        /// 그리드에서 비가동 팝업 데이타 가져오기
        /// </summary>

        public void TBM1100_POP_Grid(string sStopCode, string sStopDesc, string sStopClass, string sStopType, string sPlantCode, string sUseFlag,
                                UltraGrid Grid, string Column1, string Column2, string[] sParam)
        {

            try
            {
                if (sStopClass != "")
                {
                    if (sStopClass.StartsWith("2"))
                      sStopClass = "A";
                    else if (sStopClass.StartsWith("4"))
                      sStopClass = "B";
                  else
                      sStopClass = "";

                }
                DtTemp = SEL_TBM1100(sPlantCode, sStopType, sStopClass, sStopCode, sStopDesc, sUseFlag);

                if (DtTemp.Rows.Count == 1)
                {
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].SetValue(SqlDBHelper.nvlString(DtTemp.Rows[0][Column1]), true);
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].SetValue(SqlDBHelper.nvlString(DtTemp.Rows[0][Column2]), true);
                }
                else
                {
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = string.Empty;
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = string.Empty;
                    //MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                    // 비가동 POP-UP 창 처리
                    PopUpManagerEX pu = new PopUpManagerEX();
                    DtTemp = pu.OpenPopUp("TBM1100", new string[] { sStopType, sStopClass, sStopCode, sStopDesc, sUseFlag }); // 비가동항목 정보 창 Parameter(비가동코드, 비가동명)

                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].SetValue(SqlDBHelper.nvlString(DtTemp.Rows[0][Column1]), true);
                        Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].SetValue(SqlDBHelper.nvlString(DtTemp.Rows[0][Column2]), true);

                        if (sParam != null)
                        {
                            foreach (string s in sParam)
                            {
                                string[] sA = s.Split('|');

                                if (sA.Length == 2)
                                {
                                    Grid.Rows[Grid.ActiveRow.Index].Cells[sA[0]].Value = SqlDBHelper.nvlString(DtTemp.Rows[0][sA[0]]);
                                    Grid.Rows[Grid.ActiveRow.Index].Cells[sA[1]].Value = SqlDBHelper.nvlString(DtTemp.Rows[0][sA[1]]);
                                }
                                else
                                {
                                    Grid.Rows[Grid.ActiveRow.Index].Cells[s].Value = SqlDBHelper.nvlString(DtTemp.Rows[0][s]);
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
        #endregion 설비(TBM0700) Grid

 
        #region 검사항목  팝업
        /// <summary>
        /// 검사항목  정보  팝업
        /// </summary>
        /// <param name="sInspCase">검사구분</param> 
        /// <param name="sInspType">검사대상</param> 
        /// <param name="sInspCode">검사항목코드</param>   
        /// <param name="sInspName">검사항목</param>  
        /// <param name="sUseFlag ">사용여부</param>  
        /// <param name="RS_CODE">리턴 코드</param>
        /// <param name="RS_MSG">리턴 메시지</param>
        /// <returns></returns>



        public DataTable SEL_TBM1500(string sPlantCode, string sInspCase, string sInspType, string sInspCode, string sInspName, string sUseFlag)
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[6];

            try
            {
                param[0] = helper.CreateParameter("@PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input); // 검사구분(수입, 자주, 공정,…..)
                param[1] = helper.CreateParameter("@InspCase", sInspCase, SqlDbType.VarChar, ParameterDirection.Input); // 검사구분(수입, 자주, 공정,…..)
                param[2] = helper.CreateParameter("@InspType", sInspType, SqlDbType.VarChar, ParameterDirection.Input); // 검사대상(원자재, 자재, 반제품,…. )
                param[3] = helper.CreateParameter("@InspCode", sInspCode, SqlDbType.VarChar, ParameterDirection.Input); // 검사항목코드   
                param[4] = helper.CreateParameter("@InspName", sInspName, SqlDbType.VarChar, ParameterDirection.Input); // 검사항목 
                param[5] = helper.CreateParameter("@UseFlag ", sUseFlag, SqlDbType.VarChar, ParameterDirection.Input);  // 사용여부 

                rtnDtTemp = helper.FillTable("USP_BM1500_POP_UNION", CommandType.StoredProcedure, param);


                return rtnDtTemp;
            }
            catch (Exception)
            {

                return new DataTable();
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
        }
        
      
        //팝업창 입력 버턴
        public void INS_TBM1500(string I_InspCode, string I_InspName, string I_Maker,string I_Unit, ref string RS_CODE,ref string RS_MSG)
        {  
          
            SqlDBHelper helper = new SqlDBHelper(false);
            SqlParameter[] param = new SqlParameter[6];

            try
            {
                param[0] = helper.CreateParameter("@InspCase",  I_InspCode, SqlDbType.VarChar,   ParameterDirection.Input);        // 항목코드
                param[1] = helper.CreateParameter("@InspName", I_InspName, SqlDbType.VarChar,   ParameterDirection.Input);        // 항목이름
                param[2] = helper.CreateParameter("@Maker",     I_Maker,    SqlDbType.VarChar,   ParameterDirection.Input);        // 등록자   
                param[3] = helper.CreateParameter("@Unit",     I_Unit,    SqlDbType.VarChar,   ParameterDirection.Input);        // 등록자   
                param[4] = helper.CreateParameter("@RS_CODE", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
                param[5] = helper.CreateParameter("@RS_MSG", SqlDbType.VarChar, ParameterDirection.Output, null, 200);

                helper.FillTable("USP_BM1500_POP_I2", CommandType.StoredProcedure, param);

                RS_CODE =  param[4].Value.ToString();
                RS_MSG  =  param[5].Value.ToString();

                if (RS_CODE == "S")
                {
                  helper.Transaction.Commit();
                }
              
               
            }
            catch (Exception)
            {
                helper.Transaction.Rollback();
               
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
        }
        #endregion  검사항목정보  팝업

        #region  검사항목  call
        public void TBM1500_POP(string sPlantCode, string sInspCase, string sInspType, string sInspCode, string sInspName, string sUseFlag,
                                TextBox Code_ID, TextBox Code_Name)
        {
            try
            {

                DtTemp = SEL_TBM1500(sPlantCode, "", "", sInspCode, sInspName, sUseFlag);

                if (DtTemp.Rows.Count == 1)
                {
                    Code_ID.Text = Convert.ToString(DtTemp.Rows[0]["InspCode"]);
                    Code_Name.Text = Convert.ToString(DtTemp.Rows[0]["InspName"]);
                }
                else
                {
                    Code_ID.Text = string.Empty;
                    Code_Name.Text = string.Empty;
                    // MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                    // 검사항목  팝업 창 처리
                    PopUpManagerEX pu = new PopUpManagerEX();
                    DtTemp = pu.OpenPopUp("TBM1500", new string[] { sPlantCode, sInspType, sInspCode, sInspName, sUseFlag }); // 검사항목 정보 창 Parameter(검사항목코드, 검사항목명)

                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        Code_ID.Text = Convert.ToString(DtTemp.Rows[0]["InspCode"]);
                        Code_Name.Text = Convert.ToString(DtTemp.Rows[0]["InspName"]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }

        }
        #endregion    

        #region 검사항목 Grid
        /// <summary>
        /// 그리드에서 검사항목 팝업 데이타 가져오기
        /// </summary>
        /// <param name="sInspCase">검사구분</param> 
        /// <param name="sInspType">검사대상</param> 
        /// <param name="sInspCode">검사항목코드</param>   
        /// <param name="sInspName">검사항목</param>  
        /// <param name="sUseFlag ">사용여부</param>
        /// <param name="Grid">대상 그리드</param>
        /// <param name="Column1">리턴 품목 코드(그리드 해당 컬럼 명)</param>
        /// <param name="Column2">리턴 품목 명(그리드 해당 컬럼 명)</param>
        public void TBM1500_POP_Grid(string sPlantCode, string sInspCase, string sInspType, string sInspCode, string sInspName, string sUseFlag,
                                     UltraGrid Grid, string Column1, string Column2 , string[] sParam)
        {

            try
            {

                DtTemp = SEL_TBM1500(sPlantCode, sInspCase, sInspType, sInspCode, sInspName, sUseFlag);

                if (DtTemp.Rows.Count == 1)
                {
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = Convert.ToString(DtTemp.Rows[0]["InspCode"]);
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = Convert.ToString(DtTemp.Rows[0]["InspName"]);

                    if (sParam != null)
                    {
                        foreach (string s in sParam)
                        {
                            string[] sA = s.Split('|');

                            if (sA.Length == 2)
                            {
                                Grid.Rows[Grid.ActiveRow.Index].Cells[sA[0]].Value = SqlDBHelper.nvlString(DtTemp.Rows[0][sA[1]]);
                            }
                            else
                            {
                                Grid.Rows[Grid.ActiveRow.Index].Cells[s].Value = SqlDBHelper.nvlString(DtTemp.Rows[0][s]);
                            }
                        }
                    }
                }
                else
                {
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = string.Empty;
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = string.Empty;
                    //MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                    // 품목 POP-UP 창 처리
                    PopUpManagerEX pu = new PopUpManagerEX();
                    DtTemp = pu.OpenPopUp("TBM1500", new string[] { sInspCase, sInspType, sInspCode, sInspName, sUseFlag }); // 품목 조회 POP-UP창 Parameter(비가동코드, 비가동명, 비가동그룹)

                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = Convert.ToString(DtTemp.Rows[0]["InspCode"]);
                        Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = Convert.ToString(DtTemp.Rows[0]["InspName"]);
                    }

                    if (sParam != null)
                    {
                        foreach (string s in sParam)
                        {
                            string[] sA = s.Split('|');

                            if (sA.Length == 2)
                            {
                                Grid.Rows[Grid.ActiveRow.Index].Cells[sA[0]].Value = SqlDBHelper.nvlString(DtTemp.Rows[0][sA[1]]);
                            }
                            else
                            {
                                Grid.Rows[Grid.ActiveRow.Index].Cells[s].Value = SqlDBHelper.nvlString(DtTemp.Rows[0][s]);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
        #endregion

        #region 금형 팝업
        /// <summary>
        /// 품목정보 팝업
        /// </summary>
        /// <param name="sPlantCD">공장</param>
        /// <param name="sItemCD">품목</param>
        /// <param name="sItemNM">품목명</param>
        /// <param name="sItemType">품목 유형</param>
        /// <param name="RS_CODE">리턴 코드</param>
        /// <param name="RS_MSG">리턴 메시지</param>
        /// <returns></returns>
        public DataTable SEL_TBM1600(string sPlantCD, string sMoldCode, string sMoldName, string sMoldType)
        {
            return SEL_TBM1600(sPlantCD, sMoldCode, sMoldName, "", "");
        }
        public DataTable SEL_TBM1600(string sPlantCD, string sMoldCode, string sMoldName, string sCarType, string sItemName)
        {
            SqlDBHelper helper = new SqlDBHelper(false);
            SqlParameter[] param = new SqlParameter[5];

            try
            {
                param[0] = helper.CreateParameter("@PlantCode", sPlantCD, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@MoldCode", sMoldCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@MoldName", sMoldName, SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("@CarType", sCarType, SqlDbType.VarChar, ParameterDirection.Input);
                param[4] = helper.CreateParameter("@ItemName", sItemName, SqlDbType.VarChar, ParameterDirection.Input);

                // param[4].Direction = System.Data.ParameterDirection.Output;
                //param[5].Direction = System.Data.ParameterDirection.Output;

                rtnDtTemp = helper.FillTable("USP_BM1600_POP", CommandType.StoredProcedure, param);


                return rtnDtTemp;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return new DataTable();
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
        }
        #endregion 금형 팝업
        #region 금형 Call
        /// <summary>
        /// 품목 팝업 데이타 가져오기
        /// </summary>
        /// <param name="ITEM_CD">품목</param>
        /// <param name="ITEM_NAME">품목명</param>
        /// <param name="PLANT_CD">사업장</param>
        /// <param name="ITEM_TYPE">품목 타입</param>
        /// <param name="code_id">Return TextBox 컨트롤 이름(CODE_ID)</param>
        /// <param name="code_nm">Return TextBox 컨트롤 이름(CODE_NAME)</param>
        public void TBM1600_POP(string sMoldCode, string sMoldName, string sPlantCode, string sMoldType, TextBox code_id, TextBox code_nm)
        {

            try
            {

                DtTemp = SEL_TBM1600(sPlantCode, sMoldCode, sMoldName, sMoldType);

                if (DtTemp.Rows.Count == 1)
                {
                    code_id.Text = Convert.ToString(DtTemp.Rows[0]["MoldCode"]);
                    code_nm.Text = Convert.ToString(DtTemp.Rows[0]["MoldName"]);
                }
                else
                {
                    if (DtTemp.Rows.Count == 0)
                    {
                        if (sMoldCode.Trim() != "" || sMoldName.Trim() != "")
                        {
                            code_id.Text = string.Empty;
                            code_nm.Text = string.Empty;
                          //  MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                        }
                    }

                    // 품목 POP-UP 창 처리
                    PopUpManagerEX pu = new PopUpManagerEX();
                    DtTemp = pu.OpenPopUp("TBM1600", new string[] { sPlantCode, sMoldType, sMoldCode, sMoldName }); // 품목 조회 POP-UP창 Parameter(비가동코드, 비가동명, 비가동그룹)

                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        code_id.Text = Convert.ToString(DtTemp.Rows[0]["MoldCode"]);
                        code_nm.Text = Convert.ToString(DtTemp.Rows[0]["MoldName"]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
        #endregion

        #region 품목 Grid
        /// <summary>
        /// 품목 팝업 데이타 가져오기
        /// </summary>
        /// <param name="ITEM_CD">품목</param>
        /// <param name="ITEM_NAME">품목명</param>
        /// <param name="PLANT_CD">사업장</param>
        /// <param name="ITEM_TYPE">품목 타입</param>
        /// <param name="Grid">대상 그리드</param>
        /// <param name="Column1">리턴 품목 코드(그리드 해당 컬럼 명)</param>
        /// <param name="Column2">리턴 품목 명(그리드 해당 컬럼 명)</param>
        public void TBM1600_POP_Grid(string sMoldCode, string sMoldName, string sPlantCode, string sMoldType, UltraGrid Grid, string Column1, string Column2)
        {
            try
            {
                DtTemp = SEL_TBM1600(sPlantCode, sMoldCode, sMoldName, sMoldType);

                if (DtTemp.Rows.Count == 1)
                {
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = Convert.ToString(DtTemp.Rows[0]["MoldCode"]);
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = Convert.ToString(DtTemp.Rows[0]["MoldName"]);
                }
                else
                {
                    if (DtTemp.Rows.Count == 0)
                    {
                        if (sMoldCode.Trim() != "" || sMoldName.Trim() != "")
                        {
                            Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = string.Empty;
                            Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = string.Empty;
                           // MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                        }
                    }
                    // 품목 POP-UP 창 처리
                    PopUpManagerEX pu = new PopUpManagerEX();
                    DtTemp = pu.OpenPopUp("TBM1600", new string[] { sPlantCode, sMoldType, sMoldCode, sMoldName }); // 품목 조회 POP-UP창 Parameter(비가동코드, 비가동명, 비가동그룹)

                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = Convert.ToString(DtTemp.Rows[0]["MoldCode"]);
                        Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = Convert.ToString(DtTemp.Rows[0]["MoldName"]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
        #endregion

        #region 고장항목  팝업
        /// <summary>
        /// 고장항목  정보  팝업
        /// </summary>
        /// <param name="sFaultType">고장유형</param> 
        /// <param name="sFaultCode">고장코드</param> 
        /// <param name="sFaultName">고장명</param> 
        /// <param name="sUseFlag">사용여부</param>          
        /// <param name="RS_CODE">리턴 코드</param>
        /// <param name="RS_MSG">리턴 메시지</param>
        /// <returns></returns>



        public DataTable SEL_TBM3400(string sFaultType, string sFaultCode, string sFaultName, string sUseFlag)
        {
            SqlDBHelper helper = new SqlDBHelper(false);
            SqlParameter[] param = new SqlParameter[4];

            try
            {
               param[0] = helper.CreateParameter("@FaultType", sFaultType, SqlDbType.VarChar, ParameterDirection.Input);    // 고장유형
               param[1] = helper.CreateParameter("@FaultCode", sFaultCode, SqlDbType.VarChar, ParameterDirection.Input);    // 고장코드
               param[2] = helper.CreateParameter("@FaultName", sFaultName, SqlDbType.VarChar, ParameterDirection.Input);     // 고장명
               param[3] = helper.CreateParameter("@UseFlag  ", sUseFlag  , SqlDbType.VarChar, ParameterDirection.Input);    // 사용여부  
  

                rtnDtTemp = helper.FillTable("USP_BM3400_POP", CommandType.StoredProcedure, param);


                return rtnDtTemp;
            }
            catch (Exception )
            {

                return new DataTable();
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
        }
        #endregion  고장항목정보  팝업
        #region  고장항목  call
        public void TBM3400_POP(string sFaultType, string sFaultCode, string sFaultName, string sUseFlag,
                                TextBox Code_ID, TextBox Code_Name)
        {
            try
            {

                DtTemp = SEL_TBM3400(sFaultType, sFaultCode, sFaultName, sUseFlag);

                if (DtTemp.Rows.Count == 1)
                {
                    Code_ID.Text = Convert.ToString(DtTemp.Rows[0]["FaultCode"]);
                    Code_Name.Text = Convert.ToString(DtTemp.Rows[0]["FaultName"]);
                }
                else
                {
                    Code_ID.Text = string.Empty;
                    Code_Name.Text = string.Empty;
                    // MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                    // 고장항목  팝업 창 처리
                    PopUpManagerEX pu = new PopUpManagerEX();
                    DtTemp = pu.OpenPopUp("TBM3400", new string[] { sFaultType, sFaultCode, sFaultName, sUseFlag }); // 고장항목 정보 창 Parameter(고장항목코드, 고장항목명)

                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        Code_ID.Text = Convert.ToString(DtTemp.Rows[0]["FaultCode"]);
                        Code_Name.Text = Convert.ToString(DtTemp.Rows[0]["FaultName"]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }

        }
        #endregion            
 
        #region 운행차량  팝업
        /// <summary>
        /// 운행차량  정보  팝업
        /// </summary>
        /// <param name="sCarGubun">구분</param>
        /// <param name="sCarNo">차량번호</param>
        /// <param name="sCarDesc">차량내역</param>        
        /// <param name="sUseFlag">사용여부</param>          
        /// <param name="RS_CODE">리턴 코드</param>
        /// <param name="RS_MSG">리턴 메시지</param>
        /// <returns></returns>



        public DataTable SEL_TBM3700(string sCarGubun, string sCarNo, string sCarDesc, string sUseFlag)
        {
            SqlDBHelper helper = new SqlDBHelper(false);
            SqlParameter[] param = new SqlParameter[4];

            try
            {
                param[0] = helper.CreateParameter("@CarGubun", sCarGubun, SqlDbType.VarChar, ParameterDirection.Input);    // 차량구분
                param[1] = helper.CreateParameter("@CarNo", sCarNo, SqlDbType.VarChar, ParameterDirection.Input);          // 차량번호
                param[2] = helper.CreateParameter("@CarDesc", sCarDesc, SqlDbType.VarChar, ParameterDirection.Input);      // 차량내경 
                param[3] = helper.CreateParameter("@UseFlag  ", sUseFlag, SqlDbType.VarChar, ParameterDirection.Input);  // 사용여부  

                rtnDtTemp = helper.FillTable("USP_BM3700_POP", CommandType.StoredProcedure, param);

                return rtnDtTemp;
            }
            catch (Exception)
            {

                return new DataTable();
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
        }
        #endregion  운행차량정보  팝업
        #region  운행차량  call
        public void TBM3700_POP(string sCarGubun, string sCarNo, string sCarDesc, string sUseFlag,
                                TextBox Code_ID, TextBox Code_Name)
        {
            try
            {

                DtTemp = SEL_TBM3700(sCarGubun, sCarNo, sCarDesc, sUseFlag);


                if (DtTemp.Rows.Count == 1)
                {
                    Code_ID.Text = Convert.ToString(DtTemp.Rows[0]["CarNo"]);
                    Code_Name.Text = Convert.ToString(DtTemp.Rows[0]["CarDesc"]);
                }
                else
                {
                    Code_ID.Text = string.Empty;
                    Code_Name.Text = string.Empty;
                    //MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                    // 운행차량  팝업 창 처리
                    PopUpManagerEX pu = new PopUpManagerEX();
                    DtTemp = pu.OpenPopUp("TBM3700", new string[] { sCarGubun, sCarNo, sCarDesc, sUseFlag }); // 운행차량 정보 창 Parameter(운행차량코드, 운행차량명)

                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        Code_ID.Text = Convert.ToString(DtTemp.Rows[0]["CarNo"]);
                        Code_Name.Text = Convert.ToString(DtTemp.Rows[0]["CarDesc"]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }

        }
        #endregion     

        #region 사유항목  팝업
        /// <summary>
        /// 사유항목  정보  팝업
        /// </summary>
        /// <param name="sResType">구분</param>
        /// <param name="sResCode">사유코드</param>
        /// <param name="sResName">사유명</param>        
        /// <param name="sUseFlag">사용여부</param>          
        /// <param name="RS_CODE">리턴 코드</param>
        /// <param name="RS_MSG">리턴 메시지</param>
        /// <returns></returns>



        public DataTable SEL_TBM4100(string sResType, string sResCode, string sResName, string sUseFlag)
        {
            SqlDBHelper helper = new SqlDBHelper(false);
            SqlParameter[] param = new SqlParameter[4];

            try
            {

                param[0] = helper.CreateParameter("@ResType", sResType, SqlDbType.VarChar, ParameterDirection.Input);    // 사유구분
                param[1] = helper.CreateParameter("@ResCode", sResCode, SqlDbType.VarChar, ParameterDirection.Input);    // 사유코드
                param[2] = helper.CreateParameter("@ResName", sResName, SqlDbType.VarChar, ParameterDirection.Input);    // 사유명
                param[3] = helper.CreateParameter("@UseFlag", sUseFlag, SqlDbType.VarChar, ParameterDirection.Input);    // 사용여부  

                rtnDtTemp = helper.FillTable("USP_BM4100_POP", CommandType.StoredProcedure, param);

                return rtnDtTemp;
            }
            catch (Exception)
            {

                return new DataTable();
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
        }
        #endregion  사유항목정보  팝업

        #region  사유항목  call
        public void TBM4100_POP(string sResType, string sResCode, string sResName, string sUseFlag,
                                TextBox Code_ID, TextBox Code_Name)
        {
            try
            {

                DtTemp = SEL_TBM4100(sResType, sResCode, sResName, sUseFlag);


                if (DtTemp.Rows.Count == 1)
                {
                    Code_ID.Text = Convert.ToString(DtTemp.Rows[0]["ResCode"]);
                    Code_Name.Text = Convert.ToString(DtTemp.Rows[0]["ResName"]);
                }
                else
                {
                    Code_ID.Text = string.Empty;
                    Code_Name.Text = string.Empty;
                    // MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                    // 사유항목  팝업 창 처리
                    PopUpManagerEX pu = new PopUpManagerEX();
                    DtTemp = pu.OpenPopUp("TBM4100", new string[] { sResType, sResCode, sResName, sUseFlag }); // 사유항목 정보 창 Parameter(사유항목코드, 사유항목명)

                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        Code_ID.Text = Convert.ToString(DtTemp.Rows[0]["ResCode"]);
                        Code_Name.Text = Convert.ToString(DtTemp.Rows[0]["ResName"]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }

        }
        #endregion     

        #region 관리 규격 팝업
        /// <summary>
        /// 관리 규격
        /// </summary>
        /// <param name="sPlantCD">공장</param>
        /// <param name="sItemCD">품목</param>
        /// <param name="sItemNM">품목명</param>
        /// <param name="sItemType">품목 유형</param>
        /// <param name="RS_CODE">리턴 코드</param>
        /// <param name="RS_MSG">리턴 메시지</param>
        /// <returns></returns>
        public DataTable SEL_TBM5100(string sItemCD, string sItemNM)
        {
            SqlDBHelper helper = new SqlDBHelper(false);
            SqlParameter[] param = new SqlParameter[2];

            try
            {
                param[0] = helper.CreateParameter("@ITEMCODE",  sItemCD,   SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@ITEMNAME",  sItemNM,   SqlDbType.VarChar, ParameterDirection.Input);
                

               // param[4].Direction = System.Data.ParameterDirection.Output;
                //param[5].Direction = System.Data.ParameterDirection.Output;

                rtnDtTemp = helper.FillTable("USP_BM5100_POP", CommandType.StoredProcedure, param);


                return rtnDtTemp;
            }
            catch (Exception)
            {
            
                return new DataTable();
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
        }
        #endregion 관리규격 팝업

        #region 관리 규격 Grid
        /// <summary>
        /// 관리 규격 데이타 가져오기
        /// </summary>
        /// <param name="ITEM_CD">품목</param>
        /// <param name="ITEM_NAME">품목명</param>
        /// <param name="PLANT_CD">사업장</param>
        /// <param name="ITEM_TYPE">품목 타입</param>
        /// <param name="Grid">대상 그리드</param>
        /// <param name="Column1">리턴 품목 코드(그리드 해당 컬럼 명)</param>
        /// <param name="Column2">리턴 품목 명(그리드 해당 컬럼 명)</param>
        public void TBM5100_POP_Grid(string ITEM_CD, string ITEM_NAME , UltraGrid Grid, string Column1, string Column2 )
        {

            try
            {

                DtTemp = SEL_TBM5100(ITEM_CD, ITEM_NAME);


                if (DtTemp.Rows.Count == 1)
                {
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = Convert.ToString(DtTemp.Rows[0]["CODE_ID"]);
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = Convert.ToString(DtTemp.Rows[0]["CODE_NAME"]);
                }
                else
                {
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = string.Empty;
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = string.Empty;
                    // MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                    // 품목 POP-UP 창 처리
                    PopUpManagerEX pu = new PopUpManagerEX();
                    DtTemp = pu.OpenPopUp("TBM5100", new string[] { ITEM_CD, ITEM_NAME }); // 품목 조회 POP-UP창 Parameter(비가동코드, 비가동명, 비가동그룹)

                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = Convert.ToString(DtTemp.Rows[0]["CODE_ID"]);
                        Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = Convert.ToString(DtTemp.Rows[0]["CODE_NAME"]);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
        #endregion

        #region 특성 규격 팝업
        /// <summary>
        /// 특성 
        /// </summary>
        /// <param name="sPlantCD">공장</param>
        /// <param name="sItemCD">품목</param>
        /// <param name="sItemNM">품목명</param>
        /// <param name="sItemType">품목 유형</param>
        /// <param name="RS_CODE">리턴 코드</param>
        /// <param name="RS_MSG">리턴 메시지</param>
        /// <returns></returns>
        public DataTable SEL_TBM5200(string sItemCD, string sItemNM,string sMAJOR)
        {
            SqlDBHelper helper = new SqlDBHelper(false);
            SqlParameter[] param = new SqlParameter[3];

            try
            {
                param[0] = helper.CreateParameter("@ITEMCODE",  sItemCD,   SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@ITEMNAME",  sItemNM,   SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@MAJOR"   ,  sMAJOR,   SqlDbType.VarChar, ParameterDirection.Input);
                

               // param[4].Direction = System.Data.ParameterDirection.Output;
                //param[5].Direction = System.Data.ParameterDirection.Output;

                rtnDtTemp = helper.FillTable("USP_BM5200_POP", CommandType.StoredProcedure, param);


                return rtnDtTemp;
            }
            catch (Exception)
            {
            
                return new DataTable();
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
        }
        #endregion 관리규격 팝업

        #region 특성 규격 Grid
        /// <summary>
        /// 특성 데이타 가져오기
        /// </summary>
        /// <param name="ITEM_CD">품목</param>
        /// <param name="ITEM_NAME">품목명</param>
        /// <param name="Grid">대상 그리드</param>
        /// <param name="Column1">리턴 품목 코드(그리드 해당 컬럼 명)</param>
        /// <param name="Column2">리턴 품목 명(그리드 해당 컬럼 명)</param>
        public void TBM5200_POP_Grid(string ITEM_CD, string ITEM_NAME, UltraGrid Grid, string Column1, string Column2 )
        {

            try
            {

                DtTemp = SEL_TBM5200(ITEM_CD, ITEM_NAME, Column1);


                if (DtTemp.Rows.Count == 1)
                {
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = Convert.ToString(DtTemp.Rows[0]["CODE_ID"]);
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = Convert.ToString(DtTemp.Rows[0]["CODE_NAME"]);
                }
                else
                {
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = string.Empty;
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = string.Empty;
                    // MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                    // 품목 POP-UP 창 처리
                    PopUpManagerEX pu = new PopUpManagerEX();
                    DtTemp = pu.OpenPopUp("TBM5200", new string[] { ITEM_CD, ITEM_NAME, Column1 }); //  POP-UP창 Parameter(비가동코드, 비가동명, 비가동그룹)

                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = Convert.ToString(DtTemp.Rows[0]["CODE_ID"]);
                        Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = Convert.ToString(DtTemp.Rows[0]["CODE_NAME"]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
        #endregion

        #region 코드마스터
        public DataTable SEL_TBM0000(string sCode, string sName, string major)
        {
            SqlDBHelper helper = new SqlDBHelper(false);
            SqlParameter[] param = new SqlParameter[3];

            try
            {
                param[0] = helper.CreateParameter("@ITEMCODE", sCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@ITEMNAME", sName, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@MAJOR", major, SqlDbType.VarChar, ParameterDirection.Input);

                rtnDtTemp = helper.FillTable("USP_BM0000_POP", CommandType.StoredProcedure, param);

                return rtnDtTemp;
            }
            catch (Exception)
            {
                return new DataTable();
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
        }

        public void TBM0000_POP(string sCode, string sName, string major, TextBox codeId, TextBox codeName
                                , string sCodeColumnCaption = "", string sNameColumnCaption = "")
        {
            try
            {
                DtTemp = SEL_TBM0000(sCode, sName, major);

                if (DtTemp.Rows.Count == 1)
                {
                    codeId.Text = Convert.ToString(DtTemp.Rows[0]["CODE_ID"]);
                    codeName.Text = Convert.ToString(DtTemp.Rows[0]["CODE_NAME"]);
                }
                else
                {
                    codeId.Text = string.Empty;
                    codeName.Text = string.Empty;
                    //  MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                    // 작업자 POP-UP 창 처리
                    PopUpManagerEX pu = new PopUpManagerEX();
                    DtTemp = pu.OpenPopUp("TBM0000", new string[] { sCode, sName, major, sCodeColumnCaption, sNameColumnCaption });

                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        codeId.Text = Convert.ToString(DtTemp.Rows[0]["CODE_ID"]);
                        codeName.Text = Convert.ToString(DtTemp.Rows[0]["CODE_NAME"]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }

        public void TBM0000_POP_Grid(string sCode, string sName, string major, UltraGrid Grid, string Column1, string Column2)
        {
            try
            {
                DtTemp = SEL_TBM0000(sCode, sName, major);


                if (DtTemp.Rows.Count == 1)
                {
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = Convert.ToString(DtTemp.Rows[0]["CODE_ID"]);
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = Convert.ToString(DtTemp.Rows[0]["CODE_NAME"]);
                }
                else
                {
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = string.Empty;
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = string.Empty;
                    // MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                    // 품목 POP-UP 창 처리
                    PopUpManagerEX pu = new PopUpManagerEX();

                    string sCodeColumnCaption = Grid.DisplayLayout.Bands[0].Columns[Column1].Header.Caption;
                    string sNameColumnCaption = Grid.DisplayLayout.Bands[0].Columns[Column2].Header.Caption;

                    DtTemp = pu.OpenPopUp("TBM0000", new string[] { sCode, sName, major, sCodeColumnCaption, sNameColumnCaption });

                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = Convert.ToString(DtTemp.Rows[0]["CODE_ID"]);
                        Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = Convert.ToString(DtTemp.Rows[0]["CODE_NAME"]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }

        public DataTable SEL_TMO0000(string sCode, string sName)
        {
            SqlDBHelper helper = new SqlDBHelper(false);
            SqlParameter[] param = new SqlParameter[2];

            try
            {
                param[0] = helper.CreateParameter("@ImageSeq", sCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@ImageName", sName, SqlDbType.VarChar, ParameterDirection.Input);

                rtnDtTemp = helper.FillTable("USP_MO0000_POP", CommandType.StoredProcedure, param);

                return rtnDtTemp;
            }
            catch (Exception)
            {
                return new DataTable();
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
        }

        public void TMO0000_POP_Grid(string sCode, string sName, UltraGrid Grid, string Column1, string Column2)
        {
            try
            {
                DtTemp = SEL_TMO0000(sCode, sName);


                if (DtTemp.Rows.Count == 1)
                {
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = Convert.ToString(DtTemp.Rows[0]["ImageSeq"]);
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = Convert.ToString(DtTemp.Rows[0]["ImageName"]);
                }
                else
                {
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = string.Empty;
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = string.Empty;
                    // MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                    // 품목 POP-UP 창 처리
                    PopUpManagerEX pu = new PopUpManagerEX();

                    string sCodeColumnCaption = Grid.DisplayLayout.Bands[0].Columns[Column1].Header.Caption;
                    string sNameColumnCaption = Grid.DisplayLayout.Bands[0].Columns[Column2].Header.Caption;

                    DtTemp = pu.OpenPopUp("TMO0000", new string[] { sCode, sName, sCodeColumnCaption, sNameColumnCaption });

                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = Convert.ToString(DtTemp.Rows[0]["ImageSeq"]);
                        Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = Convert.ToString(DtTemp.Rows[0]["ImageName"]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }

        public DataTable SEL_TCM0200(string sCode, string sName, string searchflag)
        {
            SqlDBHelper helper = new SqlDBHelper(false);
            SqlParameter[] param = new SqlParameter[3];

            try
            {
                param[0] = helper.CreateParameter("@CODE", sCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@NAME", sName, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@searchflag", searchflag, SqlDbType.VarChar, ParameterDirection.Input);

                rtnDtTemp = helper.FillTable("USP_CM0200_POP", CommandType.StoredProcedure, param);

                return rtnDtTemp;
            }
            catch (Exception)
            {
                return new DataTable();
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
        }
        public void TCM0200_POP(string sCode, string sName, string major, TextBox codeId, TextBox codeName,string title
                               , string sCodeColumnCaption = "", string sNameColumnCaption = "")
        {
            try
            {
                DtTemp = SEL_TCM0200(sCode, sName, major);

                if (DtTemp.Rows.Count == 1)
                {
                    codeId.Text = Convert.ToString(DtTemp.Rows[0]["CODE_ID"]);
                    codeName.Text = Convert.ToString(DtTemp.Rows[0]["CODE_NAME"]);
                }
                else
                {
                    codeId.Text = string.Empty;
                    codeName.Text = string.Empty;
                    //MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                    // 작업자 POP-UP 창 처리
                    PopUpManagerEX pu = new PopUpManagerEX();
                    DtTemp = pu.OpenPopUp("TCM0200", new string[] { sCode, sName, major, sCodeColumnCaption, sNameColumnCaption, title });

                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        codeId.Text = Convert.ToString(DtTemp.Rows[0]["CODE_ID"]);
                        codeName.Text = Convert.ToString(DtTemp.Rows[0]["CODE_NAME"]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
        public void TCM0200_POP_Grid(string sCode, string sName, string div, UltraGrid Grid, string Column1, string Column2,string title)
        {
            try
            {
                //DtTemp = SEL_TBM0000(sCode, sName, div);

                //if (DtTemp.Rows.Count > 1)
                //{
                // 품목 POP-UP 창 처리
                PopUpManagerEX pu = new PopUpManagerEX();

                string sCodeColumnCaption = Grid.DisplayLayout.Bands[0].Columns[Column1].Header.Caption;
                string sNameColumnCaption = Grid.DisplayLayout.Bands[0].Columns[Column2].Header.Caption;

                DtTemp = pu.OpenPopUp("TCM0200", new string[] { sCode, sName, div, sCodeColumnCaption, sNameColumnCaption, title });

                if (DtTemp != null && DtTemp.Rows.Count > 0)
                {
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = Convert.ToString(DtTemp.Rows[0]["CODE"]);
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = Convert.ToString(DtTemp.Rows[0]["NAME"]);
                }
                //}
                //else
                //{
                //    if (DtTemp.Rows.Count == 1)
                //    {
                //        Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = Convert.ToString(DtTemp.Rows[0]["CODE_ID"]);
                //        Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = Convert.ToString(DtTemp.Rows[0]["CODE_NAME"]);
                //    }
                //    else
                //    {
                //        Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = string.Empty;
                //        Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = string.Empty;
                //        MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                //    }
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }

        #endregion

        #region Tool마스터
        //
        public DataTable SEL_TTO0100(string sCode, string sName, string sPlantCode, string sSeq, string sEquip, string sUseInfo)
        {
            SqlDBHelper helper = new SqlDBHelper(false);
            SqlParameter[] param = new SqlParameter[9];

            try
            {
                param[0] = helper.CreateParameter("@pPlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@pToolCode", sCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@pToolName", sName, SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("@pEquip", sEquip, SqlDbType.VarChar, ParameterDirection.Input);
                param[4] = helper.CreateParameter("@pUseInfo", sUseInfo, SqlDbType.VarChar, ParameterDirection.Input);
                param[5] = helper.CreateParameter("@pSeq", sSeq, SqlDbType.VarChar, ParameterDirection.Input);
                param[6] = helper.CreateParameter("@pSeq1", "", SqlDbType.VarChar, ParameterDirection.Input);
                param[7] = helper.CreateParameter("@pSeq2", "", SqlDbType.VarChar, ParameterDirection.Input);
                param[8] = helper.CreateParameter("@pSeq3", "", SqlDbType.VarChar, ParameterDirection.Input);

                rtnDtTemp = helper.FillTable("USP_TO0100_POP", CommandType.StoredProcedure, param);

                return rtnDtTemp;
            }
            catch (Exception)
            {
                return new DataTable();
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
        }

        public void TTO0100_POP(string sCode, string sName, string sPlantCode, string sSeq, string sEquip, string sUseInfo
                                , TextBox codeId, TextBox codeName)
        {
            try
            {
                DtTemp = SEL_TTO0100(sCode, sName, sPlantCode, sSeq, sEquip, sUseInfo);


                if (DtTemp.Rows.Count == 1)
                {
                    codeId.Text = Convert.ToString(DtTemp.Rows[0]["ToolCode"]);
                    codeName.Text = Convert.ToString(DtTemp.Rows[0]["ToolName"]);
                }
                else
                {
                    codeId.Text = string.Empty;
                    codeName.Text = string.Empty;
                    // MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                    // 작업자 POP-UP 창 처리
                    PopUpManagerEX pu = new PopUpManagerEX();
                    DtTemp = pu.OpenPopUp("TTO0100", new string[] { sCode, sName, sPlantCode, sSeq, sEquip, sUseInfo, "N" });

                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        codeId.Text = Convert.ToString(DtTemp.Rows[0]["ToolCode"]);
                        codeName.Text = Convert.ToString(DtTemp.Rows[0]["ToolName"]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }

        public void TTO0100_POP_Grid(string sCode, string sName, string sPlantCode, string sSeq, string sEquip, string sUseInfo
                    , UltraGrid Grid, string Column1, string Column2)
        {
            try
            {
                DtTemp = SEL_TTO0100(sCode, sName, sPlantCode, sSeq, sEquip, sUseInfo);


                if (DtTemp.Rows.Count == 1)
                {
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = Convert.ToString(DtTemp.Rows[0]["ToolCode"]);
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = Convert.ToString(DtTemp.Rows[0]["ToolName"]);
                }
                else
                {
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = string.Empty;
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = string.Empty;
                    //  MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                    // 품목 POP-UP 창 처리
                    PopUpManagerEX pu = new PopUpManagerEX();

                    string sCodeColumnCaption = Grid.DisplayLayout.Bands[0].Columns[Column1].Header.Caption;
                    string sNameColumnCaption = Grid.DisplayLayout.Bands[0].Columns[Column2].Header.Caption;

                    DtTemp = pu.OpenPopUp("TTO0100", new string[] { sCode, sName, sPlantCode, sSeq, sEquip, sUseInfo, "N" });

                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = Convert.ToString(DtTemp.Rows[0]["ToolCode"]);
                        Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = Convert.ToString(DtTemp.Rows[0]["ToolName"]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }

        /// <summary>
        /// 툴 팝업 창 조회 후 선택한 데이터를 해당 데이터로우, 입력한 컬럼에 삽입.
        /// 파라메터에 들어가는 컬럼 Key는 툴 팝업창에 있는 그리드의 컬럼 Key와 동일해야 한다.
        /// </summary>
        /// <param name="sCode">코드<</param>
        /// <param name="sName">이름</param>
        /// <param name="sPlantCode"> 사업장</param>
        /// <param name="sSeq">순번</param>
        /// <param name="sEquip"> 장착 여부</param>
        /// <param name="sUseInfo"> 사용 여부</param>
        /// <param name="sEquipEnable"> Y/N 으로 입력 Y일 경우 장착된 툴은 선택할 수 없다. </param>
        /// <param name="dr">데이터를 입력받을 데이터로우</param>
        /// <param name="Column1">코드 컬럼</param>
        /// <param name="Column2">이름 컬럼</param>
        /// <param name="sParam">그 외 나머지 데이터를 받을 컬럼
        /// ## ex )Seq, ProdQty, Shelflife, UseRate </param>
        public void TTO0100_POP_DataRow(string sCode, string sName
                , string sPlantCode, string sSeq, string sEquip, string sUseInfo
                , string sEquipEnable
                , UltraGridRow dr, string Column1, string Column2
                , string[] sParam)
        {
            try
            {
                DtTemp = SEL_TTO0100(sCode, sName, sPlantCode, sSeq, sEquip, sUseInfo);

               // if (DtTemp.Rows.Count > 1)
                {
                    // 품목 POP-UP 창 처리
                    PopUpManagerEX pu = new PopUpManagerEX();
                    DtTemp = pu.OpenPopUp("TTO0100", new string[] { sCode, sName, sPlantCode, sSeq, sEquip, sUseInfo, sEquipEnable });

                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        dr.Cells[Column1].Value = SqlDBHelper.nvlString(DtTemp.Rows[0]["ToolCode"]);
                        dr.Cells[Column2].Value = SqlDBHelper.nvlString(DtTemp.Rows[0]["ToolName"]);

                        if (sParam != null)
                        {
                            foreach (string s in sParam)
                            {
                                string[] sA = s.Split('|');

                                if (sA.Length == 2)
                                {
                                    dr.Cells[sA[0]].Value = SqlDBHelper.nvlString(DtTemp.Rows[0][sA[1]]);
                                }
                                else
                                {
                                    dr.Cells[s].Value = SqlDBHelper.nvlString(DtTemp.Rows[0][s]);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
        #endregion

        #region 사용자(TSY0200:WorkerID)
        /// <summary>
        /// 작업자 정보 팝업
        /// </summary>
        /// <param name="sPlantCode">공장(사업장)</param>
        /// <param name="sOPCode">공정코드</param>
        /// <param name="sLineCode">라인코드</param>
        /// <param name="sWorkCenterCode">작업장코드</param>
        /// <param name="sWorkerID">작업자 ID</param>
        /// <param name="sWorkerName">작업자명</param>
        /// <param name="sUseFlag">사용여부</param>
        /// <param name="RS_CODE">리턴 코드</param>
        /// <param name="RS_MSG">리턴 메시지</param>
        /// <returns></returns>
        /// 
        public DataTable SEL_TSY0200(string sPlantCode, string sOPCode, string sLineCode, string sWorkCenterCode, string sWorkerID, string sWorkerName, string sUseFlag)
        {
            SqlDBHelper helper = new SqlDBHelper(false);
            SqlParameter[] param = new SqlParameter[7];

            try
            {
                param[0] = helper.CreateParameter("@PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@OPCode", sOPCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@LineCode", sLineCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("@WorkCenterCode", sWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[4] = helper.CreateParameter("@WorkerID", sWorkerID, SqlDbType.VarChar, ParameterDirection.Input);
                param[5] = helper.CreateParameter("@WorkerName", sWorkerName, SqlDbType.VarChar, ParameterDirection.Input);
                param[6] = helper.CreateParameter("@UseFlag", sUseFlag, SqlDbType.VarChar, ParameterDirection.Input);

                rtnDtTemp = helper.FillTable("USP_SY0200_POP", CommandType.StoredProcedure, param);


                return rtnDtTemp;
            }
            catch (Exception)
            {
                return new DataTable();
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
        }
        /// <summary>
        /// 작업자 팝업 데이타 가져오기
        /// </summary>
        /// <param name="PlantCode">사업장</param>
        /// <param name="OPCode">공정코드</param>
        /// <param name="LineCode">라인코드</param>
        /// <param name="WorkCenterCode">작업장코드</param>
        /// <param name="WorkerID">작업자</param>
        /// <param name="WorkerName">작업자명</param>
        /// <param name="UseFlag">사용여부</param>
        /// <param name="Code_ID">Return TextBox 컨트롤 이름(CODE_ID)</param>
        /// <param name="Code_Name">Return TextBox 컨트롤 이름(CODE_NAME)</param>
        public void TSY0200_POP(string PlantCode, string OPCode, string LineCode, string WorkCenterCode, string WorkerID, string WorkerName, string UseFlag,
                                TextBox Code_ID, TextBox Code_Name)
        {
            try
            {
                DtTemp = SEL_TSY0200(PlantCode, OPCode, LineCode, WorkCenterCode, WorkerID, WorkerName, UseFlag);

                if (DtTemp.Rows.Count == 1)
                {
                    Code_ID.Text = Convert.ToString(DtTemp.Rows[0]["WorkerID"]);
                    Code_Name.Text = Convert.ToString(DtTemp.Rows[0]["WorkerName"]);
                }
                else
                {
                    Code_ID.Text = string.Empty;
                    Code_Name.Text = string.Empty;
                    //MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                    // 작업자 POP-UP 창 처리
                    PopUpManagerEX pu = new PopUpManagerEX();
                    DtTemp = pu.OpenPopUp("TSY0200", new string[] { PlantCode, OPCode, LineCode, WorkCenterCode, WorkerID, WorkerName, UseFlag }); // 작업  POP-UP창 Parameter(작업자 ID, 작업자명)

                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        Code_ID.Text = Convert.ToString(DtTemp.Rows[0]["WorkerID"]);
                        Code_Name.Text = Convert.ToString(DtTemp.Rows[0]["WorkerName"]);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }

        }
        /// <summary>
        /// 작업자 가져오기
        /// </summary>
        /// <param name="sPlantCode">공장(사업장)</param>
        /// <param name="sOPCode">공정코드</param>
        /// <param name="sLineCode">라인코드</param>
        /// <param name="sWorkCenterCode">작업장코드</param>
        /// <param name="sWorkerID">작업자 ID</param>
        /// <param name="sWorkerName">작업자명</param>
        /// <param name="sUseFlag">사용여부</param>
        /// <param name="Grid">대상 그리드</param>
        /// <param name="Column1">리턴 품목 코드(그리드 해당 컬럼 명)</param>
        /// <param name="Column2">리턴 품목 명(그리드 해당 컬럼 명)</param>

        public void TSY0200_POP_Grid(string sPlantCode, string sOPCode, string sLineCode, string sWorkCenterCode, string sWorkerID, string sWorkerName, string sUseFlag, UltraGrid Grid, string Column1, string Column2)
        {
            try
            {
                DtTemp = SEL_TSY0200(sPlantCode, sOPCode, sLineCode, sWorkCenterCode, sWorkerID, sWorkerName, sUseFlag);


                if (DtTemp.Rows.Count == 1)
                {
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = Convert.ToString(DtTemp.Rows[0]["WorkerID"]);
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = Convert.ToString(DtTemp.Rows[0]["WorkerName"]);
                }
                else
                {
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = string.Empty;
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = string.Empty;
                    // MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                    // 품목 POP-UP 창 처리
                    PopUpManagerEX pu = new PopUpManagerEX();
                    DtTemp = pu.OpenPopUp("TSY0200", new string[] { sPlantCode, sOPCode, sLineCode, sWorkCenterCode, sWorkerID, sWorkerName, sUseFlag }); // 작업자  POP-UP창 Parameter(작업자ID, 작업자명)

                    if (DtTemp != null && DtTemp.Rows.Count > 0)
                    {
                        Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = Convert.ToString(DtTemp.Rows[0]["WorkerID"]);
                        Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = Convert.ToString(DtTemp.Rows[0]["WorkerName"]);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
        #endregion

        #region 작업지시서 팝업
        /// <summary>
        /// 작업지시서 팝업
        /// </summary>
        /// <param name="sPlantCD">공장</param>
        /// <param name="sItemCD">품목</param>
        /// <param name="sItemNM">품목명</param>
        /// <param name="sItemType">품목 유형</param>
        /// <param name="RS_CODE">리턴 코드</param>
        /// <param name="RS_MSG">리턴 메시지</param>
        /// <returns></returns>
        public DataTable SEL_ORDERNO_HG(string sPLANTCODE, string sRECDATE, string sWORKCENTERCODE)
        {
           SqlDBHelper helper = new SqlDBHelper(true, false);
           SqlParameter[] param = new SqlParameter[3];

           try
           {
              param[0] = helper.CreateParameter("@PLANTCODE", sPLANTCODE, SqlDbType.VarChar, ParameterDirection.Input);
              param[1] = helper.CreateParameter("@RECDATE", sRECDATE, SqlDbType.VarChar, ParameterDirection.Input);
              param[2] = helper.CreateParameter("@WORKCENTER", sWORKCENTERCODE, SqlDbType.VarChar, ParameterDirection.Input);

              rtnDtTemp = helper.FillTable("USP_ORDERNO_POP", CommandType.StoredProcedure, param);

              return rtnDtTemp;
           }
           catch (Exception)
           {
              return new DataTable();
           }
           finally
           {
              if (helper._sConn != null) { helper._sConn.Close(); }
              if (param != null) { param = null; }
           }
        }
        #endregion 품목 팝업

        #region 품목 Grid
        /// <summary>
        /// 품목 팝업 데이타 가져오기
        /// </summary>
        /// <param name="ITEM_CD">품목</param>
        /// <param name="ITEM_NAME">품목명</param>
        /// <param name="sPlantCd">사업장</param>
        /// <param name="ITEM_TYPE">품목 타입</param>
        /// <param name="Grid">대상 그리드</param>
        /// <param name="Column1">리턴 품목 코드(그리드 해당 컬럼 명)</param>
        /// <param name="Column2">리턴 품목 명(그리드 해당 컬럼 명)</param>
        public void ORDERNO_HG_POP_Grid(string sRecDate, string sWorkCenterCode, string sPlantCd, UltraGrid Grid, string Column1, string Column2)
        {
           try
           {
              if (!string.IsNullOrEmpty(sRecDate) && !string.IsNullOrEmpty(sWorkCenterCode) && !string.IsNullOrEmpty(sPlantCd))
              {
                 PopUpManagerEX popUpManagerEX = new PopUpManagerEX();
                 DtTemp = popUpManagerEX.OpenPopUp("ORDERNO_HG", new string[] { sPlantCd, sRecDate, sWorkCenterCode });

                 if (DtTemp != null && DtTemp.Rows.Count > 0)
                 {
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = DtTemp.Rows[0]["PLANNO"].ToString();
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = DtTemp.Rows[0]["ITEMNAME"].ToString();
                 }
                 else
                 {
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column1].Value = string.Empty;
                    Grid.Rows[Grid.ActiveRow.Index].Cells[Column2].Value = string.Empty;
                 }
              }
           }
           catch (Exception ex)
           {
              MessageBox.Show(ex.Message, "ERROR");
           }
        }

        #endregion
    }
}
