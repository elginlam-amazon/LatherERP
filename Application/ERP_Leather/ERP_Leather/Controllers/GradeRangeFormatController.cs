using Microsoft.AspNetCore.Http;
﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

using ERP.DatabaseAccessLayer.AppSetupGateway;
using ERP.EntitiesModel.AppSetupModel;
using ERP.EntitiesModel.BaseModel;
using ERP_Leather.ActionFilters;
using ERP.EntitiesModel.OperationModel;
using System;

namespace ERP_Leather.Controllers
{
    public class GradeRangeFormatController : Controller
    {
        private DalSysGradeRangeFormat _dalGradeRangeFormat;
        private SysGradeRangeFormat _sysGradeRangeFormat = new SysGradeRangeFormat();
        private ValidationMsg _vmMsg;

        public GradeRangeFormatController()
        {
            _vmMsg = new ValidationMsg();
            _dalGradeRangeFormat = new DalSysGradeRangeFormat();
        }

        [CheckUserAccess("GradeRangeFormat/GradeRangeFormat")]
        public ActionResult GradeRangeFormat()
        {
            ViewBag.formTiltle = "Grade Range Format Form";
            return View(_sysGradeRangeFormat);
        }

        [HttpPost]
        public ActionResult Update(string models)
        {
            var sysGradeRangeFormat = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<SysGradeRangeFormat>>(models).FirstOrDefault();
            _vmMsg = _dalGradeRangeFormat.Update(sysGradeRangeFormat, Convert.ToInt32(HttpContext.Session.GetString("UserID")));

            var result = new { data = sysGradeRangeFormat, message = _vmMsg };
            return Json(result);
        }

        public JsonResult GetGradeRangeFormatList()
        {
            return Json(_dalGradeRangeFormat.GetAll().OrderByDescending(s => s.FormatID));
        }
        public JsonResult GetGradeRangeFormatList2()
        {
            return Json("");
        }

        public JsonResult btnSaveGradeRange(GradeRangeTitle model)
        {
            var allGradeItem = _dalGradeRangeFormat.SaveGradeRange(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
            return Json(allGradeItem);
        }

        public ActionResult ListOfGradeRange()
        {
            var allData = _dalGradeRangeFormat.GetGradeformation();
            return Json(allData);

        }

        [HttpPost]
        public ActionResult Delete(string models)
        {
            var sysGradeRangeFormat = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<SysGradeRangeFormat>>(models).FirstOrDefault();
            _vmMsg = _dalGradeRangeFormat.Delete(sysGradeRangeFormat, Convert.ToInt32(HttpContext.Session.GetString("UserID")));

            return Json(_vmMsg);
        }



        public ActionResult GetAllGradeType()
        {
            var dalGradeRangeFormat = new DalSysGradeRangeFormat();
            var allGradeItem = dalGradeRangeFormat.GetGradeList();

            return Json(allGradeItem);
        }

        public ActionResult GetAllGradeRangeType()
        {
            var dalGradeRangeFormat = new DalSysGradeRangeFormat();
            var allGradeItem = dalGradeRangeFormat.GetGradeRangeList();

            return Json(allGradeItem);
        }

        public ActionResult GetDetailGradeRangeInformation(string GradeRangeID)
        {
            var allGradeItem = _dalGradeRangeFormat.GetDetailGradeRangeInformation(GradeRangeID);
            return Json(allGradeItem);
        }

        public ActionResult DeleteGradeRangeInfo(string GradeRangeID)
        {
            var allGradeItem = _dalGradeRangeFormat.DeleteGradeRangeInfo(GradeRangeID, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
            return Json(allGradeItem);
        }
        [HttpPost]
        public ActionResult DeleteGradeRangeFormatById(string FormatID)
        {
            _vmMsg = _dalGradeRangeFormat.DeleteGradeRangeFormatById(FormatID, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
            return Json(new { msg = _vmMsg });
        }

    }
}