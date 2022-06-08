using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAMMI.QM
{
    /// <summary>
    /// Fault code nodes calss
    /// </summary>
    public class FaultCodeNodes
    {
        #region Methods

        /// <summary>
        /// Shallow copy
        /// </summary>
        /// <returns></returns>
        public object ShallowCopy()
        {
            return this.MemberwiseClone();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Front back
        /// </summary>
        public string LOCATION { get; set; }

        /// <summary>
        /// Fault code
        /// </summary>
        public string FAULT_CODE { get; set; }

        /// <summary>
        /// Fault count
        /// </summary>
        public int FAULT_COUNT { get; set; }

        #endregion
    }
}
