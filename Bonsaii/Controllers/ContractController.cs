using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bonsaii.Models;
using System.IO;

namespace Bonsaii.Controllers
{
    /// <summary>
    /// 判断文件是否为空
    /// </summary>
    public static class HasFiles
    {
        public static bool HasFile(this HttpPostedFileBase file)
        {
            return (file != null && file.ContentLength > 0) ? true : false;
        }
    }
    public class ContractController : BaseController
    {
        //private BonsaiiDbContext db = new BonsaiiDbContext();

        // GET: Contract
        public ActionResult Index()
        {
            return View(db.Contracts.ToList());
        }
        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        //public FileContentResult GetImage(int Id)
        //{
        //    //var fileinfo = db.Contracts.Find(Id);
        //    //return File(fileinfo.FileUrl, fileinfo.MimeType, fileinfo.FileName);
        //    Contract com = db.Contracts.FirstOrDefault(p => p.Id == Id);
        //    if (com != null)
        //    {
        //        return File(com.ContractAttachment, com.ContractAttachmentType);//File方法直接将二进制转化为指定类型了。
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        // GET: Contract/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contract contract = db.Contracts.Find(id);
            if (contract == null)
            {
                return HttpNotFound();
            }
            return View(contract);
        }

        // GET: Contract/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Contract/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,BillTypeNumber,BillTypeName,BillNumber,ContractNumber,FirstParty,SecondParty,SignDate,DueDate,Amount,ContractObject,ContractAttachmentType,ContractAttachmentName,ContractAttachmentUrl,Remark")] Contract contract, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
               
                //{if (file != null)
                //    contract.ContractAttachmentType = file.ContentType;//获取图片类型
                //   contract.ContractAttachment = new byte[file.ContentLength];//新建一个长度等于图片大小的二进制地址
                //   file.InputStream.Read(contract.ContractAttachment, 0, file.ContentLength);//将image读取到Logo中
                //}
               
                if (!HasFiles.HasFile(file))
                {
                    ModelState.AddModelError("", "文件不能为空！");
                    return View(contract);
                }

                string miniType = file.ContentType;
                Stream fileStream =file.InputStream;
                string path = AppDomain.CurrentDomain.BaseDirectory + "files\\";
                string filename = Path.GetFileName(file.FileName);
                 file.SaveAs(Path.Combine(path, filename));

               
                   contract.ContractAttachmentType = miniType;
                    contract.ContractAttachmentName = filename;
                    contract.ContractAttachmentUrl = Path.Combine(path, filename);
                
                db.Contracts.Add(contract);//存储到数据库
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(contract);
        }
        public FilePathResult Download(int id)
        {
            var fileinfo = db.Contracts.Find(id);
            return File(fileinfo.ContractAttachmentUrl, fileinfo.ContractAttachmentType, fileinfo.ContractAttachmentName);
        }
        // GET: Contract/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contract contract = db.Contracts.Find(id);
            if (contract == null)
            {
                return HttpNotFound();
            }
            return View(contract);
        }

        // POST: Contract/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BillTypeNumber,BillTypeName,BillNumber,ContractNumber,FirstParty,SecondParty,SignDate,DueDate,Amount,ContractObject,ContractAttachmentType,ContractAttachmentName,ContractAttachmentUrl,Remark")] Contract contract,HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string miniType = file.ContentType;
                    Stream fileStream = file.InputStream;
                    string path = AppDomain.CurrentDomain.BaseDirectory + "files\\";
                    string filename = Path.GetFileName(file.FileName);
                    file.SaveAs(Path.Combine(path, filename));


                    contract.ContractAttachmentType = miniType;
                    contract.ContractAttachmentName = filename;
                    contract.ContractAttachmentUrl = Path.Combine(path, filename);
                    db.Entry(contract).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return View(contract);
        }

        // GET: Contract/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contract contract = db.Contracts.Find(id);
            if (contract == null)
            {
                return HttpNotFound();
            }
            return View(contract);
        }

        // POST: Contract/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Contract contract = db.Contracts.Find(id);
            db.Contracts.Remove(contract);
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
