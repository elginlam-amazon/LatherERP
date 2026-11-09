using Microsoft.AspNetCore.Http;
﻿using System;
using Microsoft.AspNetCore.Mvc;
using ERP.DatabaseAccessLayer.AppSetupGateway;
using ERP.BusinessLogicLayer.OperationManager;
using ERP.EntitiesModel.OperationModel;
using ERP.DatabaseAccessLayer.OperationGateway;
using ERP_Leather.ActionFilters;

namespace ERP_Leather.Controllers
{
    public class ChemicalConsumptionForFinishingController : Controller
    {
        DalChemicalConsumptionForFinishing objDal = new DalChemicalConsumptionForFinishing();
        BllChemicalConsumptionForFinishing objBll = new BllChemicalConsumptionForFinishing();

        [HttpGet]
        [CheckUserAccess("ChemicalConsumptionForFinishing/ChemicalConsumptionForFinishing")]
        public ActionResult ChemicalConsumptionForFinishing()
        {
            ViewBag.formTiltle = "Chemical Consumption In Finishing Process";
            ViewBag.ProductionProcess = DalCommon.GetCategoryWiseProductionProcess("CR");
            return View();
        }

        [HttpPost]
        [CheckUserAccess("ChemicalConsumptionForFinishing/ChemicalConsumptionForFinishing")]
        public ActionResult ChemicalConsumptionForFinishing(ChemicalConsumptionForCrusting model)
        {
            if (model.CLProductionID == 0)
            {
                var msg = objBll.Save(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")), "ChemicalConsumptionForCrusting/ChemicalConsumptionForCrusting");
                long CLProductionID = objBll.GetProductionID();
                long CLProductionItemID = objDal.GetCLProductionItemID(CLProductionID);
                long CLProductionColorID = objDal.GetCLProductionColorID(CLProductionID); ;

                return Json(new
                {
                    Msg = msg,
                    CLProductionID = CLProductionID,
                    CLProductionItemID = CLProductionItemID,
                    CLProductionColorID = CLProductionColorID,
                    LeatherList = objDal.GetLeatherListAfterSave(CLProductionID),
                    ChemicalList = objDal.GetChemicalListAfterSave(CLProductionID, model.ProductionFloor, model.CalculationBase)
                });

            }
            else
            {
                var msg = objBll.Update(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
                //var LeatherList = objDal.GetLeatherListAfterSave(model.WBProductionID);
                var ChemicalList = objDal.GetChemicalListAfterSave(model.CLProductionID, model.ProductionFloor, model.CalculationBase);


                return Json(new { Msg = msg, ChemicalList = ChemicalList });
            }

        }

        public ActionResult GetAllScheduleDate(string _ScheduleID)
        {
            return Json(objDal.GetAllScheduleDate(_ScheduleID));
        }

        public ActionResult GetAllScheduleItem(string _ScheduleDateID)
        {
            return Json(objDal.GetAllScheduleItem(_ScheduleDateID));
        }

        public ActionResult GetAllDrumList(string _SdulItemColorID)
        {
            return Json(objDal.GetAllDrumList(_SdulItemColorID));
        }

        public ActionResult GetSearchInformaiton()
        {
            var Data = objDal.GetSearchInformaiton();
            return Json(Data);
        }

        public ActionResult GetDetailsSearchInformaiton(long _CLProductionID)
        {
            var Data = objDal.GetDetailsSearchInformaiton(_CLProductionID);
            return Json(Data);
        }


        public ActionResult GetChemicalItemListForFixedLeather(long _CLProductionID, long _CLProductionDrumID, byte? _StoreID)
        {
            var Data = objDal.GetChemicalItemListForFixedLeather(_CLProductionID, _CLProductionDrumID, _StoreID);

            return Json(Data);
        }

        public ActionResult DeleteProductionItem(string _CLProdChemicalID, string _PageStatus)
        {
            bool CheckStatus = false; ;
            if (_PageStatus == "NCF" || _PageStatus == "Not Confirmed" || _PageStatus == "CHK" || _PageStatus == "Checked")
            {
                CheckStatus = objDal.DeleteProductionItem(_CLProdChemicalID);
                if (CheckStatus)
                {
                    return Json(new { Msg = "Success" });
                }
                else
                {
                    return Json(new { Msg = "Fail" });
                }
            }
            else if (_PageStatus == "CNF" || _PageStatus == "Confirmed")
            {
                return Json(new { Msg = "CNF" });
            }
            else
            {
                return Json(new { Msg = "Fail" });
            }
        }


        public ActionResult DeleteChemicalConsumption(string _CLProductionID, string _PageStatus)
        {
            bool CheckStatus = false; ;
            if (_PageStatus == "NCF" || _PageStatus == "Not Confirmed")
            {
                CheckStatus = objDal.DeleteChemicalConsumption(_CLProductionID);
                if (CheckStatus)
                {
                    return Json(new { Msg = "Success" });
                }
                else
                {
                    return Json(new { Msg = "Fail" });
                }
            }
            else if (_PageStatus == "CNF" || _PageStatus == "Confirmed")
            {
                return Json(new { Msg = "CNF" });
            }
            else
            {
                return Json(new { Msg = "Fail" });
            }
        }

        public ActionResult CheckChemicalConsumtion(string _CLProductionID, string _ConfirmComment)
        {

            var Data = objDal.CheckChemicalConsumtion(_CLProductionID, _ConfirmComment);

            if (Data)
            {
                return Json(new { Msg = "Success" });
            }
            else
            {
                return Json(new { Msg = "Fail" });
            }
        }

        public ActionResult ConfirmChemicalConsumption(long _CLProductionID, string _ConfirmComment)
        {

            var Data = objDal.ConfirmChemicalConsumption(_CLProductionID, _ConfirmComment, Convert.ToInt32(HttpContext.Session.GetString("UserID")));

            return Json(Data);
        }
	}
}