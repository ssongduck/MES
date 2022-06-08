using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SAMMI.QM
{
    /// <summary>
    /// Six sigma level class
    /// </summary>
    public partial class SixSigmaLevel : Form
    {
        #region Constructors

        /// <summary>
        /// Six sigma level constructor
        /// </summary>
        public SixSigmaLevel()
        {
            InitializeComponent();
            Initialize();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initialize
        /// </summary>
        private void Initialize()
        {
            this.CenterToScreen();
            this.TopMost = true;
        }

        #endregion
    }
}
