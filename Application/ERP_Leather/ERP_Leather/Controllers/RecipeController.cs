using Microsoft.AspNetCore.Http;
﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Sql;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ERP.DatabaseAccessLayer.AppSetupGateway;
using ERP.DatabaseAccessLayer.OperationGateway;
using ERP.EntitiesModel.BaseModel;
using ERP.EntitiesModel.OperationModel;
using ERP_Leather.ActionFilters;

namespace ERP_Leather.Controllers
{
    public class RecipeController : Controller
    {
        private ValidationMsg _vmMsg;
        private readonly DalRecipe _objDalRecipe;
        public RecipeController()
        {
            _vmMsg = new ValidationMsg();
            _objDalRecipe = new DalRecipe();
        }

       [CheckUserAccess("Recipe/Recipe")]
        public ActionResult Recipe()
        {
            ViewBag.Unit = new DalSysUnit().GetAllActiveLeatherUnit();
            ViewBag.ProductionProcess = new DalSysProductionProces().GetAll();
            ViewBag.Color = new DalSysColor().GetAll();
            ViewBag.formTiltle = "Recipe Preparation";
            return View();
        }
        [HttpPost]
        public ActionResult Recipe(PrdRecipe recipeModel )
        {
            _vmMsg = recipeModel != null && recipeModel.RecipeID == 0
                ? _objDalRecipe.SaveProductionRecipe(recipeModel, Convert.ToInt32(HttpContext.Session.GetString("UserID")),"Recipe/Recipe")
                : _objDalRecipe.UpdateProductionRecipe(recipeModel, Convert.ToInt32(HttpContext.Session.GetString("UserID")));

            return Json(new { msg = _vmMsg });
        }

        public ActionResult GetAllNcfRecipeList()
        {
            var recipes = _objDalRecipe.GetAllNcfRecipeList();
             return Json(recipes);
        }
        public ActionResult GetRecipeItemList(int recipeId)
        {
            var recipes = _objDalRecipe.GetRecipeItemList(recipeId);

            return Json(recipes);
        }

        public ActionResult DeletedRecipe(int recipeId)
        {
            _vmMsg = _objDalRecipe.DeletedRecipe(recipeId);
            return Json(new { msg = _vmMsg });

        }

        public ActionResult DeletedRecipeItem(int recipeItemId, string recordStatus)
        {
            _vmMsg = _objDalRecipe.DeletedRecipeItem(recipeItemId, recordStatus);
            return Json(new { msg = _vmMsg });

        }
        public ActionResult ConfirmData(int recipeId, string cnfComment)
        {
            _vmMsg = _objDalRecipe.ConfirmData(recipeId, cnfComment, Convert.ToInt32(HttpContext.Session.GetString("UserID")));

            return Json(new { msg = _vmMsg });
        }

        public ActionResult ApprovedData(int recipeId, string apvComment)
        {
            _vmMsg = _objDalRecipe.ApprovedData(recipeId,apvComment, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
            return Json(new { msg = _vmMsg });
        }
    }
}