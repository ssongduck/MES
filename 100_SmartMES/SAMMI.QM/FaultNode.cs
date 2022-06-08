using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace SAMMI.QM
{
    /// <summary>
    /// Fault detail node class
    /// </summary>
    public class FaultDetailNode
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
        public string FRONT_BACK { get; set; }

        /// <summary>
        /// Location
        /// </summary>
        public string LOCATION { get; set; }

        /// <summary>
        /// X start position
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// Y start position
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// Width
        /// </summary>
        public float WIDTH { get; set; }

        /// <summary>
        /// Height
        /// </summary>
        public float HEIGHT { get; set; }

        /// <summary>
        /// Selected status
        /// </summary>
        public string SELECTEDSTATUS { get; set; }

        #endregion
    }
}
