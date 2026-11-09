using Microsoft.AspNetCore.Http;
﻿using ERP.DatabaseAccessLayer.AppSetupGateway;
using ERP.DatabaseAccessLayer.OperationGateway;
using ERP.EntitiesModel.AppSetupModel;
using ERP.EntitiesModel.BaseModel;
using ERP.EntitiesModel.OperationModel;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ERP_Leather.ActionFilters;

namespace ERP_Leather.Controllers
{
    public class LoanReceiveController : Controller
    {
        DalSysStore objDalSysStore = new DalSysStore();
        DalSysCurrency objDalSysCurrency = new DalSysCurrency();
        DalSysSupplier objDalSysSupplier = new DalSysSupplier();
        DalSysSize objDalSysSize = new DalSysSize();
        DalSysUnit objDalSysUnit = new DalSysUnit();
        private DalLoanReceive Dalobject;
        private ValidationMsg _vmMsg;

        public LoanReceiveController()
        {
            _vmMsg = new ValidationMsg();
            Dalobject = new DalLoanReceive();
        }

        [CheckUserAccess("LoanReceive/LoanReceive")]
        public ActionResult LoanReceive()
        {
            ViewBag.formTiltle = "Loan Receive";

            ViewBag.ddlReceiveFromList = objDalSysStore.GetAllActiveLoanStore();
            ViewBag.ddlReceiveToList = objDalSysStore.GetAllActiveChemicalStore();
            return View();
        }

        [HttpPost]
        public ActionResult LoanReceive(INVStoreTrans model)
        {
            _vmMsg = model.TransactionID == 0 ? Dalobject.Save(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")), "LoanReceive/LoanReceive") : Dalobject.Update(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
            return Json(new { TransactionID = Dalobject.GetReceiveID(), TransactionNo = Dalobject.GetReceiveNo(), msg = _vmMsg });
        }

        [HttpPost]
        public ActionResult ConfirmChemicalPurchase(INVStoreTrans model)
        {
            _vmMsg = Dalobject.ConfirmChemicalPurchase(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
            return Json(new { msg = _vmMsg });
        }

        [HttpPost]
        public ActionResult CheckedChemicalPurchase(string receiveId, string checkComment)
        {
            _vmMsg = Dalobject.CheckedChemicalPurchase(receiveId, checkComment, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
            return Json(new { msg = _vmMsg });
        }

        public ActionResult GetChemicalSupplierList()
        {
            var chemicalSupplier = objDalSysSupplier.GetAllChemicalSupplier();
            return Json(chemicalSupplier);
        }

        public ActionResult GetLoanRequestInfoList(string RequestID)
        {
            INVStoreTrans model = new INVStoreTrans();
            model.InvStoreTransRequestList = Dalobject.GetLoanRequestInfoList(RequestID);
            if (model.InvStoreTransRequestList.Count() > 0)
                model.InvStoreTransItemList = Dalobject.GetLoanRequestItemList(model.InvStoreTransRequestList[0].RequestID.ToString());
            return Json(model);
        }

        public ActionResult GetLoanRequestItemList(string RequestID)
        {
            var packingInfoList = Dalobject.GetLoanRequestItemList(RequestID);
            return Json(packingInfoList);
        }

        public ActionResult GetLoanRequestAfterSaveList(string TransactionID)
        {
            var packItemList = Dalobject.GetLoanRequestAfterSaveList(TransactionID);
            return Json(packItemList);
        }

        public ActionResult GetLoanRequestItemAfterSaveList(string TransactionID)
        {
            var packItemList = Dalobject.GetRecvItemListList(TransactionID);
            return Json(packItemList);
        }

        public ActionResult GetChallanAfterSaveList(string TransactionID)
        {
            var challanList = Dalobject.GetChallanList(TransactionID);
            return Json(challanList);
        }

        public ActionResult GetAllChemicalSupplierList(string supplier)
        {
            SysSupplier sysSupplier = new SysSupplier();

            var supplierList = objDalSysSupplier.GetAllChemicalSupplierList(supplier);
            if (supplierList.Count > 1)
            {
                sysSupplier.Count = 0;
            }
            else
            {
                sysSupplier.Count = 1;
            }
            sysSupplier.ChemicalSupplierList = supplierList;
            return Json(sysSupplier);
        }

        public ActionResult GetChemicalPurchaseReceiveList()
        {
            var chemicalPurchaseReceiveList = Dalobject.GetChemicalPurchaseReceiveList();
            return Json(chemicalPurchaseReceiveList);
        }

        public ActionResult GetLoanReceiveRequestList()
        {
            var loanReceiveRequestList = Dalobject.GetLoanReceiveRequestList();
            return Json(loanReceiveRequestList);
        }

        public ActionResult GetSupplierSoruceList()
        {
            var SupplierSoruceList = Dalobject.GetSupplierSoruceList();
            return Json(SupplierSoruceList);
        }

        public ActionResult GetStoreSoruceList()
        {
            var StoreSoruceList = Dalobject.GetStoreSoruceList();
            return Json(StoreSoruceList);
        }

        public ActionResult GetAllGridList(string TransactionID)
        {
            INVStoreTrans model = new INVStoreTrans();
            if (TransactionID != null)
            {
                model.InvStoreTransRequestList = Dalobject.GetPurcRecvPlList(TransactionID);
                model.InvStoreTransChallanList = Dalobject.GetChallanList(TransactionID);
                model.InvStoreTransItemList = Dalobject.GetRecvItemListList(TransactionID);
            }
            return Json(model);
        }

        public ActionResult GetAllActiveChemicalPackSize()
        {
            var chemicalItemSizeList = objDalSysSize.GetAllActiveChemicalPackSize();
            return Json(chemicalItemSizeList);
        }

        public ActionResult GetAllActiveChemicalPackUnit()
        {
            var chemicalUnitList = objDalSysUnit.GetAllActiveChemicalPack();
            return Json(chemicalUnitList);
        }

        public ActionResult GetAllActiveChemicalUnit()
        {
            var chemicalUnitList = objDalSysUnit.GetAllActiveLeatherChemical();
            return Json(chemicalUnitList);
        }

        public ActionResult GetChemicalItemList()
        {
            var chemicalList = Dalobject.GetChemicalItemList();
            return Json(chemicalList);
        }

        [HttpPost]
        public ActionResult DeletedReceive(string TransactionID)
        {
            long? transactionID = Convert.ToInt64(TransactionID);
            _vmMsg = Dalobject.DeletedReceive(transactionID);
            return Json(new { msg = _vmMsg });
        }

        [HttpPost]
        public ActionResult DeletedReceiveChallan(string TransChallanID)
        {
            long? transChallanID = Convert.ToInt64(TransChallanID);
            _vmMsg = Dalobject.DeletedReceiveChallan(transChallanID);
            return Json(new { msg = _vmMsg });
        }

        [HttpPost]
        public ActionResult DeletedReceiveItem(string TransItemID)
        {
            long? transItemID = Convert.ToInt64(TransItemID);
            _vmMsg = Dalobject.DeletedReceiveItem(transItemID);
            return Json(new { msg = _vmMsg });
        }
    }
}