using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bonsaii.Models;
using Bonsaii.Models.Train;

namespace Bonsaii.Controllers
{
    public class TrainStartController : BaseController
    {
        // GET: TrainStart
        public ActionResult Index()
        {
            return View(db.TrainStarts.ToList());
        }

        // GET: TrainStart/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrainStart trainStart = db.TrainStarts.Find(id);
            if (trainStart == null)
            {
                return HttpNotFound();
            }
            return View(trainStart);
        }

        // GET: TrainStart/Create
        public ActionResult Create()
        {
            return View();
        }

        /// <树状结构>
        ///  
        public JsonResult tree()
        {
            /*查找员工表*/
            /*查找部门表*/


            QTree tree1 = new QTree()
            {
                id = 1,
                text = "1",
                check = false,
               // children = 
            };
            QTree tree2 = new QTree()
            {
                id = 2,
                text = "2",
                check = false,
                children = null
            };
            List<QTree> tmp = new List<QTree>();
            tmp.Add(tree1);
            tmp.Add(tree2);

            /*获取公司全称*/
            string companyFullName = this.CompanyFullName;
            /*树的根节点*/
            QTree root = new QTree()
            {
                id = 3,
                text = companyFullName,/*根节点为公司*/
                children = tmp
            };

            return Json(
                new
                { success = true,
                    msg="jkjkl",
                    type="Test",
                    obj= root,
                });
         }
        /// </树状结构>
    

        /*实现单据类别搜索：显示单据类别编号和单据类别名称*/
        [HttpPost]
        public JsonResult BillTypeNumberSearch(string number)
        {

            try
            {
                var items = (from p in db.BillProperties where p.Type.Contains(number) || p.TypeName.Contains(number) select p.Type + " " + p.TypeName).ToList();

                return Json(new
                {
                    success = true,
                    data = items
                });
            }
            catch (Exception e) { return Json(new { success = false, msg = e.Message }); }

        }

        /*实现:自动填充单据名称*/
        [HttpPost]
        public JsonResult SendBillTypeNumber(string BillTypeNumber)
        {
            StaffChange staffChange = new StaffChange();
            var item = (from p in db.BillProperties where BillTypeNumber == p.Type select p).FirstOrDefault();
            /*生成单号*/
            string str = Generate.GenerateBillNumber(BillTypeNumber,this.ConnectionString);
            /*生成单号*/
            staffChange.BillNumber = str;
            staffChange.BillTypeName = item.TypeName;
            return Json(staffChange);
        }

        /*:*/
        //[HttpPost]
        //public JsonResult SendBillType

        // POST: TrainStart/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TrainStart trainStart)
        {
            if (ModelState.IsValid)
            {
                db.TrainStarts.Add(trainStart);
                db.SaveChanges();
                /**/
                return RedirectToAction("Index");
            }
            return View(trainStart);
        }

        // GET: TrainStart/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrainStart trainStart = db.TrainStarts.Find(id);
            if (trainStart == null)
            {
                return HttpNotFound();
            }
            return View(trainStart);
        }

        // POST: TrainStart/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BillTypeNumber,BillTypeName,BillNumber,TrainType,StartDate,EndDate,TrainCost,TellNumber,AuditStatus,Remark")] TrainStart trainStart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trainStart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(trainStart);
        }

        // GET: TrainStart/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrainStart trainStart = db.TrainStarts.Find(id);
            if (trainStart == null)
            {
                return HttpNotFound();
            }
            return View(trainStart);
        }

        // POST: TrainStart/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TrainStart trainStart = db.TrainStarts.Find(id);
            db.TrainStarts.Remove(trainStart);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
