using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace SAMMI.QM
{
    /// <summary>
    /// Front back class
    /// </summary>
    public static class FrontBack
    {
        /// <summary>
        /// Front
        /// </summary>
        public static string FRONT = "FRONT";

        /// <summary>
        /// Back
        /// </summary>
        public static string BACK = "BACK";
    }

    /// <summary>
    /// Selected status class
    /// </summary>
    public static class SelectedStatus
    {
        /// <summary>
        /// None
        /// </summary>
        public static string NONE = "NONE";

        /// <summary>
        /// Selected
        /// </summary>
        public static string SELECTED = "SELECTED";
    }

    /// <summary>
    /// Dop class
    /// </summary>
    public static class Dop
    {
        /// <summary>
        /// Panel back color
        /// </summary>
        public static Color PanelBackColor = Color.White;

        /// <summary>
        /// Back color
        /// </summary>
        public static Color BackColor = Color.FromArgb(214, 228, 243);

        /// <summary>
        /// Line color
        /// </summary>
        public static Color LineColor = Color.Red;

        /// <summary>
        /// Font color
        /// </summary>
        public static Color FontColor = Color.FromArgb(30, 57, 91);

        /// <summary>
        /// Fault font color
        /// </summary>
        public static Color FaultFontColor = Color.FromArgb(0, 0, 255);

        /// <summary>
        /// Selected color
        /// </summary>
        public static Color SelectedColor = Color.FromArgb(100, 255, 255, 0);

        /// <summary>
        /// Transparent color
        /// </summary>
        public static Color TransparentrColor = Color.Transparent;
    }

    /// <summary>
    /// Sub code type class
    /// </summary>
    public static class SUBCODETYPE
    {
        /// <summary>
        /// Information
        /// </summary>
        public static string INF = "INF";

        /// <summary>
        /// Warning
        /// </summary>
        public static string WAR = "WAR";

        /// <summary>
        /// Error
        /// </summary>
        public static string ERR = "ERR";
    }

    /// <summary>
    /// Fault code class
    /// </summary>
    public static class FAULT_CODE
    {
        /// <summary>
        /// Fault code 102
        /// </summary>
        public static string FAULT102 = "102";

        /// <summary>
        /// Fault code 105
        /// </summary>
        public static string FAULT105 = "105";

        /// <summary>
        /// Fault code 106
        /// </summary>
        public static string FAULT106 = "106";

        /// <summary>
        /// Fault code 110
        /// </summary>
        public static string FAULT110 = "110";

        /// <summary>
        /// Fault code 112
        /// </summary>
        public static string FAULT112 = "112";

        /// <summary>
        /// Fault code 113
        /// </summary>
        public static string FAULT113 = "113";

        /// <summary>
        /// Fault code 124
        /// </summary>
        public static string FAULT124 = "124";

        /// <summary>
        /// Fault code 206
        /// </summary>
        public static string FAULT206 = "206";

        /// <summary>
        /// Fault code 208
        /// </summary>
        public static string FAULT208 = "208";

        /// <summary>
        /// Fault code 215
        /// </summary>
        public static string FAULT215 = "215";

    }
}
