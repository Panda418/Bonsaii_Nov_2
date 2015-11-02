using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Bonsaii.Controllers
{
    public class Generate
    {
        /// <summary>
        /// 调用存储过程生成10位流水号（流水号的初始值在存储过程当中设置，现在的初始值是1）
        /// </summary>
        /// <returns>返回创建好的企业ID号</returns>
        public static string GenerateCompanyId()
        {
            string serialNumber;
            // Create an connection instance
            string Connection = ConfigurationManager.AppSettings["SystemDbConnectionString"];//"Data Source = localhost,1433;Network Library = DBMSSOCN;Initial Catalog = BonsaiiSys_Test;User ID = test;Password = admin;";
            SqlConnection DataConn = new SqlConnection(Connection);
            // Open connection
            try
            {
                if (DataConn.State == ConnectionState.Closed)
                {
                    DataConn.Open();
                }
            }
            catch (SqlException sqex)
            {
                // Create connection failed
                return null;
            }

            SqlCommand DBCmd = new SqlCommand("GetSerialNumber", DataConn);
            DBCmd.CommandType = CommandType.StoredProcedure;

            // Output parameter（值得注意的是：这里Add的输出变量必须与存储过程里的输出变量同名，否则会报告“dpIDS_GetSerialNumber 的@SerialNumber参数not supplied” 错误！）
            SqlParameter param = new SqlParameter("@SerialNumber", SqlDbType.VarChar, 10);
            param.Direction = System.Data.ParameterDirection.Output;
            DBCmd.Parameters.Add(param);

            try
            {

                DBCmd.ExecuteNonQuery();
                serialNumber = param.Value.ToString();  //得到输出参数的值
            }
            catch (SqlException sqex)
            {
                return null;
            }

            try
            {
                if (DataConn.State == ConnectionState.Open)
                {
                    DataConn.Close();
                }
            }
            catch (SqlException sqex)
            {
                return null;
            }
            return serialNumber;
        }

        /// <summary>
        /// 将生成的企业号，转换为合法的数据库名称，作为该企业的数据库名称
        /// </summary>
        /// <param name="CompanyId">企业ID号</param>
        /// <returns>可以作为企业数据库名称的合法的字符串</returns>
        public static string GetCompanyDbName(string CompanyId)
        {
            //string result = null;
            //for (int i = 0; i < CompanyId.Length; i++)
            //{
            //    char tmp = Convert.ToChar(CompanyId[i] + 17);
            //    result += tmp;
            //}

            return "Bonsaii" + CompanyId;
        }
    }
}