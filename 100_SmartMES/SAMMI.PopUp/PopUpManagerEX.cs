using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAMMI.PopUp
{
    public class PopUpManagerEX : SAMMI.PopManager.PopUPManager
    {
        public override System.Data.DataTable OpenPopUp(string strPopUpName, string[] param)
        {

                System.Data.DataTable rtnDtTemp = null;
                switch (strPopUpName.Trim().ToUpper())
                {
                    // 품목코드 검색
                    case "ITEM":
                        rtnDtTemp = OpenPopupShow("SAMMI.PopUp", "POP_TBM0100", "품목정보 검색", param);
                        break;
                    case "TBM0101":
                        rtnDtTemp = OpenPopupShow("SAMMI.PopUp", "POP_TBM0101", "품목정보 검색", param);
                        break;
                   // 공정 검색
                    case "OP":
                        rtnDtTemp = OpenPopupShow("SAMMI.PopUp", "POP_TBM0400", "작업장정보 검색", param);
                        break;
                    //작업자 검색
                    case "TBM200":
                        rtnDtTemp = OpenPopupShow("SAMMI.PopUp", "POP_TBM0200", "작업자정보 검색", param);
                        break;
                    //거래선 검색
                    case "TBM300":
                        rtnDtTemp = OpenPopupShow("SAMMI.PopUp", "POP_TBM0300", "거래선정보 검색", param);
                        break;
                    // 거래선 검색 - 사업장 추가
                    case "TBM301":
                        rtnDtTemp = OpenPopupShow("SAMMI.PopUp", "POP_TBM0301", "거래선정보 검색", param);
                        break;
                    //공정(작업장) 검색
                    case "TBM400":
                        rtnDtTemp = OpenPopupShow("SAMMI.PopUp", "POP_TBM0400", "공정(작업장( 검색", param);
                        break;
                    //작업장 ( 라우팅 )
                    case "TBM401":
                        rtnDtTemp = OpenPopupShow("SAMMI.PopUp", "POP_TBM0401", "작업장(라우팅) 검색", param);
                        break;
                    //라인 검색
                    case "TBM500":
                        rtnDtTemp = OpenPopupShow("SAMMI.PopUp", "POP_TBM0500", "라인정보 검색", param);
                        break;
                    //WorkCenter 검색
                    case "TBM600":
                        rtnDtTemp = OpenPopupShow("SAMMI.PopUp", "POP_TBM0600", "작업라인정보 검색", param);
                        break;
                    //WorkCenterOP 검색
                    case "TBM610":
                        rtnDtTemp = OpenPopupShow("SAMMI.PopUp", "POP_TBM0610", "작업장 Operation 검색", param);
                        break;
                    //설비 검색
                    case "TBM700":
                        rtnDtTemp = OpenPopupShow("SAMMI.PopUp", "POP_TBM0700", "설비 검색", param);
                        break;
                    //창고 검색
                    case "TBM800":
                        rtnDtTemp = OpenPopupShow("SAMMI.PopUp", "POP_TBM0800", "창고정보 검색", param);
                        break;
                    //저장위치 검색
                    case "TBM900":
                        rtnDtTemp = OpenPopupShow("SAMMI.PopUp", "POP_TBM0900", "저장위치정보 검색", param);
                        break;
                    //불량 검색
                    case "TBM1000":
                        rtnDtTemp = OpenPopupShow("SAMMI.PopUp", "POP_TBM1000", "불량항목정보 검색", param);
                        break;
                    //품목별 불량 검색
                    case "TBM2500":
                        rtnDtTemp = OpenPopupShow("SAMMI.PopUp", "POP_TBM2500", "품목별 불량항목정보 검색", param);
                        break;
                    //비가동 검색
                    case "TBM1100":
                        rtnDtTemp = OpenPopupShow("SAMMI.PopUp", "POP_TBM1100", "비가동항목정보 검색", param);
                        break;
                    //검사항목 검색
                    case "TBM1500":
                        rtnDtTemp = OpenPopupShow("SAMMI.PopUp", "POP_TBM1500", "검사항목정보 검색", param);
                        break;
                    case "TBM1600":
                        rtnDtTemp = OpenPopupShow("SAMMI.PopUp", "POP_TBM1600", "금형항목정보 검색", param);
                        break;
                    //고장항목 검색
                    case "TBM3400":
                        rtnDtTemp = OpenPopupShow("SAMMI.PopUp", "POP_TBM3400", "고장항목정보 검색", param);
                        break;
                    //운행차량  검색
                    case "TBM3700":
                        rtnDtTemp = OpenPopupShow("SAMMI.PopUp", "POP_TBM3700", "운행차량정보 검색", param);
                        break;
                    //사유항목  검색
                    case "TBM4100":
                        rtnDtTemp = OpenPopupShow("SAMMI.PopUp", "POP_TBM4100", "사유항목정보 검색", param);
                        break;
                    //관리규격  검색
                    case "TBM5100":
                        rtnDtTemp = OpenPopupShow("SAMMI.PopUp", "POP_TBM5100", "과리규격 검색", param);
                        break;
                    //관리규격  검색
                    case "TBM5200":
                        rtnDtTemp = OpenPopupShow("SAMMI.PopUp", "POP_TBM5200", "특성규격 검색", param);
                        break;

                    case "TBM5210":
                        rtnDtTemp = OpenPopupShow("SAMMI.PopUp", "POP_TBM5210", "가공라인 검색", param);
                        break;

                    case "TBM0000":
                        rtnDtTemp = OpenPopupShow("SAMMI.PopUp", "POP_TBM0000", "검색", param);
                        break;

                    case "TCM0200":
                        rtnDtTemp = OpenPopupShow("SAMMI.PopUp", "POP_TCM0200", "검색", param);
                        break;

                    //툴 검색
                    case "TTO0100":
                        rtnDtTemp = OpenPopupShow("SAMMI.PopUp", "POP_TTO0100", "Tool 검색", param);
                        break;

                    //사용자 검색
                    case "TSY0200":
                        rtnDtTemp = OpenPopupShow("SAMMI.PopUp", "POP_TSY0200", "사용자 검색", param);
                        break;

                    case "TMO0000":
                        rtnDtTemp = OpenPopupShow("SAMMI.PopUp", "POP_TMO0000", "메시지 이미지 검색", param);
                        break;

                    case "ORDERNO_HG":
                        rtnDtTemp = OpenPopupShow("SAMMI.PopUp", "POP_ORDERNO_HG", "합금 작업지시서 검색", param);
                        break;
                }
            return rtnDtTemp;
        }
    }
}
