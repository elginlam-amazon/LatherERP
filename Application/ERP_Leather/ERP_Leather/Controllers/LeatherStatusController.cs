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
    public class LeatherStatusController : Controller
    {
        private DalSysLeatherStatus _dalLeatherStatus;
        private SysLeatherStatus _sysLeatherStatus = new SysLeatherStatus();
        private ValidationMsg _vmMsg;

        public LeatherStatusController()
        {
            _vmMsg = new ValidationMsg();
            _dalLeatherStatus = new DalSysLeatherStatus();
        }

        [CheckUserAccess("LeatherStatus/LeatherStatus")]
        public ActionResult LeatherStatus()
        {
            ViewBag.formTiltle = "Leather Status Form";
            return View(_sysLeatherStatus);
        }
        [HttpPost]
        public ActionResult LeatherStatus(SysLeatherStatus model)
        {
            if (model != null && model.LeatherStatusID != 0)
            {
                _vmMsg = _dalLeatherStatus.Update(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
            }
            else
            {
                _vmMsg = _dalLeatherStatus.Save(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
            }
            return Json(new { SourceID = _dalLeatherStatus.GetLeatherStatusID(), msg = _vmMsg });
        }

        [HttpPost]
        public ActionResult Delete(string leatherStatusId)
        {
            _vmMsg = _dalLeatherStatus.Delete(leatherStatusId, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
            return Json(new { msg = _vmMsg });
        }

        [HttpGet]
        public ActionResult GetLeatherStatus()
        {
            var sysSource = _dalLeatherStatus.GetAll().OrderByDescending(s => s.LeatherStatusID);
            return Json(sysSource);
        }
        //[HttpPost]
        //public ActionResult Create(string models)
        //{
            
        //    var leatherStatus = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<SysLeatherStatus>>(models).FirstOrDefault();
        //    if (leatherStatus != null)
        //    {
        //       _vmMsg = _dalLeatherStatus.Save(leatherStatus);
        //    }
        //    var result = new { data = leatherStatus, message = _vmMsg };
        //    return Json(result);
        //}

        //[HttpPost]
        //public ActionResult Update(string models)
        //{
        //    var leatherStatus = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<SysLeatherStatus>>(models).FirstOrDefault();

        //    if (leatherStatus != null)
        //    {
        //     _vmMsg = _dalLeatherStatus.Update(leatherStatus);
        //    }
        //    var result = new { data = leatherStatus, message = _vmMsg };
        //    return Json(result);
        //}

        //public JsonResult GetLeatherStatus()
        //{
        //    return Json(_dalLeatherStatus.GetAll().OrderByDescending(s=>s.LeatherStatusID));
        //}

        //[HttpPost]
        //public ActionResult Delete(string models)
        //{
        //    var leatherStatus = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<SysLeatherStatus>>(models).FirstOrDefault();
        //    _vmMsg = _dalLeatherStatus.Delete(leatherStatus);
        //    ViewBag.Message = _vmMsg.Msg;

        //    return Json(_vmMsg);
        //}
    }
	}
