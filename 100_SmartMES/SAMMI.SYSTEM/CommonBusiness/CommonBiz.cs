using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using Infragistics.Win.UltraWinGrid;

namespace SAMMI.SY
{
    /// <summary>
    /// Common business class
    /// </summary>
    public class CommonBiz
    {
        #region Methods

        /// <summary>
        /// Get master code
        /// </summary>
        /// <param name="ultraCombo"></param>
        /// <param name="dt"></param>
        /// <param name="sDisplayMember"></param>
        /// <param name="sValueMember"></param>
        public static void GetMasterCode(UltraCombo ultraCombo, DataTable dt, string sDisplayMember, string sValueMember)
        {
            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    ultraCombo.DisplayMember = sDisplayMember;
                    ultraCombo.ValueMember = sValueMember;
                    ultraCombo.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
                    ultraCombo.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;

                    ultraCombo.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.None;
                    ultraCombo.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.None;
                    ultraCombo.DisplayLayout.Override.BorderStyleSpecialRowSeparator = Infragistics.Win.UIElementBorderStyle.None;

                    ultraCombo.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
                    ultraCombo.Font = new System.Drawing.Font("맑은 고딕", 10, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 129);
                    ultraCombo.Size = new System.Drawing.Size(100, 26);
                    ultraCombo.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
                    ultraCombo.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;


                    ultraCombo.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        #endregion
    }
}
