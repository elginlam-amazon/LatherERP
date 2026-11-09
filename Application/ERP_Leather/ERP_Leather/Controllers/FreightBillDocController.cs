using Microsoft.AspNetCore.Http;
﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ERP.DatabaseAccessLayer.AppSetupGateway;
using ERP.DatabaseAccessLayer.OperationGateway;
using ERP.DatabaseAccessLayer.Utility;
using ERP.EntitiesModel.BaseModel;
using ERP.EntitiesModel.OperationModel;
using ERP_Leather.ActionFilters;

namespace ERP_Leather.Controllers
{
    public class FreightBillDocController : Controller
    {
        private readonly DalSysCurrency _objCurrency;
        private readonly UnitOfWork _unit;
        private readonly DalExpFreightBillDoc _dalExpFreightBillDoc;
        private ValidationMsg _validationMsg;
        private int _userId;

        public FreightBillDocController()
        {
            _unit = new UnitOfWork();
            _objCurrency = new DalSysCurrency();
            _dalExpFreightBillDoc = new DalExpFreightBillDoc();
            _validationMsg = new ValidationMsg();
        }

        // GET: FreightBillDoc
        [CheckUserAccess("FreightBillDoc/FreightBillDoc")]
        public ActionResult FreightBillDoc()
        {
            ViewBag.CurrencyList = _objCurrency.GetAllActiveCurrency();
            return View();
        }

        public ActionResult GetComInv()
        {
            var comInv = _unit.ExpCommercialInvoiceRepository.Get().Select(inv => new
            {
                inv.CIID,
                inv.CINo,
                inv.CIRefNo,
                OrdDeliveryMode = DalCommon.ReturnOrderDeliveryMode(inv.OrdDeliveryMode),
                inv.ShipmentFrom,
                inv.ShipmentTo,
                CIDate = string.Format("{0:dd/MM/yyyy}", inv.CIDate),
                RecordStatus = DalCommon.ReturnRecordStatus(inv.RecordStatus)
            }).ToList();

            return Json(comInv.OrderByDescending(ob => ob.CIID));
        }

        public ActionResult GetBillOfLadingListA()
        {
            var pl = _unit.ExpBillOfLadingRepository.Get().Select(ob => new
            {
                ob.BLID,
                ob.BLNo,
                ob.RefBLNo,
                ob.Shipper,
                ShipperCode = ob.Shipper == null ? "" : _unit.SysBuyerRepository.GetByID(ob.Shipper).BuyerCode,
                ShipperName = ob.Shipper == null ? "" : _unit.SysBuyerRepository.GetByID(ob.Shipper).BuyerName,
                BLDate = string.Format("{0:dd/MM/yyyy}", ob.BLDate),
                ob.CIID,
                CINo = ob.CIID == null ? "" : _unit.ExpCommercialInvoiceRepository.GetByID(ob.CIID).CINo,
                CIRefNo = ob.CIID == null ? "" : _unit.ExpCommercialInvoiceRepository.GetByID(ob.CIID).CIRefNo,
                CIDate = ob.CIID == null ? "" : string.Format("{0:dd/MM/yyyy}",
                _unit.ExpCommercialInvoiceRepository.GetByID(ob.CIID).CIDate),
                OrdDeliveryMode = ob.CIID == null ? "" : DalCommon.ReturnOrderDeliveryMode(
                _unit.ExpCommercialInvoiceRepository.GetByID(ob.CIID).OrdDeliveryMode),
                ShipmentFrom = ob.CIID == null ? "" : _unit.ExpCommercialInvoiceRepository.GetByID(ob.CIID).ShipmentFrom,
                ShipmentTo = ob.CIID == null ? "" : _unit.ExpCommercialInvoiceRepository.GetByID(ob.CIID).ShipmentTo,
                RecordStatus = ob.CIID == null ? "" : DalCommon.ReturnRecordStatus(_unit.ExpBillOfLadingRepository.GetByID(ob.BLID).RecordStatus)
            }).ToList();
            return Json(pl.OrderByDescending(ob => ob.BLID));
        }

        public ActionResult GetBillOfLadingListB(long ciid)
        {
            var pl = _unit.ExpBillOfLadingRepository.Get().Where(ob => ob.CIID == ciid).Select(ob => new
            {
                ob.BLID,
                ob.BLNo,
                ob.RefBLNo,
                ob.Shipper,
                ShipperCode = ob.Shipper == null ? "" : _unit.SysBuyerRepository.GetByID(ob.Shipper).BuyerCode,
                ShipperName = ob.Shipper == null ? "" : _unit.SysBuyerRepository.GetByID(ob.Shipper).BuyerName,
                ob.CIID,
                CINo = ob.CIID == null ? "" : _unit.ExpCommercialInvoiceRepository.GetByID(ob.CIID).CINo,
                CIRefNo = ob.CIID == null ? "" : _unit.ExpCommercialInvoiceRepository.GetByID(ob.CIID).CIRefNo,
                CIDate = ob.CIID == null ? "" : string.Format("{0:dd/MM/yyyy}", _unit.ExpCommercialInvoiceRepository.GetByID(ob.CIID).CIDate),
                BLDate = string.Format("{0:dd/MM/yyyy}", ob.BLDate),
                RecordStatus = DalCommon.ReturnRecordStatus(ob.RecordStatus)
            }).ToList();
            return Json(pl.OrderByDescending(ob => ob.BLID));
        }

        public ActionResult GetFrghtAgnt()
        {
            var pl = _unit.SysBuyerRepository.Get().Where(ob => ob.BuyerCategory == "Forwarder").Select(ob => new
            {
                ob.BuyerID,
                ob.BuyerCode,
                ob.BuyerName
            }).ToList();
            return Json(pl.OrderBy(ob => ob.BuyerName));
        }

        public ActionResult Save(ExpFreightBill model)
        {
            _userId = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
            _validationMsg = _dalExpFreightBillDoc.Save(model, _userId, "EXPPackingList/EXPPackingList");
            return Json(_validationMsg);
        }

        public ActionResult GetFreightBillDocs()
        {
            var frghtBillDocs = _dalExpFreightBillDoc.GetFrghtBillDocs();
            return Json(frghtBillDocs);
        }

        public ActionResult GetFrghtBillDoc(long fbdId)
        {
            var frghtBillDoc = _dalExpFreightBillDoc.GetFrghtBillDoc(fbdId);
            return Json(frghtBillDoc);
        }

        public ActionResult Delete(long id)
        {
            _validationMsg = _dalExpFreightBillDoc.Delete(id);
            return Json(_validationMsg);
        }

        public ActionResult Confirm(long id, string note)
        {
            _userId = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
            _validationMsg = _dalExpFreightBillDoc.Confirm(id, _userId, note);
            return Json(_validationMsg);
        }

        public ActionResult Check(long id, string note)
        {
            _userId = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
            _validationMsg = _dalExpFreightBillDoc.Check(id, _userId, note);
            return Json(_validationMsg);
        }
    }
}