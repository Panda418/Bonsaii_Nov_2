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
<<<<<<< HEAD
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
=======

            //左联：查询上级部门的名称
            var q = from d in db.Departments
                    join x in db.Departments on d.ParentDepartmentId equals x.DepartmentId
                        into gc
                    from x in gc.DefaultIfEmpty()
                    where d.Id == id
                    select new { Name = x.Name };
            Department department = db.Departments.Find(id);
            DepartmentViewModel qq = new DepartmentViewModel();

            /*Step1：部门表的固定字段*/
            if (q != null)
            {
                foreach (var temp in q)
                {
                    qq.DepartmentId = department.DepartmentId;
                    qq.Name = department.Name;
                    qq.ParentDepartmentName = temp.Name;
                    qq.StaffSize = department.StaffSize;
                    qq.Remark = department.Remark;
                }
            }
            else
            {
                qq.DepartmentId = department.DepartmentId;
                qq.Name = department.Name;
                qq.ParentDepartmentName = null;
                qq.StaffSize = department.StaffSize;
                qq.Remark = department.Remark;
            }
            /*Step2：查找部门表预留字段*/
            var p = (from df in db.DepartmentReserves
                     join rf in db.ReserveFields on df.FieldId equals rf.Id
                     where df.Number == id
                     select new DepartmentViewModel {Id= df.Number,Description = rf.Description, Value = df.Value }).ToList();
            ViewBag.List = p;

            if (qq == null)
            {
                return HttpNotFound();
            }
            return View(qq);

>>>>>>> 7f6daae59d3f52aeb49e3b88babd3194b0c3112d
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
<<<<<<< HEAD
                /*先保存部门固定的字段（为了生成主键Id）*/
                db.Departments.Add(department);
                db.SaveChanges();
                /*选出ReserveFields中部门相关的记录*/
                var recordList = (from p in db.ReserveFields where p.TableName == "Departments" select p).ToList();
                ViewBag.recordList = recordList;
                /*生成部门编号*/
                // 
                /*遍历*/
=======
                /*Step1：如果上级部门为空则上级部门编号为公司Id*/
                if (department.ParentDepartmentId == null) { department.ParentDepartmentId = this.CompanyId; }

                /*Step2：部门编号唯一,应该用Ajax？？*/
                //var find = (from p in db.Departments where p.DepartmentId == department.DepartmentId select p).FirstOrDefault();
                //if (find != null) { ModelState.AddModelError("","部门编号已存在！" );} 

                /*Step3：保存固定字段（为了生成主键Id）*/
                db.Departments.Add(department);
                db.SaveChanges();

                /*Step4：显示预留字段名称*/
                var recordList = (from p in db.ReserveFields where p.TableName == "Departments" select p).ToList();
                ViewBag.recordList = recordList;
              
                
                /*Step5：保存预留字段的值*/
>>>>>>> 7f6daae59d3f52aeb49e3b88babd3194b0c3112d
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
<<<<<<< HEAD
=======

            //实现下拉列表
            var item = db.Departments.ToList().Select(c => new SelectListItem
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

            //DepartmentViewModel显示部门信息（部门表变化的字段）
            var pp = (from df in db.DepartmentReserves
                      join rf in db.ReserveFields on df.FieldId equals rf.Id
                      where df.Number == id
                      select new DepartmentViewModel { Description = rf.Description, Value = df.Value }).ToList();
            ViewBag.ValueList = pp;

>>>>>>> 7f6daae59d3f52aeb49e3b88babd3194b0c3112d
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
<<<<<<< HEAD
            if (ModelState.IsValid)
            {
                db.Entry(department).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
=======
            //如果公司的上级部门编号ParentDepartmentId为空，将它置为null
            if (department.ParentDepartmentId == "") department.ParentDepartmentId = this.CompanyId;

            //模型状态错误（为空）
            if (ModelState.IsValid)
            {
                Department d = db.Departments.Find(department.Id);
                if (d != null)
                {
                    // 得到部门department.Number对应的所有动态变化的字段
                    var pp = (from df in db.DepartmentReserves
                              join rf in db.ReserveFields on df.FieldId equals rf.Id
                              where df.Number == department.Id
                              select new DepartmentViewModel { Id = df.Id, Description = rf.Description, Value = df.Value }).ToList();
                    //对每个动态变化的字段进行赋值
                    foreach (var temp in pp)
                    {
                        DepartmentReserve dr = db.DepartmentReserves.Find(temp.Id);
                        dr.Value = Request[temp.Description];
                        db.SaveChanges();
                    }


                    d.Name = department.Name;
                    d.DepartmentId = department.DepartmentId;
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
>>>>>>> 7f6daae59d3f52aeb49e3b88babd3194b0c3112d
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
<<<<<<< HEAD
            Department department = db.Departments.Find(id);
=======
            //左联：查询上级部门的名称
            var q = from d in db.Departments
                    join x in db.Departments on d.ParentDepartmentId equals x.DepartmentId
                        into gc
                    from x in gc.DefaultIfEmpty()
                    where d.Id == id
                    select new { Name = x.Name };
            Department department = db.Departments.Find(id);
            DepartmentViewModel qq = new DepartmentViewModel();
            //DepartmentViewModel显示部门信息（部门表的固定字段）
            if (q != null)
            {
                foreach (var temp in q)
                {
                    qq.DepartmentId = department.DepartmentId;
                    qq.Name = department.Name;
                    qq.ParentDepartmentName = temp.Name;
                    qq.StaffSize = department.StaffSize;
                    qq.Remark = department.Remark;
                }
            }
            else
            {
                qq.DepartmentId = department.DepartmentId;
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
>>>>>>> 7f6daae59d3f52aeb49e3b88babd3194b0c3112d
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
<<<<<<< HEAD
            Department department = db.Departments.Find(id);
            db.Departments.Remove(department);
            db.SaveChanges();
=======
            /*Step1：删除预留字段*/
            // 由于主外键关系，Departments是主表，DepartmentReserves是引用Departments表的信息。
            //只有先删除对应DepartmentReserve的动态变化的字段的信息
            var item = (from dr in db.DepartmentReserves
                        where dr.Number == id
                        select new DepartmentViewModel { Id = dr.Id }).ToList();
            foreach (var temp in item)
            {
                DepartmentReserve drs = db.DepartmentReserves.Find(temp.Id);
                db.DepartmentReserves.Remove(drs);
            }
            db.SaveChanges();

            /*Step2：删除固定字段*/
            //删除Departments表对应的信息
            Department department = db.Departments.Find(id);

            db.Departments.Remove(department);
            db.SaveChanges();

>>>>>>> 7f6daae59d3f52aeb49e3b88babd3194b0c3112d
            return RedirectToAction("Index");
        }

        /*判断部门编号是否唯一*/
        [HttpPost]
        public JsonResult DepartIdTest(string departmentId)
        {
            if (!String.IsNullOrEmpty(departmentId))
            {
                var find = (from p in db.Departments where p.DepartmentId == departmentId select p).ToList();
                if (find.Count != 0)
                {
                    return Json(new { result = true, });
                }
                else { return Json(new { result = false, }); }
            }
            else
                return null;
           
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
