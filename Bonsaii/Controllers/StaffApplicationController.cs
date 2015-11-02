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
    public class StaffApplicationController : BaseController
    {
        // GET: StaffApplication
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            return View(db.StaffApplications.ToList());
        }

        // GET: StaffApplication/Details/5
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
            else if (flag == 1)
            {
                    staffApplications.AuditStatus = "未审核";
                    staffApplications.ApplyDate = DateTime.Today;
                   
            }
             db.SaveChanges();
            return View(staffApplications);
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
            StaffApplications staffApplication = new StaffApplications();
            var item = (from p in db.BillProperties where BillTypeNumber == p.Type select p).FirstOrDefault();

            staffApplication.BillTypeName = item.TypeName;
            return Json(staffApplication);
        }



        [HttpPost]
        //返回Json对象
        public JsonResult StaffNumberSearch(string number)
        {
            try{
            var items = (from p in db.Staffs where p.StaffNumber.Contains(number) || p.Name.Contains(number)  select p.StaffNumber +" "+ p.Name).ToList();
            
            //return Json(item);
               return Json(new
           {
               success = true,
               data = items
           });
        }
        catch(Exception e){return Json(new{success = false,msg=e.Message});}
        }

        [HttpPost]
        public JsonResult SendData(string StaffNumber)
        {
            StaffApplications staffApplication = new StaffApplications();
            var itemStaff = (from p in db.Staffs
                             where StaffNumber == p.StaffNumber
                             select p.Name).FirstOrDefault();
            //赋值
            staffApplication.StaffName = itemStaff; 
            //staffChange.EffectiveDate = null;
            staffApplication.HopeLeaveDate = null;
            staffApplication.ApplyDate = null;
            return Json(staffApplication);

        }

        // GET: StaffApplication/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StaffApplication/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,BillTypeNumber,BillTypeName,BillNumber,StaffNumber,StaffName,ApplyDate,HopeLeaveDate,LeaveType,LeaveReason,Remark,AuditStatus")]StaffApplications staffApplications)
        {
            if (ModelState.IsValid)
            {
                staffApplications.AuditStatus = "未提交";
           
                db.StaffApplications.Add(staffApplications);
                db.SaveChanges();
                return RedirectToAction("Index");
            }


            return View(staffApplications);
        }

        // GET: StaffApplication/Edit/5
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

        // POST: StaffApplication/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(StaffApplications staffApplications)
        {
            if (ModelState.IsValid)
            {
                db.Entry(staffApplications).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(staffApplications);
        }

        // GET: StaffApplication/Delete/5
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

        // POST: StaffApplication/Delete/5
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
