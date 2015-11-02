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
    public class StaffApplicationManageController : BaseController
    {
        // GET: StaffApplicationManage
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List(int? listchoose)
        {
            if (listchoose == 0)
            {
                var item = from p in db.StaffApplications where p.AuditStatus == "未审核" select p;
                return View(item.ToList());
            }
            else
                if (listchoose == 1)
                {
                    var item = from p in db.StaffApplications where p.AuditStatus != "未审核" select p;
                    return View(item.ToList());
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
        }

        // GET: StaffApplicationManage/Details/5

        /*?代表：可为空*/
        public ActionResult Details(int? id,int? flag)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffApplications staffApplications = db.StaffApplications.Find(id);
            if (staffApplications == null)
            {
                return HttpNotFound();
            }
            else if(flag==1)
            {
                staffApplications.AuditStatus = "审核通过";

                var items = (from p in db.Staffs where p.StaffNumber == staffApplications.StaffNumber select p).FirstOrDefault();
                items.JobState = "离职";
                items.LeaveType = staffApplications.LeaveType;
                items.LeaveDate = staffApplications.HopeLeaveDate;
                items.LeaveReason=staffApplications.LeaveReason;
            }
            else if (flag == 0)
            {
                staffApplications.AuditStatus = "审核不通过";
            }
            db.SaveChanges();
            return View(staffApplications);
        }

        // GET: StaffApplicationManage/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StaffApplicationManage/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,BillTypeNumber,BillTypeName,BillNumber,StaffNumber,StaffName,ApplyDate,HopeLeaveDate,LeaveType,LeaveReason,Remark,AuditStatus")] StaffApplications staffApplications)
        {
            if (ModelState.IsValid)
            {
                db.StaffApplications.Add(staffApplications);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(staffApplications);
        }

        // GET: StaffApplicationManage/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffApplications staffApplications = db.StaffApplications.Find(id);
            if (staffApplications == null)
            {
                return HttpNotFound();
            }
            return View(staffApplications);
        }

        // POST: StaffApplicationManage/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BillTypeNumber,BillTypeName,BillNumber,StaffNumber,StaffName,ApplyDate,HopeLeaveDate,LeaveType,LeaveReason,Remark,AuditStatus")] StaffApplications staffApplications)
        {
            if (ModelState.IsValid)
            {
                db.Entry(staffApplications).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(staffApplications);
        }

        // GET: StaffApplicationManage/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffApplications staffApplications = db.StaffApplications.Find(id);
            if (staffApplications == null)
            {
                return HttpNotFound();
            }
            return View(staffApplications);
        }

        // POST: StaffApplicationManage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StaffApplications staffApplications = db.StaffApplications.Find(id);
            db.StaffApplications.Remove(staffApplications);
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
