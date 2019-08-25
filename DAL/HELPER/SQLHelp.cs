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
        /// <param name="parameters">参数信息,没有请传递NULL</param>
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
        /// <param name="parameters">参数信息,没有请传递NULL</param>
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
        /// <summary>
        /// 执行带参数的GetResult方法
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数信息,没有请传递NULL</param>
        /// <returns>返回一个结果集</returns>
        public static SqlDataReader GetResult(string sql,SqlParameter[] parameters)
        {
            SqlConnection sqlcon = new SqlConnection(connstring);
            SqlCommand command = new SqlCommand(sql, sqlcon);
            sqlcon.Open();
            if (parameters != null)
            {
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

        #region 带参数的事务执行更新数据
        /// <summary>
        /// 执行事务的方法
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameter">参数信息,没有请传递NULL</param>
        /// <returns></returns>

        public static bool UpdateByTran(List<string> sql,SqlParameter[] parameter)
        {
            SqlConnection sqlcon = new SqlConnection(connstring);
            SqlCommand command = new SqlCommand();
            command.Connection = sqlcon;
            if (parameter != null)
            {
                command.Parameters.AddRange(parameter);
            }
            try
            {
                sqlcon.Open();
                command.Transaction = sqlcon.BeginTransaction();
                foreach (string item in sql)
                {
                    command.CommandText = item;
                    command.ExecuteNonQuery();
                }
                command.Transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                if (command.Transaction != null)
                {
                    command.Transaction.Rollback();
                    string info = "执行 public static bool UpdateByTran(List<string> sql,SqlParameter[] parameter 方法时出错" + ex.Message;
                    throw new Exception(info);
                }
                throw new Exception(ex.Message);
            }
            finally
            {
                if (command.Transaction != null)
                {
                    command.Transaction = null;
                }
                sqlcon.Close();
            }

        }
        #endregion

        #region 获取服务器时间
        public static DateTime GetServerTime()
        {
            string sql = "select getdate()";
            try
            {
                return (DateTime)GetSingleResult(sql, null);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 带参数的返回数据集方法
        public static DataSet GetDataSet(string sql,SqlParameter[] parameter)//这里可以传入一个name
        {
            SqlConnection sqlconn = new SqlConnection(connstring);
            SqlCommand sqlcommand = new SqlCommand(sql, sqlconn);
            SqlDataAdapter da = new SqlDataAdapter(sqlcommand);
            DataSet ds = new DataSet();
            if (parameter != null)
            {
                sqlcommand.Parameters.AddRange(parameter);
            }
            try
            {
                sqlconn.Open();
                da.Fill(ds);//fill(ds,name)这里把名字为name的datatable加入ds数据集，后面可以根据名字检索
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlconn.Close();
            }
        }
        #endregion
    }
}
