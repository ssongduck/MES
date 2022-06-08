using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAMMI.Windows.Forms;

namespace SAMMI.PopUp
{
    public class BizTextBoxManagerEX : SAMMI.PopManager.BizTextBoxManager
    {

        static SAMMI.PopUp.PopUp_Biz _biz = new PopUp_Biz();
        public override void Bz_Pop(string sFunctionName, System.Windows.Forms.TextBox tCodeBox, System.Windows.Forms.TextBox tNameBox, string sValueCode, string sValueName, string[] aParam, string[] ColumnList, object[] ObjectList)
        {
           // System.Windows.Forms.MessageBox.Show(sFunctionName);
            switch (sFunctionName)
            {
                case "TBM0100":
                    // ITEM_CD, ITEM_NAME, PLANT_CD, ITEM_TYPE, TextBox1, TextBox2 
                    _biz.TBM0100_POP(sValueCode, sValueName, aParam[0], aParam[1], tCodeBox, tNameBox);
                    break;

                case "TBM0101":
                    // ITEM_CD, ITEM_NAME, PLANT_CD, ITEM_TYPE, TextBox1, TextBox2 

                    if (aParam.Length == 4)
                    {
                        _biz.TBM0101_POP(sValueCode, sValueName, aParam[0], aParam[1], aParam[2], aParam[3], tCodeBox, tNameBox);
                        break;    
                    }
                    else
                    {
                        _biz.TBM0101_POP(sValueCode, sValueName, aParam[0], aParam[1], aParam[2], string.Empty,  tCodeBox, tNameBox);
                        break;
                    }
                case "TBM0200":
                    // PlantCode, OPCode, LineCode, WorkCenterCode, WorkerID, WorkerName, UseFlag, TextBox1, TextBox2
                    _biz.TBM0200_POP(aParam[0], aParam[1], aParam[2], aParam[3], sValueCode, sValueName, aParam[4], aParam[5], tCodeBox, tNameBox);
                    break;
                case "TBM0300":
                    // CustCode, CustName, CustType, UseFlag, TextBox1, TextBox2
                    _biz.TBM0300_POP(sValueCode, sValueName, aParam[0], aParam[1], tCodeBox, tNameBox);
                    break;
                case "TBM0301":
                    // CustCode, CustName, CustType, UseFlag, TextBox1, TextBox2
                    _biz.TBM0301_POP(sValueCode, sValueName, aParam[0], aParam[1], aParam[2], tCodeBox, tNameBox);
                    break;
                case "TBM0400":
                    // PlantCode, OPCode, OPName, UseFlag, TextBox1, TextBox2
                    _biz.TBM0400_POP(aParam[0], sValueCode, sValueName, aParam[1], tCodeBox, tNameBox);
                    break;
                case "TBM0401":
                    // PlantCode, OPCode, OPName, UseFlag, TextBox1, TextBox2
                    _biz.TBM0401_POP(aParam[0], sValueCode, sValueName, aParam[1], aParam[2], aParam[3], tCodeBox, tNameBox);
                    break;
                case "TBM0500":
                    // PlantCode, LineCode, LineName, UseFlag, TextBox1, TextBox2
                    // PlantCode , OPCode , LineCode, LineName, UseFlag, txtbox1, txtbox2
                    _biz.TBM0500_POP(aParam[0], aParam[1], sValueCode, sValueName, aParam[2], tCodeBox, tNameBox);
                    break;
                case "TBM0501":
                    // PlantCode, LineCode, LineName, UseFlag, TextBox1, TextBox2
                    // PlantCode , OPCode , LineCode, LineName, UseFlag, txtbox1, txtbox2
                    _biz.TBM0501_POP(aParam[0], aParam[1], sValueCode, sValueName, aParam[2], tCodeBox, tNameBox, ColumnList, ObjectList);
                    break;
                case "TBM0600":
                    // sPlantCode, sWorkCenterCode, sWorkCenterName, sOpCode, sLineCode, sUseFlag, TextBox1, TextBox2

                    _biz.TBM0600_POP(aParam[0], sValueCode, sValueName, aParam[1], aParam[2], aParam[3], tCodeBox, tNameBox, ColumnList, ObjectList);
                    break;
                //차승영 팝업 추가
                case "TBM0610":
                    _biz.TBM0610_POP(aParam[0], sValueCode, sValueName, aParam[1], aParam[2], tCodeBox, tNameBox, ColumnList, ObjectList);
                    break;

                case "TBM5210":
                    _biz.TBM5210_POP(aParam[0], sValueCode, sValueName, aParam[1], aParam[2], tCodeBox, tNameBox, ColumnList, ObjectList);
                    break;

                case "TBM0700":
                    // sMachCode, sMachname, sMachType, sMachType1, sMachType2, sUseFlag, TextBox1, TextBox2
                    if (ObjectList != null)
                        _biz.TBM0700_POP(aParam[0], sValueCode, sValueName, aParam[1], aParam[2], aParam[3], aParam[4], tCodeBox, tNameBox, ObjectList[0], ObjectList[1]);
                    else
                        _biz.TBM0700_POP(aParam[0], sValueCode, sValueName, aParam[1], aParam[2], aParam[3], aParam[4], tCodeBox, tNameBox);

                    break;
                case "TBM0800":
                    // sPlantCode, sWHCode, sWHName, sBaseWHFlag, sProdWHFlag, sMetWHFlag, sUseFlag, TextBox1, TextBox2
                    _biz.TBM0800_POP(aParam[0], sValueCode, sValueName, aParam[1], aParam[2], aParam[3], aParam[4], tCodeBox, tNameBox);
                    break;
                case "TBM0900":
                    // PlantCode, WHCode, StorageLOCCode, StorageLOCName, StorageLOCType, UseFlag, TextBox1, TextBox2
                    _biz.TBM0900_POP(aParam[0], aParam[1], sValueCode, sValueName, aParam[2], aParam[3], tCodeBox, tNameBox);
                    break;
                case "TBM1000":
                    // ErrorType, ErrorClass, ErrorCode, ErrorDesc, UseFlag, TextBox1, TextBox2
                    _biz.TBM1000_POP(aParam[0], aParam[1], sValueCode, sValueName, aParam[2], tCodeBox, tNameBox);
                    break;
                case "TBM1100":
                    //  StopCode, StopDesc, PlantCode, StopType, StopClass, UseFlag, TextBox1, TextBox2
                    _biz.TBM1100_POP(sValueCode, sValueName, aParam[0], aParam[1], aParam[2], aParam[3], tCodeBox, tNameBox);
                    break;
                case "TBM1500":
                    // PlantCode, inspCase, inspType, inspCode, inspName, UseFlag, TextBox1, TextBox2
                    _biz.TBM1500_POP(aParam[0], aParam[1], aParam[2], sValueCode, sValueName, aParam[3], tCodeBox, tNameBox);
                    break;

                case "TBM1600":
                    // ITEM_CD, ITEM_NAME, PLANT_CD, ITEM_TYPE, TextBox1, TextBox2 
                    _biz.TBM1600_POP(sValueCode, sValueName, aParam[0], aParam[1], tCodeBox, tNameBox);
                    break;

                case "TBM3400":
                    // sFaultType,sFaultCode,FaultName,sUseFlag,
                    _biz.TBM3400_POP(aParam[0], sValueCode, sValueName, aParam[1], tCodeBox, tNameBox);
                    break;

                case "TBM0000":
                    // Code, CodeName, MajorCode, TextBox1, TextBox2
                    _biz.TBM0000_POP(sValueCode, sValueName, aParam[0], tCodeBox, tNameBox);
                    break;
                case "TCM0200":
                    // Code, CodeName, MajorCode, TextBox1, TextBox2
                    _biz.TCM0200_POP(sValueCode, sValueName, aParam[0], tCodeBox, tNameBox, aParam[1]);
                    break;
                case "TTO0100":
                    // Code, CodeName, PlantCode, OPCode, WorkCenterCode,MachCode, TextBox1, TextBox2
                    _biz.TTO0100_POP(sValueCode, sValueName, aParam[0], aParam[1], aParam[2], aParam[3], tCodeBox, tNameBox);
                    break;
                default:
                    DialogForm dialogform;

                    dialogform = new DialogForm("C:S00014");

                    dialogform.ShowDialog();

                    break;
            }
            //base.Bz_Pop(sFunctionName, tCodeBox, tNameBox, sValueCode, sValueName, aParam, ColumnList, ObjectList);
        }

    }
}
