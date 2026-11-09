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
    public class ItemTypeController : Controller
    {
        private DalSysItemType _dalSysItemType;
        private SysItemType _sysItemType = new SysItemType();

        private ValidationMsg _vmMsg;

        public ItemTypeController()
        {
            _vmMsg = new ValidationMsg();
            _dalSysItemType = new DalSysItemType();
        }

        [CheckUserAccess("ItemType/ItemType")]
        public ActionResult ItemType()
        {
            ViewBag.formTiltle = "Item Type Form";
            return View(_sysItemType);
        }

        [HttpPost]
        public ActionResult ItemType(SysItemType model)
        {
            if (model != null && model.ItemTypeID != 0)
            {
                _vmMsg = _dalSysItemType.Update(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
            }
            else
            {
                _vmMsg = _dalSysItemType.Save(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
            }
            return Json(new { ItemTypeID = _dalSysItemType.GetItemTypeID(), msg = _vmMsg });
        }

        //[HttpPost]
        //public ActionResult Create(string models)
        //{
        //    var sysItemType = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<SysItemType>>(models).FirstOrDefault();

        //    _vmMsg = _dalSysItemType.Save(sysItemType);

        //    var result = new { data = sysItemType, message = _vmMsg };
        //    return Json(result);
        //}

        //[HttpPost]
        //public ActionResult Update(string models)
        //{
        //    var sysItemType = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<SysItemType>>(models).FirstOrDefault();

        //    _vmMsg = _dalSysItemType.Update(sysItemType);

        //    var result = new { data = sysItemType, message = _vmMsg };
        //    return Json(result);

        //}


        public JsonResult GetItemType()
        {
            var AllItem = _dalSysItemType.GetAll().OrderByDescending(s => s.ItemTypeID);
            return Json(AllItem);

        }

        [HttpPost]
        public ActionResult Delete(string ItemTypeID)
        {
            _vmMsg = _dalSysItemType.Delete(ItemTypeID, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
            return Json(new { msg = _vmMsg });
        }

    }
}