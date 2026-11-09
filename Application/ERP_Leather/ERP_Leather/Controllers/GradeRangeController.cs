using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

using ERP.DatabaseAccessLayer.AppSetupGateway;
using ERP.EntitiesModel.AppSetupModel;
using ERP.EntitiesModel.BaseModel;
using ERP_Leather.ActionFilters;

namespace ERP_Leather.Controllers
{
    public class GradeRangeController : Controller
    {
        //
        // GET: /GradeRange/
         private DalSysGradeRange _dalGradeRange;
         private SysGradeRange _sysGradeRange = new SysGradeRange();
         private ValidationMsg _vmMsg;

        public GradeRangeController()
        {
            _vmMsg = new ValidationMsg();
            _dalGradeRange = new DalSysGradeRange();
        }

        [CheckUserAccess("GradeRange/GradeRange")]
        public ActionResult GradeRange()
        {
            ViewBag.formTiltle = "Grade Range Form";
            return View(_sysGradeRange);
        }

        [HttpPost]
        public ActionResult Update(string models)
        {
            var sysGradeRange = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<SysGradeRange>>(models).FirstOrDefault();
            _vmMsg = _dalGradeRange.Update(sysGradeRange);

            var result = new { data = sysGradeRange, message = _vmMsg };
            return Json(result);
        }

        [HttpPost]
        public ActionResult Create(string models)
        {
            var sysGradeRange = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<SysGradeRange>>(models).FirstOrDefault();
            _vmMsg = _dalGradeRange.Create(sysGradeRange); 

            var result = new { data = sysGradeRange, message = _vmMsg };
            return Json(result);
        }

        public JsonResult GetGradeRangeList()
        {
            return Json(_dalGradeRange.GetAll().OrderByDescending(s => s.GradeRangeID));
        }

        [HttpPost]
        public ActionResult Delete(string models)
        {
            var sysGradeRange = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<SysGradeRange>>(models).FirstOrDefault();
            _vmMsg = _dalGradeRange.Delete(sysGradeRange);
            ViewBag.Message = _vmMsg.Msg;

            return Json(_vmMsg);
        }

        //GenerateGradeReport
       

	}
}