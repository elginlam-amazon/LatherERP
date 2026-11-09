using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

using ERP.DatabaseAccessLayer.AppSetupGateway;
using ERP.EntitiesModel.AppSetupModel;
using ERP.EntitiesModel.BaseModel;
using ERP_Leather.ActionFilters;

namespace ERP_Leather.Controllers
{
    public class SupplierAddressController : Controller
    {
        private DalSysSupplierAddress _dalSysSupplierAddress;
        private SysSupplierAddress _sysSupplierAddress = new SysSupplierAddress();
        private ValidationMsg _vmMsg;

        public SupplierAddressController()
        {
            _vmMsg = new ValidationMsg();
            _dalSysSupplierAddress = new DalSysSupplierAddress();
        }

        [CheckUserAccess("SupplierAddress/SupplierAddress")]
        public ActionResult SupplierAddress()
        {
            ViewBag.formTiltle = "Supplier Address Form";
            return View(_sysSupplierAddress);
        }

        //[HttpPost]
        //public ActionResult Update(string models)
        //{
        //    var sysSupplierAddress = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<SysSupplierAddress>>(models).FirstOrDefault();
        //    _vmMsg = _dalSysSupplierAddress.Update(sysSupplierAddress);

        //    var result = new { data = sysSupplierAddress, message = _vmMsg };
        //    return Json(result);
        //}

        //[HttpPost]
        //public ActionResult Create(string models)
        //{
        //    var sysSupplierAddress = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<SysSupplierAddress>>(models).FirstOrDefault();
        //    _vmMsg = _dalSysSupplierAddress.Create(sysSupplierAddress);

        //    var result = new { data = sysSupplierAddress, message = _vmMsg };
        //    return Json(result);
        //}

        //public JsonResult GetSupplierAddressList()
        //{
        //    return Json(_dalSysSupplierAddress.GetAll().OrderByDescending(s => s.SupplierAddressID));
        //}

        //public ActionResult GetSupplierList()
        //{
        //    var allGradeItem = _dalSysSupplierAddress.GetSupplierList();

        //    return Json(allGradeItem);
        //}


        //[HttpPost]
        //public ActionResult Delete(string models)
        //{
        //    var sysSupplierAddress = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<SysSupplierAddress>>(models).FirstOrDefault();
        //    _vmMsg = _dalSysSupplierAddress.Delete(sysSupplierAddress);

        //    return Json(_vmMsg);
        //}
    }
}