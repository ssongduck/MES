#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID        : ZZ0200
//   Form Name      : SPLASH WINDOW
//   Name Space     : MAIN
//   Created Date   : 2012.03.19
//   Made By        : SAMMI INFORMATION SYSTEM CO.,LTD
//   Description    : 
//   DB Table       : 
//   StoreProcedure : 
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
#endregion

#region < SmartMES >
namespace SmartMES
{
    public partial class ZZ0200 : Form
    {
        #region < FIELD >
        private System.Drawing.Point temp;
        private Configuration appConfig;
        private bool ismove = false;
        #endregion

        #region < CONSTRUCTOR >
        public ZZ0200()
        {
            InitializeComponent();

            appConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }
        #endregion

        #region < EVENT AREA >
           private void pnlSplash_MouseUp(object sender, MouseEventArgs e)
        {
            this.ismove = false;
        }

        private void pnlSplash_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void pnlSplash_MouseDown(object sender, MouseEventArgs e)
        {
            this.ismove = true;

            temp.X = Control.MousePosition.X - this.Location.X;
            temp.Y = Control.MousePosition.Y - this.Location.Y;
        }

        private void ZZ0200_Load(object sender, EventArgs e)
        {
            this.txtDBSERVER1.Text        = appConfig.AppSettings.Settings["DBSERVER1"].Value.ToString();
            this.txtDBUSER1.Text          = appConfig.AppSettings.Settings["DBUSER1"].Value.ToString();
            this.txtDBPASSWORD1.Text      = appConfig.AppSettings.Settings["DBPASSWORD1"].Value.ToString();
            this.txtDATABASE1.Text        = appConfig.AppSettings.Settings["DATABASE1"].Value.ToString();
            this.txtCONNECTTIMEOUT1.Text  = appConfig.AppSettings.Settings["CONNECTTIMEOUT1"].Value.ToString();
            this.txtPROVIDER1.Text        = appConfig.AppSettings.Settings["PROVIDER1"].Value.ToString();

            //this.txtLiveUpdateServer.Text = appConfig.AppSettings.Settings["LUSERVER"].Value.ToString();
            //this.txtUserID.Text           = appConfig.AppSettings.Settings["LUUSERID"].Value.ToString();
            //this.txtPassword.Text         = appConfig.AppSettings.Settings["LUPASSWORD"].Value.ToString();
            //this.txtPort.Text             = appConfig.AppSettings.Settings["LUPORT"].Value.ToString();
            //this.txtPath.Text             = appConfig.AppSettings.Settings["LUPATH"].Value.ToString();

        }

        private void btnDBConfig_Click(object sender, EventArgs e)
        {
            try
            {
                string connectionstring = "Persist Security Info=True"
                                        + ";Data Source="     + this.txtDBSERVER1.Text.Trim()
                                        + ";Initial Catalog=" + this.txtDATABASE1.Text.Trim()
                                        + ";User ID="         + this.txtDBUSER1.Text.Trim()
                                        + ";Password="        + this.txtDBPASSWORD1.Text.Trim()
                                        + ";Connect Timeout=" + this.txtCONNECTTIMEOUT1.Text.Trim();

                SqlConnection conn = new SqlConnection(connectionstring);
                conn.Open();
                conn.Close();
                MessageBox.Show("정상연결되었습니다.");
            }
            catch
            {
                MessageBox.Show("연결설정오류입니다.");
            }
        }
        #endregion

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string connectionstring = string.Empty;
                appConfig.AppSettings.Settings["DBSERVER1"].Value        = this.txtDBSERVER1.Text.Trim();      
                appConfig.AppSettings.Settings["DBUSER1"].Value          = this.txtDBUSER1.Text.Trim();        
                appConfig.AppSettings.Settings["DBPASSWORD1"].Value      = this.txtDBPASSWORD1.Text.Trim();    
                appConfig.AppSettings.Settings["DATABASE1"].Value        = this.txtDATABASE1.Text.Trim();      
                appConfig.AppSettings.Settings["CONNECTTIMEOUT1"].Value  = this.txtCONNECTTIMEOUT1.Text.Trim();
               
                connectionstring = "Persist Security Info=True"
                                 + ";Data Source="     + this.txtDBSERVER1.Text.Trim()
                                 + ";Initial Catalog=" + this.txtDATABASE1.Text.Trim()
                                 + ";User ID="         + this.txtDBUSER1.Text.Trim()
                                 + ";Password="        + this.txtDBPASSWORD1.Text.Trim()
                                 + ";Connect Timeout=" + this.txtCONNECTTIMEOUT1.Text.Trim();
                for (int i = 0; i < appConfig.ConnectionStrings.ConnectionStrings.Count; i++)
                {
                    appConfig.ConnectionStrings.ConnectionStrings[i].ConnectionString = connectionstring;
                    appConfig.ConnectionStrings.ConnectionStrings[i].ProviderName     = txtPROVIDER1.Text.Trim();
                }
                SqlConnection conn = new SqlConnection(connectionstring);
                conn.Open();
                conn.Close();
                //MessageBox.Show("정상연결되었습니다.");
               
                //appConfig.AppSettings.Settings["LUSERVER"].Value    = this.txtLiveUpdateServer.Text.Trim();
                //appConfig.AppSettings.Settings["LUUSERID"].Value    = this.txtUserID.Text.Trim();
                //appConfig.AppSettings.Settings["LUPASSWORD"].Value  = this.txtPassword.Text.Trim();
                //appConfig.AppSettings.Settings["LUPORT"].Value      = this.txtPort.Text.Trim();
                //appConfig.AppSettings.Settings["LUPATH"].Value      = this.txtPath.Text.Trim();
               
                appConfig.Save();
            }
            catch (Exception )
            {
                MessageBox.Show("연결설정오류입니다. 저장되지 않습니다.");
            }            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
#endregion