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
    public class StaffSkillController : BaseController
    {
        // GET: StaffSkill
        public ActionResult Index()
        {
            /*员工技能表固定信息*/
            var q = from ss in db.StaffSkills
                    join s in db.Staffs on ss.StaffNumber equals s.StaffNumber
                    join sp in db.SkillParameters on ss.SkillNumber equals sp.SkillNumber
                    select new StaffSkillView{ Id=ss.Id,BillType = ss.BillType, BillNumber = ss.BillNumber, StaffNumber = ss.StaffNumber, StaffName=s.Name,SkillNumber=ss.SkillNumber,SkillName=sp.SkillName,SkillGrade=ss.SkillGrade,SkillRemark=ss.SkillRemark }
                     ;
            /*查找员工技能预留字段(name)*/
            var fieldNameList = (from p in db.ReserveFields where p.TableName == "StaffSkills" select p).ToList();
            ViewBag.fieldNameList = fieldNameList;
            /*查找员工技能预留字段(value)*/
            var fieldValueList = (from ssr in db.StaffSkillReserves
                                  join rf in db.ReserveFields on ssr.FieldId equals rf.Id
                                  select new StaffSkillView { Id=ssr.Number ,Description = rf.Description, Value = ssr.Value }).ToList();
            ViewBag.fieldValueList=fieldValueList;
            return View(q);
        }
        public JsonResult Index1()
        {
            List<Object> obj = new List<Object>();
            var Staffs = db.Staffs.ToList();
            foreach (var temp in Staffs)
            {



                obj.Add(new { number = temp.StaffNumber,name=temp.Name });


               // return Json(obj);
            }
            return Json(obj);
        }

        // GET: StaffSkill/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffSkill staffSkill = db.StaffSkills.Find(id);
            if (staffSkill == null)
            {
                return HttpNotFound();
            }
            var skillParameters = db.SkillParameters.Where(sp => sp.SkillNumber.Equals(staffSkill.SkillNumber));
            var staffs = db.Staffs.Where(s => s.StaffNumber.Equals(staffSkill.StaffNumber));
            foreach (var temp in skillParameters)
            {
                staffSkill.SkillName = temp.SkillName;
            }
            foreach (var temp in staffs)
            {
                staffSkill.StaffName = temp.Name;
            }
            /*查找员工技能预留字段(name)*/
            var fieldNameList = (from p in db.ReserveFields where p.TableName == "StaffSkills" select p).ToList();
            ViewBag.fieldNameList = fieldNameList;
            /*查找员工技能预留字段(value)*/
            var fieldValueList = (from ssr in db.StaffSkillReserves
                                  join rf in db.ReserveFields on ssr.FieldId equals rf.Id
                                  where ssr.Number == staffSkill.Id
                                  select new StaffSkillView { Id = ssr.Number, Description = rf.Description, Value = ssr.Value }).ToList();
            ViewBag.fieldValueList = fieldValueList;

            return View(staffSkill);
        }

        // GET: StaffSkill/Create
        public ActionResult Create()
        {
            /*查找员工技能预留字段(name)*/
            var fieldList = (from p in db.ReserveFields where p.TableName == "StaffSkills" select p).ToList();
            ViewBag.fieldList = fieldList;
            return View();
        }

        // POST: StaffSkill/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StaffSkill staffSkill)
        {
            if (ModelState.IsValid)
            {
                /*先保存员工技能固定的字段（为了生成主键Id）*/
                db.StaffSkills.Add(staffSkill);
                db.SaveChanges();
                /*查找员工技能预留字段(name)*/
                var fieldList = (from p in db.ReserveFields where p.TableName == "StaffSkills" select p).ToList();
                ViewBag.fieldList = fieldList;
                /*遍历，保存员工技能变化的字段*/
                foreach (var temp in fieldList) {
                    StaffSkillReserve ssr = new StaffSkillReserve();
                    ssr.Number=staffSkill.Id;
                    ssr.FieldId=temp.Id;
                    ssr.Value=Request[temp.FieldName];
                    /*占位，为了在Index中显示整齐的格式*/
                    if (ssr.Value == null) ssr.Value = " ";
                    db.StaffSkillReserves.Add(ssr);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return View(staffSkill);
        }

        // GET: StaffSkill/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            StaffSkill staffSkill = db.StaffSkills.Find(id);
           
            if (staffSkill == null)
            {
                return HttpNotFound();
            } 
          
             var skillParameters = db.SkillParameters.Where(sp => sp.SkillNumber.Equals(staffSkill.SkillNumber));
             var staffs = db.Staffs.Where(s => s.StaffNumber.Equals(staffSkill.StaffNumber));
             foreach(var temp in skillParameters)
            {
                staffSkill.SkillName = temp.SkillName;
            }
             foreach (var temp in staffs)
             {
                 staffSkill.StaffName = temp.Name;
             }

             /*查找员工技能预留字段(value)*/
             var fieldValueList = (from ssr in db.StaffSkillReserves
                                   join rf in db.ReserveFields on ssr.FieldId equals rf.Id
                                   //where ssr.Number == staffSkill.Id
                                   where ssr.Number == id 
                                   select new StaffSkillView {Description = rf.Description,Value = ssr.Value}).ToList();
             ViewBag.fieldValueList = fieldValueList;

            return View(staffSkill);
        }

        // POST: StaffSkill/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(StaffSkill staffSkill)
        {
            if (ModelState.IsValid)
            {
                StaffSkill ss = db.StaffSkills.Find(staffSkill.Id);
                /*查找员工技能预留字段(value)*/
                var fieldValueList = (from ssr in db.StaffSkillReserves
                                      join rf in db.ReserveFields on ssr.FieldId equals rf.Id
                                      where ssr.Number == staffSkill.Id
                                      select new StaffSkillView { Id = ssr.Id, Description = rf.Description,Value=ssr.Value }).ToList();
                //ViewBag.fieldValueList = fieldValueList;
                /*给预留字段赋值*/
                foreach (var temp in fieldValueList) {
                    StaffSkillReserve ssr = db.StaffSkillReserves.Find(temp.Id);
                    ssr.Value = Request[temp.Description];
                    db.SaveChanges();
                }

                ss.StaffNumber = staffSkill.StaffNumber;
                ss.StaffName = staffSkill.StaffName;
                ss.SkillRemark = staffSkill.SkillRemark;
                ss.SkillNumber = staffSkill.SkillNumber;
                ss.SkillName = staffSkill.SkillName;
                ss.SkillGrade = staffSkill.SkillGrade;
                ss.BillType = staffSkill.BillType;
                ss.BillNumber = staffSkill.BillNumber;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(staffSkill);
        }

        // GET: StaffSkill/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffSkill staffSkill = db.StaffSkills.Find(id);
            if (staffSkill == null)
            {
                return HttpNotFound();
            }
            var skillParameters = db.SkillParameters.Where(sp => sp.SkillNumber.Equals(staffSkill.SkillNumber));
            var staffs = db.Staffs.Where(s => s.StaffNumber.Equals(staffSkill.StaffNumber));
            foreach (var temp in skillParameters)
            {
                staffSkill.SkillName = temp.SkillName;
            }
            foreach (var temp in staffs)
            {
                staffSkill.StaffName = temp.Name;
            }
            /*显示预留字段以及预留字段的值*/
            /*查找员工技能预留字段(name)*/
            var fieldNameList = (from p in db.ReserveFields where p.TableName == "StaffSkills" select p).ToList();
            ViewBag.fieldNameList = fieldNameList;
            /*查找员工技能预留字段(value)*/
            var fieldValueList = (from ssr in db.StaffSkillReserves
                                  join rf in db.ReserveFields on ssr.FieldId equals rf.Id
                                  where ssr.Number == staffSkill.Id
                                  select new StaffSkillView { Id = ssr.Number, Description = rf.Description, Value = ssr.Value }).ToList();
            ViewBag.fieldValueList = fieldValueList;
            return View(staffSkill);
        }

        // POST: StaffSkill/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
   
            /*Step1：删除预留字段*/
            var item = (from ssr in db.StaffSkillReserves 
                        where ssr.Number == id 
                        select new StaffSkillView { Id = ssr.Id }).ToList();
            foreach (var temp in item){
                StaffSkillReserve ssr = db.StaffSkillReserves.Find(temp.Id);
                db.StaffSkillReserves.Remove(ssr);
            }
            db.SaveChanges();

            /*Step2：删除固定字段*/
            StaffSkill staffSkill = db.StaffSkills.Find(id);
            db.StaffSkills.Remove(staffSkill);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        
        public JsonResult SearchStaff(string staffNumber)
        {
            List<Object> obj = new List<Object>();
            var Staffs = from s in db.Staffs select s;
            if (!String.IsNullOrEmpty(staffNumber))
            {

                var tmp = db.Staffs.Where(s => s.StaffNumber.Equals(staffNumber));
                if (tmp.Count() != 0)
                {
                    Staffs = Staffs.Where(s => s.StaffNumber.Equals(staffNumber));
                    foreach (var temp in Staffs)
                    {



                        obj.Add(new { text = temp.Name });


                        return Json(obj);
                    }
                }
                else
                {
                    obj.Add(new { text = "" });
                    return Json(obj);
                }


            }
            return Json(obj);
        }
        public JsonResult SearchSkill(string skillNumber)
        {
            List<Object> obj = new List<Object>();
            var SkillParameters = from s in db.SkillParameters select s;
            if (!String.IsNullOrEmpty(skillNumber))
            {

                var tmp = db.SkillParameters.Where(s => s.SkillNumber.Equals(skillNumber));
                if (tmp.Count() != 0)
                {
                    SkillParameters = SkillParameters.Where(s => s.SkillNumber.Equals(skillNumber));
                    foreach (var temp in SkillParameters)
                    {



                        obj.Add(new { text = temp.SkillName });


                        return Json(obj);
                    }
                }
                else
                {
                    obj.Add(new { text = "" });
                    return Json(obj);
                }


            }
            return Json(obj);
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
