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
    public class BuyerController : Controller
    {
        private DalSysBuyer _dalSysBuyer;
        private SysBuyer _sysBuyer = new SysBuyer();

        private ValidationMsg _vmMsg;

        public BuyerController()
        {
            _vmMsg = new ValidationMsg();
            _dalSysBuyer = new DalSysBuyer();
        }

        [CheckUserAccess("Buyer/Buyer")]
        public ActionResult Buyer()
        {
            ViewBag.formTiltle = "Buyer Form";
            return View(_sysBuyer);
        }

        [HttpPost]
        public ActionResult Buyer(SysBuyer model)
        {
            if (model != null && model.BuyerID != 0)
            {
                _vmMsg = _dalSysBuyer.Update(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
            }
            else
            {
                _vmMsg = _dalSysBuyer.Save(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
            }
            return Json(new { BuyerID = _dalSysBuyer.GetBuyerId(), msg = _vmMsg });
        }
        [HttpPost]
        public ActionResult Delete(string BuyerID)
        {
            _vmMsg = _dalSysBuyer.Delete(BuyerID);
            return Json(new { msg = _vmMsg });
        }

        [HttpPost]
        public ActionResult DeletedAddress(string buyerAddressId)
        {
            _vmMsg = _dalSysBuyer.DeletedAddress(buyerAddressId);
            return Json(new { msg = _vmMsg });
        }

        [HttpPost]
        public ActionResult DeletedAgent(string buyerAgentId)
        {
            _vmMsg = _dalSysBuyer.DeletedAgent(buyerAgentId);
            return Json(new { msg = _vmMsg });
        }

        public ActionResult GetBuyerAgentList()
        {
            var buyerAgentList = _dalSysBuyer.GetBuyerAgentList();
            return Json(buyerAgentList);
        }

        public ActionResult GetBuyerForSearchList()
        {
            var buyerAgentList = _dalSysBuyer.GetBuyerForSearchList().OrderByDescending(m => m.BuyerID);
            return Json(buyerAgentList);
        }

        public ActionResult GetBuyerList(string buyer)
        {
            SysBuyer sysBuyer = new SysBuyer();

            var buyerList = _dalSysBuyer.GetBuyerList(buyer);
            if (buyerList.Count > 1)
            {
                sysBuyer.Count = 0;
            }
            else
            {
                sysBuyer.Count = 1;
            }
            sysBuyer.BuyerList = buyerList;
            return Json(sysBuyer);
        }

        public ActionResult GetBuyerAgentSearchList(string buyer)
        {
            SysBuyer sysBuyer = new SysBuyer();


            var buyerList = _dalSysBuyer.GetBuyerAgentSearchList(buyer);
            if (buyerList.Count > 1)
            {
                sysBuyer.Count = 0;
            }
            else
            {
                sysBuyer.Count = 1;
            }
            sysBuyer.BuyerAgentList = buyerList;
            return Json(sysBuyer);
        }

        public JsonResult GetBuyerListForSearch()
        {
            var buyerAgentList = _dalSysBuyer.GetBuyerListForSearch();
            return Json(buyerAgentList);
        }

        public ActionResult GetBuyerAddressAndAgentList(string BuyerID)
        {
            var _sysBuyer = _dalSysBuyer.GetBuyerAddressAndAgentList(BuyerID);
            return Json(_sysBuyer);
        }
    }
}