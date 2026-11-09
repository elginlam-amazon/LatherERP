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
    public class StoreTransferIssueController : Controller
    {
        DalProductionRequisition objDal = new DalProductionRequisition();

        DalStoreTransferIssue objDalIssue = new DalStoreTransferIssue();
        BllStoreTransferIssue objBllIssue = new BllStoreTransferIssue();
        
        
         [HttpGet]
         [CheckUserAccess("StoreTransferIssue/StoreTransferIssue")]
        public ActionResult StoreTransferIssue()
        {
            //ViewBag.RequisitionFrom = objDal.GetCategoryTypeWiseStoreList("Chemical", "Chemical");
            ViewBag.RequisitionTo = objDal.GetCategoryTypeWiseStoreList("Chemical", "Chemical");

            ViewBag.formTiltle = "Store Transfer Issue";
            return View();
        }


         [HttpPost]
         [CheckUserAccess("StoreTransferIssue/StoreTransferIssue")]
         public ActionResult StoreTransferIssue(INVStoreTrans model)
         {
             if (model.TransactionID == 0)
             {

                 var msg = objBllIssue.Save(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")), "StoreTransferIssue/StoreTransferIssue");
                 var TransactionID = objBllIssue.GetTransactionID();
                 var TransactionNo = objDalIssue.GetTransactionNo(TransactionID);

                 var TransactionItemList = objDalIssue.GetTransactionItemList(TransactionID, model.IssueFrom);
                 return Json(new { Msg = msg, TransactionID = TransactionID, TransactionNo = TransactionNo, TransactionItemList = TransactionItemList });
             }
             else
             {
                 var msg = objBllIssue.Update(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
                 var TransactionItemList = objDalIssue.GetTransactionItemList(model.TransactionID, model.IssueFrom);

                 return Json(new { Msg = msg, TransactionItemList = TransactionItemList });
             }
         }

        public ActionResult GetStoreWiseChemicalItemStock(string _IssueFrom)
         {
             var Data = objDalIssue.GetStoreWiseChemicalItemStock(_IssueFrom);
            return Json(Data);
         }

        public ActionResult GetTransactionInfoForSearch()
        {
            var Data = objDalIssue.GetTransactionInfoForSearch();
            return Json(Data);
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

        //public ActionResult CheckStoreTranserIssueTransaction(string _TransactionID, string _CheckComment)
        //{

        //    var Data = objDalIssue.CheckIssueToProductionTransaction(_TransactionID, _CheckComment);

        //    if (Data)
        //    {
        //        return Json(new { Msg = "Success" });
        //    }
        //    else
        //    {
        //        return Json(new { Msg = "Fail" });
        //    }
        //}

        public ActionResult ConfirmIssueToProductionTransaction(long _TransactionID, string _CheckComment)
        {

            var Data = objDalIssue.ConfirmIssueToProductionTransaction(_TransactionID, _CheckComment, Convert.ToInt32(HttpContext.Session.GetString("UserID")));

            return Json(Data);
        }
         
	}
}