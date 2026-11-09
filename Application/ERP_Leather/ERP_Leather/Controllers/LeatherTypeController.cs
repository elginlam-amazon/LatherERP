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
    public class LeatherTypeController : Controller
    {
        private DalSysLeatherType _dalSysLeatherType;
        private SysLeatherType _sysLeatherType = new SysLeatherType();

        private ValidationMsg _vmMsg;

        public LeatherTypeController()
        {
            _vmMsg = new ValidationMsg();
            _dalSysLeatherType = new DalSysLeatherType();
        }

        [CheckUserAccess("LeatherType/LeatherType")]
        public ActionResult LeatherType()
        {
            ViewBag.formTiltle = "Leather Type Form";
            return View(_sysLeatherType);
        }




        [HttpPost]
        public ActionResult LeatherType(SysLeatherType model)
        {
            if (model != null && model.LeatherTypeID != 0)
            {
                _vmMsg = _dalSysLeatherType.Update(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
            }
            else
            {
                _vmMsg = _dalSysLeatherType.Save(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
            }
            return Json(new { SourceID = _dalSysLeatherType.GetLeatherTypeID(), msg = _vmMsg });
        }

        [HttpPost]
        public ActionResult Delete(string sourceId)
        {
            _vmMsg = _dalSysLeatherType.Delete(sourceId, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
            return Json(new { msg = _vmMsg });
        }

        [HttpGet]
        public ActionResult GetLeatherType()
        {
            var sysSource = _dalSysLeatherType.GetAll().OrderByDescending(s => s.LeatherTypeID);
            return Json(sysSource);
        }
    }
}