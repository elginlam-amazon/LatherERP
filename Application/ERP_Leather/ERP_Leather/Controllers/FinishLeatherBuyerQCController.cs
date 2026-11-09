using Microsoft.AspNetCore.Http;
﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ERP.DatabaseAccessLayer.AppSetupGateway;
using ERP.DatabaseAccessLayer.OperationGateway;
using ERP.EntitiesModel.OperationModel;
using ERP.EntitiesModel.BaseModel;
using ERP_Leather.ActionFilters;

namespace ERP_Leather.Controllers
{
    public class FinishLeatherBuyerQCController : Controller
    {
        DalSysStore objDalSysStore = new DalSysStore();
        private DalFinishLeatherBuyerQC Dalobject;
        private ValidationMsg _vmMsg;

        public FinishLeatherBuyerQCController()
        {
            _vmMsg = new ValidationMsg();
            Dalobject = new DalFinishLeatherBuyerQC();
        }

        [CheckUserAccess("FinishLeatherBuyerQC/FinishLeatherBuyerQC")]
        public ActionResult FinishLeatherBuyerQC()
        {
            ViewBag.formTiltle = "Finish Leather Buyer QC";
            ViewBag.ddlBuyerQCStoreList = objDalSysStore.GetAllActiveFinishBuyerQCStore();
            ViewBag.ddlPackingStoreList = objDalSysStore.GetAllActivePackingStores();
            ViewBag.ddlFinishStoreList = objDalSysStore.GetAllActiveStoreTransferReceiveStore();

            return View();
        }
        [HttpPost]
        public ActionResult FinishLeatherBuyerQC(PrdCrustLeatherQC model)
        {
            _vmMsg = model.CrustLeatherQCID == 0 ? Dalobject.Save(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")), "FinishLeatherQC/FinishLeatherQC") : Dalobject.Update(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
            //_vmMsg = model.CrustLeatherQCID == 0 ? Dalobject.Save(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")), "ChemicalConsumption/ChemConsumption") : Dalobject.Update(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
            return Json(new { CrustLeatherQCID = Dalobject.GetQCID(), CrustLeatherQCNo = Dalobject.GetQCNo(), msg = _vmMsg });
        }

        public ActionResult GetFinishIssueToList(string QCTransactionOf)
        {
            switch (QCTransactionOf)
            {
                case "FOQC":
                    {
                        var ddlFinishOwnQCStoreList = objDalSysStore.GetAllActiveFinishOwnQCStore();
                        return Json(ddlFinishOwnQCStoreList);
                    }
                case "FBQC":
                    {
                        var ddlFinishBuyerQCStoreList = objDalSysStore.GetAllActiveFinishBuyerQCStore();
                        return Json(ddlFinishBuyerQCStoreList);
                    }
                default:
                    return null;
            }
        }

        public ActionResult GetQCScheduleItemInfo(string ProductionFloor)
        {
            var crustYearMonthScheduleList = Dalobject.GetScheduleItemInfo(ProductionFloor);
            return Json(crustYearMonthScheduleList);
        }

        public ActionResult GetCrustLeatherQCInfo()
        {
            var crustLeatherQCInfoList = Dalobject.GetCrustLeatherQCInfo();
            return Json(crustLeatherQCInfoList);
        }

        public ActionResult GetColorAndGradeList(string ProductionFloor, string BuyerID, string BuyerOrderID, string ArticleID, string ItemTypeID, string LeatherStatusID, string ColorID, string ArticleChallanID)
        {
            PrdCrustLeatherQC model = new PrdCrustLeatherQC();
            //if (string.IsNullOrEmpty(ScheduleItemID)) return Json(model);
            model.PrdCrustLeatherQCItemList = Dalobject.GetScheduleColorList(ProductionFloor, BuyerID, BuyerOrderID, ArticleID, ItemTypeID, LeatherStatusID, ColorID, ArticleChallanID);
            if (model.PrdCrustLeatherQCItemList.Count > 0)
                model.PrdCrustLeatherQCSelectionList = Dalobject.GetScheduleGradeList(ProductionFloor, BuyerID, BuyerOrderID, ArticleID, ItemTypeID, LeatherStatusID, ColorID, ArticleChallanID);
            return Json(model);
        }

        public ActionResult GetAllGridList(string CrustLeatherQCID, string StoreId)
        {
            PrdCrustLeatherQC model = new PrdCrustLeatherQC();
            if (string.IsNullOrEmpty(CrustLeatherQCID)) return Json(model);
            model.PrdCrustLeatherQCItemList = Dalobject.GetQCColorList(CrustLeatherQCID);
            if (model.PrdCrustLeatherQCItemList.Count > 0)
                model.PrdCrustLeatherQCSelectionList = Dalobject.GetQCSelectionList(model.PrdCrustLeatherQCItemList[0].CLQCItemID.ToString(), StoreId);
            return Json(model);
        }

        [HttpPost]
        public ActionResult ConfirmedCrustQC(PrdCrustLeatherQC model)
        {
            _vmMsg = Dalobject.ConfirmedCrustQC(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
            return Json(new { msg = _vmMsg });
        }

        [HttpPost]
        public ActionResult CheckedCrustProductionSchedule(PrdCrustLeatherQC model)
        {
            _vmMsg = Dalobject.CheckedCrustProductionSchedule(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
            return Json(new { msg = _vmMsg });
        }

        [HttpPost]
        public ActionResult DeletedCrustLeatherQC(string CrustLeatherQCID)
        {
            long crustLeatherQCID = Convert.ToInt64(CrustLeatherQCID);
            _vmMsg = Dalobject.DeletedCrustLeatherQC(crustLeatherQCID);
            return Json(new { msg = _vmMsg });
        }

        [HttpPost]
        public ActionResult DeletedCrustQCItem(string CLQCItemID)
        {
            long itemID = Convert.ToInt64(CLQCItemID);
            //_vmMsg = Dalobject.DeletedCrustQCItem(scheduleDateID, RecordStatus);
            _vmMsg = Dalobject.DeletedCrustQCItem(itemID);
            return Json(new { msg = _vmMsg });
        }

        [HttpPost]
        public ActionResult DeletedQCSelection(string CLQCSelectionID)
        {
            long QCSelectionID = Convert.ToInt64(CLQCSelectionID);
            //_vmMsg = Dalobject.DeletedScheduleDrum(schedulePurchaseID, RecordStatus);
            _vmMsg = Dalobject.DeletedQCSelection(QCSelectionID);
            return Json(new { msg = _vmMsg });
        }

        public ActionResult GetGradeInfoInFromQCStock(string ProductionFloor, string BuyerID, string BuyerOrderID, string ArticleID, string ItemTypeID, string LeatherStatusID, string ColorID, string ArticleChallanID)
        {
            var gradeList = Dalobject.GetGradeInfoInFromQCStock(ProductionFloor, BuyerID, BuyerOrderID, ArticleID, ItemTypeID, LeatherStatusID, ColorID, ArticleChallanID);
            return Json(gradeList);
        }
    }
}