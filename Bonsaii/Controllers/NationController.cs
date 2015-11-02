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
    public class NationController : BaseController
    {
        //private BonsaiiDbContext db = new BonsaiiDbContext();

        // GET: Nation
        public ActionResult Index()
        {
            return View(db.Nations.ToList());
        }

        // GET: Nation/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nation nation = db.Nations.Find(id);
            if (nation == null)
            {
                return HttpNotFound();
            }
            return View(nation);
        }

        // GET: Nation/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Nation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nationality")] Nation nation)
        {
            if (ModelState.IsValid)
            {
                db.Nations.Add(nation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nation);
        }

        // GET: Nation/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nation nation = db.Nations.Find(id);
            if (nation == null)
            {
                return HttpNotFound();
            }
            return View(nation);
        }

        // POST: Nation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nationality")] Nation nation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nation);
        }

        // GET: Nation/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nation nation = db.Nations.Find(id);
            if (nation == null)
            {
                return HttpNotFound();
            }
            return View(nation);
        }

        // POST: Nation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Nation nation = db.Nations.Find(id);
            db.Nations.Remove(nation);
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
