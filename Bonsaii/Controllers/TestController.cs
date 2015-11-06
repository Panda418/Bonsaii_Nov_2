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
    public class TestController : BaseController
    {

        public JsonResult GetStaff(){
            if (HttpContext.Request.Headers["Accept"].Contains("application/json"))
            {
                Staff staff = db.Staffs.Find(int.Parse(Request["Id"]));
                return Json(staff);
            }
            return null;
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

        public ActionResult TestTree()
        {
            return View();
        }

        public JsonResult tree()
        {
            QTree tree1 = new QTree()
            {
                id = 1,
                url = "/Test/Test",
                text = "MenuOne",
                check = false,
                children = null
            };
            QTree tree2 = new QTree()
            {
                id = 2,
                text = "MenuTwo",
                children = null
            };
            List<QTree> tmp = new List<QTree>();
            tmp.Add(tree1);
            tmp.Add(tree2);
            QTree tree3 = new QTree()
            {
                id = 3,
                text = "MainMenu",
                children = tmp
            };

            return Json(new
            {
                success = true,
                msg = "haha",
                type = "Test",
                obj = tree3,
            });
        }

        public class QTree
        {
            public int id;
            public String url;
            public String text;
            public bool check;
            public List<QTree> children;
        }

    }
}