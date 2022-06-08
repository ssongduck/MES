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
    public partial class LayoutControl1 : Panel
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
        public LayoutControl1()
        {
        }

        /// <summary>
        /// Container
        /// </summary>
        /// <param name="container"></param>
        public LayoutControl1(IContainer container)
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

            //Shelf shelf = GetClickedShelfNode(mouseEventArgs.X, mouseEventArgs.Y);
            //if (shelf != null && !string.IsNullOrEmpty(shelf.Id)) CallDiagramDoubleClickHandler(shelf);
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
            Font font = new Font(new FontFamily("휴먼모음T"), 10F * 2f, FontStyle.Regular);
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
                _BufferedGraphics.Graphics.DrawImage(_Image, 0, 0);
            }

            _BufferedGraphics.Graphics.DrawString("J", font, brush(Dop.LineColor), 62, 62, stringFormat);
            _BufferedGraphics.Graphics.DrawRectangle(pen(Dop.LineColor, 3), 52, 90, 41, 125);

            _BufferedGraphics.Graphics.DrawString("L", font, brush(Dop.LineColor), 108, 1, stringFormat);
            _BufferedGraphics.Graphics.DrawRectangle(pen(Dop.LineColor, 3), 102, 28, 41, 125);

            _BufferedGraphics.Graphics.DrawString("M", font, brush(Dop.LineColor), 188, 45, stringFormat);
            _BufferedGraphics.Graphics.DrawRectangle(pen(Dop.LineColor, 3), 185, 73, 41, 125);

            _BufferedGraphics.Graphics.DrawString("O", font, brush(Dop.LineColor), 248, 3, stringFormat);
            _BufferedGraphics.Graphics.DrawRectangle(pen(Dop.LineColor, 3), 243, 31, 41, 125);

            _BufferedGraphics.Graphics.DrawString("K", font, brush(Dop.LineColor), 80, 380, stringFormat);
            _BufferedGraphics.Graphics.DrawRectangle(pen(Dop.LineColor, 3), 73, 253, 41, 125);

            _BufferedGraphics.Graphics.DrawString("N", font, brush(Dop.LineColor), 232, 375, stringFormat);
            _BufferedGraphics.Graphics.DrawRectangle(pen(Dop.LineColor, 3), 221, 245, 41, 125);

            _BufferedGraphics.Graphics.DrawString("P", font, brush(Dop.LineColor), 338, 370, stringFormat);
            _BufferedGraphics.Graphics.DrawRectangle(pen(Dop.LineColor, 3), 325, 243, 41, 125);

            FaultDetailNode faultDetailNode1 = new FaultDetailNode();
            faultDetailNode1.LOCATION = "J";
            faultDetailNode1.X = 52;
            faultDetailNode1.Y = 90;
            faultDetailNode1.WIDTH = 41;
            faultDetailNode1.HEIGHT = 125;
            faultDetailNode1.FRONT_BACK = _FrontBack;

            _FaultDetailNodeList.Add(faultDetailNode1);

            FaultDetailNode faultDetailNode2 = new FaultDetailNode();
            faultDetailNode2.LOCATION = "L";
            faultDetailNode2.X = 102;
            faultDetailNode2.Y = 28;
            faultDetailNode2.WIDTH = 41;
            faultDetailNode2.HEIGHT = 125;
            faultDetailNode2.FRONT_BACK = _FrontBack;

            _FaultDetailNodeList.Add(faultDetailNode2);

            FaultDetailNode faultDetailNode3 = new FaultDetailNode();
            faultDetailNode3.LOCATION = "M";
            faultDetailNode3.X = 185;
            faultDetailNode3.Y = 73;
            faultDetailNode3.WIDTH = 41;
            faultDetailNode3.HEIGHT = 125;
            faultDetailNode3.FRONT_BACK = _FrontBack;

            _FaultDetailNodeList.Add(faultDetailNode3);

            FaultDetailNode faultDetailNode4 = new FaultDetailNode();
            faultDetailNode4.LOCATION = "O";
            faultDetailNode4.X = 243;
            faultDetailNode4.Y = 31;
            faultDetailNode4.WIDTH = 41;
            faultDetailNode4.HEIGHT = 125;
            faultDetailNode4.FRONT_BACK = _FrontBack;

            _FaultDetailNodeList.Add(faultDetailNode4);

            FaultDetailNode faultDetailNode5 = new FaultDetailNode();
            faultDetailNode5.LOCATION = "K";
            faultDetailNode5.X = 73;
            faultDetailNode5.Y = 253;
            faultDetailNode5.WIDTH = 41;
            faultDetailNode5.HEIGHT = 125;
            faultDetailNode5.FRONT_BACK = _FrontBack;

            _FaultDetailNodeList.Add(faultDetailNode5);

            FaultDetailNode faultDetailNode6 = new FaultDetailNode();
            faultDetailNode6.LOCATION = "N";
            faultDetailNode6.X = 221;
            faultDetailNode6.Y = 245;
            faultDetailNode6.WIDTH = 41;
            faultDetailNode6.HEIGHT = 125;
            faultDetailNode6.FRONT_BACK = _FrontBack;

            _FaultDetailNodeList.Add(faultDetailNode6);

            FaultDetailNode faultDetailNode7 = new FaultDetailNode();
            faultDetailNode7.LOCATION = "P";
            faultDetailNode7.X = 325;
            faultDetailNode7.Y = 243;
            faultDetailNode7.WIDTH = 41;
            faultDetailNode7.HEIGHT = 125;
            faultDetailNode7.FRONT_BACK = _FrontBack;

            _FaultDetailNodeList.Add(faultDetailNode7);

            _BufferedGraphics.Render();
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
        /// Pen
        /// </summary>
        /// <param name="color"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        private Pen pen(Color color, float width)
        {
            Hashtable pens = new Hashtable();

            if (pens.Contains(color) == false)
            {
                Pen p = new Pen(color);
                p.Width = width;
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
        private void SelectedNode(FaultDetailNode selectedNode)
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
        /// Get selected node
        /// </summary>
        /// <returns></returns>
        public FaultDetailNode GetSelectedNode()
        {
            FaultDetailNode faultDetailNode = null;

            if (_FaultDetailNodeList != null)
            {
                faultDetailNode = _FaultDetailNodeList.Where(t => t.SELECTEDSTATUS == SelectedStatus.SELECTED).FirstOrDefault();
            }

            return faultDetailNode;
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
}
