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
    public class LocationController : Controller
    {
        private DalSysLocation _dalSysLocation;
        private SysLocation _sysLocation = new SysLocation();

        private ValidationMsg _vmMsg;

        public LocationController()
        {
            _vmMsg = new ValidationMsg();
            _dalSysLocation = new DalSysLocation();
        }

        [CheckUserAccess("Location/Location")]
        public ActionResult Location()
        {
            ViewBag.formTiltle = "Location Information";
            return View();
        }

        [HttpPost]
        public ActionResult Location(SysLocation model)
        {
            if (model != null && model.LocationID != 0)
            {
                _vmMsg = _dalSysLocation.Update(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
            }
            else
            {
                _vmMsg = _dalSysLocation.Save(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
            }
            return Json(new { LocationID = _dalSysLocation.GetLocationId(), msg = _vmMsg });
        }
        [HttpPost]
        public ActionResult Delete(string locationId)
        {
            _vmMsg = _dalSysLocation.Delete(locationId, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
            return Json(new { msg = _vmMsg });
        }

        [HttpGet]
        public ActionResult GetSourceList()
        {
            var sysSource = _dalSysLocation.GetAll().OrderByDescending(s => s.LocationID);
            return Json(sysSource);
        }
	}
}