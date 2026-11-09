using Microsoft.AspNetCore.Http;
﻿using System;
using Microsoft.AspNetCore.Mvc;
using ERP.DatabaseAccessLayer.AppSetupGateway;
using ERP.BusinessLogicLayer.OperationManager;
using ERP.EntitiesModel.OperationModel;
using ERP.DatabaseAccessLayer.OperationGateway;
using ERP_Leather.ActionFilters;

namespace ERP_Leather.Controllers
{
    public class CLRequisitionForFinishingController : Controller
    {
        DalCLRequisitionForFinishing objDal = new DalCLRequisitionForFinishing();
        BllCLRequisitionForFinishing objBll = new BllCLRequisitionForFinishing();

        [HttpGet]
        [CheckUserAccess("CLRequisitionForFinishing/CLRequisitionForFinishing")]
        public ActionResult CLRequisitionForFinishing()
        {
            ViewBag.formTiltle = "Crust Leather Requisition For Finishing";
            ViewBag.ProductionFloor = DalCommon.GetStoreListForFixedCategoryType("Production", "FN Production");
            ViewBag.ConcernStore = DalCommon.GetStoreListForFixedCategoryType("Leather", "Crust");
            return View();
        }

        [HttpPost]
        [CheckUserAccess("CLRequisitionForFinishing/CLRequisitionForFinishing")]
        public ActionResult CLRequisitionForFinishing(PRDYearMonthCrustReqItem model)
        {
            var msg = objBll.Save(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")));

            var RequistionItemList = objDal.GetRequisitionItemList(model.RequisitionDateID.ToString());
            return Json(new { Msg = msg, RequistionItemList = RequistionItemList });
        }


        public ActionResult AddNewYearMonth(string _ScheduleYear, string _ScheduleMonth, string _ProductionFloor, string _ConcernStore)
        {
            return Json(new { Msg = objDal.AddNewYearMonth(_ScheduleYear, _ScheduleMonth, _ProductionFloor, _ConcernStore), YearMonID = objDal.ReturnYearMonthID() });
        }

        public ActionResult AddNewRequisitionDate(string _ScheduleID, string _RequiredDate)
        {
            var RequisitionDateID = objDal.AddNewRequisitionDate(_ScheduleID, _RequiredDate);
            var RequisitionNo = objDal.ReturnRequisitionNo(RequisitionDateID);
            return Json(new { RequisitionDateID = RequisitionDateID, RequisitionNo = RequisitionNo });
        }

        public ActionResult GetAllRequisitionDate(string _ScheduleID)
        {
            return Json(objDal.GetAllRequisitionDate(_ScheduleID));
        }

        public ActionResult GetAllActiveColor()
        {
            return Json(objDal.GetAllActiveColor());
        }

        public ActionResult GetAllActiveBuyer()
        {
            return Json(objDal.GetAllActiveBuyer());
        }

        public ActionResult GetCategoryWiseUnit(string _UnitCategory)
        {
            return Json(objDal.GetCategoryWiseUnit(_UnitCategory));
        }

        public ActionResult GetAllLeatherStatus()
        {
            return Json(objDal.GetAllLeatherStatus());
        }

        public ActionResult GetScheduleList(string _ScheduleYear, string _ScheduleMonth, string _ProductionFloor)
        {
            return Json(objDal.GetScheduleList(_ScheduleYear, _ScheduleMonth, _ProductionFloor));
        }

        public ActionResult GetColorListForScheduleItem(string _ScheduleItemID)
        {
            return Json(objDal.GetColorListForScheduleItem(_ScheduleItemID));
        }

        public ActionResult GetBuyerOrderList(string _BuyerID)
        {
            return Json(objDal.GetBuyerOrderList(_BuyerID));
        }

        public ActionResult GetColorListForOrderItem(string _BuyerOrderItemID)
        {
            return Json(objDal.GetColorListForOrderItem(_BuyerOrderItemID));
        }

        public ActionResult GetRequisitionItemList(string _RequisitionDateID)
        {
            return Json(objDal.GetRequisitionItemList(_RequisitionDateID));
        }

        public ActionResult GetColorListForRequisitionItem(string _RequisitionItemID)
        {
            return Json(objDal.GetColorListForRequisitionItem(_RequisitionItemID));
        }

        public ActionResult DeleteItem(string _RequisitionItemID, string _PageStatus)
        {
            bool CheckStatus = false; ;
            if (_PageStatus == "NCF" || _PageStatus == "Not Confirmed")
            {
                CheckStatus = objDal.DeleteItem(_RequisitionItemID);
                if (CheckStatus)
                {
                    return Json(new { Msg = "Success" });
                }
                else
                {
                    return Json(new { Msg = "Fail" });
                }
            }
            else if (_PageStatus == "CNF" || _PageStatus == "Confirmed")
            {
                return Json(new { Msg = "CNF" });
            }
            else
            {
                return Json(new { Msg = "Fail" });
            }



        }

        public ActionResult DeleteColorItem(string _ReqItemColorID, string _PageStatus)
        {
            bool CheckStatus = false; ;
            if (_PageStatus == "NCF" || _PageStatus == "Not Confirmed")
            {
                CheckStatus = objDal.DeleteColorItem(_ReqItemColorID);
                if (CheckStatus)
                {
                    return Json(new { Msg = "Success" });
                }
                else
                {
                    return Json(new { Msg = "Fail" });
                }
            }
            else if (_PageStatus == "CNF" || _PageStatus == "Confirmed")
            {
                return Json(new { Msg = "CNF" });
            }
            else
            {
                return Json(new { Msg = "Fail" });
            }
        }

        public ActionResult GetSearchInformation()
        {
            return Json(objDal.GetSearchInformation());
        }

        public ActionResult ConfirmRequisition(string _RequisitionDateID, string confirmComment)
        {
            bool checkConfirm = objDal.ConfirmRequisition(_RequisitionDateID, confirmComment);

            if (checkConfirm)
            {
                return Json(new { Msg = "Success" });
            }
            else
            {
                return Json(new { Msg = "Fail" });
            }
        }
	}
}