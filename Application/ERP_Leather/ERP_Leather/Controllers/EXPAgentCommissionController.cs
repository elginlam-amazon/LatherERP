using Microsoft.AspNetCore.Http;
﻿using ERP.DatabaseAccessLayer.AppSetupGateway;
using ERP.DatabaseAccessLayer.OperationGateway;
using ERP.EntitiesModel.AppSetupModel;
using ERP.EntitiesModel.BaseModel;
using ERP.EntitiesModel.OperationModel;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ERP_Leather.ActionFilters;

namespace ERP_Leather.Controllers
{
    public class EXPAgentCommissionController : Controller
    {
        private DalEXPAgentCommission Dalobject;
        private ValidationMsg _vmMsg;
        DalSysCurrency objCurrency = new DalSysCurrency();

        public EXPAgentCommissionController()
        {
            _vmMsg = new ValidationMsg();
            Dalobject = new DalEXPAgentCommission();
        }

        [CheckUserAccess("EXPAgentCommission/EXPAgentCommission")]
        public ActionResult EXPAgentCommission()
        {
            ViewBag.formTiltle = "Agent Commission";
            ViewBag.ddlCurrencyList = objCurrency.GetAllActiveCurrency();
            return View();
        }

        [HttpPost]
        public ActionResult EXPAgentCommission(EXPAgentCommission model)
        {
            _vmMsg = model.AgentComID == 0 ? Dalobject.Save(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")), "EXPAgentCommission/EXPAgentCommission") : Dalobject.Update(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
            return Json(new { AgentComID = Dalobject.GetAgentComID(), AgentComNo = Dalobject.GetAgentComNo(), msg = _vmMsg });
        }

        [HttpPost]
        public ActionResult ConfirmedEXPAgentCommission(EXPAgentCommission model)
        {
            _vmMsg = Dalobject.ConfirmedEXPAgentCommission(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
            return Json(new { msg = _vmMsg });
        }

        [HttpPost]
        public ActionResult CheckedEXPAgentCommission(EXPAgentCommission model)
        {
            _vmMsg = Dalobject.CheckedEXPAgentCommission(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
            return Json(new { msg = _vmMsg });
        }

        //public ActionResult GetBuyerAgentList(string BuyerType)
        //{
        //    var buyerlist = Dalobject.GetBuyerAgentList(BuyerType);
        //    return Json(buyerlist);
        //}

        public ActionResult GetBuyerAgentList(string CIID, string CIAmount)
        {
            var buyerlist = Dalobject.GetBuyerAgentList(CIID, CIAmount);
            return Json(buyerlist);
        }

        public ActionResult GetPIInfoList(string CIID)
        {
            var pIInfoList = Dalobject.GetPIInfoList(CIID);
            return Json(pIInfoList);
        }

        public ActionResult GetBuyerAgentPopupList()
        {
            var agentPopupList = Dalobject.GetBuyerAgentPopupList();
            return Json(agentPopupList);
        }

        //public ActionResult GetBuyerList(string BuyerAgentID)
        //{
        //    var buyerlist = Dalobject.GetBuyerList(BuyerAgentID);
        //    return Json(buyerlist);
        //}

        //public ActionResult GetCINoList(string BuyerID)
        public ActionResult GetCINoList()
        {
            //var buyerlist = Dalobject.GetCINoList(BuyerID);
            var buyerlist = Dalobject.GetCINoList();
            return Json(buyerlist);
        }

        public ActionResult GetAllActiveCurrency()
        {
            var buyerlist = objCurrency.GetAllActiveCurrency();
            return Json(buyerlist);
        }

        public ActionResult GetAgentCommissionInformation()
        {
            return Json(Dalobject.GetAgentCommissionInformation());
        }

        public ActionResult GetAgentCommissionBuyerList(string AgentComID)
        {
            return Json(Dalobject.GetAgentCommissionBuyerList(AgentComID));
        }

        public ActionResult GetAgentCommissionBuyerCIList(string AgentComID, string BuyerID)
        {
            EXPAgentCommission model = new EXPAgentCommission();
            if (string.IsNullOrEmpty(AgentComID)) return Json(model);
            model.EXPAgentCommissionBuyerList = Dalobject.GetAgentCommissionBuyerList(AgentComID);
            if (model.EXPAgentCommissionBuyerList.Count > 0)
                model.EXPAgentCommissionBuyerCIList = Dalobject.GetAgentCommissionBuyerCIList(AgentComID, BuyerID);
            return Json(model);
        }

        public ActionResult GetAllGridList(string AgentComID)
        {
            EXPAgentCommission model = new EXPAgentCommission();
            if (string.IsNullOrEmpty(AgentComID)) return Json(model);
            //model.EXPAgentCommissionBuyerList = Dalobject.GetAgentCommissionBuyerList(AgentComID);
            //if (model.EXPAgentCommissionBuyerList.Count > 0)
            model.EXPAgentCommissionBuyerCIList = Dalobject.GetAgentCommissionBuyerCIList(AgentComID);
            return Json(model);
        }

        public ActionResult GetAgentCommBuyerCIList(string AgentComID, string BuyerID)
        {
            return Json(Dalobject.GetAgentCommissionBuyerCIList(AgentComID, BuyerID));
        }

        [HttpPost]
        public ActionResult DeletedAgentCom(string AgentComID)
        {
            long? agentComID = Convert.ToInt64(AgentComID);
            _vmMsg = Dalobject.DeletedAgentCom(agentComID);
            return Json(new { msg = _vmMsg });
        }

        [HttpPost]
        public ActionResult DeletedAgentComBuyer(string AgentComID, string BuyerID)
        {
            long? agentComID = Convert.ToInt64(AgentComID);
            int? buyerID = Convert.ToInt32(BuyerID);
            _vmMsg = Dalobject.DeletedAgentComBuyer(agentComID, buyerID);
            return Json(new { msg = _vmMsg });
        }

        [HttpPost]
        //public ActionResult DeletedAgentComBuyerCI(string AgentComID, string BuyerID, string CIID)
        public ActionResult DeletedAgentComBuyerCI(string AgentComCIID)
        {
            long? agentComCIID = Convert.ToInt64(AgentComCIID);
            //int? buyerID = Convert.ToInt32(BuyerID);
            //long? cIID = Convert.ToInt64(CIID);
            _vmMsg = Dalobject.DeletedAgentComBuyerCI(agentComCIID);
            return Json(new { msg = _vmMsg });
        }
    }
}