using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bonsaii.Models;
using PagedList;
namespace Bonsaii.Controllers
{
    public class StaffController : BaseController
    {
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Number" : "";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            var Staffs = from s in db.Staffs select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                Staffs = Staffs.Where(s => s.StaffNumber.Contains(searchString) || s.BillTypeName.Contains(searchString));

            }
            switch (sortOrder)
            {
                case "Number":
                    Staffs = Staffs.OrderByDescending(s => s.StaffNumber);
                    break;
                default:
                    Staffs = Staffs.OrderBy(s => s.StaffNumber);
                    break;
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);

            /*查找员工基本信息表预留字段(name)*/
            var fieldNameList = (from p in db.ReserveFields where p.TableName == "Staffs" select p).ToList();
            ViewBag.fieldNameList = fieldNameList;
            /*查找员工基本信息表预留字段(value)*/
            var fieldValueList = (from sr in db.StaffReserves
                                  join rf in db.ReserveFields on sr.FieldId equals rf.Id
                                  select new StaffViewModel { Number = sr.Number, Description = rf.Description, Value = sr.Value }).ToList();
            ViewBag.fieldValueList = fieldValueList;

            return View(Staffs.ToPagedList(pageNumber, pageSize));//使用ToPagedList方法时，需要引入using PagedList系统集成的分页函数。
        }


        // GET: Staff/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = db.Staffs.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }

            /*查找员工基本信息表预留字段(value)*/
            var fieldValueList = (from sr in db.StaffReserves
                                  join rf in db.ReserveFields on sr.FieldId equals rf.Id
                                  where staff.Number == sr.Number
                                  select new StaffViewModel { Number = sr.Number, Description = rf.Description, Value = sr.Value }).ToList();
            ViewBag.fieldValueList = fieldValueList;

            return View(staff);
        }

        // GET: Staff/Create
        public ActionResult Create()
        {
            List<SelectListItem> item = db.Departments.ToList().Select(c => new SelectListItem
            {
                Value = c.Name,//保存的值
                Text = c.Name//显示的值
            }).ToList();

            //增加一个null选项
            SelectListItem i = new SelectListItem();
            i.Value = "";
            i.Text = " ";
            i.Selected = true;
            item.Add(i);

            ViewBag.List = item;
          
            List<SelectListItem> item1 = db.Nationalities.ToList().Select(c => new SelectListItem
            {
                Value = c.Nation,//保存的值
                Text = c.Nation//显示的值
            }).ToList();

            ViewBag.List1 = item1;

            List<SelectListItem> item2 = db.Healths.ToList().Select(c => new SelectListItem
            {
                Value = c.HealthCondition,//保存的值
                Text = c.HealthCondition//显示的值
            }).ToList();

          
            ViewBag.List2 = item2;
          
            List<SelectListItem> item3 = db.Nations.ToList().Select(c => new SelectListItem
            {
                Value = c.Nationality,//保存的值
                Text = c.Nationality//显示的值
            }).ToList();

            ViewBag.List3 = item3;

            List<SelectListItem> item4 = db.Backgrounds.ToList().Select(c => new SelectListItem
            {
                Value = c.XueLi,//保存的值
                Text = c.XueLi//显示的值
            }).ToList();
            
           
            ViewBag.List4 = item4;
            
           

            /*查找员工基本信息表预留字段*/
            var fieldList = (from p in db.ReserveFields where p.TableName == "Staffs" select p).ToList();
            ViewBag.fieldList = fieldList;

            return View();
        }

        // POST: Staff/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Staff staff)
        {
            if (ModelState.IsValid)
            {

                var tmp = db.Staffs.Where(p => p.StaffNumber.Equals(staff.StaffNumber)).ToList();
                    if (tmp.Count != 0)
                    {
                        //List<SelectListItem> item = db.Departments.ToList().Select(c => new SelectListItem
                        //{
                        //    Value = c.Name,//保存的值
                        //    Text = c.Name//显示的值
                        //}).ToList();
                        //ViewBag.List = item;

                        ModelState.AddModelError("", "抱歉，该工号已经被注册！");

                        return View(staff);

                    }
                    else
                    {
                        /*Step1：先保存员工固定字段*/
                        db.Staffs.Add(staff);
                        db.SaveChanges();

                        /*查找员工基本信息表预留字段(name)*/
                        var fieldList = (from p in db.ReserveFields where p.TableName == "Staffs" select p).ToList();
                        ViewBag.fieldList = fieldList;

                        /*遍历，保存员工基本信息预留字段*/
                        foreach (var temp in fieldList)
                        {
                            StaffReserve sr = new StaffReserve();
                            sr.Number = staff.Number;
                            sr.FieldId = temp.Id;
                            sr.Value = Request[temp.FieldName];
                            /*占位，为了在Index中显示整齐的格式*/
                            if (sr.Value == null) sr.Value = " ";
                            db.StaffReserves.Add(sr);
                            db.SaveChanges();
                        } 
                        return RedirectToAction("Index");
                    }
            }

            return View(staff);
        }

        // GET: Staff/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = db.Staffs.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            List<SelectListItem> item = new List<SelectListItem>();
            SelectListItem i = new SelectListItem();
            i.Value = staff.Department;
            i.Text = staff.Department;
            item.Add(i);
            item = db.Departments.ToList().Select(c => new SelectListItem
            {
                Value = c.Name,//保存的值
                Text = c.Name//显示的值
            }).ToList();
            ViewBag.List = item;

            /*查找员工基本信息表预留字段*/
            var fieldList = (from sr in db.StaffReserves
                                  join rf in db.ReserveFields on sr.FieldId equals rf.Id
                                  where staff.Number == sr.Number
                                  select new StaffViewModel { Description = rf.Description, Value = sr.Value }).ToList();
            ViewBag.fieldList = fieldList;


            return View(staff);
        }

        // POST: Staff/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Number,BillTypeName,BillTypeNumber,StaffNumber,Name,Gender,Department,WorkType,Position,IdentificationType,Nationality,IdentificationNumber,Entrydate,ClassOrder,AppSwitch,JobState,AbnormalChange,FreeCard,WorkProperty,ApplyOvertimeSwitch,Source,QualifyingPeriodFull,MaritalStatus,BirthDate,NativePlace,HealthCondition,Nation,Address,VisaOffice,HomeTelNumber,EducationBackground,GraduationSchool,SchoolMajor,Degree,Introducer,IndividualTelNumber,BankCardNumber,UrgencyContactMan,UrgencyContactAddress,UrgencyContactPhoneNumber,InBlacklist,PhysicalCardNumber,LeaveDate,LeaveType,LeaveReason,AccountVersion,AuditStatus,BindingNumber")] Staff staff)
        {
            if (ModelState.IsValid)
            {
                /*查找员工信息预留字段(value)*/
                var fieldValueList = (from sr in db.StaffReserves
                                      join rf in db.ReserveFields on sr.FieldId equals rf.Id
                                      where sr.Number == staff.Number
                                      select new StaffViewModel { Number = sr.Id, Description = rf.Description, Value = sr.Value }).ToList();
                /*给预留字段赋值*/
                foreach (var temp in fieldValueList)
                {
                    StaffReserve sr = db.StaffReserves.Find(temp.Number);
                    sr.Value = Request[temp.Description];
                    db.SaveChanges();
                }
                /*保存固定字段*/
                db.Entry(staff).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(staff);
        }

        // GET: Staff/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = db.Staffs.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }

            /*查找员工基本信息表预留字段(value)*/
            var fieldValueList = (from sr in db.StaffReserves
                                  join rf in db.ReserveFields on sr.FieldId equals rf.Id
                                  where sr.Number==id
                                  select new StaffViewModel { Number = sr.Number, Description = rf.Description, Value = sr.Value }).ToList();
            ViewBag.fieldValueList = fieldValueList;

            return View(staff);
        }

        // POST: Staff/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            /*Step1：删除预留字段*/
            var item = (from sr in db.StaffReserves
                        where sr.Number == id
                        select new StaffViewModel { Number = sr.Id }).ToList();
            foreach (var temp in item)
            {
                StaffReserve sr = db.StaffReserves.Find(temp.Number);
                db.StaffReserves.Remove(sr);
            }
            db.SaveChanges();

            /*Step2：删除固定字段*/
            Staff staff = db.Staffs.Find(id);
            db.Staffs.Remove(staff);
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
