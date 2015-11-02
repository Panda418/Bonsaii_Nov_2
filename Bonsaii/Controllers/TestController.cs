using Bonsaii.Authorize;
using Bonsaii.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Bonsaii.Controllers
{
    public class TestController : Controller
    {
        public ActionResult Tree()
        {
            return View();
        }

        public ActionResult JsTree()
        {

            return View();
        }

        public ActionResult JsTreeTest()
        {
            return View();
        }

        public ActionResult Panda()
        {
            return View();
        }
    }
}