using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SAMMI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;


namespace SAMMI.SY
{
    public partial class SY0400 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        #region <MEMBER AREA>
        public  bool            SaveCheck  = false;
        private Configuration   appConfig;
        private AutoResetEvent  eventflag  = new AutoResetEvent(false);
        private Database        db;
        private SqlConnection   conn;
        //private SqlTransaction  trans;
        private string          luServer                 = string.Empty;
        private string          luUserID                 = string.Empty;
        private string          luPassword               = string.Empty;
        private string          luPort                   = string.Empty;
        private string          luPath                   = string.Empty;
        public bool grid2_open = false;
        DataSet rtnDsTemp = new DataSet(); // return DataSet 공통
        DataTable rtnDtTemp = new DataTable(); // return DataTable 공통

        Common.Common _Common = new Common.Common();

        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();
        //internal class LiveUpdator : WebClient
        //{
        //    protected override WebRequest GetWebRequest(Uri address)
        //    {
        //        FtpWebRequest req = (FtpWebRequest)base.GetWebRequest(address);
        //        req.UsePassive = false;
        //        return req;
        //    }

        //    public string MakeDirectory(string uri)
        //    {
        //        try 
        //        {
        //            FtpWebRequest ftpReq   = WebRequest.Create(uri) as FtpWebRequest;
        //            ftpReq.Method          = WebRequestMethods.Ftp.MakeDirectory;
        //            ftpReq.Credentials     = this.Credentials;
        //            FtpWebResponse ftpResp = ftpReq.GetResponse() as FtpWebResponse;
        //            MessageBox.Show(ftpResp.StatusDescription.ToString());

        //            return uri;
        //        }    
        //        catch (Exception ex) 
        //        {
        //            return ex.Message;
        //        }
        //    }
        //}
        #endregion

        #region < CONSTRUCTOR >
        public SY0400()
        {
            InitializeComponent();

            appConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            this.db                           = DatabaseFactory.CreateDatabase();;
            this.conn                         = (SqlConnection)this.db.CreateConnection();
            this.daTable1.Connection          = conn;
            this.daTable2.Connection          = conn;
            this.daTable1.Adapter.RowUpdating += new SqlRowUpdatingEventHandler(Adapter_RowUpdating1);
            this.daTable1.Adapter.RowUpdated  += new SqlRowUpdatedEventHandler(Adapter_RowUpdated1);
            this.daTable2.Adapter.RowUpdating += new SqlRowUpdatingEventHandler(Adapter_RowUpdating2);
            this.daTable2.Adapter.RowUpdated  += new SqlRowUpdatedEventHandler(Adapter_RowUpdated2);

            // LIVE UPDATE 서버 정보 가지고 오기
            DataSet dsConfig = this.db.ExecuteDataSet(CommandType.Text, "SELECT * FROM TSY9000");
            if (dsConfig.Tables[0].Rows.Count > 0)
            {
                this.luServer   = dsConfig.Tables[0].Rows[0]["LUSERVER"].ToString();
                this.luUserID   = dsConfig.Tables[0].Rows[0]["LUUSERID"].ToString();
                this.luPassword = dsConfig.Tables[0].Rows[0]["LUPASSWORD"].ToString();
                this.luPath     = dsConfig.Tables[0].Rows[0]["LUPath"].ToString();
                this.luPort     = dsConfig.Tables[0].Rows[0]["LUPort"].ToString();
            }
            this.ds.dTable1.Clear();
            this.ds.dTable2.Clear();
        }

        private void SY0400_Load(object sender, EventArgs e)
        {
            rtnDtTemp = _Common.GET_TBM0000_CODE("SYSTEMID");     //
            SAMMI.Common.Common.FillComboboxMaster(this.cboSystemID_H, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");

        }
        #endregion

        #region <TOOL BAR AREA >
        /// <summary>
        /// ToolBar의 조회 버튼 클릭
        /// </summary>
        public override void DoInquire()
        {
            this.DoSave();

            base.DoInquire();

            string sSystemid = string.Empty;

            if (this.cboSystemID_H.SelectedValue != null)
                sSystemid = this.cboSystemID_H.SelectedValue.ToString() == "ALL" ? "" : this.cboSystemID_H.SelectedValue.ToString();

            this.ds.dTable1.Clear();
            this.ds.dTable2.Clear();

            this.daTable1.Fill(this.ds.dTable1, sSystemid);
                              
            if (this.ds.dTable1.Rows.Count == 0)
                this.ds.dTable2.Clear();
        }


        /// <summary>
        /// ToolBar의 신규 버튼 클릭
        /// </summary>
        public override void DoNew()
        {
            if (this.grid2.IsActivate)
            {
                this.grid2_open = true;
                if ((this.grid1.ActiveRow.Cells["SYSTEMID"].Value.ToString() == "") || (this.grid1.ActiveRow.Cells["VER"].Value.ToString() == ""))
                {
                    throw (new SException("E00001", null));
                }

                // 파일을 선택하기위한 DialogBox를 OPEN 한다.
                OpenFileDialog openfiledialog   = new OpenFileDialog();
                openfiledialog.InitialDirectory = Application.StartupPath;
                openfiledialog.Multiselect = true;

                if (openfiledialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }

                foreach (string sFileName in openfiledialog.FileNames)
                {
                    this.grid2.InsertRow();

                    this.grid2.ActiveRow.Cells["SYSTEMID"].Value = this.grid1.ActiveRow.Cells["SYSTEMID"].Value;
                    this.grid2.ActiveRow.Cells["ClientID"].Value = "*";
                    this.grid2.ActiveRow.Cells["VER"].Value = this.grid1.ActiveRow.Cells["VER"].Value;
                    this.grid2.ActiveRow.Cells["SPath"].Value = this.luPath + this.grid1.ActiveRow.Cells["SYSTEMID"].Value + "/" + this.grid1.ActiveRow.Cells["VER"].Value + "/";

                    FileInfo fileinfo = new FileInfo(sFileName);
                    this.grid2.ActiveRow.Cells["FileID"].Value = fileinfo.Name;
                    this.grid2.ActiveRow.Cells["FileSize"].Value = fileinfo.Length;
                    this.grid2.ActiveRow.Cells["CPath"].Value = this.db.ExecuteScalar(CommandType.Text
                                                                                        , "SELECT ISNULL(FilePath, '\') FROM TSY2400 WHERE SystemID = '" + this.grid1.ActiveRow.Cells["SYSTEMID"].Value + "'"
                                                                                          + " AND FileID  = '" + fileinfo.Name + "'");
                    if (this.grid2.ActiveRow.Cells["CPath"].Value.ToString() == "") this.grid2.ActiveRow.Cells["CPath"].Value = @"\";
                    this.grid2.ActiveRow.Cells["UploadPath"].Value = fileinfo.FullName;

                    this.grid2.ActiveRow.Cells["UploadPath"].Value = fileinfo.FullName;
                    this.grid2.ActiveRow.Cells["ProcGB"].Value = "COPY";

                    FileStream fs = new FileStream(fileinfo.FullName, FileMode.OpenOrCreate, FileAccess.Read);
                    byte[] fileimage = new byte[fs.Length];
                    fs.Read(fileimage, 0, System.Convert.ToInt32(fs.Length));
                    this.grid2.ActiveRow.Cells["FileImage"].Value = fileimage;
                    fs.Close();
                }
                
            }
            else
            {
                this.grid1.InsertRow();
            }
        }
        /// <summary>
        /// ToolBar의 삭제 버튼 Click
        /// </summary>
        public override void DoDelete()
        {
            base.DoDelete();

            if (this.grid2.IsActivate)
                this.grid2.DeleteRow();
            else
            {
                int grid2rowcount = this.grid2.Rows.Count;
                for (int i = 0; i < grid2rowcount; i++)
                    this.grid2.DeleteRow();

                this.grid1.DeleteRow();
            }
        }
        /// <summary>
        /// ToolBar의 저장 버튼 Click
        /// </summary>
        public override void DoSave()
        {
            try
            {
                this.grid1.UpdateData();
                this.grid2.UpdateData();

                if (this.ds.HasChanges())
                {
                    if (this.ShowDialog("C:Q00009") == System.Windows.Forms.DialogResult.Cancel)
                        return;
                }
                else return;

                base.DoSave();

                conn.Open();
                //trans = conn.BeginTransaction();
                //this.daTable1.Transaction = trans;
                //this.daTable2.Transaction = trans;

                this.daTable1.Adapter.Update(this.ds.dTable1);
                this.daTable2.Adapter.Update(this.ds.dTable2);

                //trans.Commit();
                conn.Close();
            }
            catch (SException ex)
            {
                //trans.Rollback();
                
                throw (ex);
            }
            catch (Exception ex)
            {
                //trans.Rollback();
                
                throw (ex);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }
        #endregion

        #region < EVENT AREA >
        /// <summary>
        /// DATABASE UPDATE전 VALIDATEION CHECK 및 값을 수정한다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Adapter_RowUpdating1(object sender, SqlRowUpdatingEventArgs e)
        {
           
            if (e.Row.RowState == DataRowState.Modified)
            {
                e.Command.Parameters["@Editor"].Value = this.WorkerID;
                return;
            }

            if (e.Row.RowState == DataRowState.Added)
            {
                e.Command.Parameters["@Maker"].Value  = this.WorkerID;
                e.Command.Parameters["@Editor"].Value = this.WorkerID;
                return;
            }
        }

        /// <summary>
        /// 저장처리시 오류가 발생한 경우 오류 메세지에 대한 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Adapter_RowUpdated1(object sender, SqlRowUpdatedEventArgs e)
        {
            if (e.Errors == null) return;

            switch (((SqlException)e.Errors).Number)
            {
                // 중복
                case 2627:
                     e.Row.RowError = "검사항목이 있습니다.";
                     throw (new SException("C:S00099", e.Errors));
                case 515:
                     e.Row.RowError = "부코드를 입력하세요.";
                     throw (new SException("E00002", e.Errors));
                default:
                    break;
            }
        }

        /// <summary>
        /// DATABASE UPDATE전 VALIDATEION CHECK 및 값을 수정한다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Adapter_RowUpdating2(object sender, SqlRowUpdatingEventArgs e)
        {
            //if ((e.Row.RowState == DataRowState.Added) || (e.Row.RowState == DataRowState.Modified))
            //{
            //    this.DoNewValidate2(e.Row);
            //}

            //if (e.Row.RowState == DataRowState.Modified)
            //{
            //    e.Command.Parameters["@Editor"].Value = this.WorkerID;
            //    return;
            //}

            //if (e.Row.RowState == DataRowState.Added)
            //{
            //    e.Command.Parameters["@Maker"].Value  = this.WorkerID;
            //    e.Command.Parameters["@Editor"].Value = this.WorkerID;
            //    return;
            //}
        }

        /// <summary>
        /// 저장처리시 오류가 발생한 경우 오류 메세지에 대한 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Adapter_RowUpdated2(object sender, SqlRowUpdatedEventArgs e)
        {
            if (e.Errors == null) return;

            switch (((SqlException)e.Errors).Number)
            {
                // 중복
                case 2627:
                     e.Row.RowError = "검사항목이 있습니다.";
                     throw (new SException("C:S00099", e.Errors));
                case 515:
                     e.Row.RowError = "부코드를 입력하세요.";
                     throw (new SException("C:E00002", e.Errors));
                default:
                    break;
            }
        }

        private void grid1_ClickCellButton(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            OpenFileDialog openfiledialog   = new OpenFileDialog();
            openfiledialog.InitialDirectory = Application.StartupPath;
            if (e.Cell.Row.Cells["FileID"].Value == null)
                openfiledialog.Filter = "ALL|*.*";
            else
            {
                openfiledialog.FileName = e.Cell.Row.Cells["FileID"].Value.ToString();
                openfiledialog.Filter   = "FILE|" + e.Cell.Row.Cells["FileID"].Value.ToString() + "|ALL|*.*";
            }

            if (openfiledialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FileInfo fileinfo = new FileInfo(openfiledialog.FileName);
                e.Cell.Row.Cells["FileID"].Value       = fileinfo.Name;
                e.Cell.Row.Cells["Uploader"].Value     = this.WorkerID;
                e.Cell.Row.Cells["FileSize"].Value     = fileinfo.Length;
                e.Cell.Row.Cells["ModifiedDate"].Value = fileinfo.LastWriteTime;
                e.Cell.Row.Cells["LocalFile"].Value    = fileinfo.FullName;
                e.Cell.Row.Cells["CreateDate"].Value   = fileinfo.CreationTime;
                return;
            };
        }

        private void grid1_AfterRowActivate(object sender, EventArgs e)
        {
            this.daTable2.Fill(this.ds.dTable2
                             , this.grid1.ActiveRow.Cells["SYSTEMID"].Value.ToString()
                             , this.grid1.ActiveRow.Cells["VER"].Value.ToString());
        }

        private void grid1_AfterRowInsert(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
        {
            e.Row.Cells["VerDate"].Value   = System.DateTime.Now;
            e.Row.Cells["RegUserID"].Value = this.WorkerID;
        }

        private void grid2_ClickCellButton(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            if (e.Cell.Row.Tag == null) return;
            if (e.Cell.Row.Tag.ToString() != "NEW") return;

            // 파일을 선택하기위한 DialogBox를 OPEN 한다.
            OpenFileDialog openfiledialog   = new OpenFileDialog();
            openfiledialog.InitialDirectory = Application.StartupPath;
            if (e.Cell.Row.Cells["FileID"].Value == null)
                openfiledialog.Filter = "ALL|*.*";
            else
            {
                openfiledialog.FileName = e.Cell.Row.Cells["FileID"].Value.ToString();
                openfiledialog.Filter   = "FILE|" + e.Cell.Row.Cells["FileID"].Value.ToString() + "|ALL|*.*";
            }

            if (openfiledialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FileInfo fileinfo = new FileInfo(openfiledialog.FileName);
                e.Cell.Row.Cells["FileID"].Value   = fileinfo.Name;
                e.Cell.Row.Cells["FileSize"].Value = fileinfo.Length;
                e.Cell.Row.Cells["CPath"].Value    = this.db.ExecuteScalar(CommandType.Text
                                                                     ,"SELECT ISNULL(FilePath, '\') FROM TSY0300 WHERE SystemID = '" + this.grid1.ActiveRow.Cells["SYSTEMID"].Value + "'"
                                                                     +" AND FileID  = '" + fileinfo.Name + "'");
                if (e.Cell.Row.Cells["CPath"].Value.ToString() == "") e.Cell.Row.Cells["CPath"].Value = @"\";
                e.Cell.Row.Cells["UploadPath"].Value = fileinfo.FullName;

                FileStream fs        = new FileStream(fileinfo.FullName, FileMode.OpenOrCreate, FileAccess.Read);
                byte[]     fileimage = new byte[fs.Length];
                fs.Read(fileimage, 0, System.Convert.ToInt32(fs.Length));
                e.Cell.Row.Cells["FileImage"].Value = fileimage;
                fs.Close();
                                                              
                return;
            }
        }
        #endregion

        #region <METHOD AREA>
        #endregion

        
    }
}
