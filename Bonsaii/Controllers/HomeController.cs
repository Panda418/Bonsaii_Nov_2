using Bonsaii.Models;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;

namespace Bonsaii.Controllers
{
    public class HomeController : Controller
    {
        //private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles="abc")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult CreateExcel()
        {
            HSSFWorkbook wk = new HSSFWorkbook();
            ISheet tb = wk.CreateSheet("MyFirst");
            //创建一行，此行为第二行:创建行是从0开始
            IRow row = tb.CreateRow(1);
            ICell cell;
            for (int i = 0; i < 20; i++)
            {
                cell = row.CreateCell(i);  //在第二行中创建单元格
                cell.SetCellValue("This is a test");//循环往第二行的单元格中添加数据
            }
//            ISheet tb = wk.CreateSheet("MyFirst");
//            HSSFPatriarch patr = sheet.CreateDrawingPatriarch();
//            HSSFComment comment1 = patr.CreateComment(new HSSFClientAnchor(0, 0, 0, 0, 1, 2, 4, 4));
//            comment1.String = new HSSFRichTextString("Hello World");
//            comment1.Author = "NPOI Team"
//;
//            cell.CellComment = comment1;

            FileStream fs = new FileStream(@"f:/test.xls", FileMode.Create);
            wk.Write(fs);
            fs.Close();
            return View();
        }

        //读取xls文件
        private ActionResult ReadExcel()
        {
            StringBuilder sbr = new StringBuilder();
            FileStream fs = new FileStream(@"f:/test.xls",FileMode.Open);//打开myxls.xls文件
            
                HSSFWorkbook wk = new HSSFWorkbook(fs);   //把xls文件中的数据写入wk中
                for (int i = 0; i < wk.NumberOfSheets; i++)  //NumberOfSheets是myxls.xls中总共的表数
                {
                    ISheet sheet = wk.GetSheetAt(i);   //读取当前表数据
                    for (int j = 0; j <= sheet.LastRowNum; j++)  //LastRowNum 是当前表的总行数
                    {
                        IRow row = sheet.GetRow(j);  //读取当前行数据
                        if (row != null)
                        {
                            sbr.Append("-------------------------------------\r\n"); //读取行与行之间的提示界限
                            for (int k = 0; k <= row.LastCellNum; k++)  //LastCellNum 是当前行的总列数
                            {
                                ICell cell = row.GetCell(k);  //当前表格
                                if (cell != null)
                                {
                                    sbr.Append(cell.ToString());   //获取表格中的数据并转换为字符串类型
                                }
                            }
                        }
                    }
                }
            
            sbr.ToString();
            StreamWriter wr = new StreamWriter(new FileStream(@"f:/myText.txt", FileMode.Append));  //把读取xls文件的数据写入myText.txt文件中
            
                wr.Write(sbr.ToString());
                wr.Flush();

                return View();
        }
        
    }
}