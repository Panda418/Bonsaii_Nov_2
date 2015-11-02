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
    public class NationalityController : BaseController
    {
        //private BonsaiiDbContext db = new BonsaiiDbContext();

        // GET: Nationality
        public ActionResult Index()
        {
            return View(db.Nationalities.ToList());
        }

        // GET: Nationality/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nationality nationality = db.Nationalities.Find(id);
            if (nationality == null)
            {
                return HttpNotFound();
            }
            return View(nationality);
        }

        // GET: Nationality/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Nationality/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Sort,Nation")] Nationality nationality)
        {
            if (ModelState.IsValid)
            {
                var tmp = db.Nationalities.Where(p => p.Sort.Equals(nationality.Sort)).ToList();
                var tmp1 = db.Nationalities.Where(p => p.Nation.Equals(nationality.Nation)).ToList();
                if(tmp.Count()!=0)
                {
                    ModelState.AddModelError("", "抱歉，该序号已经存在！");
                    return View(nationality);
                }
                if (tmp1.Count() != 0)
                {
                    ModelState.AddModelError("", "抱歉，该国家已经存在！");
                    return View(nationality);
                }
                db.Nationalities.Add(nationality);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nationality);
        }

        // GET: Nationality/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nationality nationality = db.Nationalities.Find(id);
            if (nationality == null)
            {
                return HttpNotFound();
            }
            return View(nationality);
        }

        // POST: Nationality/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Sort,Nation")] Nationality nationality)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nationality).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nationality);
        }

        // GET: Nationality/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nationality nationality = db.Nationalities.Find(id);
            if (nationality == null)
            {
                return HttpNotFound();
            }
            return View(nationality);
        }

        // POST: Nationality/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Nationality nationality = db.Nationalities.Find(id);
            db.Nationalities.Remove(nationality);
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
