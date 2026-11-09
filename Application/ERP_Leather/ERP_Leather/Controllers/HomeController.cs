using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ERP.BusinessLogicLayer.AppSetupManager;
using ERP.DatabaseAccessLayer.AppSetupGateway;
using ERP.EntitiesModel.AppSetupModel;

namespace ERP_Leather.Controllers
{
    public class HomeController : Controller
    {
        DalMenuItems dalObj = new DalMenuItems();
        DalUser objUser = new DalUser();

        public ActionResult Index()
        {
            if (HttpContext.Session.GetString("UserID") == null) return RedirectToAction("SessionOutMsg", "Home");
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginModel objModellogin)
        {
            var strErrorMsg = string.Empty;
            var strExpiryValMsg = string.Empty;

            try
            {
                if (objModellogin.LoginID != null)
                {
                    if (string.IsNullOrEmpty(strExpiryValMsg))
                    {
                        var userInfo = objUser.GetUserInformation(null, objModellogin.LoginID).FirstOrDefault();
                        if (userInfo != null)
                        {
                            if (UserAuthenticator.UserAuthenticator.ValidatePassword(objModellogin.Password.Trim(), userInfo.Password))
                            {
                                HttpContext.Session.SetString("UserID", Convert.ToInt32(userInfo.UserID).ToString());
                                HttpContext.Session.SetString("UserName", userInfo.FullName);
                                if (!userInfo.IsActive || userInfo.IsLocked && userInfo.IsLocked)
                                {
                                    return Json(new { val = 1, result = "User not valid", url = Url.Action("Index", "Home") });
                                }

                                if (userInfo.IsPasswordAccepted != null && (bool)userInfo.IsPasswordAccepted)
                                {
                                    HttpContext.Session.SetString("IsLogged", "true");
                                    var claims = new[] { new Claim(ClaimTypes.Name, userInfo.UserID.ToString()) };
                                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
                                    var userUrl = dalObj.UserAccessPermissionList(Convert.ToInt32(userInfo.UserID));
                                    HttpContext.Session.SetString("userUrlPermission", Newtonsoft.Json.JsonConvert.SerializeObject(userUrl));

                                    return RedirectToAction("Index", "Home");
                                }
                                HttpContext.Session.SetString("IsLogged", "true");
                                return RedirectToAction("ChangePassword", "Home");
                            }
                            strErrorMsg = "Incorrect User Password!";
                        }
                        else
                        {
                            strErrorMsg = "Incorrect User Information!";
                        }
                    }
                    else
                    {
                        strErrorMsg = strExpiryValMsg;
                    }
                }
                ViewBag.Msg = strErrorMsg;
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        public ActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("login", "Home");
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(int userID, string password)
        {
            var count = objUser.ChangePassword(userID, UserAuthenticator.UserAuthenticator.GetHashPassword(password));
            return count > 0
                ? (ActionResult)Json(new { result = "Password has been changed", url = Url.Action("Index", "Home") })
                : RedirectToAction("Error", "Home");
        }

        [HttpGet]
        public ActionResult LoadMenu(int? id)
        {
            var nodes = dalObj.LoadMenu(id, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
            return Json(nodes);
        }

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult SessionOutMsg()
        {
            return View();
        }
    }
}
