using Infragistics.Win.UltraWinTree;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SAMMI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.Common;
using SAMMI.Control;


namespace SAMMI.SY
{
    public partial class SY0100 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        #region <MEMBER AREA>

        private Database db;

        private SqlConnection conn;

        private SqlTransaction trans;

        private UltraTree_DropHightLight_DrawFilter_Class UltraTree_DropHightLight_DrawFilter =
            new UltraTree_DropHightLight_DrawFilter_Class();

        #endregion

        #region < CONSTRUCTOR >

        public SY0100()
        {
            InitializeComponent();

            UltraTree_DropHightLight_DrawFilter.Invalidate += new EventHandler(this.UltraTree_DropHightLight_DrawFilter_Invalidate);
            UltraTree_DropHightLight_DrawFilter.QueryStateAllowedForNode += new UltraTree_DropHightLight_DrawFilter_Class.QueryStateAllowedForNodeEventHandler(this.UltraTree_DropHightLight_DrawFilter_QueryStateAllowedForNode);


            this.db = DatabaseFactory.CreateDatabase();
            conn = (SqlConnection)this.db.CreateConnection();
            this.daTable2.Adapter.RowUpdating += new SqlRowUpdatingEventHandler(Adapter_RowUpdating);
            this.daTable2.Adapter.RowUpdated += new SqlRowUpdatedEventHandler(Adapter_RowUpdated);

            this.daTable2.Connection = conn;
        }

        #endregion

        #region <TOOL BAR AREA >

        /// <summary>
        /// ToolBar의 조회 버튼 클릭
        /// </summary>
        public override void DoInquire()
        {

            // 조회전 변경된 사항을 확인
            //try
            //{
            //    if ((this.ds.dTable1.HasErrors) || (this.ds.dTable1.GetChanges() != null))
            //    {
            //        if (this.ShowDialog("C:Q00009") == System.Windows.Forms.DialogResult.OK)
            //            this.DoSave();
            //    };
            //}
            //catch (Exception ex)
            //{
            //}

            base.DoInquire();

            //this.daTable1.Fill(this.ds.dTable1, this.WorkerID);
            //this.ds.dTable1.DefaultView.Sort = "Sort";
            this.daTable2.Fill(this.ds.dTable2, this.WorkerID, "SmartMES", "KO");
            this.ds.dTable2.DefaultView.Sort = "Sort";


        }

        private void treMenu_InitializeDataNode(
            object sender, Infragistics.Win.UltraWinTree.InitializeDataNodeEventArgs e)
        {
            if (e.Node.Parent == null && e.Node.Cells["ParMenuID"].Value.ToString() != "0")
            {
                e.Node.Visible = false;
                return;
            }
            ;

            //e.Node.Expanded = true;
            if (e.Node.Cells["MenuType"].Value.ToString() == "M")
            {
                e.Node.Override.NodeAppearance.Image = 0;
            }
            else
            {
                e.Node.Override.NodeAppearance.Image = 1;
            }
            e.Node.Override.ImageSize = new System.Drawing.Size(16, 16);
        }

        /// <summary>
        /// ToolBar의 신규 버튼 클릭
        /// </summary>
        public override void DoNew()
        {
            try
            {
                base.DoNew();

                //DataRow addrow = this.ds.dTable1.NewRow();
                //addrow["Sort"] = Convert.ToInt32(this.treMenu.ActiveNode.Cells["Sort"].Value) + 1;
                //addrow["WorkerID"] = "SYSTEM";
                //addrow["ParMenuID"] = this.treMenu.ActiveNode.Cells["ParMenuID"].Value;
                //this.ds.dTable1.Rows.Add(addrow);
                DataRow addrow = this.ds.dTable2.NewRow();
                addrow["Sort"] = Convert.ToInt32(this.treMenu.ActiveNode.Cells["Sort"].Value) + 1;
                addrow["WorkerID"] = "SYSTEM";
                addrow["ParMenuID"] = this.treMenu.ActiveNode.Cells["ParMenuID"].Value;
                this.ds.dTable2.Rows.Add(addrow);
            }
            catch (Exception )
            {
            }
        }

        /// <summary>
        /// ToolBar의 삭제 버튼 Click
        /// </summary>
        public override void DoDelete()
        {
            try
            {
                base.DoDelete();


                if (this.treMenu.ActiveNode == null) return;

                // 삭제하고자 하는 노드의 하위 노드를 삭제
                //for (int i = 0; i < this.ds.dTable1.Rows.Count; i++)
                //{
                //    if (this.ds.dTable1.Rows[i].RowState == DataRowState.Deleted) continue;

                //    if (this.ds.dTable1.Rows[i]["ParMenuID"].ToString() == this.treMenu.ActiveNode.Cells["MenuID"].Value.ToString())
                //    {
                //        this.ds.dTable1.Rows[i].Delete();
                //    }
                //}

                //this.ds.dTable1.Rows.Find(this.treMenu.ActiveNode.Cells["MenuID"].Value.ToString()).Delete();

                for (int i = 0; i < this.ds.dTable2.Rows.Count; i++)
                {
                    if (this.ds.dTable2.Rows[i].RowState == DataRowState.Deleted) continue;

                    if (this.ds.dTable2.Rows[i]["ParMenuID"].ToString() == this.treMenu.ActiveNode.Cells["MenuID"].Value.ToString())
                    {
                        this.ds.dTable2.Rows[i].Delete();
                    }
                }

                this.ds.dTable2.Rows.Find(this.treMenu.ActiveNode.Cells["MenuID"].Value.ToString()).Delete();
            }
            catch (Exception )
            {
            }
        }

        /// <summary>
        /// ToolBar의 저장 버튼 Click
        /// </summary>
        public override void DoSave()
        {
            try
            {
                ((CurrencyManager)this.BindingContext[bs]).EndCurrentEdit();
                if (this.ds.HasChanges())
                {
                    if (this.ShowDialog("C:Q00009") == System.Windows.Forms.DialogResult.Cancel) return;
                }
                else return;

                base.DoSave();
                this.bs.EndEdit();
                conn.Open();
                trans = conn.BeginTransaction();
                this.daTable2.Transaction = trans;
                this.daTable2.Adapter.Update(this.ds.dTable2);

                trans.Commit();
                conn.Close();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                if (conn.State == ConnectionState.Open) conn.Close();
                throw (ex);
            }
        }

        private void SetNodesSort(UltraTreeNode node)
        {

            if (node == null) return;


            for (int i = 0; i < node.Nodes.Count; i++)
            {
                this.ds.dTable2.Rows.Find(node.Nodes[i].Cells["MenuID"].Value.ToString())["Sort"] = node.Nodes[i].Index;
            }
        }

        #endregion

        #region < EVENT AREA >

        /// <summary>
        /// Form이 Close 되기전에 발생
        /// e.Cancel을 true로 설정 하면, Form이 close되지 않음
        /// 수정 내역이 있는지를 확인 후 저장여부를 물어보고 저장, 저장하지 않기, 또는 화면 닫기를 Cancel 함

        /// <summary>
        /// DATABASE UPDATE전 VALIDATEION CHECK 및 값을 수정한다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Adapter_RowUpdating(object sender, SqlRowUpdatingEventArgs e)
        {
            if (e.Row.RowState == DataRowState.Modified)
            {
                e.Command.Parameters["@Editor"].Value = this.WorkerID;
                e.Command.Parameters["@Lang"].Value = SAMMI.Common.Common.Lang;
                e.Command.Parameters["@SYSTEMID"].Value = "SmartMES";

                return;
            }

            if (e.Row.RowState == DataRowState.Added)
            {
                e.Command.Parameters["@Editor"].Value = this.WorkerID;
                e.Command.Parameters["@Maker"].Value = this.WorkerID;
                e.Command.Parameters["@Lang"].Value = SAMMI.Common.Common.Lang;
                e.Command.Parameters["@SYSTEMID"].Value = "SmartMES";
                return;
            }
        }

        /// <summary>
        /// 저장처리시 오류가 발생한 경우 오류 메세지에 대한 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Adapter_RowUpdated(object sender, SqlRowUpdatedEventArgs e)
        {
            if (e.Errors == null) return;

            switch (((SqlException)e.Errors).Number)
            {
                    // 중복
                case 2627:
                    e.Row.RowError = "중복데이터 입니다.";
                    throw (new SException("C:S00099", e.Errors));
                case 50000:
                    e.Row.RowError = "이미 등록된 프로그램 입니다.";
                    throw (new SException(((SqlException)e.Errors).Message, e.Errors));
                default:
                    break;
            }
        }

        private void treMenu_DragDrop(object sender, DragEventArgs e)
        {
            SelectedNodesCollection SelectedNodes;
            Infragistics.Win.UltraWinTree.UltraTreeNode DropNode;
            int i;

            DropNode = UltraTree_DropHightLight_DrawFilter.DropHightLightNode;

            SelectedNodes = (SelectedNodesCollection)e.Data.GetData(typeof(SelectedNodesCollection));
            SelectedNodes = SelectedNodes.Clone() as SelectedNodesCollection;

            SelectedNodes.SortByPosition();

            this.SetStatusBarMessage(UltraTree_DropHightLight_DrawFilter.DropLinePosition.ToString());

            switch (UltraTree_DropHightLight_DrawFilter.DropLinePosition)
            {
                case DropLinePositionEnum.OnNode: //Drop ON the node
                    {
                        for (i = 0; i <= (SelectedNodes.Count - 1); i++)
                        {
                            this.ds.dTable2.Rows.Find(SelectedNodes[i].Cells["MenuID"].Value.ToString())["ParMenuID"] =
                                DropNode.Cells["MenuID"].Value;
                            this.treMenu.RefreshSort();
                            SetNodesSort(SelectedNodes[i]);
                        }
                        break;
                    }
                case DropLinePositionEnum.BelowNode: //Drop Below the node
                    {
                        for (i = 0; i <= (SelectedNodes.Count - 1); i++)
                        {
                            this.ds.dTable2.Rows.Find(SelectedNodes[i].Cells["MenuID"].Value.ToString())["Sort"] =
                                Convert.ToInt32(DropNode.Cells["Sort"].Value) + 1;
                            if (DropNode.Cells["MenuType"].Value.ToString() == "M" && SelectedNodes[i].Cells["MenuType"].Value.ToString() == "P")
                            {
                                this.ds.dTable2.Rows.Find(SelectedNodes[i].Cells["MenuID"].Value.ToString())["ParMenuID"] =
                                    DropNode.Cells["MenuID"].Value;
                            }
                            else
                            {
                                this.ds.dTable2.Rows.Find(SelectedNodes[i].Cells["MenuID"].Value.ToString())["ParMenuID"] =
                                    DropNode.Cells["ParMenuID"].Value;
                            }
                            this.treMenu.RefreshSort();
                            SetNodesSort(SelectedNodes[i].Parent);
                        }
                        break;
                    }
                case DropLinePositionEnum.AboveNode: //New Index should be the same as the Drop
                    {
                        for (i = 0; i <= (SelectedNodes.Count - 1); i++)
                        {
                            this.ds.dTable2.Rows.Find(SelectedNodes[i].Cells["MenuID"].Value.ToString())["Sort"] =
                                Convert.ToInt32(DropNode.Cells["Sort"].Value) - 1;
                            this.ds.dTable2.Rows.Find(SelectedNodes[i].Cells["MenuID"].Value.ToString())["ParMenuID"] =
                                DropNode.Cells["ParMenuID"].Value;
                            this.treMenu.RefreshSort();
                            SetNodesSort(SelectedNodes[i].Parent);
                        }
                        break;
                    }
            }
            this.treMenu.RefreshSort();
            UltraTree_DropHightLight_DrawFilter.ClearDropHighlight();
        }

        private void treMenu_DragLeave(object sender, EventArgs e)
        {
            UltraTree_DropHightLight_DrawFilter.ClearDropHighlight();
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
                UltraTree_DropHightLight_DrawFilter.ClearDropHighlight();
                //Exit stage left
                return;
            }


            //	Don't let continent nodes be dropped onto other continent nodes
            if (this.IsContinentNode(aNode) && this.IsContinentNodeSelected(this.treMenu))
            {
                if (PointInTree.Y > (aNode.Bounds.Top + 2) && PointInTree.Y < (aNode.Bounds.Bottom - 2))
                {
                    e.Effect = DragDropEffects.None;
                    UltraTree_DropHightLight_DrawFilter.ClearDropHighlight();
                    return;
                }
            }

            if (IsAnyParentSelected(aNode))
            {
                e.Effect = DragDropEffects.None;
                UltraTree_DropHightLight_DrawFilter.ClearDropHighlight();
                return;
            }

            UltraTree_DropHightLight_DrawFilter.SetDropHighlightNode(aNode, PointInTree);
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
                UltraTree_DropHightLight_DrawFilter.ClearDropHighlight();
            }
        }

        private void treMenu_SelectionDragStart(object sender, EventArgs e)
        {
            this.treMenu.DoDragDrop(this.treMenu.SelectedNodes, DragDropEffects.Move);
        }

        private void treMenu_ColumnSetGenerated(object sender, ColumnSetGeneratedEventArgs e)
        {
            e.ColumnSet.Columns["Sort"].SortType = SortType.Ascending;
        }
        
        private void treMenu_Click(object sender, EventArgs e)
        {
            if (this.treMenu.ActiveNode != null)
            if (this.treMenu.ActiveNode.Cells["MenuID"].Value != null) this.bs.Position = this.bs.Find("MenuID", this.treMenu.ActiveNode.Cells["MenuID"].Value);
        }

        #endregion

        #region <METHOD AREA>

        // Form에서 사용할 함수나 메소드를 정의
        //Proc to check whether a node is a continent or not. 
        //This is necessary because we only want continents to be 
        //parent nodes. We don't want countries to contain other
        //countries
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
                if (this.IsContinentNode(selectedNode)) return true;
            }

            return false;
        }

        //Walks up the parent chain for a node to determine if any
        //of it's parent nodes are selected
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

        #endregion

        private void scboProgramIDCode1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dtrslt;
                string SelectCmd = "   SELECT *                      " + "     FROM TSY0100                "
                                   + "    WHERE WorkerID  = @WorkerID  " + "      AND ProgramID = @ProgramID ";

                DbCommand selectcommand = this.db.GetSqlStringCommand(SelectCmd);
                this.db.AddInParameter(selectcommand, "@WorkerID", DbType.String, "SYSTEM");
                this.db.AddInParameter(selectcommand, "@ProgramID", DbType.String, this.scboProgramIDCode1.Value);
                dtrslt = this.db.ExecuteDataSet(selectcommand).Tables[0];
                if (dtrslt.Rows.Count > 0)
                {
                    this.txtFileID.Text = dtrslt.Rows[0]["FILEID"].ToString();
                    this.txtNameSpace.Text = dtrslt.Rows[0]["NameSpace"].ToString();
                    this.txtProgramType.Text = dtrslt.Rows[0]["ProgType"].ToString();
                }
            }
            catch (Exception )
            {
            }
        }

        private void UltraTree_DropHightLight_DrawFilter_Invalidate(object sender, System.EventArgs e)
        {
            //Any time the drophighlight changes, the control needs 
            //to know that it has to repaint. 
            //It would be more efficient to only invalidate the area
            //that needs it, but this works and is very clean.
            this.treMenu.Invalidate();
        }

        //This event is fired by the DrawFilter to let us determine
        //what kinds of drops we want to allow on any particular node
        private void UltraTree_DropHightLight_DrawFilter_QueryStateAllowedForNode(
            Object sender, UltraTree_DropHightLight_DrawFilter_Class.QueryStateAllowedForNodeEventArgs e)
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
                UltraTree_DropHightLight_DrawFilter.EdgeSensitivity = e.Node.Bounds.Height / 2;
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
                    UltraTree_DropHightLight_DrawFilter.EdgeSensitivity = e.Node.Bounds.Height / 2;
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
                    UltraTree_DropHightLight_DrawFilter.EdgeSensitivity = e.Node.Bounds.Height / 3;
                }
            }
        }

        private void SY0100_Load(object sender, EventArgs e)
        {
            this.treMenu.DrawFilter = UltraTree_DropHightLight_DrawFilter;
            this.treMenu.Override.SelectionType = Infragistics.Win.UltraWinTree.SelectType.ExtendedAutoDrag;

            this.treMenu.Appearances.Add("DropHighLightAppearance");
            this.treMenu.Appearances["DropHighLightAppearance"].BackColor = Color.Cyan;
        }
    }
}
