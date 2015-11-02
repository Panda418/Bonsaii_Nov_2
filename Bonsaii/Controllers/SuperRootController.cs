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
using System.Collections;
using System.Data.SqlClient;
using System.Configuration;

namespace Bonsaii.Controllers
{
    public class SuperRootController : Controller
    {
        private SystemDbContext db = new SystemDbContext();

        // GET: SuperRoot
        public ActionResult Index()
        {
            var tmp = db.Users.Where(p => p.IsRoot == true);
            return View(db.Users.ToList());
        }

        // GET: SuperRoot/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserModels user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        public ActionResult Audit(string id)
        {
            /**
             * 生成企业的数据库名称
             * */
            string CompanyDbName = "Bonsaii" + id;
            var flag1 = CreateDbForCompany(CompanyDbName);
            var tmp = db.Users.Where(p => p.CompanyId.Equals(id)).Single();
            tmp.IsProved = true;
            db.Entry(tmp).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            var flag2 = CreateCompanyTables(tmp.ConnectionString);

            //创建该企业用户的私有数据表
            if (flag1 && flag2)
                return RedirectToAction("Index");
            else
                return View("Error");
        }

        public ActionResult Close(string id)
        {
            using (SystemDbContext mydb = new SystemDbContext()) {
                List<UserModels> tmp = mydb.Users.Where(p => p.CompanyId.Equals(id)).ToList();
                foreach(UserModels user in tmp)
                    user.IsProved = false;
                mydb.Entry(tmp).State = System.Data.Entity.EntityState.Modified;
                mydb.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        public ActionResult Error()
        {
            return View();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        /// <summary>
        ///  为注册的企业用户创建数据库，数据库名称为企业ID
        /// </summary>
        /// <param name="CompanyId">企业ID</param>
        /// <returns></returns>
        public bool CreateDbForCompany(string CompanyDbName)
        {
            String str;
            string ConnString = ConfigurationManager.AppSettings["MasterDbConnectionString"];
            SqlConnection myConn = new SqlConnection(ConnString);
            str = "CREATE DATABASE " + CompanyDbName;
            SqlCommand myCommand = new SqlCommand(str, myConn);
            try
            {
                myConn.Open();
                myCommand.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            {
                return false;
            }
            finally
            {
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }
            }
            return true;
        }

        /// <summary>
        /// 创建企业数据库所需要的所有数据表
        /// </summary>
        /// <param name="ConnectionString">连接字符串</param>
        /// <returns></returns>
        public bool CreateCompanyTables(string ConnectionString)
        {
            string SqlFilePath = ConfigurationManager.AppSettings["SQLFilePath"];
            string varFileName = HttpContext.Server.MapPath("~/App_Data/company.sql");
            if (!System.IO.File.Exists(varFileName))
            {
                return false;
            }

            StreamReader sr = System.IO.File.OpenText(varFileName);

            ArrayList alSql = new ArrayList();
            string commandText = "";
            string varLine = "";
            while (sr.Peek() > -1)
            {
                varLine = sr.ReadLine();
                if (varLine == "")
                {
                    continue;
                }
                if (varLine != "GO")
                {
                    commandText += varLine;
                    commandText += "\r\n";
                }
                else
                {
                    alSql.Add(commandText);
                    commandText = "";
                }
            }

            sr.Close();
            try
            {
                ExecuteCommand(alSql, ConnectionString);
            }
            catch
            {
                return false;
            }
            return true;
        }

        private static void ExecuteCommand(ArrayList varSqlList, string ConString)
        {
            SqlConnection MyConnection = new SqlConnection(ConString);
            MyConnection.Open();
            SqlTransaction varTrans = MyConnection.BeginTransaction();
            SqlCommand command = new SqlCommand();
            command.Connection = MyConnection;
            command.Transaction = varTrans;
            try
            {
                foreach (string varcommandText in varSqlList)
                {
                    command.CommandText = varcommandText;
                    command.ExecuteNonQuery();
                }
                varTrans.Commit();
            }
            catch (Exception ex)
            {
                varTrans.Rollback();
                throw ex;
            }
            finally
            {
                MyConnection.Close();
            }
        }




    }
}
