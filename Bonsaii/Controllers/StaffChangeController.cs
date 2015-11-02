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
    public class StaffChangeController : BaseController
    {
        // GET: StaffChange
        public ActionResult Index()
        {
            return View(db.StaffChanges.ToList());
        }

        public ActionResult List()
        {
            return View(db.StaffChanges.ToList());
        }

        // GET: StaffChange/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffChange staffChange = db.StaffChanges.Find(id);
            if (staffChange == null)
            {
                return HttpNotFound();
            }
            return View(staffChange);
        }

        // GET: StaffChange/Create
        public ActionResult Create()
        {
            /*单据类别编号下拉列表*/


            /*单据类别名称下拉列表*/


            /*单号*/


            /*员工工号下拉列表StaffNumberList*/
            //List<SelectListItem> item = db.Staffs.ToList().Select(c => new SelectListItem
            //{
            //    Value = c.StaffNumber,
            //    Text = c.StaffNumber
            //}).ToList();

            //SelectListItem i = new SelectListItem();
            //i.Value = "";
            //i.Text = " ";
            //item.Insert(0, i);

            //ViewBag.StaffNumberList = item;
            
            return View();
        }

        // POST: StaffChange/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,BillTypeNumber,BillTypeName,BillNumber,StaffNumber,Name,Gender,Department,WorkType,Position,IdentificationType,Nationality,IdentificationNumber,Entrydate,ClassOrder,JobState,AbnormalChange,FreeCard,WorkProperty,ApplyOvertimeSwitch,Source,QualifyingPeriodFull,MaritalStatus,BirthDate,NativePlace,HealthCondition,Nation,Address,VisaOffice,HomeTelNumber,EducationBackground,GraduationSchool,SchoolMajor,Degree,Introducer,IndividualTelNumber,BankCardNumber,UrgencyContactMan,UrgencyContactAddress,UrgencyContactPhoneNumber,InBlacklist,PhysicalCardNumber,AuditStatus,EffectiveDate,Remark,LeaveType,LeaveDate,LeaveReason")] StaffChange staffChange)
        {
            //List<SelectListItem> item = db.Staffs.ToList().Select(c => new SelectListItem
            //{
            //    Value = c.StaffNumber,
            //    Text = c.StaffNumber
            //}).ToList();

            //SelectListItem i = new SelectListItem();
            //i.Value = "";
            //i.Text = " ";
            //item.Insert(0,i);

            //ViewBag.StaffNumberList = item;

            if (ModelState.IsValid)
            {
                staffChange.AuditStatus = "未审核";
                db.StaffChanges.Add(staffChange);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(staffChange);
        }

        /*实现单据类别搜索：显示单据类别编号和单据类别名称*/
        [HttpPost]
        public JsonResult BillTypeNumberSearch(string number)
        {

            try
            {
                var items = (from p in db.BillProperties where p.Type.Contains(number) || p.TypeName.Contains(number) select p.Type+" "+p.TypeName).ToList();

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

            staffChange.BillTypeName = item.TypeName;
            return Json(staffChange);
        }

        /*实现员工工号搜索：显示姓名和工号*/
        [HttpPost]
        public JsonResult StaffnumberSearch(string number)
       {
         
           try {
              // var item = db.Staffs.Where(w => (w.StaffNumber).Contains(number)).ToList().Select(w => new { id=w.StaffNumber,name=w.StaffNumber});
               var items = (from p in db.Staffs where p.StaffNumber.Contains(number) || p.Name.Contains(number) select p.StaffNumber +" "+ p.Name).ToList();//.ToList().Select p;

           
               return Json(new
           {
               success = true,
               data = items
           });
        }
        catch(Exception e){return Json(new{success = false,msg=e.Message});}

       }
        /*根据工号调出员工的信息*/
        [HttpPost]
        public JsonResult SendData(string StaffNumber)
        {
            StaffChange staffChange = new StaffChange();
            var itemStaff = (from p in db.Staffs
                             where StaffNumber == p.StaffNumber 
                            select p).FirstOrDefault();
            //赋值
            staffChange.Name = itemStaff.Name;
            staffChange.Gender = itemStaff.Gender;
            staffChange.Department = itemStaff.Department;
            staffChange.WorkType = itemStaff.WorkType;
            staffChange.Position = itemStaff.Position;
            staffChange.IdentificationType = itemStaff.IdentificationType;
            staffChange.Nationality = itemStaff.Nationality;
            staffChange.IdentificationNumber = itemStaff.IdentificationNumber;
            staffChange.Entrydate = itemStaff.Entrydate;
            staffChange.ClassOrder = itemStaff.ClassOrder;
            staffChange.JobState = itemStaff.JobState;
            staffChange.AbnormalChange = itemStaff.AbnormalChange;
            staffChange.FreeCard = itemStaff.FreeCard;
            staffChange.WorkProperty = itemStaff.WorkProperty;
            staffChange.ApplyOvertimeSwitch = itemStaff.ApplyOvertimeSwitch;
            staffChange.Source = itemStaff.Source;
            staffChange.QualifyingPeriodFull = itemStaff.QualifyingPeriodFull;
            staffChange.MaritalStatus = itemStaff.MaritalStatus;
            staffChange.BirthDate = itemStaff.BirthDate;
            staffChange.NativePlace = itemStaff.NativePlace;
            staffChange.HealthCondition = itemStaff.HealthCondition;
            staffChange.Nation = itemStaff.Nation;
            staffChange.Address = itemStaff.Address;
            staffChange.VisaOffice = itemStaff.VisaOffice;
            staffChange.HomeTelNumber = itemStaff.HomeTelNumber;
            staffChange.EducationBackground = itemStaff.EducationBackground;
            staffChange.GraduationSchool = itemStaff.GraduationSchool;
            staffChange.SchoolMajor = itemStaff.SchoolMajor;
            staffChange.Degree = itemStaff.Degree;
            staffChange.Introducer = itemStaff.Introducer;
            staffChange.IndividualTelNumber = itemStaff.IndividualTelNumber;
            staffChange.BankCardNumber = itemStaff.BankCardNumber;
            staffChange.UrgencyContactMan = itemStaff.UrgencyContactMan;
            staffChange.UrgencyContactAddress = itemStaff.UrgencyContactAddress;
            staffChange.UrgencyContactPhoneNumber = itemStaff.UrgencyContactPhoneNumber;
            staffChange.InBlacklist = itemStaff.InBlacklist;
            staffChange.PhysicalCardNumber = itemStaff.PhysicalCardNumber;
           //staffChange.AuditStatus = itemStaff.AuditStatus;
            staffChange.LeaveDate = itemStaff.LeaveDate;
            staffChange.LeaveType = itemStaff.LeaveType;
            staffChange.LeaveReason = itemStaff.LeaveReason;
            staffChange.EffectiveDate = null;
        
            return Json(staffChange);
        
        }

        // GET: StaffChange/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    StaffChange staffChange = db.StaffChanges.Find(id);
        //    if (staffChange == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(staffChange);
        //}

        // POST: StaffChange/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,BillTypeNumber,BillTypeName,BillNumber,StaffNumber,Name,Gender,Department,WorkType,Position,IdentificationType,Nationality,IdentificationNumber,Entrydate,ClassOrder,JobState,AbnormalChange,FreeCard,WorkProperty,ApplyOvertimeSwitch,Source,QualifyingPeriodFull,MaritalStatus,BirthDate,NativePlace,HealthCondition,Nation,Address,VisaOffice,HomeTelNumber,EducationBackground,GraduationSchool,SchoolMajor,Degree,Introducer,IndividualTelNumber,BankCardNumber,UrgencyContactMan,UrgencyContactAddress,UrgencyContactPhoneNumber,InBlacklist,PhysicalCardNumber,AuditStatus,EffectiveDate,Remark")] StaffChange staffChange)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(staffChange).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(staffChange);
        //}

        // GET: StaffChange/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffChange staffChange = db.StaffChanges.Find(id);
            if (staffChange == null)
            {
                return HttpNotFound();
            }
            return View(staffChange);
        }

        // POST: StaffChange/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StaffChange staffChange = db.StaffChanges.Find(id);
            db.StaffChanges.Remove(staffChange);
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
