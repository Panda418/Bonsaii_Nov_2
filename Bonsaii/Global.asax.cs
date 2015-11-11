using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Timers;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Bonsaii
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);


            //Timer t = new Timer(6000);
            //t.Elapsed += new ElapsedEventHandler(t_Elapsed);
            //t.AutoReset = true;
            //t.Enabled = true;
        }

        //private void t_Elapsed(Object sender, EventArgs e)
        //{
        //    string body = "<h1>Hello,I am an email from the ASP.NET MVC5 program</h1>";
        //    MailMessage message = new MailMessage();
        //    message.To.Add(new MailAddress("kevinjoker@163.com"));
        //    message.From = new MailAddress("1271808441@qq.com");
        //    message.Subject = "ASP.NET MVC SEND EMAIL";
        //    message.Body = body;
        //    message.IsBodyHtml = true;

        //    using (SmtpClient smtp = new SmtpClient())
        //    {
        //        var credential = new NetworkCredential()
        //        {
        //            UserName = "1271808441@qq.com",
        //            Password = "smtpmima123"
        //        };
        //        smtp.Credentials = credential;
        //        smtp.Host = "smtp.qq.com";
        //        smtp.Port = 25;
        //        smtp.EnableSsl = false;
        //        smtp.Send(message);
        //    }
        //}
    }
}
