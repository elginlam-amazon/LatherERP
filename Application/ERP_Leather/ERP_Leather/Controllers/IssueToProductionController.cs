using Microsoft.AspNetCore.Http;
﻿using System;
using Microsoft.AspNetCore.Mvc;
using ERP.DatabaseAccessLayer.AppSetupGateway;
using ERP.BusinessLogicLayer.OperationManager;
using ERP.EntitiesModel.OperationModel;
using ERP.DatabaseAccessLayer.OperationGateway;
using ERP_Leather.ActionFilters;
using System.Collections.Generic;
using System.Linq;




namespace ERP_Leather.Controllers
{
    public class IssueToProductionController : Controller
    {

        DalProductionRequisition objDal = new DalProductionRequisition();
        BllIssueToProduction objBLL = new BllIssueToProduction();
        DalIssueToProduction objDalIssue = new DalIssueToProduction();
        //DalChemicalConsumptionForCrusting objDalCrustConsump = new DalChemicalConsumptionForCrusting();

        [HttpGet]
        [CheckUserAccess("IssueToProduction/IssueToProduction")]
        public ActionResult IssueToProduction()
        {
            ViewBag.RequisitionFrom = objDal.GetStoreListForFixedCategory("Production"); //For Development
            //ViewBag.RequisitionFrom = objDal.GetStoreListForFixedType("Leather", "WB Production"); //Instructed by Zia Bhai for live
            ViewBag.RequisitionTo = objDal.GetCategoryTypeWiseStoreList("Chemical", "Chemical");
            ViewBag.RecipeFor = objDal.GetRecipeCategoryList();
            ViewBag.UnitForLeather = objDal.GetUnitListForFixedCategory("Leather");
            ViewBag.UnitForChemical = objDal.GetUnitListForFixedCategory("ChemicalPack");

            ViewBag.formTiltle = "Issue to Production";
            return View();
        }

        [HttpPost]
        [CheckUserAccess("IssueToProduction/IssueToProduction")]
        public ActionResult IssueToProduction(INVStoreTrans model)
        {
            if (model.TransactionID == 0)
            {

                var msg = objBLL.Save(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")), "IssueToProduction/IssueToProduction");
                var TransactionID = objBLL.GetTransactionID();
                var TransactionNo = objDalIssue.GetTransactionNo(TransactionID);

                if(model.PageName=="Adjustment")
                {
                    var TransactionItemList = objDalIssue.GetTransactionItemListForStockAdjust(TransactionID, model.TransactionFrom);
                    return Json(new { Msg = msg, TransactionID = TransactionID, TransactionNo = TransactionNo, TransactionItemList = TransactionItemList });
                }
                else
                {
                    var TransactionItemList = objDalIssue.GetTransactionItemList(TransactionID, model.TransactionFrom);
                    return Json(new { Msg = msg, TransactionID = TransactionID, TransactionNo = TransactionNo, TransactionItemList = TransactionItemList });
                }

            }
            else
            {
                var msg = objBLL.Update(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")));

                if (model.PageName == "Adjustment")
                {
                    var TransactionItemList = objDalIssue.GetTransactionItemListForStockAdjust(model.TransactionID, model.TransactionFrom);
                    return Json(new { Msg = msg, TransactionItemList = TransactionItemList });
                }
                else
                {
                    var TransactionItemList = objDalIssue.GetTransactionItemList(model.TransactionID, model.TransactionFrom);
                    return Json(new { Msg = msg, TransactionItemList = TransactionItemList });
                }
            }
        }


        public ActionResult GetRequisitionFromFixedStore(string _RequisitionFrom)
        {
            var Data = objDalIssue.GetRequisitionFromFixedStore(_RequisitionFrom);
            return Json(Data);
        }

        public ActionResult GetRequisitionDetailsFromFixedStore(int _RequisitionID, byte _RequisitionAt)
        {
            var Data = objDalIssue.GetRequisitionDetailsFromFixedStore(_RequisitionID, _RequisitionAt);
            return Json(Data);
        }


        public ActionResult GetTransactionInfoForSearch(string _TransactionType, int pageSize, int skip)
        {
            var AllData = objDalIssue.GetTransactionInfoForSearch(_TransactionType);

            var filterCollection = KendoGridFilterCollection.BuildCollection(Request);
            var filteredData = AllData.MultipleFilter(filterCollection.Filters);
            var FinalData = filteredData.Skip(skip).Take(pageSize).ToList();

            foreach (var item in FinalData)
            {
                item.TransactionDate = (Convert.ToDateTime(item.TransactionDateTemp)).ToString("dd'/'MM'/'yyyy");
                item.ReqFromDate = (Convert.ToDateTime(item.ReqFromDateTemp)).ToString("dd'/'MM'/'yyyy");
                item.ReqToDate = (Convert.ToDateTime(item.ReqToDateTemp)).ToString("dd'/'MM'/'yyyy");
                item.RecordStatus = DalCommon.ReturnRecordStatus(item.RecordStatus);
            }
            
            var total = filteredData.Count();
            return Json(new { total = total, data = FinalData });
        }


        public ActionResult GetTransactionDetailsAfterSearch(long _TransactionID)
        {
            var Data = objDalIssue.GetTransactionDetailsAfterSearch(_TransactionID);
            return Json(Data);
        }


        public ActionResult DeleteTransactionItem(string _TransItemID, string _PageStatus)
        {
            bool CheckStatus = false; ;
            if (_PageStatus == "NCF" || _PageStatus == "Not Confirmed" || _PageStatus == "CHK" || _PageStatus == "Checked")
            {
                CheckStatus = objDalIssue.DeleteTransactionItem(_TransItemID);
                if (CheckStatus)
                {
                    return Json(new { Msg = "Success" });
                }
                else
                {
                    return Json(new { Msg = "Fail" });
                }
            }
            else if (_PageStatus == "CNF" || _PageStatus == "Confirmed" || _PageStatus == "APV" || _PageStatus == "Approved")
            {
                return Json(new { Msg = "CNF" });
            }
            else
            {
                return Json(new { Msg = "Fail" });
            }
        }

        public ActionResult DeleteTransaction(string _TransactionID, string _PageStatus)
        {
            bool CheckStatus = false; ;
            if (_PageStatus == "NCF" || _PageStatus == "Not Confirmed" || _PageStatus == "CHK" || _PageStatus == "Checked")
            {
                CheckStatus = objDalIssue.DeleteTransaction(_TransactionID);
                if (CheckStatus)
                {
                    return Json(new { Msg = "Success" });
                }
                else
                {
                    return Json(new { Msg = "Fail" });
                }
            }
            else if (_PageStatus == "CNF" || _PageStatus == "Confirmed" || _PageStatus == "APV" || _PageStatus == "Approved")
            {
                return Json(new { Msg = "CNF" });
            }
            else
            {
                return Json(new { Msg = "Fail" });
            }

        }

        public ActionResult CheckIssueToProductionTransaction(string _TransactionID, string _CheckComment)
        {

            var Data = objDalIssue.CheckIssueToProductionTransaction(_TransactionID, _CheckComment);

            if (Data)
            {
                return Json(new { Msg = "Success" });
            }
            else
            {
                return Json(new { Msg = "Fail" });
            }
        }

        public ActionResult ConfirmIssueToProductionTransaction(long _TransactionID, string _CheckComment)
        {

            var Data = objDalIssue.ConfirmIssueToProductionTransaction(_TransactionID, _CheckComment, Convert.ToInt32(HttpContext.Session.GetString("UserID")));

            return Json(Data);
        }

        public ActionResult GetStockPackSizeInfo(string _StoreID, int _ItemID)
        {
            var Data = objDalIssue.GetStockPackSizeInfo(_StoreID, _ItemID);
            return Json(Data);
        }

        public ActionResult GetAvailableChemicalInStock(byte _RequisitionAt)
        {
            return Json(objDalIssue.GetAvailableChemicalInStock(_RequisitionAt));
        }

	}
}