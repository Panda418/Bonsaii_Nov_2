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
    public class DepartmentController : BaseController
    {

        // GET: Department
        public ActionResult Index(string sortOrder)
        {
            /*左联：显示所有部门表的字段*/
            var q = from d in db.Departments
                    join x in db.Departments on d.ParentDepartmentId equals x.DepartmentId
                          into gc
                    from x in gc.DefaultIfEmpty()
                    select new DepartmentViewModel
                    {
                        Id = d.Id,
                        DepartmentId = d.DepartmentId,
                        Name = d.Name,
                        Remark = d.Remark,
                        ParentDepartmentName = x.Name,
                        StaffSize = d.StaffSize

                    };
            /*排序*/
            ViewBag.NumberSort = String.IsNullOrEmpty(sortOrder) ? "Number desc" : "";
            ViewBag.NameSort = String.IsNullOrEmpty(sortOrder);
            /*查找预留字段表，然后获取部门所有预留字段*/
            var recordList = (from p in db.ReserveFields where p.TableName == "Departments" select p).ToList();
            ViewBag.recordList = recordList;
            var pp = (from df in db.DepartmentReserves
                      join rf in db.ReserveFields on df.FieldId equals rf.Id
                      select new DepartmentViewModel { 
                           Id=df.Number, 
                          Description = rf.Description, 
                          Value = df.Value
                      }).ToList();//Number=df.Number是为了传到前台页面，进行判断。
            ViewBag.List = pp;

            return View(q);           
        }

        // GET: Department/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // GET: Department/Create
        public ActionResult Create()
        {
            //实现下拉列表
            List<SelectListItem> item = db.Departments.ToList().Select(c => new SelectListItem
            {
                Value = c.DepartmentId,//保存的值
                Text = c.Name//显示的值
            }).ToList();

            //增加一个null选项
            SelectListItem i = new SelectListItem();
            i.Value = "";
            i.Text = "-请选择-";
            i.Selected = true;
            item.Add(i);

            //传值到页面
            ViewBag.List = item;

            /*查找预留字段表，然后获取一个列表*/
            var recordList = (from p in db.ReserveFields where p.TableName == "Departments" select p).ToList();
            ViewBag.recordList = recordList;

            return View();
        }

        // POST: Department/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Department department)
        {
            if (ModelState.IsValid)
            {
                /*先保存部门固定的字段（为了生成主键Id）*/
                db.Departments.Add(department);
                db.SaveChanges();
                /*选出ReserveFields中部门相关的记录*/
                var recordList = (from p in db.ReserveFields where p.TableName == "Departments" select p).ToList();
                ViewBag.recordList = recordList;
                /*生成部门编号*/
                // 
                /*遍历*/
                foreach (var temp in recordList)
                {
                    DepartmentReserve dr = new DepartmentReserve();
                    dr.Number = department.Id;
                    dr.FieldId = temp.Id;
                    dr.Value = Request[temp.FieldName];
                    /*占位，为了在Index中显示整齐的格式*/
                    if (dr.Value == null) dr.Value = " ";
                    db.DepartmentReserves.Add(dr);
                    db.SaveChanges();
                }
               
                return RedirectToAction("Index");
            }

            return View(department);
        }

        // GET: Department/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // POST: Department/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Number,DepartmentId,Name,ParentDepartmentId,StaffSize,Remark")] Department department)
        {
            if (ModelState.IsValid)
            {
                db.Entry(department).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(department);
        }

        // GET: Department/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // POST: Department/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Department department = db.Departments.Find(id);
            db.Departments.Remove(department);
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
