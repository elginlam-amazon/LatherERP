using Microsoft.AspNetCore.Http;
﻿using ERP.DatabaseAccessLayer.AppSetupGateway;
using ERP.DatabaseAccessLayer.OperationGateway;
using ERP.EntitiesModel.AppSetupModel;
using ERP.EntitiesModel.BaseModel;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using ERP_Leather.ActionFilters;

namespace ERP_Leather.Controllers
{
    public class ChemicalItemController : Controller
    {
        DalSysItemType objDalSysItemType = new DalSysItemType();
        DalSysUnit objDalSysUnit = new DalSysUnit();

        private DalSysChemicalItem _dalSysChemicalItem;
        private ValidationMsg _vmMsg;

        public ChemicalItemController()
        {
            _vmMsg = new ValidationMsg();
            _dalSysChemicalItem = new DalSysChemicalItem();
        }

        [CheckUserAccess("ChemicalItem/ChemicalItem")]
        public ActionResult ChemicalItem()
        {
            ViewBag.formTiltle = "Chemical Item";
            ViewBag.ddlItemTypeList = objDalSysItemType.GetAllActiveItemTypeChemical();
            ViewBag.ddlUnitList = objDalSysUnit.GetAllActiveLeatherChemical();
            return View();
        }

        [HttpPost]
        public ActionResult ChemicalItem(SysChemicalItem model)
        {
            if (model != null && model.ItemID != 0)
            {
                _vmMsg = _dalSysChemicalItem.Update(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
            }
            else
            {
                _vmMsg = _dalSysChemicalItem.Save(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
            }
            return Json(new { ItemID = _dalSysChemicalItem.GetItemID(), msg = _vmMsg });
        }

        [HttpPost]
        public ActionResult Delete(string ItemID)
        {
            _vmMsg = _dalSysChemicalItem.Delete(ItemID);
            return Json(new { msg = _vmMsg });
        }

        [HttpGet]
        public ActionResult GetChemicalItemList()
        {
            var sysChemicalItem = _dalSysChemicalItem.GetAll().OrderByDescending(s => s.ItemID);
            return Json(sysChemicalItem);
        }
        public JsonResult GetChemicalListForSearch()
        {
            var supplierAgentList = _dalSysChemicalItem.GetChemicalListForSearch();
            return Json(supplierAgentList);
        }
        public ActionResult GetChemicalListSearchByName(string chemicalName)
        {
            var chemicalData =
                new DalChemPurcReq().GetAllChemicalPurchaseItems()
                    .Where(ob => ob.ItemName.StartsWith(chemicalName))
                    .ToList();
            return Json(chemicalData);
        }
    }
}