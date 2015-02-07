using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data.Odbc;
using System.Data;
using System.Xml;
using System.Configuration;
using System.Dynamic;

namespace QuestionTosser.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View("Index");
        }
        public ActionResult ClassRoom()
        {
            return View("ClassRoom");
        }
        public ActionResult SignalRChat()
        {
            return View("SignalRChat");
        }
        
        JsonResult Logout()
        {
            Session.RemoveAll();
            Session.Abandon();
            return Json(new { msg="You have successfully logged out", status="LogoutSucceed"});
        }
    }
}
