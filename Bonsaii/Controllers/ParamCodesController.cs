﻿using System;
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
    public class ParamCodesController : BaseController
    {
        // GET: ParamCodes
        public ActionResult Index()
        {
            return View(db.ParamCodes.ToList());
        }

        // GET: ParamCodes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParamCodes paramCodes = db.ParamCodes.Find(id);
            if (paramCodes == null)
            {
                return HttpNotFound();
            }
            return View(paramCodes);
        }

        // GET: ParamCodes/Create
        public ActionResult Create()
        {
            List<Params> paramList = db.Params.ToList();
            List<CodeMethod> list = CodeMethod.GetCodeMethod();
            List<SelectListItem> item = list.Select(c => new SelectListItem
            {
                Value = c.Id,
                Text = c.Description
            }).ToList();

            ViewBag.List = item;

            List<SelectListItem> item2 = paramList.Select(c => new SelectListItem
            {
                Value = c.ParamName,
                Text = c.ParamName
            }).ToList();
            ViewBag.List2 = item2;
            return View();
        }

        // POST: ParamCodes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CodeMethod,Code,Year,Month,Day,SerialNumber,ParamName")] ParamCodes paramCodes)
        {
            if (ModelState.IsValid)
            {
                switch (paramCodes.CodeMethod)
                {
                    case CodeMethod.Two:
                        paramCodes.Year = 4;
                        paramCodes.Month = 2;
                        paramCodes.Day = 0;
                        paramCodes.SerialNumber = 4;
                        break;
                    case CodeMethod.Three:
                        paramCodes.Year = 0;
                        paramCodes.Month = 0;
                        paramCodes.Day = 0;
                        paramCodes.SerialNumber = 10;
                        break;
                    case CodeMethod.Four:
                        paramCodes.Year = 0;
                        paramCodes.Month = 0;
                        paramCodes.Day = 0;
                        paramCodes.SerialNumber = 0;
                        break;
                    default:
                        paramCodes.Year = 4;
                        paramCodes.Month = 2;
                        paramCodes.Day = 2;
                        paramCodes.SerialNumber = 2;
                        break;
                }

                db.ParamCodes.Add(paramCodes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(paramCodes);
        }

        // GET: ParamCodes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParamCodes paramCodes = db.ParamCodes.Find(id);
            List<Params> paramList = db.Params.ToList();
            List<CodeMethod> list = CodeMethod.GetCodeMethod();
            List<SelectListItem> item = list.Select(c => new SelectListItem
            {
                Value = c.Id,
                Text = c.Description
            }).ToList();
            foreach (SelectListItem tmp in item)
            {
                if (tmp.Text == paramCodes.ParamName)
                    tmp.Selected = true;
            }
            ViewBag.List = item;

            List<SelectListItem> item2 = paramList.Select(c => new SelectListItem
            {
                Value = c.ParamName,
                Text = c.ParamName
            }).ToList();
            foreach (SelectListItem tmp in item2)
            {
                if (tmp.Text == paramCodes.CodeMethod)
                    tmp.Selected = true;
            }
            ViewBag.List2 = item2;
            if (paramCodes == null)
            {
                return HttpNotFound();
            }
            return View(paramCodes);
        }

        // POST: ParamCodes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CodeMethod,Code,Year,Month,Day,SerialNumber,ParamName")] ParamCodes paramCodes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paramCodes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(paramCodes);
        }

        // GET: ParamCodes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParamCodes paramCodes = db.ParamCodes.Find(id);
            if (paramCodes == null)
            {
                return HttpNotFound();
            }
            return View(paramCodes);
        }

        // POST: ParamCodes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ParamCodes paramCodes = db.ParamCodes.Find(id);
            db.ParamCodes.Remove(paramCodes);
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
