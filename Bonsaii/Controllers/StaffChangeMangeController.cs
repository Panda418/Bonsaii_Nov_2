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
    public class StaffChangeMangeController : BaseController
    {

        // GET: StaffChangeMange
        public ActionResult Index()
        {
            return View(db.StaffChanges.ToList());
        }
        public ActionResult List(int? listchoose)
        {
            
            if (listchoose == 0)
            { 
                var item= from p in db.StaffChanges where p.AuditStatus=="未审核" select p;
                return View(item.ToList());
            }
            else
                if (listchoose == 1)
                {
                    var item = from p in db.StaffChanges where p.AuditStatus != "未审核" select p;
                    return View(item.ToList());
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
           // return View(db.StaffChanges.ToList());
        }

       // public 
        // GET: StaffChangeMange/Details/5
        public ActionResult Details(int? id,int? flag)
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
            else 
            {
                if (flag == 0)
                {
                    staffChange.AuditStatus = "审核通过";

                    var itemStaff =( from p in db.Staffs where p.StaffNumber == staffChange.StaffNumber select p).FirstOrDefault();
                    {
                        itemStaff.Name = staffChange.Name;
                        itemStaff.Gender = staffChange.Gender;
                        itemStaff.Department = staffChange.Department;
                        itemStaff.WorkType = staffChange.WorkType;
                        itemStaff.Position = staffChange.Position;
                        itemStaff.IdentificationType = staffChange.IdentificationType;
                        itemStaff.Nationality = staffChange.Nationality;
                        itemStaff.IdentificationNumber = staffChange.IdentificationNumber;
                        itemStaff.Entrydate = staffChange.Entrydate;
                        itemStaff.ClassOrder = staffChange.ClassOrder;
                        itemStaff.JobState = staffChange.JobState;
                        itemStaff.AbnormalChange = staffChange.AbnormalChange;
                        itemStaff.FreeCard = staffChange.FreeCard;
                        itemStaff.WorkProperty = staffChange.WorkProperty;
                        itemStaff.ApplyOvertimeSwitch = staffChange.ApplyOvertimeSwitch;
                        itemStaff.Source = staffChange.Source;
                        itemStaff.QualifyingPeriodFull = staffChange.QualifyingPeriodFull;
                        itemStaff.MaritalStatus = staffChange.MaritalStatus;
                        itemStaff.BirthDate = staffChange.BirthDate;
                        itemStaff.NativePlace = staffChange.NativePlace;
                        itemStaff.HealthCondition = staffChange.HealthCondition;
                        itemStaff.Nation = staffChange.Nation;
                        itemStaff.Address = staffChange.Address;
                        itemStaff.VisaOffice = staffChange.VisaOffice;
                        itemStaff.HomeTelNumber = staffChange.HomeTelNumber;
                        itemStaff.EducationBackground = staffChange.EducationBackground;
                        itemStaff.GraduationSchool = staffChange.GraduationSchool;
                        itemStaff.SchoolMajor = staffChange.SchoolMajor;
                        itemStaff.Degree = staffChange.Degree;
                        itemStaff.Introducer = staffChange.Introducer;
                        itemStaff.IndividualTelNumber = staffChange.IndividualTelNumber;
                        itemStaff.BankCardNumber = staffChange.BankCardNumber;
                        itemStaff.UrgencyContactMan = staffChange.UrgencyContactMan;
                        itemStaff.UrgencyContactAddress = staffChange.UrgencyContactAddress;
                        itemStaff.UrgencyContactPhoneNumber = staffChange.UrgencyContactPhoneNumber;
                        itemStaff.InBlacklist = staffChange.InBlacklist;
                        itemStaff.PhysicalCardNumber = staffChange.PhysicalCardNumber;
                    }
                }
                else
               if(flag==1)
                 staffChange.AuditStatus = "审核不通过";
            }
            db.SaveChanges();
            return View(staffChange);
        }

        // GET: StaffChangeMange/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StaffChangeMange/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,BillTypeNumber,BillTypeName,BillNumber,StaffNumber,Name,Gender,Department,WorkType,Position,IdentificationType,Nationality,IdentificationNumber,Entrydate,ClassOrder,JobState,AbnormalChange,FreeCard,WorkProperty,ApplyOvertimeSwitch,Source,QualifyingPeriodFull,MaritalStatus,BirthDate,NativePlace,HealthCondition,Nation,Address,VisaOffice,HomeTelNumber,EducationBackground,GraduationSchool,SchoolMajor,Degree,Introducer,IndividualTelNumber,BankCardNumber,UrgencyContactMan,UrgencyContactAddress,UrgencyContactPhoneNumber,InBlacklist,PhysicalCardNumber,AuditStatus,EffectiveDate,Remark")] StaffChange staffChange)
        {
            if (ModelState.IsValid)
            {
                db.StaffChanges.Add(staffChange);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(staffChange);
        }

        // GET: StaffChangeMange/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: StaffChangeMange/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BillTypeNumber,BillTypeName,BillNumber,StaffNumber,Name,Gender,Department,WorkType,Position,IdentificationType,Nationality,IdentificationNumber,Entrydate,ClassOrder,JobState,AbnormalChange,FreeCard,WorkProperty,ApplyOvertimeSwitch,Source,QualifyingPeriodFull,MaritalStatus,BirthDate,NativePlace,HealthCondition,Nation,Address,VisaOffice,HomeTelNumber,EducationBackground,GraduationSchool,SchoolMajor,Degree,Introducer,IndividualTelNumber,BankCardNumber,UrgencyContactMan,UrgencyContactAddress,UrgencyContactPhoneNumber,InBlacklist,PhysicalCardNumber,AuditStatus,EffectiveDate,Remark")] StaffChange staffChange)
        {
            if (ModelState.IsValid)
            {
                db.Entry(staffChange).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(staffChange);
        }

        // GET: StaffChangeMange/Delete/5
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

        // POST: StaffChangeMange/Delete/5
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
