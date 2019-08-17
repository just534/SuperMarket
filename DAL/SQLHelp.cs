using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DAL
{
    public class SQLHelp
    {
        private static string connstring = ConfigurationManager.ConnectionStrings["connString"].ToString();

        #region 带参数的Update方法
        /// <summary>
        /// 执行插入、更新、删除的方法
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数信息</param>
        /// <returns>返回受影响的行数</returns>
        public static int Update(string sql, SqlParameter[] parameters)
        {
            SqlConnection sqlcon = new SqlConnection(connstring);
            SqlCommand command = new SqlCommand(sql, sqlcon);
            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }
            try
            {
                sqlcon.Open();
                return command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string info = " 执行 public static int Update(string sql, SqlParameter[] parameters)方法时发生异常" + ex.Message;
                throw new Exception(info);
            }
            finally
            {
                sqlcon.Close();
            }

        }

        #endregion

        #region 带参数的GetsingleResult方法
        /// <summary>
        /// 执行单个查询结果的方法
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数信息</param>
        /// <returns>返回单个Object类型的结果</returns>
        public static object GetSingleResult(string sql, SqlParameter[] parameters)
        {
            SqlConnection sqlcon = new SqlConnection(connstring);
            SqlCommand command = new SqlCommand(sql, sqlcon);
            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }
            try
            {
                sqlcon.Open();
                return command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                string info = " 执行public static object GetSingleResult(string sql, SqlParameter[] parameters)" + ex.Message;
                throw new Exception(info);
            }
            finally
            {
                sqlcon.Close();
            }

        }

        #endregion

        #region 带参数的GetResult方法
        public SqlDataReader GetResult(string sql,SqlParameter[] parameters)
        {
            SqlConnection sqlcon = new SqlConnection(connstring);
            SqlCommand command = new SqlCommand(sql, sqlcon);
            if (parameters != null)
            {
                sqlcon.Open();
                command.Parameters.AddRange(parameters);
            }
            try
            {
                return command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                string info = " 执行public SqlDataReader GetResult(string sql,SqlParameter[] parameters)" + ex.Message;
                throw new Exception(info);
            }
        }


        #endregion
    }
}
