using Microsoft.AspNetCore.Http;
﻿using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Mvc;

using ERP.DatabaseAccessLayer.AppSetupGateway;
using ERP.EntitiesModel.AppSetupModel;
using ERP.EntitiesModel.BaseModel;
using ERP_Leather.ActionFilters;

namespace ERP_Leather.Controllers
{
    public class SupplierAgentController : Controller
    {
        private DalSupplierAgent _dalSysSupplier;
        private SysSupplier _sysSupplier = new SysSupplier();
        private DalSysCountry dalSysCountry = new DalSysCountry();

        private ValidationMsg _vmMsg;

        public SupplierAgentController()
        {
            _vmMsg = new ValidationMsg();
            _dalSysSupplier = new DalSupplierAgent();
        }
        [CheckUserAccess("SupplierAgent/SupplierAgent")]
        public ActionResult SupplierAgent()
        {
            ViewBag.formTiltle = "Supplier Agent Form";
            ViewBag.ddlCountryList = dalSysCountry.GetAllActiveCountry();
            return View(_sysSupplier);
        }

        [HttpPost]
        public ActionResult SupplierAgent(SysSupplier model)
        {
            if (model != null && model.SupplierID != 0)
            {
                _vmMsg = _dalSysSupplier.Update(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
            }
            else
            {
                _vmMsg = _dalSysSupplier.Save(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
            }
            return Json(new { SupplierID = _dalSysSupplier.GetSupplierId(), SupplierCode = _dalSysSupplier.GetSupplierCode(), msg = _vmMsg });
        }
        [HttpPost]
        public ActionResult Delete(string supplierId)
        {
            _vmMsg = _dalSysSupplier.Delete(supplierId, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
            return Json(new { msg = _vmMsg });
        }

        [HttpPost]
        public ActionResult DeletedAddress(string supplierAddressId)
        {
            _vmMsg = _dalSysSupplier.DeletedAddress(supplierAddressId, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
            return Json(new { msg = _vmMsg });
        }

        [HttpPost]
        public ActionResult DeletedAgent(string supplierAgentId)
        {
            _vmMsg = _dalSysSupplier.DeletedAgent(supplierAgentId, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
            return Json(new { msg = _vmMsg });
        }

        public ActionResult GetSupplierAgentList()
        {
            var supplierAgentList = _dalSysSupplier.GetSupplierAgentList();
            return Json(supplierAgentList);
        }

        public ActionResult GetSupplierForSearchList()
        {
            var supplierAgentList = _dalSysSupplier.GetSupplierForSearchList();
            return Json(supplierAgentList);
        }

        public ActionResult GetSupplierList(string supplier)
        {
            SysSupplier sysSupplier = new SysSupplier();


            var supplierList = _dalSysSupplier.GetSupplierList(supplier);
            if (supplierList.Count > 1)
            {
                sysSupplier.Count = 0;
            }
            else
            {
                sysSupplier.Count = 1;
            }
            sysSupplier.SupplierList = supplierList;
            return Json(sysSupplier);
        }

        public ActionResult GetSupplierAgentSearchList(string supplier)
        {
            SysSupplier sysSupplier = new SysSupplier();


            var supplierList = _dalSysSupplier.GetSupplierAgentSearchList(supplier);
            if (supplierList.Count > 1)
            {
                sysSupplier.Count = 0;
            }
            else
            {
                sysSupplier.Count = 1;
            }
            sysSupplier.SupplierAgentList = supplierList;
            return Json(sysSupplier);
        }

        public JsonResult GetSupplierListForSearch()
        {
            var supplierAgentList = _dalSysSupplier.GetSupplierListForSearch();
            return Json(supplierAgentList);
        }


        public JsonResult GetChemicalSupplierListForSearch()
        {
            var supplierList = _dalSysSupplier.GetChemicalSupplierListForSearch();
            return Json(supplierList);
        }

        public ActionResult GetSupplierAddressAndAgentList(string supplierId)
        {
            var _sysSupplier = _dalSysSupplier.GetSupplierAddressAndAgentList(supplierId);
            return Json(_sysSupplier);
        }

        public ActionResult GetSupplierListSearchById(string supplier)
        {
            supplier = supplier.ToUpper();
            var supplierData = _dalSysSupplier.GetAllSupplier().Where(ob => ob.SupplierName.StartsWith(supplier)).ToList();
            return Json(supplierData);
        }
    }
}