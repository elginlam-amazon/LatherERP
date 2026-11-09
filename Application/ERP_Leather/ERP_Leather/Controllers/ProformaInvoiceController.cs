using Microsoft.AspNetCore.Http;
﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ERP.DatabaseAccessLayer.AppSetupGateway;
using ERP.BusinessLogicLayer.OperationManager;
using ERP.EntitiesModel.OperationModel;
using ERP.DatabaseAccessLayer.OperationGateway;
using ERP_Leather.ActionFilters;


namespace ERP_Leather.Controllers
{
    public class ProformaInvoiceController : Controller
    {

        
        DalProformaInvoice objDALPI = new DalProformaInvoice();
        BLLProformaInvoice objBLLPI = new BLLProformaInvoice();

        [HttpGet]
        [CheckUserAccess("ProformaInvoice/ProformaInvoice")]
        public ActionResult ProformaInvoice()
        {
            DalSysCurrency objCurrency = new DalSysCurrency();
            
            
            

            ViewBag.formTiltle = "PI/Indent";
            ViewBag.CurrencyList = objCurrency.GetAllActiveCurrency();
            //ViewBag.CountryList = objCountry.GetAll();
            //ViewBag.BeneficiaryBankList = objBranch.GetBeneficiaryAdvisingBankList();
            //ViewBag.AdvisingBankList = objBranch.GetBeneficiaryAdvisingBankList();
            //ViewBag.PortList = objPort.GetAll();
            return View();
        }

        [HttpPost]
        [CheckUserAccess("ProformaInvoice/ProformaInvoice")]
        public ActionResult ProformaInvoice(PRQChemicalPI model)
        {
            if (model.PIID == 0)
            {

                var msg = objBLLPI.Save(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")), "ProformaInvoice/ProformaInvoice");
                int PIID = objBLLPI.GetPIID();
                var PIItemList = objDALPI.GetPIItemList(PIID);
                return Json(new { Msg = msg, PIID = PIID, PIItemList = PIItemList });
            }
            else
            {
                var msg = objBLLPI.Update(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
                var PIItemList = objDALPI.GetPIItemList(model.PIID);
                return Json(new { Msg = msg, PIItemList = PIItemList });
            }
        }

        public ActionResult GetBuyerListForPI()
        {
            var Data = objDALPI.GetBuyerListForPI();
            return Json(Data);
        }

        public ActionResult GetBeneficiaryAdvisingBankList()
        {
            DalSysBranch objBranch = new DalSysBranch();

            var Data = objBranch.GetBeneficiaryAdvisingBankList();
            return Json(Data);
        }

        public ActionResult GetAllPortList()
        {
            DalSysPort objPort = new DalSysPort();

            var Data = objPort.GetAll();
            return Json(Data);
        }

        public ActionResult GetAllCountryList()
        {
            DalSysCountry objCountry = new DalSysCountry();

            var Data = objCountry.GetAll();
            return Json(Data);
        }

        public ActionResult GetOrderInformationForLOV()
        {
            var Orders = objDALPI.GetOrderInformationForLOV();
            return Json(Orders);
        }

        public ActionResult GetOrderItemListForLOV(string OrderID)
        {
            var Data = objDALPI.GetOrderItemListForLOV(OrderID);
            return Json(Data);
        }

        public ActionResult GetPIInformationForSearch()
        {
            var Data = objDALPI.GetPIInformationForSearch();
            return Json(Data);
        }

        public ActionResult GetPIDetailsAfterSearch(int PIID)
        {
            var Data = objDALPI.GetPIDetailsAfterSearch(PIID);

            return Json(Data);
        }

        public ActionResult ConfirPI(string PIID, string confirmComment)
        {
            bool checkConfirm = objDALPI.ConfirmPI(PIID, confirmComment);

            if (checkConfirm)
            {
                return Json(new { Msg = "Success" });
            }
            else
            {
                return Json(new { Msg = "Fail" });
            }
        }

        public ActionResult DeletePIItem(string _PIItemID, string _PageStatus)
        {
            bool CheckStatus = false; ;
            if (_PageStatus == "NCF" || _PageStatus == "Not Confirmed")
            {
                CheckStatus = objDALPI.DeletePIItem(_PIItemID);
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

        public ActionResult DeletePI(string _PIID, string _PageStatus)
        {
            bool CheckStatus = false; ;
            if (_PageStatus == "NCF" || _PageStatus == "Not Confirmed")
            {
                CheckStatus = objDALPI.DeletePI(_PIID);
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
	}
}