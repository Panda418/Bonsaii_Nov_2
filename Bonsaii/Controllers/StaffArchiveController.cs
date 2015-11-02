using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bonsaii.Models;

namespace Bonsaii.Controllers
{
    public class StaffArchiveController : BaseController
    {

        // GET: StaffArchive
        public ActionResult Index()
        {

            //var items = (from p in db.StaffArchives where p.JobState == "离职" select p).ToList();
            return View(db.StaffArchives.ToList());
            //return View(items);
        }

        // GET: StaffArchive/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffArchive staffArchive = db.StaffArchives.Find(id);
            if (staffArchive == null)
            {
                return HttpNotFound();
            }
            return View(staffArchive);
        }

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
            StaffArchive staffArchive = new StaffArchive();
            var item = (from p in db.BillProperties where BillTypeNumber == p.Type select p).FirstOrDefault();

            staffArchive.BillTypeName = item.TypeName;
            return Json(staffArchive);
        }


        [HttpPost]
        /*员工工号搜索 返回Json对象*/
        public JsonResult StaffNumberSearch(string number)
        {
            try
            {
                //var items = (from p in db.Staffs where p.StaffNumber.Contains(number) || p.Name.Contains(number) select p.StaffNumber + " " + p.Name).ToList();
                /*找到已经离职的员工0.0*/
                var items = (from p in db.Staffs where p.JobState=="离职" select p.StaffNumber + " " + p.Name).ToList();
                return Json(new
                {
                    success = true,
                    data = items
                });
            }
            catch (Exception e) { return Json(new { success = false, msg = e.Message }); }
        }

         /*实现自动填充员工名字*/
        [HttpPost]
        public JsonResult SendData(string StaffNumber)
        {
            StaffArchive staffArchive = new StaffArchive();
           
             var itemStaff = (from p in db.Staffs
                            where StaffNumber == p.StaffNumber
                             select p).FirstOrDefault();
            //赋值
             staffArchive.StaffName = itemStaff.Name;
             staffArchive.LeaveDate = itemStaff.LeaveDate;
             staffArchive.ReApplyDate= null;
             return Json(staffArchive);

        }

        // GET: StaffArchive/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StaffArchive/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,BillTypeNumber,BillTypeName,BillNumber,StaffNumber,StaffName,LeaveDate,ReApplyDate,Remark")]StaffArchive staffArchive)
        {
            if (ModelState.IsValid)
            {
                db.StaffArchives.Add(staffArchive);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(staffArchive);
        }

        // GET: StaffArchive/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffArchive staffArchive = db.StaffArchives.Find(id);
            if (staffArchive == null)
            {
                return HttpNotFound();
            }
            return View(staffArchive);
        }

        // POST: StaffArchive/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BillTypeNumber,BillTypeName,BillNumber,StaffNumber,StaffName,LeaveDate,ReApplyDate,Remark")] StaffArchive staffArchive)
        {
            if (ModelState.IsValid)
            {
                db.Entry(staffArchive).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(staffArchive);
        }

        // GET: StaffArchive/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffArchive staffArchive = db.StaffArchives.Find(id);
            if (staffArchive == null)
            {
                return HttpNotFound();
            }
            return View(staffArchive);
        }

        // POST: StaffArchive/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StaffArchive staffArchive = db.StaffArchives.Find(id);
            db.StaffArchives.Remove(staffArchive);
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
