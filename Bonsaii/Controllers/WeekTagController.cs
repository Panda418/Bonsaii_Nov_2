using Bonsaii.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bonsaii.Controllers
{
    public class WeekTagController : BaseController
    {
       // private BonsaiiDbContext db = new BonsaiiDbContext();
        // GET: WeekTag
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nian,Range,Week1,Week2,Week3,week4,Week5,Week6,Week7")] WeekTag weektag)
        {

            string date = weektag.Nian;
            DateTime begindate = Convert.ToDateTime(date + "-01-01");
            //DateTime begindate = Convert.ToDateTime(date+"01-01");
            //DateTime enddate = Convert.ToDateTime(date+"12-31");
            //System.TimeSpan tsdiffer = enddate.Date - begindate.Date;
            // int intdiffer = tsdiffer.Days + 1;
            int intdiffer;
            if (DateTime.IsLeapYear(int.Parse(weektag.Nian)))
            {
                intdiffer = 366;
            }
            else
                intdiffer = 365;
            
            List<DateTime> list = new List<DateTime>();
            for (int i = 0; i < intdiffer; i++)
            {
                DateTime dttemp = begindate.Date.AddDays(i);

                if ((dttemp.DayOfWeek == System.DayOfWeek.Monday && "1" == weektag.Week1) ||
                    (dttemp.DayOfWeek == System.DayOfWeek.Tuesday && "1" == weektag.Week2) ||
                    (dttemp.DayOfWeek == System.DayOfWeek.Wednesday && "1" == weektag.Week3) ||
                    (dttemp.DayOfWeek == System.DayOfWeek.Thursday && "1" == weektag.Week4) ||
                    (dttemp.DayOfWeek == System.DayOfWeek.Friday && "1" == weektag.Week5) ||
                    (dttemp.DayOfWeek == System.DayOfWeek.Saturday && "1" == weektag.Week6) ||
                    (dttemp.DayOfWeek == System.DayOfWeek.Sunday && "1" == weektag.Week7))
                {
                    RecordDatetime temp = new RecordDatetime();
                    temp.Recordtime = dttemp;
                    temp.Tag = "1";
                    temp.Month = dttemp.Month;
                    temp.Day = dttemp.Day;
                    db.RecordDatetimes.Add(temp);
                    db.SaveChanges();
                }
                else
                {
                    RecordDatetime temp = new RecordDatetime();
                    temp.Recordtime = dttemp;
                    temp.Tag = "0";
                    temp.Month = dttemp.Month;
                    temp.Day = dttemp.Day;
                    db.RecordDatetimes.Add(temp);
                    db.SaveChanges();
                }

            }
            db.WeekTags.Add(weektag);
            db.SaveChanges();
           
            return RedirectToAction("Index");

        }
        public JsonResult Result()
        {
            List<Object> obj = new List<Object>();
            var temp = db.WeekTags.ToList();
            foreach (var t in temp)
            {
                obj.Add(new { nian = t.Nian, week1 = t.Week1, week2 = t.Week2, week3 = t.Week3, week4 = t.Week4, week5 = t.Week5, week6 = t.Week6, week7 = t.Week7 });
            }
            return Json(obj);
        }
    }
}