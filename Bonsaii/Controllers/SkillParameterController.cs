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
    public class SkillParameterController : BaseController
    {
     

        // GET: SkillParameter
        public ActionResult Index()
        {
            return View(db.SkillParameters.ToList());
        }

        // GET: SkillParameter/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SkillParameter skillParameter = db.SkillParameters.Find(id);
            if (skillParameter == null)
            {
                return HttpNotFound();
            }
            return View(skillParameter);
        }

        // GET: SkillParameter/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SkillParameter/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,SkillNumber,SkillName")] SkillParameter skillParameter)
        {
            if (ModelState.IsValid)
            {
                db.SkillParameters.Add(skillParameter);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(skillParameter);
        }

        // GET: SkillParameter/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SkillParameter skillParameter = db.SkillParameters.Find(id);
            if (skillParameter == null)
            {
                return HttpNotFound();
            }
            return View(skillParameter);
        }

        // POST: SkillParameter/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,SkillNumber,SkillName")] SkillParameter skillParameter)
        {
            if (ModelState.IsValid)
            {
                db.Entry(skillParameter).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(skillParameter);
        }

        // GET: SkillParameter/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SkillParameter skillParameter = db.SkillParameters.Find(id);
            if (skillParameter == null)
            {
                return HttpNotFound();
            }
            return View(skillParameter);
        }

        // POST: SkillParameter/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SkillParameter skillParameter = db.SkillParameters.Find(id);
            db.SkillParameters.Remove(skillParameter);
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
