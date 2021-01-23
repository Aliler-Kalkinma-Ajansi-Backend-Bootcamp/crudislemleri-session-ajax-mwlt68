using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using CrudOperations.Models;
using CrudOperations.Common;
using Data.Models;
using Data.Services;

namespace LayoutAndPartial.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private UserServices userServices;
        public HomeController()
        {
            userServices = new UserServices();
        }

        public IActionResult Index()
        {
            var userId=HttpContext.Session.GetInt32(StaticDatas.UserIdSession);
            if (userId != null && userId>0)
            {
                var users = userServices.GetUsers();
                return View("~/Views/Home/Users.cshtml", users);
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            int userId=userServices.Login(user);
            if (userId > 0)
            {
                HttpContext.Session.SetInt32(StaticDatas.UserIdSession, userId);
                var users = userServices.GetUsers();
                return View("~/Views/Home/Users.cshtml",users);
            }
            return View("~/Views/Home/Index.cshtml");
        }
        public IActionResult SignUp()
        {
            var userId = HttpContext.Session.GetInt32(StaticDatas.UserIdSession);
            if (userId != null && userId > 0)
            {
                var users = userServices.GetUsers();
                return View("~/Views/Home/Users.cshtml", users);
            }
            return View();
        }
        [HttpPost]
        public IActionResult SignUp(User user)
        {
            bool result = userServices.SingUp(user);
            if (result)
            {
                HttpContext.Session.Remove(StaticDatas.UserIdSession);
                return View("~/Views/Home/Index.cshtml");
            }
            else
            {
                return View("~/Views/Home/SignUp.cshtml");
            }
        }
        public IActionResult Users()
        {
            return View();
        }
        public IActionResult EditProfile()
        {
            var userId = HttpContext.Session.GetInt32(StaticDatas.UserIdSession);
            
            if (userId != null && userId > 0)
            {
                User user = userServices.GetUser(int.Parse(userId.ToString()));
                return View(user);
            }
            return View("~/Views/Home/Index.cshtml");
        }
        [HttpPost]
        public IActionResult EditProfile(User user)
        {
            var userId= HttpContext.Session.GetInt32(StaticDatas.UserIdSession);
            user.Id = int.Parse(userId.ToString());
            bool editResult =userServices.EditUser(user);
            if (editResult)
            {
                HttpContext.Session.Remove(StaticDatas.UserIdSession);
                return View("~/Views/Home/Index.cshtml");
            }
            else{
                return View();
            }
        }
        [HttpPost]
        public IActionResult DeleteUser(int deleteUserId)
        {
            var userId = HttpContext.Session.GetInt32(StaticDatas.UserIdSession);
            if (userId != null && userId > 0)
            {
                User  deletedUser= userServices.DeleteUserFromId(deleteUserId);
                if (deletedUser != null && deleteUserId == userId)
                {
                    HttpContext.Session.Remove(StaticDatas.UserIdSession);
                    return View("~/Views/Home/Index.cshtml");
                }
                else
                {
                    var users = userServices.GetUsers();
                    return View("~/Views/Home/Users.cshtml", users);
                }
            }
            HttpContext.Session.Remove(StaticDatas.UserIdSession);
            return View("~/Views/Home/Index.cshtml");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
