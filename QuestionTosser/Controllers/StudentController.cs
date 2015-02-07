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
    public class StudentController : Controller
    {
        public JsonResult StudentRegister()
        {
            return Json(new { msg = "NotImplemented" });
        }
        public JsonResult StudentLogin()
        {
            return Json(new { msg = "NotImplemented" });
        }
    }
}
