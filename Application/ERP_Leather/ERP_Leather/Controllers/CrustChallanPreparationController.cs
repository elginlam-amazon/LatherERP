using Microsoft.AspNetCore.Http;
﻿using System;
using Microsoft.AspNetCore.Mvc;
using ERP.DatabaseAccessLayer.AppSetupGateway;
using ERP.BusinessLogicLayer.OperationManager;
using ERP.EntitiesModel.OperationModel;
using ERP.EntitiesModel.AppSetupModel;
using ERP.DatabaseAccessLayer.OperationGateway;
using ERP_Leather.ActionFilters;

namespace ERP_Leather.Controllers
{
    public class CrustChallanPreparationController : Controller
    {
        BllCrustChallanPreparation objBll = new BllCrustChallanPreparation();
        DalCrustChallanPreparation objDal = new DalCrustChallanPreparation();
        
        
        [CheckUserAccess("CrustChallanPreparation/CrustChallanPreparation")]
        public ActionResult CrustChallanPreparation()
        {
            ViewBag.formTiltle = "Crust Challan Preparation";
            return View();
        }

        [HttpPost]
        [CheckUserAccess("CrustChallanPreparation/CrustChallanPreparation")]
        public ActionResult CrustChallanPreparation(SysArticleChallan model)
        {
            if (model.ArticleChallanID == 0)
            {
                var msg = objBll.Save(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
                long ArticleChallanID = objBll.GetChallanID();

                var DetailList = objDal.GetArticleDetailListAfterSave(ArticleChallanID);
                var ColorList = objDal.GetArticleColorListAfterSave(ArticleChallanID);
                return Json(new { Msg = msg, ArticleChallanID = ArticleChallanID, DetailList = DetailList, ColorList = ColorList });
            }
            else
            {
                var msg = objBll.Update(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
                var DetailList = objDal.GetArticleDetailListAfterSave(model.ArticleChallanID);
                var ColorList = objDal.GetArticleColorListAfterSave(model.ArticleChallanID);


                return Json(new { Msg = msg, DetailList = DetailList, ColorList = ColorList });
            }
        }

        public ActionResult GetSearchInformation()
        {
            return Json(objDal.GetSearchInformation());
        }

        public ActionResult GetChallanDetails(string _ArticleChallanID)
        {
             var DetailList = objDal.GetArticleDetailListAfterSave(Convert.ToInt64(_ArticleChallanID));
             var ColorList = objDal.GetArticleColorListAfterSave(Convert.ToInt64(_ArticleChallanID));
             return Json(new { DetailList = DetailList, ColorList = ColorList });
        }

        public ActionResult DeleteColorItem(string _ArticleChallanIDColor, string _PageStatus)
        {
            bool CheckStatus = false; ;
            if (_PageStatus == "NCF" || _PageStatus == "Not Confirmed")
            {
                CheckStatus = objDal.DeleteColorItem(_ArticleChallanIDColor);
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


        public ActionResult DeleteArticleDetail(string _ArticleChallanDtlID, string _PageStatus)
        {
            bool CheckStatus = false; ;
            if (_PageStatus == "NCF" || _PageStatus == "Not Confirmed")
            {
                CheckStatus = objDal.DeleteArticleDetail(_ArticleChallanDtlID);
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

        public ActionResult DeleteChallan(string _ArticleChallanID, string _PageStatus)
        {
            bool CheckStatus = false; ;
            if (_PageStatus == "NCF" || _PageStatus == "Not Confirmed")
            {
                CheckStatus = objDal.DeleteChallan(_ArticleChallanID);
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

        public ActionResult ConfirmArticleChallan(string _ArticleChallanID, string _ConfirmNote)
        {
            bool checkConfirm = objDal.ConfirmArticleChallan(_ArticleChallanID, _ConfirmNote);

            if (checkConfirm)
            {
                return Json(new { Msg = "Success" });
            }
            else
            {
                return Json(new { Msg = "Fail" });
            }
        }
	}
}