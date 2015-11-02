using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bonsaii.Models;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
namespace Bonsaii.Controllers
{
    public class UserController : BaseController
    {
        private SystemDbContext SysContext = new SystemDbContext();

        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Create()
        {
            return View(new UserViewModels());
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }
        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserViewModels model)
        {
            var CurUser = UserManager.FindByName(base.UserName);
      //      string[] roles = Request["userRoles"].Split(new Char[] { ',' });      //获得该用户对应的角色集合

            var user = new ApplicationUser
            {
                CompanyFullName = CurUser.CompanyFullName,
                CompanyId = CurUser.CompanyId,
                ConnectionString = CurUser.ConnectionString,
                PhoneNumber = model.UserName,
                UserName = model.UserName,
                IsProved = CurUser.IsProved,           //是否审核的标志
                IsAvailable = true,         //是否是可用的管理员
                IsRoot = false               //非企业号的注册人默认就是非企业的超级管理员
            };

            string Password = model.UserName.Substring(5, 6);
            var result = await UserManager.CreateAsync(user, Password);


            //if (result.Succeeded)
            //{
            //    var editUser = UserManager.FindByName(user.UserName);           //根据用户提交的信息获取用户信息
            //    //遍历用户选择的所有角色，将用于添加到每一个角色当中
            //    for (int i = 0; i < roles.Length - 1; i++)
            //        UserManager.AddToRole(editUser.Id, roles[i]);

            //    return RedirectToAction("Index");
            //}
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "User");
                }
                AddErrors(result);
            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View(model);
        }

        // GET: Users
        public ActionResult Index()
        {
            var users = SysContext.Users.Where(p => p.CompanyId == CompanyId);
            return View(users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserModels user = SysContext.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }


        // GET: Users/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserModels user = SysContext.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,Discriminator,CompanyId,CompanyFullName,ConnectionString")] UserModels user)
        {
            if (ModelState.IsValid)
            {
                SysContext.Entry(user).State = EntityState.Modified;
                SysContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserModels user = SysContext.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            UserModels user = SysContext.Users.Find(id);
            //获取用户的角色信息
            string [] q = (from d in SysContext.Roles
                              join x in SysContext.UserRoles on d.Id equals x.RoleId into temp
                              from tt in temp.DefaultIfEmpty()
                              where tt.UserId == id
                              select d.Name).ToArray();
            //删除用户的角色
            //删除用户原来属于的角色
            for (int i = 0; i < q.Length; i++)
            {
                UserManager.RemoveFromRole(user.Id, q[i]);
            }
            SysContext.Users.Remove(user);
            SysContext.SaveChanges();
            return RedirectToAction("Index");
        }

        //Get
        public ActionResult Auth(string id)
        {
            List<string>  q = (from d in SysContext.Roles
                    join x in SysContext.UserRoles on d.Id equals x.RoleId into temp
                    from tt in temp.DefaultIfEmpty()
                    where tt.UserId == id
                    select d.Name).ToList();

            /** 查询SQL
                    select Roles.Name
                    from Roles left join UserRoles
                    on Roles.Id = UserRoles.RoleId
                    where UserRoles.UserId = '8e3dcc4d-17b8-4f89-8821-fed9969b221c';
             **/

            var allRoles = SysContext.Roles.ToList();
            ViewBag.Roles = q;
            ViewBag.UserId = id;
            return View(allRoles) ;
        }
        [HttpPost]
        public ActionResult Auth(FormCollection collection)
        {
            string id = collection["UserId"];
            //获取用户原来属于的角色
            string[] preRoles = (from d in SysContext.Roles
                                 join x in SysContext.UserRoles on d.Id equals x.RoleId into temp
                                 from tt in temp.DefaultIfEmpty()
                                 where tt.UserId == id
                                 select d.Name).ToArray();
            //获取用户新勾选的角色
            string[] newRoles = collection["SelectedRoles"].Split(new char[] { ',' });
            //再给某一个用户赋予新的角色之前，要把他之前所属的角色全部删除掉


            //删除用户原来属于的角色
            for (int i = 0; i < preRoles.Length; i++)
            {
                UserManager.RemoveFromRole(id, preRoles[i]);
            }
            if (newRoles[0] != "")
                //将用户添加到新选择的角色当中
                UserManager.AddToRoles(id, newRoles);
            return RedirectToAction("Index");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                SysContext.Dispose();
            }
            base.Dispose(disposing);
        }


        /**
 * 管理员信息管理函数
 * */

        // GET: Users/Create
        public ActionResult CreateAdmin()
        {

            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAdmin(UserViewModels model)
        {
         //   var CurUser = UserManager.FindByName(base.UserName);
       //     string[] roles = Request["userRoles"].Split(new Char[] { ',' });      //获得该用户对应的角色集合

            var user = new ApplicationUser
            {
                CompanyFullName = base.CompanyFullName,
                CompanyId = base.CompanyId,
                ConnectionString = base.ConnectionString,
                PhoneNumber = model.UserName,
                UserName = model.UserName,
                IsProved = base.IsProved,           //是否审核的标志
                IsAvailable = true,         //是否是可用的管理员
                IsRoot = false               //非企业号的注册人默认就是非企业的超级管理员
            };
            var result = await UserManager.CreateAsync(user, model.UserName.Substring(5,6));


            if (result.Succeeded)
            {
                //var editUser = UserManager.FindByName(user.UserName);           //根据用户提交的信息获取用户信息
                ////遍历用户选择的所有角色，将用于添加到每一个角色当中
                //for (int i = 0; i < roles.Length - 1; i++)
                //    UserManager.AddToRole(editUser.Id, roles[i]);

                return RedirectToAction("Index");
            }
            AddErrors(result);
            return View(model);
        }


        public ActionResult SwitchIsAvailable(string id)
        {
            using (SystemDbContext con = new SystemDbContext())
            {
                UserModels user = con.Users.Find(id);
                user.IsAvailable = user.IsAvailable ? false : true;
                con.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult Test()
        {
            return View();
        }

    }
}
