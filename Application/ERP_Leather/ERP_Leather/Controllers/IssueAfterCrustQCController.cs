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
    public class IssueAfterCrustQCController : Controller
    {
        DalSysStore objDalSysStore = new DalSysStore();

        private DalIssueAfterCrustQC Dalobject;
        private ValidationMsg _vmMsg;

        public IssueAfterCrustQCController()
        {
            _vmMsg = new ValidationMsg();
            Dalobject = new DalIssueAfterCrustQC();
        }

        [CheckUserAccess("IssueAfterCrustQC/IssueAfterCrustQC")]
        public ActionResult IssueAfterCrustQC()
        {
            ViewBag.formTiltle = " Issue After Crust QC";
            ViewBag.ddlCrustQCIssueFromList = objDalSysStore.GetAllActiveCrustQCStore();
            ViewBag.ddlCrustQCIssueToList = objDalSysStore.GetAllActiveCrustStore();
            return View();
        }

        [HttpPost]
        public ActionResult IssueAfterCrustQC(InvCrustLeatherIssue model)
        {
            _vmMsg = model.CrustLeatherIssueID == 0 ? Dalobject.Save(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")), "CrustLeatherQC/CrustLeatherQC") : Dalobject.Update(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
            return Json(new { CrustLeatherIssueID = Dalobject.GetQCID(), CrustLeatherQCNo = Dalobject.GetQCNo(), msg = _vmMsg });
        }

        public ActionResult GetQCItemList(string IssueFrom, string QCStatus)
        {
            var crustYearMonthScheduleList = Dalobject.GetScheduleItemInfo(IssueFrom, QCStatus);
            return Json(crustYearMonthScheduleList);
        }

        public ActionResult GetColorAndGradeList(string IssueFrom, string BuyerID, string BuyerOrderID, string ArticleID, string ItemTypeID, string LeatherStatusID, string ArticleChallanID, string QCStatus)
        {
            var colorAndGradeList = Dalobject.GetColorAndGradeList(IssueFrom, BuyerID, BuyerOrderID, ArticleID, ItemTypeID, LeatherStatusID,ArticleChallanID, QCStatus);
            return Json(colorAndGradeList);
        }

        public ActionResult GetAllGridList(string CrustLeatherIssueID, string StoreId)
        {
            InvCrustLeatherIssue model = new InvCrustLeatherIssue();
            if (string.IsNullOrEmpty(CrustLeatherIssueID)) return Json(model);
            model.IssueItemList = Dalobject.GetQCColorList(CrustLeatherIssueID);
            if (model.IssueItemList.Count > 0)
                model.IssueColorList = Dalobject.GetQCSelectionList(model.IssueItemList[0].CrustLeatherIssueItemID.ToString(), StoreId);
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
        public ActionResult DeletedCrustLeatherIssue(string CrustLeatherIssueID, string RecordStatus)
        {
            long crustLeatherIssueID = Convert.ToInt64(CrustLeatherIssueID);
            _vmMsg = Dalobject.DeletedCrustLeatherIssue(crustLeatherIssueID, RecordStatus);
            return Json(new { msg = _vmMsg });
        }

        [HttpPost]
        public ActionResult DeletedCrustLeatherIssueItem(string CrustLeatherIssueItemID, string RecordStatus)
        {
            long crustLeatherIssueItemID = Convert.ToInt64(CrustLeatherIssueItemID);
            //_vmMsg = Dalobject.DeletedCrustQCItem(scheduleDateID, RecordStatus);
            _vmMsg = Dalobject.DeletedCrustLeatherIssueItem(crustLeatherIssueItemID, RecordStatus);
            return Json(new { msg = _vmMsg });
        }

        [HttpPost]
        public ActionResult DeletedCrustLeatherIssueColor(string CrustLeatherIssueColorID, string RecordStatus)
        {
            long crustLeatherIssueColorID = Convert.ToInt64(CrustLeatherIssueColorID);
            //_vmMsg = Dalobject.DeletedScheduleDrum(schedulePurchaseID, RecordStatus);
            _vmMsg = Dalobject.DeletedCrustLeatherIssueColor(crustLeatherIssueColorID, RecordStatus);
            return Json(new { msg = _vmMsg });
        }

        public ActionResult GetIssueFromAndToList(string issuefor)
        {
            var issueFromAndToList = Dalobject.GetIssueFromAndToList(issuefor);
            return Json(issueFromAndToList);
        }
    }
}