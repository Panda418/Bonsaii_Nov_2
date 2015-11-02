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
    public class CompanyController : BaseController
    {
      
        private SystemDbContext db = new SystemDbContext(); 
        // GET: Company/Details/5
        public ActionResult Details(string id)
        {

            id = this.CompanyId; //获取当前企业的企业ID
            Company company = db.Companies.Find(id);//在系统数据库的Companies表中找到该企业对应的行。
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           // Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public FileContentResult GetImage(string CompanyId)
        {
            Company com = db.Companies.FirstOrDefault(p => p.CompanyId == CompanyId);
            if (com != null)
            {
                return File(com.Logo, com.LogoType);//File方法直接将二进制转化为指定类型了。
            }
            else
            {
                return null;
            }
        }
        // GET: Company/Edit/5
        public ActionResult Edit(string id)
        {
            id = this.CompanyId;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Company/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CompanyId,FullName,TelNumber,BusinessLicense,UserName,ParentCompany,ParentCompanyId,ShortName,EnglishName,LegalRepresentative,EstablishDate,Email,Address,Url,Logo,LogoType,Remark,IsGroupCompany,GroupCompanyNumber")] Company company, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
               
                if (image != null)
                {
                    company.LogoType = image.ContentType;//获取图片类型
                    company.Logo = new byte[image.ContentLength];//新建一个长度等于图片大小的二进制地址
                    image.InputStream.Read(company.Logo, 0, image.ContentLength);//将image读取到Logo中
                    
                }
                if (company.IsGroupCompany == false)
                {
                    company.GroupCompanyNumber = null;
                }

                db.Entry(company).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details");
            }
            return View(company);
        }

        // GET: Company/Delete/5
        //public ActionResult Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Company company = db.Companies.Find(id);
        //    if (company == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(company);
        //}

        // POST: Company/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Company company = db.Companies.Find(id);
            db.Companies.Remove(company);
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
