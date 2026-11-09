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
    public class ArticleController : Controller
    {
        private DalSysArticle _dalSysArticle;
        private DalSysColor objDalSysColor = new DalSysColor();
        private ValidationMsg _vmMsg;

        public ArticleController()
        {
            _vmMsg = new ValidationMsg();
            _dalSysArticle = new DalSysArticle();
        }

        [CheckUserAccess("Article/Article")]
        public ActionResult Article()
        {
            ViewBag.formTiltle = "Article Form";
            ViewBag.ddlColor = objDalSysColor.GetAllActiveColor();
            return View();
        }

        [HttpPost]
        public ActionResult Article(SysArticle model)
        {
            if (model != null && model.ArticleID != 0)
            {
                _vmMsg = _dalSysArticle.Update(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
            }
            else
            {
                _vmMsg = _dalSysArticle.Create(model, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
            }
            return Json(new { ArticleID = _dalSysArticle.GetArticleID(), msg = _vmMsg });
        }

        [HttpPost]
        public ActionResult Delete(string sourceId)
        {
            _vmMsg = _dalSysArticle.Delete(sourceId, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
            return Json(new { msg = _vmMsg });
        }

        [HttpGet]
        public ActionResult GetArticleList()
        {
            var sysArticle = _dalSysArticle.GetAll().OrderByDescending(s => s.ArticleID);
            return Json(sysArticle);
        }
    }
}