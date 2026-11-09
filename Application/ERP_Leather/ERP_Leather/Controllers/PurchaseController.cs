using Microsoft.AspNetCore.Http;
﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ERP.DatabaseAccessLayer.AppSetupGateway;
using ERP.BusinessLogicLayer.OperationManager;
using ERP.EntitiesModel.OperationModel;
using ERP.DatabaseAccessLayer.OperationGateway;
using ERP_Leather.ActionFilters;


namespace ERP_Leather.Controllers
{
    public class PurchaseController : Controller
    {
        DalSysSupplier objSupplier = new DalSysSupplier();
        DalPrqPurchase objPurchase = new DalPrqPurchase();
        BllPrqPurchase objBllPrqPurchase = new BllPrqPurchase();
        DalPrqPurchaseYearTarget objPurchaseTarget = new DalPrqPurchaseYearTarget();

        [HttpGet]
        [CheckUserAccess("Purchase/Purchase")]
        public ActionResult Purchase()
        {

            ViewBag.PurchaseYear = objPurchaseTarget.GetPurchaseYear();
            ViewBag.formTiltle = "Purchase Receive";
            return View();
        }

        [HttpPost]
        [CheckUserAccess("Purchase/Purchase")]
        public ActionResult Purchase(PurchaseReceive model)
        {
            if ( model.PurchaseID == 0)
            {
                
                var msg = objBllPrqPurchase.SavePurchaseReceiveWithChallanItem(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
                long purchaseID = objBllPrqPurchase.GetpurchaseID();
                string PurchaseNo = objPurchase.GetPurchaseNumber(purchaseID);
                var ChallanList = objPurchase.GetChallanList(purchaseID);

                return Json(new { Msg = msg, purchaseID = purchaseID, PurchaseNo = PurchaseNo, challanList = ChallanList });
            }
            else
            {
                var msg = objBllPrqPurchase.UpdatePurchaseReceiveWithChallanItem(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
                var ChallanList = objPurchase.GetChallanList(Convert.ToInt64(model.PurchaseID));


                return Json(new { Msg = msg, challanList = ChallanList });
            }

        }

        public ActionResult GetSupplierList()
        {
            var supplier = objSupplier.GetCategoryWiseSupplier("Leather");
            return Json(supplier);
        }


        //For Search Window Grid
        public ActionResult GetPurchaseInformation()
        {

            var allData = objPurchase.GetPurchaseInformation();
            return Json(allData);
        }

        public ActionResult GetDetailPurchaseInformation(string PurchaseNumber)
        {
            var AllData = objPurchase.GetDetailPurchaseInformation(PurchaseNumber);
            return Json(AllData);
        }

        public ActionResult GetItemListForChallan(string ChallanID)
        {
            var ItemList = objPurchase.GetItemListForChallan(ChallanID);
            return Json(ItemList);
        }

        public ActionResult ConfirmPurchase(string purchaseNumber, string confirmComment)
        {
            bool checkConfirm = objPurchase.ConfirmPurchase(purchaseNumber, confirmComment);

            if (checkConfirm)
            {
                return Json(new { Msg = "Success" });
            }
            else
            {
                return Json(new { Msg = "Fail" });
            }
        }

        public ActionResult DeleteChallanItem(string ChallanItemID, string _PageStatus)
        {
            bool CheckStatus = false; ;
            if (_PageStatus == "NCF" || _PageStatus == "Not Confirmed")
            {
                 CheckStatus = objPurchase.DeleteChallanItem(ChallanItemID);
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

        public ActionResult DeleteChallan(string ChallanID, string _PageStatus)
        {
            bool CheckStatus = false; ;
            if (_PageStatus == "NCF" || _PageStatus == "Not Confirmed")
            {
                CheckStatus = objPurchase.DeleteChallan(ChallanID);
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

        public ActionResult DeletePurchase(string PurchaseID, string _PageStatus)
        {
            bool CheckStatus = false; ;
            if (_PageStatus == "NCF" || _PageStatus == "Not Confirmed")
            {
                CheckStatus = objPurchase.DeletePurchase(PurchaseID);
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
    }
}