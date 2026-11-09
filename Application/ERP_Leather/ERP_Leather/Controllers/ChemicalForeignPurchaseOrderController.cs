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
    public class ChemicalForeignPurchaseOrderController : Controller
    {

        DalChemicalForeignPurchaseOrder objDal = new DalChemicalForeignPurchaseOrder();
        BllPrqChemicalForeignPurchaseOrder objBllChemicalForeignPurchaseOrder = new BllPrqChemicalForeignPurchaseOrder();

        [HttpGet]
        [CheckUserAccess("ChemicalForeignPurchaseOrder/ChemicalForeignPurchaseOrder")]
        public ActionResult ChemicalForeignPurchaseOrder()
        {
            ViewBag.formTiltle = "Chemical Foreign Purchase Order";
            return View();
        }

        [HttpPost]
        [CheckUserAccess("ChemicalForeignPurchaseOrder/ChemicalForeignPurchaseOrder")]
        public ActionResult ChemicalForeignPurchaseOrder(ChemicalForeignPurchaseOrder model)
        {
            if (model.OrderNo == null)
            {
                var msg = objBllChemicalForeignPurchaseOrder.Save(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")), "ChemicalForeignPurchaseOrder/ChemicalForeignPurchaseOrder");
                long OrderID = objBllChemicalForeignPurchaseOrder.GetpurchaseID();
                var OrderNo = objDal.GetOrderNo(OrderID);
                var RequisitionList = objDal.GetRequisitionInformationForSingleOrder((OrderID).ToString());

                return Json(new { Msg = msg, RequisitionList = RequisitionList, OrderID = OrderID, OrderNo = OrderNo });
            }
            else
            {
                var msg = objBllChemicalForeignPurchaseOrder.Update(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
                var RequisitionList = objDal.GetRequisitionInformationForSingleOrder(model.OrderID);


                return Json(new { Msg = msg, RequisitionList = RequisitionList });

            }
        }

        [HttpGet]
        [CheckUserAccess("ChemicalForeignPurchaseOrder/ChemicalLocalPurchaseOrder")]
        public ActionResult ChemicalLocalPurchaseOrder()
        {
            ViewBag.formTiltle = "Chemical Local Purchase Order";
            return View();
        }

        [HttpPost]
        [CheckUserAccess("ChemicalForeignPurchaseOrder/ChemicalLocalPurchaseOrder")]
        public ActionResult ChemicalLocalPurchaseOrder(ChemicalForeignPurchaseOrder model)
        {
            if (model.OrderNo == null)
            {
                var msg = objBllChemicalForeignPurchaseOrder.Save(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")), "ChemicalForeignPurchaseOrder/ChemicalForeignPurchaseOrder");
                long OrderID = objBllChemicalForeignPurchaseOrder.GetpurchaseID();
                var OrderNo = objDal.GetOrderNo(OrderID);
                var RequisitionList = objDal.GetRequisitionInformationForSingleOrder((OrderID).ToString());

                return Json(new { Msg = msg, RequisitionList = RequisitionList, OrderID = OrderID, OrderNo = OrderNo });
            }
            else
            {
                var msg = objBllChemicalForeignPurchaseOrder.Update(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
                var RequisitionList = objDal.GetRequisitionInformationForSingleOrder(model.OrderID);


                return Json(new { Msg = msg, RequisitionList = RequisitionList });

            }
        }

        public ActionResult GetSupplierListForChemicalForeignPurchaseOrder(string _SupplierType)
        {
            var Supplier = objDal.GetSupplierListForChemicalForeignPurchaseOrder(_SupplierType);
            return Json(Supplier);
        }

        public ActionResult GetLocalAgentListForChemicalForeignPurchaseOrder(string _SupplierType)
        {
            var LocalAgent = objDal.GetLocalAgentListForChemicalForeignPurchaseOrder(_SupplierType);
            return Json(LocalAgent);
        }

        public ActionResult GetForeignAgentListForChemicalForeignPurchaseOrder(string _SupplierType)
        {
            var ForiegnAgent = objDal.GetForeignAgentListForChemicalForeignPurchaseOrder(_SupplierType);
            return Json(ForiegnAgent);
        }

        public ActionResult GetOrderInformationForSearch(string _OrderCategory)
        {
            var Orders = objDal.GetOrderInformation(_OrderCategory);
            return Json(Orders);
        }

        public ActionResult GetDetailOrderInformation(string OrderID)
        {
            var AllData = objDal.GetDetailOrderInformation(OrderID);
            return Json(AllData);
        }

        public ActionResult GetAgentInformation(string SupplierID, string LocalAgentID, string ForeignAgentID)
        {
            if(SupplierID != null)
            {
                var Data = objDal.GetSupplierAgentInformation(Convert.ToInt16(SupplierID), 0, 0);
                return Json(Data);
            }
            else if (LocalAgentID != null)
            {
                var Data = objDal.GetSupplierAgentInformation(0, Convert.ToInt16(LocalAgentID), 0);
                return Json(Data);
            }
            else
            {
                var Data = objDal.GetSupplierAgentInformation(0, 0, Convert.ToInt16(ForeignAgentID));
                return Json(Data);
            }
            
        }

        public ActionResult GetChemicalRequisition( string _RequisitionCategory, string _SupplierID)
        {
            var Data = objDal.GetChemicalRequisition(_RequisitionCategory, _SupplierID);
            return Json(Data);
        }


        public ActionResult GetChemicalRequisitionItemList(string _RequisitionID, string OrderID)
        {
            if(OrderID !=null && OrderID != "")
            {
                var Data = objDal.GetChemicalRequisitionItemList(_RequisitionID, OrderID);
                return Json(Data);
            }
            else
            {
                var Data = objDal.GetChemicalRequisitionItemList(_RequisitionID);
                return Json(Data);
            }
            
        }

        public ActionResult GetAllChemicalItem()
        {
            var ItemList = objDal.GetAllChemicalItem();
            return Json(ItemList);
        }

         public ActionResult GetAllPackSizeForChemical()
         {
             var Data = objDal.GetAllPackSizeForChemical();
            return Json(Data);
         }

         public ActionResult GetAllUnitForChemicalPackSize()
         {
             var Data = objDal.GetAllUnitForChemicalPackSize();
             return Json(Data);
         }

         public ActionResult GetAllUnitForChemical()
         {
             var Data = objDal.GetAllUnitForChemical();
             return Json(Data);
         }

         public ActionResult ConfirmOrder(string OrderID, string confirmComment)
         {
             bool checkConfirm = objDal.ConfirmOrder(OrderID, confirmComment);

             if (checkConfirm)
             {
                 return Json(new { Msg = "Success" });
             }
             else
             {
                 return Json(new { Msg = "Fail" });
             }
         }


         public ActionResult DeleteRequisitionItem(string OrderItemID, string _PageStatus)
         {
             bool CheckStatus = false; ;
             if (_PageStatus == "NCF" || _PageStatus== "Not Confirmed")
             {
                 CheckStatus = objDal.DeleteRequisitionItem(OrderItemID);
                 if (CheckStatus)
                 {
                     return Json(new { Msg = "Success" });
                 }
                 else
                 {
                     return Json(new { Msg = "Fail" });
                 }
             }
             else if (_PageStatus == "CNF" || _PageStatus== "Confirmed")
             {
                 return Json(new { Msg = "CNF" });
             }
             else
             {
                 return Json(new { Msg = "Fail" });
             }
         }

         public ActionResult DeleteRequisition(string OrderID, string RequisitionID, string _PageStatus)
         {
             bool CheckStatus = false; ;
             if (_PageStatus == "NCF" || _PageStatus == "Not Confirmed")
             {
                 CheckStatus = objDal.DeleteRequisition(OrderID, RequisitionID);
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

         public ActionResult DeleteOrder(string OrderID, string _PageStatus)
         {
             bool CheckStatus = false; ;
             if (_PageStatus == "NCF" || _PageStatus == "Not Confirmed")
             {
                 CheckStatus = objDal.DeleteOrder(OrderID);
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

        public ActionResult ConsealRequisition(string RequisitionID)
         {
             var CheckStatus = objDal.ConsealRequisition(RequisitionID);

             if (CheckStatus)
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