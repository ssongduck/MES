using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAMMI.Common;
using Infragistics.Win.UltraWinGrid;
using SAMMI.Windows.Forms;

namespace SAMMI.PopUp
{
    public class BizGridManagerEX : SAMMI.PopManager.BizGridManager
    {
        public BizGridManagerEX(SAMMI.Control.Grid grid0)
        {
            base.init(grid0);
        }

        static SAMMI.PopUp.PopUp_Biz _biz = new PopUp_Biz();

        public override void getPopupGrid(string sFunctionName, string sCode, string sName, string sValueCode, string sValueName, string[] aParam1, string[] sParam2 = null)
        {
             switch (sFunctionName)
            {
                case "TBM0100":
                    _biz.TBM0100_POP_Grid(sValueCode, sValueName, aParam1[0], aParam1[1], grid, sCode, sName);
                    break;

                case "TBM0101":
                    _biz.TBM0101_POP_Grid(sValueCode, sValueName, aParam1[0], aParam1[1], aParam1[2], grid, sCode, sName);
                    break;

                case "TBM0200":
                    _biz.TBM0200_POP_Grid(aParam1[0], aParam1[1], aParam1[2], aParam1[3], sValueCode, sValueName, aParam1[4], grid, sCode, sName);
                    break;
                case "TBM0300":
                    _biz.TBM0300_POP_Grid(sValueCode, sValueName, aParam1[0], aParam1[1], grid, sCode, sName);
                    break;
                case "TBM0301":
                    _biz.TBM0301_POP_Grid(sValueCode, sValueName, aParam1[0], aParam1[1], aParam1[2], grid, sCode, sName);
                    break;
                case "TBM0400":
                    _biz.TBM0400_POP_Grid(aParam1[0], sValueCode, sValueName, aParam1[1], grid, sCode, sName);
                    break;
                case "TBM0500":
                    _biz.TBM0500_POP_Grid(aParam1[0], aParam1[1], sValueCode, sValueName, grid, sCode, sName);
                    break;
                case "TBM0600":
                    _biz.TBM0600_POP_Grid(aParam1[0], sValueCode, sValueName, aParam1[1], aParam1[2], aParam1[3], grid, sCode, sName);
                    break;
                case "TBM0610":
                    //  _biz.TBM0610_POP_Grid(aParam1[0], sValueCode, sValueName, aParam1[1], grid, sCode, sName);
                    _biz.TBM0610_POP_Grid(aParam1[0], sValueCode, sValueName, aParam1[1], aParam1[2], grid, sCode, sName);
                    break;
                case "TBM5210":
                    //  _biz.TBM0610_POP_Grid(aParam1[0], sValueCode, sValueName, aParam1[1], grid, sCode, sName);
                    _biz.TBM5210_POP_Grid(aParam1[0], sValueCode, sValueName, aParam1[1], aParam1[2], grid, sCode, sName);
                    break;

                case "TBM0700":
                    _biz.TBM0700_POP_Grid(aParam1[0], sValueCode, sValueName, aParam1[1], aParam1[2], aParam1[3], aParam1[4], grid, sCode, sName);
                    break;
                case "TBM1000":
                    _biz.TBM1000_POP_Grid(aParam1[0], aParam1[1], "", "", aParam1[2], grid, sCode, sName, sParam2);
                    break;
                case "TBM2500":
                    _biz.TBM2500_POP_Grid(aParam1[0], aParam1[1], sValueCode, sValueName, aParam1[2], aParam1[3], aParam1[4], aParam1[5], aParam1[6], grid, sCode, sName, sParam2);
                    break;
                case "TBM1100":
                    _biz.TBM1100_POP_Grid(sValueCode, sValueName, aParam1[0], aParam1[1], aParam1[2], aParam1[3], grid, sCode, sName, sParam2);
                    break;
                case "TBM1500":
                    _biz.TBM1500_POP_Grid(aParam1[0], aParam1[1], aParam1[2], sValueCode, sValueName, aParam1[3], grid, sCode, sName, sParam2);
                    break;
                case "TBM1600":
                    _biz.TBM1600_POP_Grid(sValueCode, sValueName, aParam1[0], aParam1[1], grid, sCode, sName);
                    break;
                case "TBM0000":
                    _biz.TBM0000_POP_Grid(sValueCode, sValueName, aParam1[0], grid, sCode, sName);
                    break;
                case "TCM0200":
                    _biz.TCM0200_POP_Grid(sValueCode, sValueName, aParam1[0], grid, sCode, sName, aParam1[1]);
                    break;
                case "TTO0100":
                    _biz.TTO0100_POP_Grid(sValueCode, sValueName, aParam1[0], aParam1[1], aParam1[2], aParam1[3], grid, sCode, sName);
                    break;
                case "TTO0100_GetData":
                    _biz.TTO0100_POP_DataRow(sValueCode, sValueName, aParam1[0], aParam1[1], aParam1[2], aParam1[3], aParam1[4], grid.Rows[grid.ActiveRow.Index], sCode, sName, sParam2);
                    break;
                case "TMO0000":
                    _biz.TMO0000_POP_Grid(sValueCode, sValueName, grid, sCode, sName);
                    break;
                case "ORDERNO_HG":
                    _biz.ORDERNO_HG_POP_Grid(aParam1[1], aParam1[2], aParam1[0], grid, sCode, sName);
                    break;
                default:
                    DialogForm dialogform;

                    dialogform = new DialogForm("C:S00014");

                    dialogform.ShowDialog();

                    break;
            }

        }
    }
}
