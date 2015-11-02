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
    public class ReserveFieldController : BaseController
    {
        // GET: ReserveRecord
        public ActionResult Index()
        {
            return View(db.ReserveFields.ToList());
        }

        // GET: ReserveRecord/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReserveField reserveField = db.ReserveFields.Find(id);
            if (reserveField == null)
            {
                return HttpNotFound();
            }
            return View(reserveField);
        }

        // GET: ReserveRecord/Create
        public ActionResult Create()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "部门信息表", Value = "Departments" });
            items.Add(new SelectListItem { Text = "员工档案表", Value = "Staffs" });
            items.Add(new SelectListItem { Text = "员工技能表", Value = "StaffSkills" });
            items.Add(new SelectListItem { Text = "人事变更申请表", Value = "StaffChanges" });
            items.Add(new SelectListItem { Text = "离职申请表", Value = "StaffApplications" });
            items.Add(new SelectListItem { Text = "离职档案表", Value = "StaffArchives" });
            items.Add(new SelectListItem { Text = "合同管理表", Value = "Contracts" });
            ViewBag.List = items;
            return View();
        }

        // POST: ReserveRecord/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TableName,FieldName,Description,Status")] ReserveField reserveField)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "部门信息表", Value = "Departments" });
            items.Add(new SelectListItem { Text = "员工档案表", Value = "Staffs" });
            items.Add(new SelectListItem { Text = "员工技能表", Value = "StaffSkills" });
            items.Add(new SelectListItem { Text = "员工技能表", Value = "StaffSkills" });
            items.Add(new SelectListItem { Text = "人事变更申请表", Value = "StaffChanges" });
            items.Add(new SelectListItem { Text = "离职申请表", Value = "StaffApplications" });
            items.Add(new SelectListItem { Text = "离职档案表", Value = "StaffArchives" });
            items.Add(new SelectListItem { Text = "合同管理表", Value = "Contracts" });
            ViewBag.List = items;

            if (ModelState.IsValid)
            {
                db.ReserveFields.Add(reserveField);
                /*状态设置为有效*/
                reserveField.Status = "true";
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(reserveField);
        }

        // GET: ReserveRecord/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReserveField reserveField = db.ReserveFields.Find(id);
            if (reserveField == null)
            {
                return HttpNotFound();
            }
            return View(reserveField);
        }

        // POST: ReserveRecord/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TableName,FieldName,Description,Status")] ReserveField reserveField)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reserveField).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(reserveField);
        }

        // GET: ReserveRecord/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReserveField reserveField = db.ReserveFields.Find(id);
            if (reserveField == null)
            {
                return HttpNotFound();
            }
            return View(reserveField);
        }

        // POST: ReserveRecord/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ReserveField reserveField = db.ReserveFields.Find(id);
            db.ReserveFields.Remove(reserveField);
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
