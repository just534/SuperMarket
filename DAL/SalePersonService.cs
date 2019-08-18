using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MODELS;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class SalePersonService
    {
        #region 用户登录方法
        /// <summary>
        /// 根据用户账号和密码实现用户登录
        /// </summary>
        /// <param name="objsaleperson">包含用户账号和密码的对象</param>
        /// <returns>返回包含用户 账号  密码名称的用户对象或者空</returns>
        public SalePerson UserLogin(SalePerson objsaleperson)
        {
            string sql = "select SPName from SalesPerson where SalesPersonId=@SalesPersonId ";
            sql += " and LoginPwd=@LoginPwd";
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@SalesPersonId",objsaleperson.SalesPersonId),
                new SqlParameter("@LoginPwd",objsaleperson.LoginPwd)
            };
            var result = SQLHelp.GetSingleResult(sql, parameters);
            if (result == null) return null;//如果不正确返回为NULL
            else
            {
                objsaleperson.SPName = result.ToString();
                return objsaleperson;
            }
        }
        #endregion

        #region 登录日志
        /// <summary>
        /// 添加登录日志
        /// </summary>
        /// <param name="info"></param>
        /// <returns>返回日志编号</returns>
        public int WriteLog(LoginLog info)
        {
            string sql = "insert into LoginLogs(LoginId,SPName,ServerName) ";
            sql += "values(@LoginId,@SPName,@ServerName);select @@identity";
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@LoginId",info.LoginId),
                new SqlParameter("@SPName",info.SPName),
                new SqlParameter("@ServerName",info.ServerName)
            };
            try
            {
                return Convert.ToInt32(SQLHelp.GetSingleResult(sql, param));

            }
            catch (SqlException ex)
            {

                throw new Exception("应用程序与数据库连接出错，具体内容： " + ex.Message);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion
    }
}
