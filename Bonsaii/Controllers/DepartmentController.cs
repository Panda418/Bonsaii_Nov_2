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
                     join x in db.Departments on d.ParentDepartmentId equals x.Number
                           into gc
                     from x in gc.DefaultIfEmpty()
                     select new DepartmentViewModel { 
                         Number = d.Number, Name = d.Name, Remark = d.Remark, ParentDepartmentName = x.Name, StaffSize = d.StaffSize 
                     };

            ViewBag.NumberSort = String.IsNullOrEmpty(sortOrder) ? "Number desc" : "";
            ViewBag.NameSort = String.IsNullOrEmpty(sortOrder);

            /*查找预留字段表，然后获取部门所有预留字段*/
            var recordList = (from p in db.ReserveFields where p.TableName == "Departments" select p).ToList();
            ViewBag.recordList = recordList;
            var pp = (from df in db.DepartmentReserves
                     join rf in db.ReserveFields on df.FieldId equals rf.Id
                      select new DepartmentViewModel { Description = rf.Description, Value = df.Value, Number = df.Number }).ToList();//Number=df.Number是为了传到前台页面，进行判断。
            ViewBag.List = pp;
         
            return View(q);           
        }

        // GET: Department/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //左联：查询上级部门的名称
            var q = from d in db.Departments
                    join x in db.Departments on d.ParentDepartmentId equals x.Number
                        into gc
                    from x in gc.DefaultIfEmpty()
                    where d.Number == id
                    select new { Name = x.Name };
            Department department = db.Departments.Find(id);
            DepartmentViewModel qq = new DepartmentViewModel();
            //DepartmentViewModel显示部门信息（部门表的固定字段）
            if (q != null)
            {
                foreach (var temp in q)
                {
                    qq.Number = department.Number;
                    qq.Name = department.Name;
                    qq.ParentDepartmentName = temp.Name;
                    qq.StaffSize = department.StaffSize;
                    qq.Remark = department.Remark;
                }
            }
            else
            {
                qq.Number = department.Number;
                qq.Name = department.Name;
                qq.ParentDepartmentName = null;
                qq.StaffSize = department.StaffSize;
                qq.Remark = department.Remark;
            }
            //DepartmentViewModel显示部门信息（部门表变化的字段）
            var p = (from df in db.DepartmentReserves
                     join rf in db.ReserveFields on df.FieldId equals rf.Id
                     where df.Number == id
                     select new DepartmentViewModel { Description = rf.Description, Value = df.Value }).ToList();
            ViewBag.List = p;

            if (qq == null)
            {
                return HttpNotFound();
            }
            return View(qq);
        }

        // GET: Department/Create
        public ActionResult Create()
        {
            //实现下拉列表
            List<SelectListItem> item = db.Departments.ToList().Select(c => new SelectListItem
            {
                Value = c.Number,//保存的值
                Text = c.Name//显示的值
            }).ToList();

            //增加一个null选项
            SelectListItem i = new SelectListItem();
            i.Value = "";
            i.Text = " ";
            i.Selected = true;
            item.Add(i);

            //传值到页面
            ViewBag.List = item;

            /*查找预留字段表，然后获取一个列表*/
            var recordList = (from p in db.ReserveFields where p.TableName == "Departments" select p).ToList();
            ViewBag.recordList = recordList;

            //Session["recordList"] = recordList;
            return View();
        }

        // POST: Department/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Department department)
        {
            //模型状态错误（为空）
            if (ModelState.IsValid)
            {
                /*选出ReserveFields中部门相关的记录*/
                var recordList = (from p in db.ReserveFields where p.TableName == "Departments" select p).ToList();
                ViewBag.recordList = recordList;
                /*生成部门编号*/
                department.Number = (new Random().Next(1111, 9999)).ToString();
                /*遍历*/
                foreach (var temp in recordList)
                {
                    DepartmentReserve dr = new DepartmentReserve();
                    dr.Number = department.Number;
                    dr.FieldId = temp.Id;
                    dr.Value = Request[temp.FieldName];
                    db.DepartmentReserves.Add(dr);
                    /*把这行去掉之后即可运行*/
                    //db.SaveChanges();
                }
                db.Departments.Add(department);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(department);
        }

        // GET: Department/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);

            //实现下拉列表
            var item = db.Departments.ToList().Select(c => new SelectListItem
            {
                Value = c.Number,//保存的值
                Text = c.Name//显示的值
            }).ToList();

            //增加一个null选项
            SelectListItem i = new SelectListItem();
            i.Value = "";
            i.Text = " ";
            i.Selected = true;
            item.Add(i);

            //传值到页面
            ViewBag.List = item;
         
            //DepartmentViewModel显示部门信息（部门表变化的字段）
            var pp = (from df in db.DepartmentReserves
                     join rf in db.ReserveFields on df.FieldId equals rf.Id
                     where df.Number == id
                      select new DepartmentViewModel { Description = rf.Description, Value = df.Value }).ToList();
            ViewBag.ValueList = pp;

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
        public ActionResult Edit(Department department)
        {
            //如果公司的上级部门编号ParentDepartmentId为空，将它置为null
            if (department.ParentDepartmentId == "") department.ParentDepartmentId = null;

            //模型状态错误（为空）
            if (ModelState.IsValid)
            {
                Department d = db.Departments.Find(department.Number);
                if (d != null)
                {
                    // 得到部门department.Number对应的所有动态变化的字段
                    var pp = (from df in db.DepartmentReserves
                              join rf in db.ReserveFields on df.FieldId equals rf.Id
                             where df.Number == department.Number
                              select new DepartmentViewModel { Id = df.Id, Description = rf.Description, Value = df.Value }).ToList();
                    //对每个动态变化的字段进行赋值
                    foreach (var temp in pp)
                    {
                        DepartmentReserve dr = db.DepartmentReserves.Find(temp.Id);                    
                        dr.Value=Request[temp.Description];
                        db.SaveChanges();
                    }
                    

                    d.Name = department.Name;
                    d.ParentDepartmentId = department.ParentDepartmentId;
                    d.Remark = department.Remark;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                //自带的ValidationSummary提示
                ModelState.AddModelError("", "修改失败");
            }
            return View(department);
        }

        // GET: Department/Delete/5
        public ActionResult Delete(string id)
        {
            // db = new BonsaiiDbContext(base.ConnectionString);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //左联：查询上级部门的名称
            var q = from d in db.Departments
                    join x in db.Departments on d.ParentDepartmentId equals x.Number
                        into gc
                    from x in gc.DefaultIfEmpty()
                    where d.Number == id
                    select new { Name = x.Name };
            Department department = db.Departments.Find(id);
            DepartmentViewModel qq = new DepartmentViewModel();
            //DepartmentViewModel显示部门信息（部门表的固定字段）
            if (q != null)
            {
                foreach (var temp in q)
                {
                    qq.Number = department.Number;
                    qq.Name = department.Name;
                    qq.ParentDepartmentName = temp.Name;
                    qq.StaffSize = department.StaffSize;
                    qq.Remark = department.Remark;
                }
            }
            else
            {
                qq.Number = department.Number;
                qq.Name = department.Name;
                qq.ParentDepartmentName = null;
                qq.StaffSize = department.StaffSize;
                qq.Remark = department.Remark;
            }
            //DepartmentViewModel显示部门信息（部门表变化的字段）
            var p = (from df in db.DepartmentReserves
                     join rf in db.ReserveFields on df.FieldId equals rf.Id
                     where df.Number == id
                     select new DepartmentViewModel { Description = rf.Description, Value = df.Value }).ToList();
            ViewBag.List = p;
            if (department == null)
            {
                return HttpNotFound();
            }
            if (qq == null)
            {
                return HttpNotFound();
            }
            return View(qq);
        }

        // POST: Department/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            // 由于主外键关系，Departments是主表，DepartmentReserves是引用Departments表的信息。
            //只有先删除对应DepartmentReserve的动态变化的字段的信息
            var item = (from dr in db.DepartmentReserves
                        where dr.Number == id
                        select new DepartmentViewModel { Id=dr.Id}).ToList();
            foreach(var temp in item)
            { 
            DepartmentReserve drs = db.DepartmentReserves.Find(temp.Id);
            db.DepartmentReserves.Remove(drs);
            }
            db.SaveChanges();
            //删除Departments表对应的信息
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
