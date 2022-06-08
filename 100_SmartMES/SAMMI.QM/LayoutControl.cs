using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;
using System.Drawing.Imaging;

namespace SAMMI.QM
{
    /// <summary>
    /// Layout control
    /// </summary>
    [ToolboxItem(true)]
    public partial class LayoutControl : Panel
    {
        #region Variables

        /// <summary>
        /// Front back
        /// </summary>
        private string _FrontBack = string.Empty;

        /// <summary>
        /// Image
        /// </summary>
        private Image _Image = null;

        /// <summary>
        /// Fault detail node list
        /// </summary>
        private List<FaultDetailNode> _FaultDetailNodeList = null;

        /// <summary>
        /// Fault code nodes list
        /// </summary>
        List<FaultCodeNodes> _FaultCodeNodesList = null;
        
        /// <summary>
        /// Diagram click event argument event handler
        /// </summary>
        public event EventHandler<DiagramClickEventArgs> DiagramClick;

        /// <summary>
        /// Diagram double click event argument event handler
        /// </summary>
        public event EventHandler<DiagramDoubleClickEvnetArgs> DiagramDoubleClick;

        /// <summary>
        /// Canvas
        /// </summary>
        Panel _Canvas = null;

        /// <summary>
        /// Canvas graphics
        /// </summary>
        Graphics _CanvasGraphics = null;

        /// <summary>
        /// Current context
        /// </summary>
        BufferedGraphicsContext _CurrentContext;

        /// <summary>
        /// Buffered graphics
        /// </summary>
        BufferedGraphics _BufferedGraphics;

        /// <summary>
        /// Width
        /// </summary>
        int _Width = 0;

        /// <summary>
        /// Height
        /// </summary>
        int _Height = 0;

        #endregion

        #region Constructors

        /// <summary>
        /// Layout control constructor
        /// </summary>
        public LayoutControl()
        {
        }

        /// <summary>
        /// Container
        /// </summary>
        /// <param name="container"></param>
        public LayoutControl(IContainer container)
        {
            this.DoubleBuffered = true;
            this.UpdateStyles();

            container.Add(this);

            _Canvas = new Panel();

            this.Controls.Add(_Canvas);

            Initialize();

            AttachEventHandlers();
        }

        #endregion

        #region Events

        /// <summary>
        /// Layout control diagram click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LayoutControl_DiagramClick(object sender, DiagramClickEventArgs e)
        {
            if (e != null)
            {
                SelectedNode(e._FaultDetailNode);
            }
        }

        /// <summary>
        /// Layout control size changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LayoutControl_SizeChanged(object sender, EventArgs e)
        {
            Initialize();

            this.DrawRectangle();
        }

        /// <summary>
        /// Canvas mouse douwn event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Canvas_MouseDown(object sender, MouseEventArgs e)
        {
            this.Focus();

            if (e.Button == MouseButtons.Right) return;

            FaultDetailNode faultDetailNode = GetClickedFaultDetailNode(e.X, e.Y);
            if (faultDetailNode != null) CallFatulDetailDiagramClickHandler(faultDetailNode);
        }

        /// <summary>
        /// Canvas double click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _Canvas_DoubleClick(object sender, EventArgs e)
        {
            this.Focus();

            MouseEventArgs mouseEventArgs = e as MouseEventArgs;
        }

        /// <summary>
        /// Canvas paint event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
            Display(e.Graphics);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initialize
        /// </summary>
        public void Initialize()
        {
            _FaultDetailNodeList = new List<FaultDetailNode>();
            _FaultCodeNodesList = new List<FaultCodeNodes>();

            _Canvas.Width = this.CanvasWidth;
            _Canvas.Height = this.CanvasWidth;
            _CanvasGraphics = _Canvas.CreateGraphics();

            _CurrentContext = BufferedGraphicsManager.Current;

            ResetGraphics();
        }

        /// <summary>
        /// Attach event handlers
        /// </summary>
        private void AttachEventHandlers()
        {
            this.DiagramClick += new EventHandler<DiagramClickEventArgs>(LayoutControl_DiagramClick);
            this.SizeChanged += new EventHandler(LayoutControl_SizeChanged);

            this._Canvas.MouseDown += new MouseEventHandler(Canvas_MouseDown);
            this._Canvas.DoubleClick += new EventHandler(_Canvas_DoubleClick);
            this._Canvas.Paint += new PaintEventHandler(Canvas_Paint);
        }

        /// <summary>
        /// Detach event handlers
        /// </summary>
        private void DetachEventHandlers()
        {
            this.DiagramClick -= new EventHandler<DiagramClickEventArgs>(LayoutControl_DiagramClick);
            this.SizeChanged -= new EventHandler(LayoutControl_SizeChanged);

            this._Canvas.MouseDown -= new MouseEventHandler(Canvas_MouseDown);
            this._Canvas.DoubleClick -= new EventHandler(_Canvas_DoubleClick);
            this._Canvas.Paint -= new PaintEventHandler(Canvas_Paint);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            DetachEventHandlers();

            if (_BufferedGraphics != null)
            {
                _BufferedGraphics.Dispose();
            }
        }

        /// <summary>
        /// Reset graphics
        /// </summary>
        private void ResetGraphics()
        {
            _BufferedGraphics = _CurrentContext.Allocate(_CanvasGraphics, new Rectangle(0, 0, this.Width, this.Height));
        }

        /// <summary>
        /// Draw rectangle
        /// </summary>
        public void DrawRectangle()
        {
            float x = 0f;
            float y = 0f;
            float width = 100f;
            float height = 100f;

            Font font = new Font(new FontFamily("휴먼모음T"), 5F * 3f, FontStyle.Regular);
            StringFormat stringFormat = new StringFormat(StringFormatFlags.NoWrap);

            _Canvas.Width = this.CanvasWidth;
            _Canvas.Height = this.CanvasHeight;

            _BufferedGraphics = _CurrentContext.Allocate(_CanvasGraphics, new Rectangle(0, 0, _Canvas.Width, _Canvas.Height));
            _BufferedGraphics.Graphics.FillRectangle(brush(Dop.PanelBackColor), 0, 0, _Canvas.Width, _Canvas.Height);

            if (_FaultDetailNodeList != null && _FaultDetailNodeList.Count > 0)
            {
                _FaultDetailNodeList.Clear();
            }

            if (_Image != null)
            {
                _BufferedGraphics.Graphics.DrawImage(_Image, 40, 40);
            }

            x = 10;
            y = 10;

            for (int i = 0; i < 5; i++)
            {
                _BufferedGraphics.Graphics.FillRectangle(brush(Dop.BackColor), x + (i * width) + 30, y, width, 30);
                _BufferedGraphics.Graphics.DrawRectangle(pen(Dop.LineColor), x + (i * width) + 30, y, width, 30);

                for(int j = 0; j < 5; j++)
                {
                    _BufferedGraphics.Graphics.DrawRectangle(pen(Dop.LineColor), x + (i * width) + 30, y + (j * height) + 30, width, height);

                    FaultDetailNode faultDetailNode = new FaultDetailNode();

                    faultDetailNode.X = x + (i * width) + 30;
                    faultDetailNode.Y = y + (j * height) + 30;
                    faultDetailNode.WIDTH = width;
                    faultDetailNode.HEIGHT = height;

                    faultDetailNode.FRONT_BACK = _FrontBack;

                    faultDetailNode = SetLocation(faultDetailNode);

                    _FaultDetailNodeList.Add(faultDetailNode);
                }

                _BufferedGraphics.Graphics.FillRectangle(brush(Dop.BackColor), x, y + (i * height) + 30, 30, height);
                _BufferedGraphics.Graphics.DrawRectangle(pen(Dop.LineColor), x, y + (i * height) + 30, 30, height);
            }

            _BufferedGraphics.Graphics.DrawString("A", font, brush(Dop.FontColor), 80, 15, stringFormat);
            _BufferedGraphics.Graphics.DrawString("B", font, brush(Dop.FontColor), 180, 15, stringFormat);
            _BufferedGraphics.Graphics.DrawString("C", font, brush(Dop.FontColor), 280, 15, stringFormat);
            _BufferedGraphics.Graphics.DrawString("D", font, brush(Dop.FontColor), 380, 15, stringFormat);
            _BufferedGraphics.Graphics.DrawString("E", font, brush(Dop.FontColor), 480, 15, stringFormat);

            _BufferedGraphics.Graphics.DrawString("1", font, brush(Dop.FontColor), 18, 80, stringFormat);
            _BufferedGraphics.Graphics.DrawString("2", font, brush(Dop.FontColor), 18, 180, stringFormat);
            _BufferedGraphics.Graphics.DrawString("3", font, brush(Dop.FontColor), 18, 280, stringFormat);
            _BufferedGraphics.Graphics.DrawString("4", font, brush(Dop.FontColor), 18, 380, stringFormat);
            _BufferedGraphics.Graphics.DrawString("5", font, brush(Dop.FontColor), 18, 480, stringFormat);

            _BufferedGraphics.Graphics.FillRectangle(brush(Dop.BackColor), x, y, 30, 30);
            _BufferedGraphics.Graphics.DrawRectangle(pen(Dop.LineColor), x, y, 30, 30);
            _BufferedGraphics.Render();

            DrawFalutCount();
        }

        /// <summary>
        /// Set location
        /// </summary>
        /// <param name="faultDetailNode"></param>
        /// <returns></returns>
        private FaultDetailNode SetLocation(FaultDetailNode faultDetailNode)
        {
            faultDetailNode.LOCATION = string.Format("{0}{1}", GetXNode(faultDetailNode.X), GetYNode(faultDetailNode.Y));
            return faultDetailNode;
        }

        /// <summary>
        /// Get x node
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private string GetXNode(float x)
        {
            string xValue = string.Empty;

            if (x == 40)
            {
                xValue = "A";
            }
            else if (x == 140)
            {
                xValue = "B";
            }
            else if (x == 240)
            {
                xValue = "C";
            }
            else if (x == 340)
            {
                xValue = "D";
            }
            else if (x == 440)
            {
                xValue = "E";
            }

            return xValue;
        }

        /// <summary>
        /// Get y node
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        private string GetYNode(float y)
        {
            string yValue = string.Empty;

            if (y == 40)
            {
                yValue = "1";
            }
            else if (y == 140)
            {
                yValue = "2";
            }
            else if (y == 240)
            {
                yValue = "3";
            }
            else if (y == 340)
            {
                yValue = "4";
            }
            else if (y == 440)
            {
                yValue = "5";
            }

            return yValue;
        }

        /// <summary>
        /// Get clicked fault detail node
        /// </summary>
        /// <param name="hitX"></param>
        /// <param name="hitY"></param>
        /// <returns></returns>
        private FaultDetailNode GetClickedFaultDetailNode(int hitX, int hitY)
        {
            FaultDetailNode faultDetailNode = null;

            if (_FaultDetailNodeList != null && _FaultDetailNodeList.Count > 0)
            {
                var nodeVar = _FaultDetailNodeList.Where(t => t.X <= hitX && (t.X + t.WIDTH) >= hitX
                    && t.Y <= hitY && (t.Y + t.HEIGHT) >= hitY);

                if (nodeVar != null)
                {
                    foreach (var node in nodeVar)
                    {
                        faultDetailNode = node;
                    }
                }
            }

            return (faultDetailNode == null) ? null : faultDetailNode;
        }

        /// <summary>
        /// Display
        /// </summary>
        /// <param name="g"></param>
        public void Display(Graphics g)
        {
            this._BufferedGraphics.Render(g);
        }

        /// <summary>
        /// Bind
        /// </summary>
        /// <param name="image"></param>
        /// <param name="FrontBack"></param>
        public void Bind(Image image, string frontBack)
        {
            _Image = ChangeOpacity(image, 0.65f);
            _FrontBack = frontBack;

            this.DrawRectangle();
        }

        /// <summary>
        /// Change opacity
        /// </summary>
        /// <param name="image"></param>
        /// <param name="opacityvalue"></param>
        /// <returns></returns>
        public Bitmap ChangeOpacity(Image image, float opacityvalue)
        {
            Bitmap bmp = new Bitmap(image.Width, image.Height);
            Graphics graphics = Graphics.FromImage(bmp);
            ColorMatrix colormatrix = new ColorMatrix();
            colormatrix.Matrix33 = opacityvalue;
            ImageAttributes imgAttribute = new ImageAttributes();
            imgAttribute.SetColorMatrix(colormatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            graphics.DrawImage(image, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imgAttribute);
            graphics.Dispose();
            return bmp;
        }

        /// <summary>
        /// Brush
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        private Brush brush(Color color)
        {
            Hashtable brushes = new Hashtable();

            if (brushes.Contains(color) == false)
            {
                Brush br = new SolidBrush(color);
                brushes.Add(color, br);
            }

            return (Brush)brushes[color];
        }

        /// <summary>
        /// Pen
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        private Pen pen(Color color)
        {
            Hashtable pens = new Hashtable();

            if (pens.Contains(color) == false)
            {
                Pen p = new Pen(color);
                p.Width = 2;
                pens.Add(color, p);
            }

            return (Pen)pens[color];
        }

        /// <summary>
        /// Call fault detail node diagram click handler
        /// </summary>
        /// <param name="bankInfo"></param>
        protected virtual void CallFatulDetailDiagramClickHandler(FaultDetailNode faultDetailNode)
        {
            DiagramClickEventArgs e = new DiagramClickEventArgs(faultDetailNode);

            EventHandler<DiagramClickEventArgs> handler = DiagramClick;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Selected node
        /// </summary>
        /// <param name="selectedNode"></param>
        public void SelectedNode(FaultDetailNode selectedNode)
        {
            if (_FaultDetailNodeList != null && selectedNode != null)
            {
                int selectedCnt = _FaultDetailNodeList.Where(t => t.SELECTEDSTATUS == SelectedStatus.SELECTED).Count();

                if (selectedCnt > 0)
                {
                    foreach (FaultDetailNode faultDetailNode in _FaultDetailNodeList.Where(t => t.SELECTEDSTATUS == SelectedStatus.SELECTED))
                    {
                        faultDetailNode.SELECTEDSTATUS = SelectedStatus.NONE;
                    }

                    DrawRectangle();
                }

                foreach (FaultDetailNode faultDetailNode in _FaultDetailNodeList.Where(t => t.LOCATION == selectedNode.LOCATION))
                {
                    faultDetailNode.SELECTEDSTATUS = SelectedStatus.SELECTED;
                    _BufferedGraphics.Graphics.FillRectangle(brush(Dop.SelectedColor), selectedNode.X, selectedNode.Y, selectedNode.WIDTH, selectedNode.HEIGHT);
                }
            }

            _BufferedGraphics.Render();
        }

        /// <summary>
        /// Select node
        /// </summary>
        /// <param name="sLocation"></param>
        public void SelectNode(string sLocation)
        {
            foreach (FaultDetailNode faultDetailNode in _FaultDetailNodeList.Where(t => t.LOCATION == sLocation))
            {
                faultDetailNode.SELECTEDSTATUS = SelectedStatus.SELECTED;
                _BufferedGraphics.Graphics.FillRectangle(brush(Dop.SelectedColor), faultDetailNode.X, faultDetailNode.Y, faultDetailNode.WIDTH, faultDetailNode.HEIGHT);
            }

            _BufferedGraphics.Render();
        }

        /// <summary>
        /// Select node
        /// </summary>
        /// <param name="faultCodeNodesList"></param>
        public void SelectNode(List<FaultCodeNodes> faultCodeNodesList)
        {
            foreach (FaultCodeNodes faultCodeNodes in _FaultCodeNodesList)
            {
                foreach (FaultDetailNode faultDetailNode in _FaultDetailNodeList.Where(t => t.LOCATION == faultCodeNodes.LOCATION))
                {
                    faultDetailNode.SELECTEDSTATUS = SelectedStatus.SELECTED;
                    _BufferedGraphics.Graphics.FillRectangle(brush(Dop.SelectedColor), faultDetailNode.X, faultDetailNode.Y, faultDetailNode.WIDTH, faultDetailNode.HEIGHT);
                }
            }

            _BufferedGraphics.Render();
        }

        /// <summary>
        /// Draw fault count
        /// </summary>
        /// <param name="bFlag"></param>
        private void DrawFalutCount()
        {
            string prefix = string.Empty;

            Font font = new Font(new FontFamily("휴먼모음T"), 8F * 3f, FontStyle.Bold);
            StringFormat stringFormat = new StringFormat(StringFormatFlags.NoWrap);

            if (_FaultCodeNodesList != null)
            {
                foreach (FaultCodeNodes faultCodeNodes in _FaultCodeNodesList)
                {
                    foreach (FaultDetailNode faultDetailNode in _FaultDetailNodeList.Where(t => t.LOCATION == faultCodeNodes.LOCATION))
                    {
                        _BufferedGraphics.Graphics.DrawString(faultCodeNodes.FAULT_COUNT.ToString(), font, brush(Dop.FaultFontColor), faultDetailNode.X + 10, faultDetailNode.Y + 15, stringFormat);
                    }
                }
            }

            _BufferedGraphics.Render();
        }

        /// <summary>
        /// Add fault code & count
        /// </summary>
        /// <param name="sLocaton"></param>
        /// <param name="faultCode"></param>
        public void AddFaultCodeCount(string sLocaton, string faultCode)
        {
            if (_FaultDetailNodeList != null)
            {
                if (_FaultCodeNodesList.Where(t => t.LOCATION == sLocaton && t.FAULT_CODE == faultCode).Count() > 0)
                {
                    foreach (FaultCodeNodes faultCodeNodes in _FaultCodeNodesList.Where(t => t.LOCATION == sLocaton && t.FAULT_CODE == faultCode))
                    {
                        faultCodeNodes.FAULT_COUNT += 1;
                    }
                }
                else
                {
                    FaultCodeNodes faultCodeNodes = new FaultCodeNodes();
                    faultCodeNodes.LOCATION = sLocaton;
                    faultCodeNodes.FAULT_CODE = faultCode;
                    faultCodeNodes.FAULT_COUNT = 1;
                    _FaultCodeNodesList.Add(faultCodeNodes);
                }
            }

            DrawRectangle();
        }
        
        #endregion

        #region Properties

        /// <summary>
        /// Fault detail node list
        /// </summary>
        public List<FaultDetailNode> faultDetailNodeLIst
        {
            get { return _FaultDetailNodeList; }
            set { _FaultDetailNodeList = value; }
        }

        /// <summary>
        /// Fault code nodes
        /// </summary>
        public List<FaultCodeNodes> faultCodeNodesList
        {
            get { return _FaultCodeNodesList; }
            set { _FaultCodeNodesList = value; }
        }

        /// <summary>
        /// Front back
        /// </summary>
        public string FrontBack
        {
            get { return _FrontBack; }
            set { _FrontBack = value; }
        }

        /// <summary>
        /// Canvas width
        /// </summary>
        public int CanvasWidth
        {
            get { return _Width; }
            set { _Width = value; }
        }

        /// <summary>
        /// Canvas height
        /// </summary>
        public int CanvasHeight
        {
            get { return _Height; }
            set { _Height = value; }
        }

        #endregion
    }

    /// <summary>
    /// Diagram click event args class
    /// </summary>
    public class DiagramClickEventArgs : EventArgs
    {
        #region Variables

        /// <summary>
        /// Fault detail node
        /// </summary>
        public FaultDetailNode _FaultDetailNode = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Diagram click event args constructor
        /// </summary>
        /// <param name="shelf"></param>
        public DiagramClickEventArgs(FaultDetailNode faultDetailNode)
        {
            _FaultDetailNode = faultDetailNode;
        }

        #endregion
    }

    /// <summary>
    /// Diagram double click event args class
    /// </summary>
    public class DiagramDoubleClickEvnetArgs : EventArgs
    {
        #region Variables

        /// <summary>
        /// Shelf
        /// </summary>
        //public Shelf _Shelf = null;

        #endregion

        #region Constructors

        ///// <summary>
        ///// Diagram double click event args constructor
        ///// </summary>
        ///// <param name="shelf"></param>
        //public DiagramDoubleClickEvnetArgs(Shelf shelf)
        //{
        //    _Shelf = shelf;
        //}

        #endregion
    }
}
