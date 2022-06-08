using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DevExpress.XtraEditors.Repository;
using DevExpress.Utils;

namespace SAMMI.QM
{
    /// <summary>
    /// Column property
    /// </summary>
    public class ColumnProperty
    {
        #region Variable

        /// <summary>
        /// Field name
        /// </summary>
        private string _FieldName = string.Empty;

        /// <summary>
        /// Is PK
        /// </summary>
        private bool _IsPK = false;

        /// <summary>
        /// Is input field
        /// </summary>
        private bool _IsInputField = false;

        /// <summary>
        /// Is visiable
        /// </summary>
        private bool _IsVisible = true;

        /// <summary>
        /// Is read only
        /// </summary>
        private bool _IsReadOnly = true;

        /// <summary>
        /// Repository item
        /// </summary>
        private RepositoryItem _RepositoryItem;

        /// <summary>
        /// Format type
        /// </summary>
        private FormatType _FormatType;

        /// <summary>
        /// Format string
        /// </summary>
        private string _FormatString = string.Empty;

        /// <summary>
        /// Min value
        /// </summary>
        private int _MinValue = 0;

        /// <summary>
        /// Max value
        /// </summary>
        private int _MaxValue = 0;

        /// <summary>
        /// Default boolean
        /// </summary>
        private DefaultBoolean _CellMerge = DefaultBoolean.False;

        #endregion

        #region Constructor

        /// <summary>
        /// Column property constructor
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="visible"></param>
        /// <param name="readOnly"></param>
        /// <param name="pk"></param>
        public ColumnProperty(string fieldName, bool visible, bool readOnly, bool pk)
        {
            _FieldName = fieldName;
            _IsPK = pk;
            _IsVisible = visible;
            _IsReadOnly = readOnly;
        }

        /// <summary>
        /// Column property constructor
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="visible"></param>
        /// <param name="readOnly"></param>
        /// <param name="pk"></param>
        /// <param name="inputField"></param>
        public ColumnProperty(string fieldName, bool visible, bool readOnly, bool pk, bool inputField)
        {
            _FieldName = fieldName;
            _IsPK = pk;
            _IsInputField = inputField;
            _IsVisible = visible;
            _IsReadOnly = readOnly;
        }

        /// <summary>
        /// Column property constructor
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="visible"></param>
        /// <param name="readOnly"></param>
        /// <param name="pk"></param>
        /// <param name="formatType"></param>
        /// <param name="formatString"></param>
        public ColumnProperty(string fieldName, bool visible, bool readOnly, bool pk, FormatType formatType, string formatString)
        {
            _FieldName = fieldName;
            _IsPK = pk;
            _IsVisible = visible;
            _IsReadOnly = readOnly;
            _FormatType = formatType;
            _FormatString = formatString;
        }

        /// <summary>
        /// Column property constructor
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="visible"></param>
        /// <param name="readOnly"></param>
        /// <param name="pk"></param>
        /// <param name="inputField"></param>
        /// <param name="formatType"></param>
        /// <param name="formatString"></param>
        public ColumnProperty(string fieldName, bool visible, bool readOnly, bool pk, bool inputField, FormatType formatType, string formatString)
        {
            _FieldName = fieldName;
            _IsPK = pk;
            _IsInputField = inputField;
            _IsVisible = visible;
            _IsReadOnly = readOnly;
            _FormatType = formatType;
            _FormatString = formatString;
        }

        /// <summary>
        /// Column property constructor
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="visible"></param>
        /// <param name="readOnly"></param>
        /// <param name="pk"></param>
        /// <param name="repositoryItem"></param>
        public ColumnProperty(string fieldName, bool visible, bool readOnly, bool pk, RepositoryItem repositoryItem)
        {
            _FieldName = fieldName;
            _IsPK = pk;
            _IsVisible = visible;
            _IsReadOnly = readOnly;
            _RepositoryItem = repositoryItem;
        }

        /// <summary>
        /// Column property constructor
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="visible"></param>
        /// <param name="readOnly"></param>
        /// <param name="pk"></param>
        /// <param name="inputField"></param>
        /// <param name="repositoryItem"></param>
        public ColumnProperty(string fieldName, bool visible, bool readOnly, bool pk, bool inputField, RepositoryItem repositoryItem)
        {
            _FieldName = fieldName;
            _IsPK = pk;
            _IsInputField = inputField;
            _IsVisible = visible;
            _IsReadOnly = readOnly;
            _RepositoryItem = repositoryItem;
        }

        /// <summary>
        /// Column property constructor
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="visible"></param>
        /// <param name="readOnly"></param>
        /// <param name="pk"></param>
        /// <param name="repositoryItem"></param>
        /// <param name="formatType"></param>
        /// <param name="formatString"></param>
        public ColumnProperty(string fieldName, bool visible, bool readOnly, bool pk, RepositoryItem repositoryItem, FormatType formatType, string formatString)
        {
            _FieldName = fieldName;
            _IsPK = pk;
            _IsVisible = visible;
            _IsReadOnly = readOnly;
            _RepositoryItem = repositoryItem;
            _FormatType = formatType;
            _FormatString = formatString;
        }

        /// <summary>
        /// Column property constructor
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="visible"></param>
        /// <param name="readOnly"></param>
        /// <param name="pk"></param>
        /// <param name="inputField"></param>
        /// <param name="repositoryItem"></param>
        /// <param name="formatType"></param>
        /// <param name="formatString"></param>
        public ColumnProperty(string fieldName, bool visible, bool readOnly, bool pk, bool inputField, RepositoryItem repositoryItem, FormatType formatType, string formatString)
        {
            _FieldName = fieldName;
            _IsPK = pk;
            _IsInputField = inputField;
            _IsVisible = visible;
            _IsReadOnly = readOnly;
            _RepositoryItem = repositoryItem;
            _FormatType = formatType;
            _FormatString = formatString;
        }

        /// <summary>
        /// Column property constructor
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="visible"></param>
        /// <param name="readOnly"></param>
        /// <param name="pk"></param>
        /// <param name="repositoryItem"></param>
        /// <param name="formatType"></param>
        /// <param name="formatString"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        public ColumnProperty(string fieldName, bool visible, bool readOnly, bool pk, RepositoryItem repositoryItem, FormatType formatType, string formatString,
            int minValue, int maxValue)
        {
            _FieldName = fieldName;
            _IsPK = pk;
            _IsVisible = visible;
            _IsReadOnly = readOnly;
            _RepositoryItem = repositoryItem;
            _FormatType = formatType;
            _FormatString = formatString;
            _MinValue = minValue;
            _MaxValue = maxValue;
        }

        /// <summary>
        /// Column property constructor
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="visible"></param>
        /// <param name="readOnly"></param>
        /// <param name="pk"></param>
        /// <param name="inputField"></param>
        /// <param name="repositoryItem"></param>
        /// <param name="formatType"></param>
        /// <param name="formatString"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        public ColumnProperty(string fieldName, bool visible, bool readOnly, bool pk, bool inputField, RepositoryItem repositoryItem, FormatType formatType, string formatString,
            int minValue, int maxValue)
        {
            _FieldName = fieldName;
            _IsPK = pk;
            _IsInputField = inputField;
            _IsVisible = visible;
            _IsReadOnly = readOnly;
            _RepositoryItem = repositoryItem;
            _FormatType = formatType;
            _FormatString = formatString;
            _MinValue = minValue;
            _MaxValue = maxValue;
        }

        /// <summary>
        /// Column property constructor
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="visible"></param>
        /// <param name="readOnly"></param>
        /// <param name="pk"></param>
        /// <param name="cellMerge"></param>
        public ColumnProperty(string fieldName, bool visible, bool readOnly, bool pk, DefaultBoolean cellMerge)
        {
            _FieldName = fieldName;
            _IsPK = pk;
            _IsVisible = visible;
            _IsReadOnly = readOnly;
            _CellMerge = cellMerge;
        }

        /// <summary>
        /// Column property constructor
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="visible"></param>
        /// <param name="readOnly"></param>
        /// <param name="pk"></param>
        /// <param name="inputField"></param>
        /// <param name="cellMerge"></param>
        public ColumnProperty(string fieldName, bool visible, bool readOnly, bool pk, bool inputField, DefaultBoolean cellMerge)
        {
            _FieldName = fieldName;
            _IsPK = pk;
            _IsInputField = inputField;
            _IsVisible = visible;
            _IsReadOnly = readOnly;
            _CellMerge = cellMerge;
        }

        #endregion

        #region Property

        /// <summary>
        /// Field name
        /// </summary>
        public string FieldName
        {
            get
            {
                return _FieldName;
            }
            set
            {
                _FieldName = value;
            }
        }

        /// <summary>
        /// Primary key
        /// </summary>
        public bool PK
        {
            get
            {
                return _IsPK;
            }
            set
            {
                _IsPK = (value == null) ? false : value;
            }
        }

        /// <summary>
        /// Input field
        /// </summary>
        public bool InputField
        {
            get
            {
                return _IsInputField;
            }
            set
            {
                _IsInputField = (value == null) ? false : value;
            }
        }

        /// <summary>
        /// Visible
        /// </summary>
        public bool Visible
        {
            get
            {
                return _IsVisible;
            }
            set
            {
                _IsVisible = value;
            }
        }

        /// <summary>
        /// Read only
        /// </summary>
        public bool ReadOnly
        {
            get
            {
                return _IsReadOnly;
            }
            set
            {
                _IsReadOnly = value;
            }
        }

        /// <summary>
        /// Repository item
        /// </summary>
        public RepositoryItem rRepositoryItem
        {
            get
            {
                return _RepositoryItem;
            }
            set
            {
                _RepositoryItem = value;
            }
        }

        /// <summary>
        /// Format type
        /// </summary>
        public FormatType fFormatType
        {
            get
            {
                return _FormatType;
            }
            set
            {
                _FormatType = value;
            }
        }

        /// <summary>
        /// Format string
        /// </summary>
        public string FormatString
        {
            get
            {
                return _FormatString;
            }
            set
            {
                _FormatString = value;
            }
        }

        /// <summary>
        /// Min value
        /// </summary>
        public int MinValue
        {
            get
            {
                return _MinValue;
            }
            set
            {
                _MinValue = value;
            }
        }

        /// <summary>
        /// Max value
        /// </summary>
        public int MaxValue
        {
            get
            {
                return _MaxValue;
            }
            set
            {
                _MaxValue = value;
            }
        }

        /// <summary>
        /// Cell merge
        /// </summary>
        public DefaultBoolean CellMerge
        {
            get
            {
                return _CellMerge;
            }
            set
            {
                _CellMerge = value;
            }
        }

        #endregion
    }
}
