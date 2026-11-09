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
    public class IssueAfterFinishQCController : Controller
    {
        DalSysStore objDalSysStore = new DalSysStore();

        private DalIssueAfterFinishQC Dalobject;
        private ValidationMsg _vmMsg;

        public IssueAfterFinishQCController()
        {
            _vmMsg = new ValidationMsg();
            Dalobject = new DalIssueAfterFinishQC();
        }
        [CheckUserAccess("IssueAfterFinishQC/IssueAfterFinishQC")]
        public ActionResult IssueAfterFinishQC()
        {
            ViewBag.formTiltle = "Issue From Finish Store";
            ViewBag.ddlFinishStoreList = objDalSysStore.GetAllActiveStoreTransferReceiveStore();
            return View();
        }

        [HttpPost]
        public ActionResult IssueAfterFinishQC(InvCrustLeatherIssue model)
        {
            _vmMsg = model.CrustLeatherIssueID == 0 ? Dalobject.Save(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")), "IssueAfterFinishQC/IssueAfterFinishQC") : Dalobject.Update(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
            //_vmMsg = model.CrustLeatherIssueID == 0 ? Dalobject.Save(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")), "CrustLeatherQC/CrustLeatherQC") : Dalobject.Update(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
            return Json(new { CrustLeatherIssueID = Dalobject.GetQCID(), CrustLeatherQCNo = Dalobject.GetQCNo(), msg = _vmMsg });
        }

        public ActionResult GetFinishIssueFromList(string QCTransactionOf)
        {
            switch (QCTransactionOf)
            {
                case "AOQC":
                    {
                        var ddlFinishOwnQCStoreList = objDalSysStore.GetAllActiveFinishOwnQCStore();
                        return Json(ddlFinishOwnQCStoreList);
                    }
                case "ABQC":
                    {
                        var ddlFinishBuyerQCStoreList = objDalSysStore.GetAllActiveFinishBuyerQCStore();
                        return Json(ddlFinishBuyerQCStoreList);
                    }
                default:
                    return null;
            }
        }

        public ActionResult GetFinishIssueToList(string IssueFor)
        {
            switch (IssueFor)
            {
                case "Buyer QC":
                    {
                        var ddlFinishOwnQCStoreList = objDalSysStore.GetAllActiveFinishBuyerQCStore();
                        return Json(ddlFinishOwnQCStoreList);
                    }
                case "Finish":
                    {
                        var ddlFinishBuyerQCStoreList = objDalSysStore.GetAllActiveStoreTransferReceiveStore();
                        return Json(ddlFinishBuyerQCStoreList);
                    }
                case "Packing":
                    {
                        var ddlFinishBuyerQCStoreList = objDalSysStore.GetAllActivePackingStores();
                        return Json(ddlFinishBuyerQCStoreList);
                    }
                default:
                    return null;
            }
        }

        //public ActionResult GetQCItemList(string IssueFrom, string IssueCategory, string CrustQCLabel)
            public ActionResult GetQCItemList(string IssueFrom, string IssueCategory)
        {
            //var crustYearMonthScheduleList = Dalobject.GetScheduleItemInfo(IssueFrom, IssueCategory, CrustQCLabel);
            var crustYearMonthScheduleList = Dalobject.GetScheduleItemInfo(IssueFrom, IssueCategory);
            return Json(crustYearMonthScheduleList);
        }

        public ActionResult GetColorAndGradeList(string IssueFrom, string BuyerID, string BuyerOrderID, string ArticleID, string ItemTypeID, string LeatherStatusID, string ArticleChallanID, string IssueCategory)
        {
            var colorAndGradeList = Dalobject.GetColorAndGradeList(IssueFrom, BuyerID, BuyerOrderID, ArticleID, ItemTypeID, LeatherStatusID, ArticleChallanID, IssueCategory);
            return Json(colorAndGradeList);
        }

        public ActionResult GetAllGridList(string CrustLeatherIssueID, string StoreId, string QCTransactionOf)
        {
            InvCrustLeatherIssue model = new InvCrustLeatherIssue();
            if (string.IsNullOrEmpty(CrustLeatherIssueID)) return Json(model);
            model.IssueItemList = Dalobject.GetQCColorList(CrustLeatherIssueID);
            if (model.IssueItemList.Count > 0)
                model.IssueColorList = Dalobject.GetQCSelectionList(model.IssueItemList[0].CrustLeatherIssueItemID.ToString(), StoreId, QCTransactionOf);
            return Json(model);
        }

        [HttpPost]
        public ActionResult ConfirmedAfterCrustQC(InvCrustLeatherIssue model)
        {
            _vmMsg = Dalobject.ConfirmedAfterCrustQC(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
            return Json(new { msg = _vmMsg });
        }

        public ActionResult GetCrustLeatherIssueAfterQCInfo()
        {
            var crustLeatherQCInfoList = Dalobject.GetCrustLeatherIssueAfterQCInfo();
            return Json(crustLeatherQCInfoList);
        }

        [HttpPost]
        public ActionResult DeletedCrustLeatherIssue(string CrustLeatherQCID)
        {
            long crustLeatherQCID = Convert.ToInt64(CrustLeatherQCID);
            _vmMsg = Dalobject.DeletedCrustLeatherIssue(crustLeatherQCID);
            return Json(new { msg = _vmMsg });
        }

        [HttpPost]
        public ActionResult DeletedCrustLeatherIssueItem(string CLQCItemID)
        {
            long itemID = Convert.ToInt64(CLQCItemID);
            //_vmMsg = Dalobject.DeletedCrustQCItem(scheduleDateID, RecordStatus);
            _vmMsg = Dalobject.DeletedCrustLeatherIssueItem(itemID);
            return Json(new { msg = _vmMsg });
        }

        [HttpPost]
        public ActionResult DeletedCrustLeatherIssueColor(string CLQCSelectionID)
        {
            long QCSelectionID = Convert.ToInt64(CLQCSelectionID);
            //_vmMsg = Dalobject.DeletedScheduleDrum(schedulePurchaseID, RecordStatus);
            _vmMsg = Dalobject.DeletedCrustLeatherIssueColor(QCSelectionID);
            return Json(new { msg = _vmMsg });
        }
    }
}