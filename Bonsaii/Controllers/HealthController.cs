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
    public class HealthController : BaseController
    {
        
        // GET: Health
        public ActionResult Index()
        {
            return View(db.Healths.ToList());
        }

        // GET: Health/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Health health = db.Healths.Find(id);
            if (health == null)
            {
                return HttpNotFound();
            }
            return View(health);
        }

        // GET: Health/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Health/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,HealthCondition")] Health health)
        {
            if (ModelState.IsValid)
            {
                db.Healths.Add(health);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(health);
        }

        // GET: Health/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Health health = db.Healths.Find(id);
            if (health == null)
            {
                return HttpNotFound();
            }
            return View(health);
        }

        // POST: Health/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,HealthCondition")] Health health)
        {
            if (ModelState.IsValid)
            {
                db.Entry(health).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(health);
        }

        // GET: Health/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Health health = db.Healths.Find(id);
            if (health == null)
            {
                return HttpNotFound();
            }
            return View(health);
        }

        // POST: Health/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Health health = db.Healths.Find(id);
            db.Healths.Remove(health);
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
