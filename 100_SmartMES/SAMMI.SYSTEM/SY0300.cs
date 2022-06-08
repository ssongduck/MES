#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID        : SY0300
//   Form Name      : NC 절단실적 등록
//   Name Space     : STXDNC.SY
//   Created Date   : 2012.03.23
//   Made By        : SAMMI INFORMATION SYSTEM CO.,LTD
//   Description    : 
//   DB Table       : TSY0300
//   StoreProcedure : USP_SY0300_S1
// *---------------------------------------------------------------------------------------------*
#endregion

#region <USING AREA>
using SAMMI.Common;
using System;
using System.Data;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
#endregion

namespace SAMMI.SY
{
    public partial class SY0300 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        #region <MEMBER AREA>
        // 변수나 Form에서 사용될 Class를 정의
        private Database db;
        private SqlConnection conn;
        //private SqlTransaction trans;
        #endregion

        #region < CONSTRUCTOR >
        public SY0300()
        {
            InitializeComponent();
            this.db = DatabaseFactory.CreateDatabase();
            this.conn = (SqlConnection)this.db.CreateConnection();
            this.daTable1.Connection = conn;

            this.cboPlanStartDT_H.Value = Convert.ToDateTime(((DateTime)this.cboPlanStartDT_H.Value).ToString("yyyy-MM-01") + " 00:00:00.00");
            this.cboPlanEndDT_H.Value   = Convert.ToDateTime(((DateTime)this.cboPlanEndDT_H.Value).ToString("yyyy-MM-dd") + " 23:59:59.99");
        }
        #endregion

        #region <TOOL BAR AREA >
        public override void DoInquire()
        {
            base.DoInquire();

            DateTime planstartdt = Convert.ToDateTime(((DateTime)this.cboPlanStartDT_H.Value).ToString("yyyy-MM-dd") + " 00:00:00.00");
            DateTime planenddt   = Convert.ToDateTime(((DateTime)this.cboPlanEndDT_H.Value).ToString("yyyy-MM-dd") + " 23:59:59.99");

            string maker =  (string)this.txtMaker.Value;
            if (maker    == null)
                maker    =  "";

            this.daTable1.Fill(this.ds.dTable1,
                               planstartdt, planenddt, maker);
        }
        #endregion
    }
}
