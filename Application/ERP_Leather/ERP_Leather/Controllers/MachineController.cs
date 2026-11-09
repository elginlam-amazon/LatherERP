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
    public class MachineController : Controller
    {
        private DalSysMachine _dalSysMachine;
        DalSysUnit objDalSysUnit = new DalSysUnit();
        private ValidationMsg _vmMsg;

        public MachineController()
        {
            _vmMsg = new ValidationMsg();
            _dalSysMachine = new DalSysMachine();
        }

        [CheckUserAccess("Machine/Machine")]
        public ActionResult Machine()
        {
            ViewBag.formTiltle = "Machine Form";
            ViewBag.ddlUnitList = objDalSysUnit.GetCommonUnit();
            return View();
        }

        [HttpPost]
        public ActionResult Machine(SysMachine model)
        {
            if (model != null && model.MachineID != 0)
            {
                _vmMsg = _dalSysMachine.Update(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
            }
            else
            {
                _vmMsg = _dalSysMachine.Create(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
            }
            return Json(new { MachineID = _dalSysMachine.GetMachineID(), msg = _vmMsg });
        }

        [HttpPost]
        public ActionResult Delete(string sourceId)
        {
            _vmMsg = _dalSysMachine.Delete(sourceId, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
            return Json(new { msg = _vmMsg });
        }

        [HttpGet]
        public ActionResult GetMachineList()
        {
            var sysMachine = _dalSysMachine.GetAll().OrderByDescending(s => s.MachineID);
            return Json(sysMachine);
        }
	}
}