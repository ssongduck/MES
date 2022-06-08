#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID        : ZZ0300
//   Form Name      : CHANGE PASSWORD
//   Name Space     : SmartMES
//   Created Date   : 2012.11.30
//   Made By        : SAMMI INFORMATION SYSTEM CO.,LTD
//   Description    : 비밀번호 변경
//   DB Table       : SY0300
//   StoreProcedure : USP_ZZ0300_I1
// *---------------------------------------------------------------------------------------------*
#endregion

#region <USING AREA>
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using System.Configuration;
using SAMMI.Common;
#endregion

#region < SmartMES >
namespace SmartMES
{
    public partial class ZZ0300 : Form
    {
        #region < FIELD >
        private System.Drawing.Point temp;
        private bool ismove = false;
        #endregion

        #region < CONSTRUCTOR >
        public ZZ0300()
        {
            InitializeComponent();

            txtID.Text = "";
        }

        public ZZ0300(string sWorkerID)
        {
            InitializeComponent();

            txtID.Text = sWorkerID.Trim();
        }
        #endregion

        #region < EVENT AREA >
           private void pnlSplash_MouseUp(object sender, MouseEventArgs e)
        {
            this.ismove = false;
        }

        private void pnlSplash_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.ismove)
            {
                System.Drawing.Point d = Control.MousePosition;
                d.X = d.X - temp.X;
                d.Y = d.Y - temp.Y;
                this.Location = d;
            }
        }

        private void pnlSplash_MouseDown(object sender, MouseEventArgs e)
        {
            this.ismove = true;

            temp.X = Control.MousePosition.X - this.Location.X;
            temp.Y = Control.MousePosition.Y - this.Location.Y;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SAMMI.Common.SqlDBHelper helper = new SAMMI.Common.SqlDBHelper(false);
            SqlParameter[] param = null;

            try
            {
                if ( txtPWDChk.Text.Trim() != txtPwdChg.Text.Trim())
                {
                    MessageBox.Show("변경할 비밀번호와 비밀번호 확인이 다릅니다.");
                    return;
                }
                param = new SqlParameter[3];
                param[0] = helper.CreateParameter("@pWorkerID", txtID.Text, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@pPWD", System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(txtPwdNow.Text, "MD5"), SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@pChgPWD", System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(txtPwdChg.Text, "MD5"), SqlDbType.VarChar, ParameterDirection.Input);

                DataTable dt = helper.FillTable("USP_ZZ0300_I1", CommandType.StoredProcedure, param);

                if (dt == null)
                {
                    helper.Transaction.Commit();

                    this.Close();
                }
                else
                {
                    MessageBox.Show(SqlDBHelper.nvlString(dt.Rows[0][0]));
                }
            }
            catch (Exception ex)
            {
                helper.Transaction.Rollback();
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
        }
        #endregion



    }
}
#endregion