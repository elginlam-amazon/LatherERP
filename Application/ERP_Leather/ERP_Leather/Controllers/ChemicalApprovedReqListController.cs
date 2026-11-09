using ERP.DatabaseAccessLayer.OperationGateway;
using ERP.EntitiesModel.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace ERP_Leather.Controllers
{
    public class ChemicalApprovedReqListController : Controller
    {
         private ValidationMsg _vmMsg;
        private readonly DalChemPurcReq _objChemPurcReq;

        public ChemicalApprovedReqListController()
        {
            _vmMsg = new ValidationMsg();
            _objChemPurcReq = new DalChemPurcReq();
        }

        public ActionResult ChemicalApprovedReqList()
        {     
            return View();
        }

     
	}
}