#region USING
using Infragistics.Win.UltraWinTree;
using SAMMI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.Common;
using System.Configuration;
using SAMMI.Common;
using SAMMI.Control;
#endregion

namespace SAMMI.SY
{
    public partial class SY0101 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        #region <MEMBER AREA>
    //    private UltraTree_DropHightLight_DrawFilter_Class UltraTree_DropHightLight_DrawFilter = new UltraTree_DropHightLight_DrawFilter_Class();
        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();

        private string  _sWorkerID = string.Empty;
        private DataSet rtnDsTemp  = new DataSet();
        private int     menuid     = 0;
        private int     cnt        = 0;

        Binding INQFLAG   = null;
        Binding DELFLAG   = null;
        Binding PRNFLAG   = null;
        Binding NEWFLAG   = null;
        Binding SAVEFLAG  = null;
        Binding EXCELFLAG = null;

        Boolean flag = false;
        Boolean binit = false;

        private string   LastActiveNode = string.Empty;
        private string[] Ex_Node;
        private int      ex_node_cnt    = 0;
        private bool     _bSave         = false;
        private System.Drawing.Point LastMouseDown;

        private DataTable dtTemp = new DataTable();
        #endregion

        #region < CONSTRUCTOR >
        public SY0101()
        {
            InitializeComponent();

            //UltraTree_DropHightLight_DrawFilter.Invalidate += new EventHandler(this.UltraTree_DropHightLight_DrawFilter_Invalidate);
            //UltraTree_DropHightLight_DrawFilter.QueryStateAllowedForNode += new UltraTree_DropHightLight_DrawFilter_Class.QueryStateAllowedForNodeEventHandler(this.UltraTree_DropHightLight_DrawFilter_QueryStateAllowedForNode);
        }
        #endregion

        #region <TOOL BAR AREA >
        /// <summary>
        /// ToolBar의 조회 버튼 클릭
        /// </summary>
        public override void DoInquire()
        {
            base.DoInquire();
            this.USP_SY0200_S2();

         //   this.grid1.GetRow();
        }

        private void node(Infragistics.Win.UltraWinTree.UltraTreeNode node)
        {
            try
            {
                Infragistics.Win.UltraWinTree.UltraTreeNode rootnode;
                for (int i = 0; i < node.RootNode.Nodes.Count; i++)
                {
                    if (Convert.ToString(node.RootNode.Nodes[i].Cells["MENUID"].Value) == Convert.ToString(this.Ex_Node[ex_node_cnt]))
                    {
                        this.ex_node_cnt++;
                        rootnode = node.RootNode.Nodes[i];
                        rootnode.Expanded = true;
                        this.node(rootnode);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// ToolBar의 신규 버튼 클릭
        /// </summary>
        public override void DoNew()
        {
            try
            {
                return;
/*
                if (this.grid1.Rows.Count == 0)
                    return;
                base.DoNew();
                //this.SaveCheck   = false;
                //this.InsertCheck = true;
                //if (this.COUNT == 0)
                //    return;
                if (this.treMenu.ActiveNode == null)
                {
                    return;
                }

                if (this.treMenu.ActiveNode.Nodes.Count == 0)
                    this.treMenu.ActiveNode = this.treMenu.ActiveNode.NextVisibleNode;
                if (this.grid1.Rows.Count == 0) return;

                POP_TSY0101 pop_tsy0101 = new POP_TSY0101(Convert.ToString(this.grid1.ActiveRow.Cells["WorkerID"].Value));
                pop_tsy0101.ShowDialog();
                if (pop_tsy0101.Cancel == true)
                    return;

                DataRow addrow = rtnDsTemp.Tables[0].NewRow();
                addrow["Sort"]      = Convert.ToInt32(this.treMenu.ActiveNode.Cells["Sort"].Value);
                addrow["WorkerID"]  = Convert.ToString(this.grid1.ActiveRow.Cells["WorkerID"].Value);
                addrow["MENUID"]    = pop_tsy0101.menuid;
                addrow["ParMenuID"] = this.treMenu.ActiveNode.Cells["ParMenuID"].Value;
                addrow["InqFlag"]   = pop_tsy0101.chk_inq;
                addrow["NewFlag"]   = pop_tsy0101.chk_new;
                addrow["DelFlag"]   = pop_tsy0101.chk_del;
                addrow["SaveFlag"]  = pop_tsy0101.chk_save;
                addrow["PrnFlag"]   = pop_tsy0101.chk_prn;
                addrow["ExcelFlag"] = pop_tsy0101.chk_excel;
                addrow["ProgramID"] = pop_tsy0101.ProgramID;
                addrow["MENUNAME"]  = pop_tsy0101.ProgramNM;
                addrow["MenuType"]  = pop_tsy0101.MenuType;
                addrow["ProgType"]  = pop_tsy0101.ProgType;
                addrow["NameSpace"] = pop_tsy0101.NameSpace;
                addrow["FileID"]    = pop_tsy0101.FileID;
                //addrow["SystemID"]  = this.SystemID;
                //addrow["Lang"]      = SAMMI.Common.Lang;
                addrow["UseFlag"]   = "Y";

                rtnDsTemp.Tables[0].Rows.Add(addrow);
                this.LastActiveNode = Convert.ToString(pop_tsy0101.menuid);
                System.Threading.Thread.Sleep(500);

                this.treMenu.RefreshSort();
                Infragistics.Win.UltraWinTree.UltraTreeNode node;
                node = this.treMenu.Nodes[Convert.ToString(pop_tsy0101.menuid)];
                this.treMenu.ActiveNode = node;

                this.treMenu_Click(this.treMenu, null);
                return;
 * */
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// ToolBar의 삭제 버튼 Click
        /// </summary>
        public override void DoDelete()
        {
            if (this.treMenu.ActiveNode == null)
                return;
            try
            {
                base.DoDelete();

                if (this.treMenu.ActiveNode.Cells["MENUTYPE"].Value.ToString() == "M")
                {
                    if (this.treMenu.ActiveNode.RootNode.Nodes.Count > 0)
                    {
                        SAMMI.Windows.Forms.DialogForm dialogForm = new SAMMI.Windows.Forms.DialogForm("Q00019");
                        dialogForm.ShowDialog();
                        if (dialogForm.result == "OK")
                        {
                            string menuid = this.treMenu.ActiveNode.Cells["MENUID"].Value.ToString();
                            for (int i = 0; i < this.rtnDsTemp.Tables[0].Rows.Count; i++)
                            {
                                try
                                {
                                    if (this.rtnDsTemp.Tables[0].Rows[i]["PARMENUID"].ToString() == menuid)
                                    {
                                        this.rtnDsTemp.Tables[0].Rows.Find(this.rtnDsTemp.Tables[0].Rows[i]["MENUID"].ToString()).Delete();
                                    }
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                            this.rtnDsTemp.Tables[0].Rows.Find(menuid).Delete();
                            return;
                        }
                    }
                        string menuid1    = this.treMenu.ActiveNode.Cells["MENUID"].Value.ToString();
                        int    sort       = Convert.ToInt32(this.treMenu.ActiveNode.Cells["SORT"].Value);
                        string parmenuid1 = this.treMenu.ActiveNode.Cells["PARMENUID"].Value.ToString();
                        for (int i = 0; i < this.rtnDsTemp.Tables[0].Rows.Count; i++)
                        {
                            try
                            {
                                if (this.rtnDsTemp.Tables[0].Rows[i]["PARMENUID"].ToString() == menuid1)
                                {
                                    this.rtnDsTemp.Tables[0].Rows[i]["SORT"] = sort;
                                    this.rtnDsTemp.Tables[0].Rows[i]["PARMENUID"] = parmenuid1;
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        this.rtnDsTemp.Tables[0].Rows.Find(menuid1).Delete();
                }
                else
                {
                    string menuid = Convert.ToString(this.treMenu.ActiveNode.Cells["MENUID"].Value);
                    string parmenuid = this.treMenu.ActiveNode.Cells["PARMENUID"].Value.ToString();
                    for (int i = 0; i < this.rtnDsTemp.Tables[0].Rows.Count; i++)
                    {
                        if (this.rtnDsTemp.Tables[0].Rows[i]["PARMENUID"].ToString() == menuid)
                        {
                            this.rtnDsTemp.Tables[0].Rows[i]["PARMENUID"] = parmenuid;
                        }
                    }
                    this.rtnDsTemp.Tables[0].Rows.Find(menuid).Delete();
                }
                //this.treMenu.SetDataBinding(rtnDsTemp, "Table1");
                this.treMenu.RefreshSort();
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// ToolBar의 저장 버튼 Click
        /// </summary>
        public override void DoSave()
        {
            if (this.grid1.Rows.Count == 0)
                return;
            this.txtProgramID.Focus();
            base.DoSave();
            this.treMenu.PerformAction(UltraTreeAction.DeactivateCell, false, true);
            ((CurrencyManager)this.BindingContext[this.bs]).EndCurrentEdit();
            USP_SY0100_CRUD(this.rtnDsTemp.Tables[0], Convert.ToString(this.grid1.ActiveRow.Cells["WorkerID"].Value));
        }
        #endregion

        #region < EVENT AREA >
        private void scboProgramIDCode1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (_sWorkerID != "")
                {
                    DataTable dtrslt;
                    string SelectCmd = "   SELECT *                      "
                                     + "     FROM TSY0100                "
                                     + "    WHERE WorkerID  = @WorkerID  "
                                     + "      AND ProgramID = @ProgramID ";

                    //DbCommand selectcommand = this.db.GetSqlStringCommand(SelectCmd);
                    //this.db.AddInParameter(selectcommand, "WorkerID", DbType.String, "SYSTEM");
                    //this.db.AddInParameter(selectcommand, "ProgramID", DbType.String, this.scboProgramIDCode1.Value);
                    //dtrslt = this.db.ExecuteDataSet(selectcommand).Tables[0];
                    //if (dtrslt.Rows.Count > 0)
                    //{
                    //    this.txtFileID.Text = dtrslt.Rows[0]["FILEID"].ToString();
                    //    this.txtNameSpace.Text = dtrslt.Rows[0]["NameSpace"].ToString();
                    //    this.txtProgramType.Text = dtrslt.Rows[0]["ProgType"].ToString();
                    //}
                }
            }
            catch (Exception)
            {
            }
        }
        UltraTreeNode DragNode = null;
        UltraTreeNode DropNode = null;
        private void treMenu_DragDrop(object sender, DragEventArgs e)
        {
            if (DragNode == DropNode)
            {
                DragNode = null;
                return;
            }
            try
            {
                DataRow dr = this.rtnDsTemp.Tables["Table1"].Rows.Find(DragNode.Cells["MenuID"].Value.ToString());
                DataRow dr1 = this.rtnDsTemp.Tables["Table1"].Rows.Find(DropNode.Cells["MenuID"].Value.ToString());
                if (dr1["MENUTYPE"].ToString() == "M" && ModifierKeys == Keys.Control)
                {
                    dr["Sort"] = 0;
                    dr["ParMenuID"] = DropNode.Cells["MenuID"].Value;
                    DropNode = DropNode.NextVisibleNode;


                }
                else
                {
                    dr["Sort"] = DropNode.Cells["Sort"].Value;
                    dr["ParMenuID"] = DropNode.Cells["ParMenuID"].Value;
                }
                while (DropNode != null)
                {
                    if (DropNode != DragNode)
                    {
                        dr1 = this.rtnDsTemp.Tables["Table1"].Rows.Find(DropNode.Cells["MenuID"].Value.ToString());
                        if (dr["ParMenuID"].ToString() != dr1["ParMenuID"].ToString())
                            break;
                        dr1["Sort"] = Convert.ToInt32(dr1["Sort"]) + 1;
                    }
                    DropNode = DropNode.NextVisibleNode;
                }

                this.treMenu.RefreshSort();
                //SetNodesSort(DropNode.Parent);
            }
            catch { }
            DragNode = null;
  
      /*      SelectedNodesCollection SelectedNodes;
            UltraTreeNode DropNode;
            int i;
            DropNode = //UltraTree_DropHightLight_DrawFilter.DropHightLightNode;

            SelectedNodes = (SelectedNodesCollection)e.Data.GetData(typeof(SelectedNodesCollection));
            SelectedNodes = SelectedNodes.Clone() as SelectedNodesCollection;

            SelectedNodes.SortByPosition();

            switch (UltraTree_DropHightLight_DrawFilter.DropLinePosition)
            {
                case DropLinePositionEnum.OnNode: //Drop ON the node
                    {
                        for (i = 0; i <= (SelectedNodes.Count - 1); i++)
                        {
                            this.rtnDsTemp.Tables["Table1"].Rows.Find(SelectedNodes[i].Cells["MenuID"].Value.ToString())["ParMenuID"] = DropNode.Cells["MenuID"].Value;
                            this.treMenu.RefreshSort();
                            SetNodesSort(SelectedNodes[i]);
                            //aNode = SelectedNodes[i];
                            //aNode.Reposition(DropNode.Nodes);
                        }
                        break;
                    }
                case DropLinePositionEnum.BelowNode: //Drop Below the node
                    {
                        for (i = 0; i <= (SelectedNodes.Count - 1); i++)
                        {
                            this.rtnDsTemp.Tables["Table1"].Rows.Find(SelectedNodes[i].Cells["MenuID"].Value.ToString())["Sort"] = Convert.ToInt32(DropNode.Cells["Sort"].Value) + 1;
                            this.rtnDsTemp.Tables["Table1"].Rows.Find(SelectedNodes[i].Cells["MenuID"].Value.ToString())["ParMenuID"] = DropNode.Cells["ParMenuID"].Value;
                            this.treMenu.RefreshSort();
                            SetNodesSort(SelectedNodes[i].Parent);
                        }
                        break;
                    }
                case DropLinePositionEnum.AboveNode: //New Index should be the same as the Drop
                    {
                        for (i = 0; i <= (SelectedNodes.Count - 1); i++)
                        {
                            this.rtnDsTemp.Tables["Table1"].Rows.Find(SelectedNodes[i].Cells["MenuID"].Value.ToString())["Sort"] = Convert.ToInt32(DropNode.Cells["Sort"].Value) - 1;
                            this.rtnDsTemp.Tables["Table1"].Rows.Find(SelectedNodes[i].Cells["MenuID"].Value.ToString())["ParMenuID"] = DropNode.Cells["ParMenuID"].Value;
                            this.treMenu.RefreshSort();
                            SetNodesSort(SelectedNodes[i].Parent);
                            //aNode = SelectedNodes[i];
                            //aNode.Reposition(DropNode, Infragistics.Win.UltraWinTree.NodePosition.Previous);

                        }
                        break;
                    }
            }
            this.treMenu.RefreshSort();
            UltraTree_DropHightLight_DrawFilter.ClearDropHighlight();

            DataRow [] row = this.rtnDsTemp.Tables[0].Select("PROGRAMNAME = '" + SelectedNodes.All.GetValue(0) + "'");

            try
            {
                this.LastActiveNode = Convert.ToString(row[0]["MenuID"]);
            }
            catch (Exception ex)
            {
            }*/
        }

        private void treMenu_DragLeave(object sender, EventArgs e)
        {
           // UltraTree_DropHightLight_DrawFilter.ClearDropHighlight();
        }

        private void treMenu_DragOver(object sender, DragEventArgs e)
        {
            UltraTreeNode aNode;
            //The Point that the mouse cursor is on, in Tree coords. 
            //This event passes X and Y in form coords. 
            System.Drawing.Point PointInTree;

            //Get the position of the mouse in the tree, as opposed
            //to form coords
            PointInTree = this.treMenu.PointToClient(new Point(e.X, e.Y));

            //Get the node the mouse is over.
            aNode = this.treMenu.GetNodeFromPoint(PointInTree);

            //Make sure the mouse is over a node
            if (aNode == null)
            {
                //The Mouse is not over a node
                //Do not allow dropping here
                e.Effect = DragDropEffects.None;
                //Erase any DropHighlight
              //  UltraTree_DropHightLight_DrawFilter.ClearDropHighlight();
                //Exit stage left
                return;
            }
            if (DragNode == null)
                DragNode = aNode;
            DropNode = aNode;


            //	Don't let continent nodes be dropped onto other continent nodes
            if (this.IsContinentNode(aNode) && this.IsContinentNodeSelected(this.treMenu))
            {
                if (PointInTree.Y > (aNode.Bounds.Top + 2) &&
                     PointInTree.Y < (aNode.Bounds.Bottom - 2))
                {
                    e.Effect = DragDropEffects.None;
                 //   UltraTree_DropHightLight_DrawFilter.ClearDropHighlight();
                    return;
                }
            }

            if (IsAnyParentSelected(aNode))
            {
                e.Effect = DragDropEffects.None;
               // UltraTree_DropHightLight_DrawFilter.ClearDropHighlight();
                return;
            }

          //  UltraTree_DropHightLight_DrawFilter.SetDropHighlightNode(aNode, PointInTree);
            e.Effect = DragDropEffects.Move;
        }

        private void treMenu_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            //Did the user press escape? 
            if (e.EscapePressed)
            {
                //User pressed escape
                //Cancel the Drag
                e.Action = DragAction.Cancel;
                //Clear the Drop highlight, since we are no longer
                //dragging
                //UltraTree_DropHightLight_DrawFilter.ClearDropHighlight();
            }
        }

        private void treMenu_SelectionDragStart(object sender, EventArgs e)
        {
            this.treMenu.DoDragDrop(this.treMenu.SelectedNodes, DragDropEffects.Move);
        }

        private void treMenu_ColumnSetGenerated(object sender, ColumnSetGeneratedEventArgs e)
        {
            e.ColumnSet.Columns["Sort"].SortType = SortType.Ascending;

            switch (e.ColumnSet.Key)
            {
                case "rel_db":
                    e.ColumnSet.NodeTextColumn = e.ColumnSet.Columns[this.rtnDsTemp.Tables["Table1"].Columns["MENUNAME"].ToString()];
                    break;
                case "Table1":
                    e.ColumnSet.NodeTextColumn = e.ColumnSet.Columns[this.rtnDsTemp.Tables["Table1"].Columns["MENUNAME"].ToString()];
                    break;
            }
        }

        private void treMenu_Click(object sender, EventArgs e)
        {
            if (this.treMenu.ActiveNode != null)
            {
                if (this.treMenu.ActiveNode.Cells["MenuID"].Value != null) this.bs.Position = this.bs.Find("MenuID", this.treMenu.ActiveNode.Cells["MenuID"].Value);
            }
        }

        private void treMenu_InitializeDataNode(object sender, Infragistics.Win.UltraWinTree.InitializeDataNodeEventArgs e)
        {
            if (e.Node.Parent == null && e.Node.Cells["ParMenuID"].Value.ToString() != "0")
            {
                e.Node.Visible = false;
                try
                {
                    e.Node.Key = Convert.ToString(e.Node.Cells["MENUID"].Value);
                }
                catch (Exception ex)
                {
                }
                return;
            };

            if (e.Node.Cells["MenuType"].Value.ToString() == "M")
            {
                e.Node.Override.NodeAppearance.Image = 0;
                try
                {
                    e.Node.Key = Convert.ToString(e.Node.Cells["MENUID"].Value);
                }
                catch (Exception ex)
                {
                }

                if (this._bSave)
                {
                    if (e.Node.Key == this.Ex_Node[this.ex_node_cnt])
                    {
                        e.Node.Expanded = true;
                        this.ex_node_cnt++;
                        this.node(e.Node);
                        this._bSave = false;
                    }
                }
            }
            else
            {
                e.Node.Override.NodeAppearance.Image = 1;
                try
                {
                    e.Node.Visible = true;
                    e.Node.Key = Convert.ToString(e.Node.Cells["MENUID"].Value);
                }
                catch (Exception ex)
                {
                }
            }
            e.Node.Override.ImageSize = new System.Drawing.Size(16, 16);
        }

        private void treMenu_MouseDown(object sender, MouseEventArgs e)
        {
            this.LastMouseDown = new System.Drawing.Point(e.X, e.Y);

            try
            {
                this.LastActiveNode = this.treMenu.GetNodeFromPoint(this.LastMouseDown).Key;
            }
            catch (Exception ex)
            {
            }
        }

        private void treMenu_MouseUp(object sender, MouseEventArgs e)
        {
            this.LastMouseDown = new System.Drawing.Point(e.X, e.Y);
        }

        private void grid1_AfterRowActivate(object sender, EventArgs e)
        {
            this.treeSetting();
        }

        #region LOAD
        private void SY0101_Load(object sender, EventArgs e)
        {
            #region Grid 셋팅
            _GridUtil.InitializeGrid(this.grid1, true, true, false, "", false);


            // InitColumnUltraGrid            
            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode",  "사업장",   true, GridColDataType_emu.VarChar, 125, 120, Infragistics.Win.HAlign.Left,   true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkerID",   "사용자ID", true, GridColDataType_emu.VarChar, 125, 100, Infragistics.Win.HAlign.Left,   true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkerName", "사용자명", true, GridColDataType_emu.VarChar, 130, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Pwd",        "비밀번호", true, GridColDataType_emu.VarChar, 160, 100, Infragistics.Win.HAlign.Left,   false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "GRPID",      "그룹명",   true, GridColDataType_emu.VarChar, 130, 100, Infragistics.Win.HAlign.Center, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Lang",       "Lang",    true, GridColDataType_emu.VarChar, 130, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "UseFlag",    "사용",    true, GridColDataType_emu.VarChar, 160, 100, Infragistics.Win.HAlign.Center,  true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Maker",      "등록자",   true, GridColDataType_emu.VarChar, 160, 100, Infragistics.Win.HAlign.Left,   false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MakeDate",   "등록일",   true, GridColDataType_emu.VarChar, 130, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Editor",     "수정자",   true, GridColDataType_emu.VarChar, 160, 100, Infragistics.Win.HAlign.Left,   false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "EditDate",   "수정일",   true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, false, true, null, null, null, null, null);

            _GridUtil.SetInitUltraGridBind(grid1);
            #endregion
            this.cboSystemType_H.Value = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).AppSettings.Settings["SYSTEMID"].Value;

            //this.treMenu.DrawFilter = UltraTree_DropHightLight_DrawFilter;
            //this.treMenu.Override.SelectionType = Infragistics.Win.UltraWinTree.SelectType.ExtendedAutoDrag;

            //this.treMenu.Appearances.Add("DropHighLightAppearance");
            //this.treMenu.Appearances["DropHighLightAppearance"].BackColor = Color.Cyan;
            binit = true;
        }
        #endregion

        private void UltraTree_DropHightLight_DrawFilter_Invalidate(object sender, System.EventArgs e)
        {
            //Any time the drophighlight changes, the control needs 
            //to know that it has to repaint. 
            //It would be more efficient to only invalidate the area
            //that needs it, but this works and is very clean.
            this.treMenu.Invalidate();
        }

        private void UltraTree_DropHightLight_DrawFilter_QueryStateAllowedForNode(Object sender, UltraTree_DropHightLight_DrawFilter_Class.QueryStateAllowedForNodeEventArgs e)
        {
            //	Don't let continent nodes be dropped onto other continent nodes
            if (this.IsContinentNode(e.Node) && this.IsContinentNodeSelected(this.treMenu))
            {
                e.StatesAllowed = DropLinePositionEnum.AboveNode | DropLinePositionEnum.BelowNode;
                return;
            }

            //Check to see if this is a continent node. 
            if (!IsContinentNode(e.Node))
            {
                //This is not a continent
                //Allow users to drop above or below this node - but not on
                //it, because countries don//t have child countries
                e.StatesAllowed = DropLinePositionEnum.AboveNode | DropLinePositionEnum.BelowNode;

                //Since we can only drop above or below this node, 
                //we don//t want a middle section. So we set the 
                //sensitivity to half the height of the node
                //This means the DrawFilter will respond to the top half
                //bottom half of the node, but not the middle. 
                //UltraTree_DropHightLight_DrawFilter.EdgeSensitivity = e.Node.Bounds.Height / 2;
            }
            else
            {
                if (e.Node.Selected)
                {
                    //This is a selected Continent node. 
                    //Since it is selected, we don't want to allow
                    //dropping ON this node. But we can allow the
                    //the user to drop above or below it. 
                    e.StatesAllowed = DropLinePositionEnum.AboveNode | DropLinePositionEnum.BelowNode;
                   // UltraTree_DropHightLight_DrawFilter.EdgeSensitivity = e.Node.Bounds.Height / 2;
                }
                else
                {
                    //This is a continent node and is not selected
                    //We can allow dropping here above, below, or on this
                    //node. Since the StatesAllow defaults to All, we don't 
                    //need to change it. 
                    //We set the EdgeSensitivity to 1/3 so that the 
                    //Drawfilter will respond at the top, bottom, or 
                    //middle of the node. 
                    //UltraTree_DropHightLight_DrawFilter.EdgeSensitivity = e.Node.Bounds.Height / 3;
                }
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            string[] para = new string[3];

            int idx = this.grid1.ActiveRow == null ? 0 : this.grid1.ActiveRow.Index;

            // 행이 없을 경우 SKIP
            if (this.grid1.Rows.Count == 0)
            {
                this.IsShowDialog = false;
                this.ShowDialog("C:R00111");

                return;
            }

            // 정보 넘김

            string sWorkerID = this.grid1.ActiveRow.Cells["WorkerID"].Value.ToString();
            string sWorkerName = this.grid1.ActiveRow.Cells["WorkerName"].Value.ToString();
            if (sWorkerID != "" && sWorkerName != "")
            {
                para[0] = sWorkerID;
                para[1] = sWorkerName;
                SY0201 Form = new SY0201(para);
                Form.ShowDialog();
            }
            else
            {
                this.ShowDialog("권한 대상자를 선택하십시오.");
            }
        }

        #endregion

        #region <METHOD AREA>
        // Form에서 사용할 함수나 메소드를 정의
        private bool IsContinentNode(UltraTreeNode Node)
        {
            //The Key of the node
            string NodeKey;
            //The beginning of the key
            string[] ParsedNodeKey;

            //Get the key of the node
            NodeKey = Node.Key;

            //Parse it out by colon
            ParsedNodeKey = NodeKey.Split(':');

            //If the beginning of the key says Continent, then 
            //we know it's a continent node. 
            return (ParsedNodeKey[0] == "Continent");
        }

        /// <summary>
        /// Returns whether any of the currently selected nodes in the
        /// specified UltraTree control represent continents.
        /// </summary>
        /// <param name="tree">The UltraTree control to evaluate.</param>
        /// <returns>A boolean indicating whether any continent nodes are selected.</returns>
        private bool IsContinentNodeSelected(UltraTree tree)
        {
            foreach (UltraTreeNode selectedNode in tree.SelectedNodes)
            {
                if (this.IsContinentNode(selectedNode))
                    return true;
            }

            return false;
        }

        private bool IsAnyParentSelected(UltraTreeNode Node)
        {
            UltraTreeNode ParentNode;
            bool ReturnValue = false;

            ParentNode = Node.Parent;
            while (ParentNode != null)
            {
                if (ParentNode.Selected)
                {
                    ReturnValue = true;
                    break;
                }
                else
                {
                    ParentNode = ParentNode.Parent;
                }
            }

            return ReturnValue;
        }

        private void treeSetting()
        {
            if (this.grid1.ActiveRow != null)
            {
                _sWorkerID = this.grid1.ActiveRow.Cells["WorkerID"].Value.ToString();

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                dt = this.USP_SY0101_S1();
                if (dt.Rows.Count > 0)
                {
                    if (this.flag == false)
                    {
                        DataTable dt1 = dt.Copy();
                        dt1.TableName = "Table1";
                        this.rtnDsTemp.Tables.Add(dt1);

                        rtnDsTemp.Tables[0].PrimaryKey = new DataColumn[] { rtnDsTemp.Tables[0].Columns["MenuID"] };
                        rtnDsTemp.Relations.Add("rel_db", rtnDsTemp.Tables["Table1"].Columns["MenuID"], rtnDsTemp.Tables["Table1"].Columns["ParMenuID"], false);

                        rtnDsTemp.Tables[0].DefaultView.Sort = "Sort";

                        this.treMenu.SetDataBinding(rtnDsTemp, "Table1");
                        this.bs.DataSource = rtnDsTemp;
                        this.bs.DataMember = "Table1";

                        this.rtnDsTemp.EnforceConstraints = false;
                        this.treMenu.SynchronizeCurrencyManager = true;

                        this.treMenu.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
                        this.treMenu.CausesValidation = false;
                        this.treMenu.Override.ShowColumns = Infragistics.Win.DefaultableBoolean.False;
                        this.treMenu.ColumnSettings.LabelPosition = NodeLayoutLabelPosition.None;
                        this.treMenu.ColumnSettings.ColumnAutoSizeMode = Infragistics.Win.UltraWinTree.ColumnAutoSizeMode.VisibleNodes;
                        this.treMenu.ColumnSettings.AllowSorting = Infragistics.Win.DefaultableBoolean.True;
                        this.treMenu.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
                        this.treMenu.AllowDrop = true;
                        this.treMenu.ScrollBounds = Infragistics.Win.UltraWinTree.ScrollBounds.ScrollToFill;
                        this.flag = true;
                        this.Binding();
                        return;
                    }
                    this.ActiveNode_Select();
                    this.rtnDsTemp.Tables[0].Rows.Clear();
                    DataTable dt2 = dt.Copy();
                    dt2.TableName = "Table1";
                    ds.Tables.Add(dt2);

                    this.rtnDsTemp.Merge(ds);

                    rtnDsTemp.Tables[0].DefaultView.Sort = "Sort";

                    this.treMenu.SetDataBinding(rtnDsTemp, "Table1");
                    this.bs.DataSource = rtnDsTemp;
                    this.bs.DataMember = "Table1";
                    this.Binding();
                }
                else
                {
                    this.treMenu.DataSource = null;
                }
            }
            else
            {
                _sWorkerID = "";
            }
        }

        void SetNodesSort(UltraTreeNode node)
        {

            if (node == null) return;


            for (int i = 0; i < node.Nodes.Count; i++)
            {
                this.rtnDsTemp.Tables[0].Rows.Find(node.Nodes[i].Cells["MenuID"].Value.ToString())["Sort"] = node.Nodes[i].Index;
            }
        }

        private void BindingClear()
        {
            #region 컨트롤 바인딩 (메뉴정보)
            txtMenuName.DataBindings.Clear();

            cboMenuType.DataBindings.Clear();

            UseFlag.DataBindings.Clear();

            txtRemark.DataBindings.Clear();

            txtProgramID.DataBindings.Clear();
            #endregion

            #region 프로그램정보
            txtProgramType.DataBindings.Clear();

            this.uceInqFlag.DataBindings.Clear();
            this.uceDelFlag.DataBindings.Clear();
            this.ucePrnFlag.DataBindings.Clear();
            this.uceNewFlag.DataBindings.Clear();
            this.uceSaveFlag.DataBindings.Clear();
            this.uceExcelFlag.DataBindings.Clear();

            txtNameSpace.DataBindings.Clear();

            txtFileID.DataBindings.Clear();

            txtRemark1.DataBindings.Clear();
            #endregion
        }

        private void Binding()
        {
            #region 컨트롤 바인딩 (메뉴정보)
            txtMenuName.DataBindings.Clear();
            txtMenuName.DataBindings.Add("Value", this.bs, "MENUNAME");

            cboMenuType.DataBindings.Clear();
            cboMenuType.DataBindings.Add("Value", this.bs, "MENUTYPE");

            UseFlag.DataBindings.Clear();
            UseFlag.DataBindings.Add("Value", this.bs, "USEFLAG");

            txtRemark.DataBindings.Clear();
            txtRemark.DataBindings.Add("Value", this.bs, "REMARK");

            txtProgramID.DataBindings.Clear();
            txtProgramID.DataBindings.Add("Value", this.bs, "PROGRAMID");
            #endregion

            #region 프로그램정보
            scboProgramIDCode1.DataBindings.Clear();
            scboProgramIDCode1.DataBindings.Add("Value", this.bs, "PROGRAMID");

            txtProgramType.DataBindings.Clear();
            txtProgramType.DataBindings.Add("Value", this.bs, "PROGTYPE");

            INQFLAG = new Binding("Checked", this.bs, "INQFLAG");
            DELFLAG = new Binding("Checked", this.bs, "DELFLAG");
            PRNFLAG = new Binding("Checked", this.bs, "PRNFLAG");
            NEWFLAG = new Binding("Checked", this.bs, "NEWFLAG");
            SAVEFLAG = new Binding("Checked", this.bs, "SAVEFLAG");
            EXCELFLAG = new Binding("Checked", this.bs, "EXCELFLAG");

            INQFLAG.Format += ComboBind;
            DELFLAG.Format += ComboBind;
            PRNFLAG.Format += ComboBind;
            NEWFLAG.Format += ComboBind;
            SAVEFLAG.Format += ComboBind;
            EXCELFLAG.Format += ComboBind;

            this.uceInqFlag.DataBindings.Clear();
            this.uceInqFlag.DataBindings.Add(INQFLAG);
            this.uceDelFlag.DataBindings.Clear();
            this.uceDelFlag.DataBindings.Add(DELFLAG);
            this.ucePrnFlag.DataBindings.Clear();
            this.ucePrnFlag.DataBindings.Add(PRNFLAG);
            this.uceNewFlag.DataBindings.Clear();
            this.uceNewFlag.DataBindings.Add(NEWFLAG);
            this.uceSaveFlag.DataBindings.Clear();
            this.uceSaveFlag.DataBindings.Add(SAVEFLAG);
            this.uceExcelFlag.DataBindings.Clear();
            this.uceExcelFlag.DataBindings.Add(EXCELFLAG);

            txtNameSpace.DataBindings.Clear();
            txtNameSpace.DataBindings.Add("Value", this.bs, "NAMESPACE");

            txtFileID.DataBindings.Clear();
            txtFileID.DataBindings.Add("Value", this.bs, "FILEID");

            txtRemark1.DataBindings.Clear();
            txtRemark1.DataBindings.Add("Value", this.bs, "PROGRAMREMARK");
            #endregion
        }

        private void ComboBind(object send, ConvertEventArgs e)
        {
            if (e.Value == DBNull.Value) e.Value = false;
            e.Value = (Convert.ToString(e.Value) != "0" && (Convert.ToString(e.Value) == "1" || Convert.ToBoolean(e.Value)));
        }

        #region USP_SY0200_S2
        private void USP_SY0200_S2()
        {
            string sWorkerID = Convert.ToString(this.txtWorkerID_H.Value);
            string sWorkerName = Convert.ToString(this.txtWorkerName_H.Value);
            string sUseFlag = Convert.ToString(this.cboUseFlag_H.Value) == "ALL" ? "" : Convert.ToString(this.cboUseFlag_H.Value);

            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[3];

            try
            {
                //param[0] = helper.CreateParameter("@Plantcode", "", SqlDbType.VarChar, ParameterDirection.Input);
                param[0] = helper.CreateParameter("@AS_WorkerID", sWorkerID, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@AS_WorkerName", sWorkerName, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@AS_UseFlag", sUseFlag, SqlDbType.VarChar, ParameterDirection.Input);

               this.grid1.DataSource = helper.FillTable("USP_SY0200_S2_New", CommandType.StoredProcedure, param);

                grid1.DataBind();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
        }
        #endregion

        #region USP_SY0101_S1
        private DataTable USP_SY0101_S1()
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[1];

            string workerid = Convert.ToString(this.grid1.ActiveRow.Cells["WORKERID"].Value);

            try
            {
                param[0] = helper.CreateParameter("@WORKERID", workerid, SqlDbType.VarChar, ParameterDirection.Input);
              //  param[1] = helper.CreateParameter("@SYSTEMID", cboSystemType_H.Value.ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                 //base.DoInquire();
                //this.grid1.DataSource = helper.FillTable("USP_SY0200_S2", CommandType.StoredProcedure
                //                                , helper.CreateParameter("WORKERID", sWorkerID, DbType.String, ParameterDirection.Input)
                //                                , helper.CreateParameter("WORKERNAME", sWorkerName, DbType.String, ParameterDirection.Input)
                //                                , helper.CreateParameter("USEFLAG", sUseFlag, DbType.String, ParameterDirection.Input));
                 return helper.FillTable("USP_SY0101_S1", CommandType.StoredProcedure, param);
                //return helper.FillTable("USP_SY0101_S1", CommandType.StoredProcedure
                //                                , helper.CreateParameter("WORKERID", workerid, DbType.String, ParameterDirection.Input)
                //                                , helper.CreateParameter("SYSTEMID", cboSystemType_H.Value.ToString(), DbType.String, ParameterDirection.Input)
                //                                );

            }
            catch (Exception ex)
            {
                //SAMMI.Windows.Forms.MessageForm msgform = new SAMMI.Windows.Forms.MessageForm(ex);
                //msgform.ShowDialog();
                return new DataTable();
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
        }
        #endregion

        #region 저장/수정/삭제
        public void USP_SY0100_CRUD(DataTable DtChange, string USER_ID)
        {
            if (DtChange.GetChanges() == null)
                return;
            //this.grid1.SetRow();
            SqlDBHelper helper = new SqlDBHelper(false, false);
            SqlParameter[] param = new SqlParameter[2];
          //  helper._sTran = helper._sConn.BeginTransaction();
            string workerid = Convert.ToString(this.grid1.ActiveRow.Cells["WORKERID"].Value);
            int menuid = 0;
            try
            {
                foreach (DataRow drRow in DtChange.GetChanges().Rows)
                {
                    switch (drRow.RowState)
                    {
                        case DataRowState.Deleted:
                            #region 삭제
                            drRow.RejectChanges();
                param[0] = helper.CreateParameter("@WORKERID", workerid, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@MENUID", Convert.ToString(drRow["MENUID"]), SqlDbType.VarChar, ParameterDirection.Input);
                helper.ExecuteNoneQuery("USP_SY0101_D1", CommandType.StoredProcedure, param);

                            //helper.ExecuteNoneQuery("USP_SY0101_D1", CommandType.StoredProcedure
                            //, helper.CreateParameter("WORKERID", workerid, DbType.String, ParameterDirection.Input)
                            //, helper.CreateParameter("MENUID", Convert.ToString(drRow["MENUID"]), DbType.String, ParameterDirection.Input));
                            #endregion
                             drRow.Delete();
                           break;

                        case DataRowState.Added:
                            #region [신규 추가]
                            //sList.Clear();
                            menuid = Convert.ToInt32(drRow["MENUID"]);
                       /*  helper.ExecuteNoneQuery("USP_SY0101_I1", CommandType.StoredProcedure
                            , helper.CreateParameter("AS_WORKERID", workerid, DbType.String, ParameterDirection.Input)
                            , helper.CreateParameter("AS_MENUID", menuid, DbType.Int32, ParameterDirection.Input)
                            , helper.CreateParameter("AS_MENUNAME", Convert.ToString(drRow["MENUNAME"]), DbType.String, ParameterDirection.Input)
                            , helper.CreateParameter("AS_PARMENUID", Convert.ToInt32(drRow["PARMENUID"]), DbType.Int32, ParameterDirection.Input)
                            , helper.CreateParameter("AS_SORT", Convert.ToInt32(drRow["SORT"]), DbType.Int32, ParameterDirection.Input)
                            , helper.CreateParameter("AS_PROGRAMID", Convert.ToString(drRow["PROGRAMID"]), DbType.String, ParameterDirection.Input)
                            , helper.CreateParameter("AS_MENUTYPE", Convert.ToString(drRow["MENUTYPE"]), DbType.String, ParameterDirection.Input)
                            , helper.CreateParameter("AS_USEFLAG", Convert.ToString(drRow["USEFLAG"]), DbType.String, ParameterDirection.Input)
                            , helper.CreateParameter("AS_REMARK", Convert.ToString(drRow["REMARK"]), DbType.String, ParameterDirection.Input)
                            , helper.CreateParameter("AS_MAKER", this.WorkerID, DbType.String, ParameterDirection.Input)
                            , helper.CreateParameter("AS_EDITOR", this.WorkerID, DbType.String, ParameterDirection.Input)
                            , helper.CreateParameter("AS_PROGRAMNAME", Convert.ToString(drRow["PROGRAMNAME"]), DbType.String, ParameterDirection.Input)
                            , helper.CreateParameter("AS_PROGTYPE", Convert.ToString(drRow["PROGTYPE"]), DbType.String, ParameterDirection.Input)
                            , helper.CreateParameter("AS_FILEID", Convert.ToString(drRow["FILEID"]), DbType.String, ParameterDirection.Input)
                            , helper.CreateParameter("AS_NAMESPACE", Convert.ToString(drRow["NAMESPACE"]), DbType.String, ParameterDirection.Input)
                            , helper.CreateParameter("AS_INQFLAG", Convert.ToString(drRow["INQFLAG"]), DbType.String, ParameterDirection.Input)
                            , helper.CreateParameter("AS_NEWFLAG", Convert.ToString(drRow["NEWFLAG"]), DbType.String, ParameterDirection.Input)
                            , helper.CreateParameter("AS_DELFLAG", Convert.ToString(drRow["DELFLAG"]), DbType.String, ParameterDirection.Input)
                            , helper.CreateParameter("AS_SAVEFLAG", Convert.ToString(drRow["SAVEFLAG"]), DbType.String, ParameterDirection.Input)
                            , helper.CreateParameter("AS_EXCELFLAG", Convert.ToString(drRow["EXCELFLAG"]), DbType.String, ParameterDirection.Input)
                            , helper.CreateParameter("AS_PRNFLAG", Convert.ToString(drRow["PRNFLAG"]), DbType.String, ParameterDirection.Input)
                            , helper.CreateParameter("AS_PROGRAMREMARK", Convert.ToString(drRow["PROGRAMREMARK"]), DbType.String, ParameterDirection.Input));
                            */
                            #endregion
                            break;

                        case DataRowState.Modified:
                            menuid = Convert.ToInt32(drRow["MENUID"]);
                            #region [수정]
                 param = new SqlParameter[22];
                         //helper.ExecuteNoneQuery("", CommandType.StoredProcedure
                            param[0] = helper.CreateParameter("WORKERID", workerid, SqlDbType.VarChar, ParameterDirection.Input);
                            param[1] = helper.CreateParameter("MENUID", menuid, SqlDbType.Int, ParameterDirection.Input);
                           param[2] = helper.CreateParameter("MENUNAME", Convert.ToString(drRow["MENUNAME"]), SqlDbType.VarChar, ParameterDirection.Input);
                          param[3] =  helper.CreateParameter("PARMENUID", Convert.ToInt32(drRow["PARMENUID"]), SqlDbType.Int, ParameterDirection.Input);
                          param[4] =  helper.CreateParameter("SORT", Convert.ToInt32(drRow["SORT"]), SqlDbType.Int, ParameterDirection.Input);
                          param[5] =  helper.CreateParameter("PROGRAMID", Convert.ToString(drRow["PROGRAMID"]), SqlDbType.VarChar, ParameterDirection.Input);
                          param[6] =  helper.CreateParameter("MENUTYPE", Convert.ToString(drRow["MENUTYPE"]), SqlDbType.VarChar, ParameterDirection.Input);
                          param[7] =  helper.CreateParameter("USEFLAG", Convert.ToString(drRow["USEFLAG"]), SqlDbType.VarChar, ParameterDirection.Input);
                          param[8] =  helper.CreateParameter("REMARK", Convert.ToString(drRow["REMARK"]), SqlDbType.VarChar, ParameterDirection.Input);
                          param[9] = helper.CreateParameter("MAKER", this.WorkerID, SqlDbType.VarChar, ParameterDirection.Input);
                          param[10] = helper.CreateParameter("EDITOR", this.WorkerID, SqlDbType.VarChar, ParameterDirection.Input);
                          param[11] =  helper.CreateParameter("PROGRAMNAME", Convert.ToString(drRow["PROGRAMNAME"]), SqlDbType.VarChar, ParameterDirection.Input);
                          param[12] =  helper.CreateParameter("PROGTYPE", Convert.ToString(drRow["PROGTYPE"]), SqlDbType.VarChar, ParameterDirection.Input);
                          param[13] =  helper.CreateParameter("FILEID", Convert.ToString(drRow["FILEID"]), SqlDbType.VarChar, ParameterDirection.Input);
                          param[14] =  helper.CreateParameter("NAMESPACE", Convert.ToString(drRow["NAMESPACE"]), SqlDbType.VarChar, ParameterDirection.Input);
                          param[15] =  helper.CreateParameter("INQFLAG", Convert.ToString(drRow["INQFLAG"]), SqlDbType.VarChar, ParameterDirection.Input);
                          param[16] =  helper.CreateParameter("NEWFLAG", Convert.ToString(drRow["NEWFLAG"]), SqlDbType.VarChar, ParameterDirection.Input);
                          param[17] =  helper.CreateParameter("DELFLAG", Convert.ToString(drRow["DELFLAG"]), SqlDbType.VarChar, ParameterDirection.Input);
                          param[18] =  helper.CreateParameter("SAVEFLAG", Convert.ToString(drRow["SAVEFLAG"]), SqlDbType.VarChar, ParameterDirection.Input);
                          param[19] =  helper.CreateParameter("EXCELFLAG", Convert.ToString(drRow["EXCELFLAG"]), SqlDbType.VarChar, ParameterDirection.Input);
                          param[20] =  helper.CreateParameter("PRNFLAG", Convert.ToString(drRow["PRNFLAG"]), SqlDbType.VarChar, ParameterDirection.Input);
                          param[21] =  helper.CreateParameter("PROGRAMREMARK", Convert.ToString(drRow["PROGRAMREMARK"]), SqlDbType.VarChar, ParameterDirection.Input);
                helper.ExecuteNoneQuery("USP_SY0101_U1", CommandType.StoredProcedure, param);
                     #endregion
                            break;
                    }
                }
                DtChange.AcceptChanges();
              //  helper._sTran.Commit();
            }
            catch (Exception ex)
            {
                CancelProcess = true;
             //   helper._sTran.Rollback();
                this.ShowDialog(ex.ToString());
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
        }
        #endregion

        #region 마지막 Node 위치 Active
        private void ActiveNode_Select()
        {
            this._bSave = false;
            this.ex_node_cnt = 0;
            // 마지막 Node 위치 Active
            if (this.LastActiveNode != "")
            {
                SqlDBHelper helper = new SqlDBHelper(true, false);
                SqlParameter[] param = new SqlParameter[2];
                string workerid = Convert.ToString(this.grid1.ActiveRow.Cells["WORKERID"].Value);
                string query = string.Empty;
                try
                {
                        query = " WITH TEMP                         "
                              + " AS                                "
                              + " (                                 "
                              + " 	SELECT 1 AS LEVEL, *            "
                              + " 	FROM tsy0200 A                  "
                              + " 	WHERE A.menuid   = @AS_MENUID   "
                              + " 	  and A.workerid = @AS_WORKERID "
                              + " 	UNION ALL                       "
                              + " 	SELECT LEVEL, B.*               "
                              + " 	FROM tsy0200 B JOIN TEMP C      "
                              + "         ON B.menuid = C.parmenuid "
                              + "     where b.workerid = 'system'   "
                              + " )                                 "
                              + " SELECT * FROM TEMP                "
                              + " order by LEVEL                    ";
                           param[0] = helper.CreateParameter("@AS_WORKERID", workerid, SqlDbType.VarChar, ParameterDirection.Input);
                        param[1] = helper.CreateParameter("@AS_MENUID", this.LastActiveNode, SqlDbType.VarChar, ParameterDirection.Input);
                       this.dtTemp =  helper.FillTable(query, CommandType.Text, param);
                      

                    if (this.dtTemp.Rows.Count > 0)
                    {
                        _bSave = true;
                    }
                    this.Ex_Node = new string[this.dtTemp.Rows.Count];
                    for (int i = 0; i < this.Ex_Node.Length; i++)
                    {
                        this.Ex_Node[i] = Convert.ToString(this.dtTemp.Rows[i]["MENUID"]);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    if (helper._sConn != null) { helper._sConn.Close(); }
                    if (param != null) { param = null; }
                }
            }
        }
        #endregion

        private void cboSystemType_H_ValueChanged(object sender, EventArgs e)
        {
            if (binit)
            {
//                this.treeSetting();
//                ClosePrgForm();
            }
        }
        #endregion
    }
}
