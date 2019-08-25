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
    public class SMMembersService
    {
        #region 根据会员卡ID返回会员卡对象
        public SMMembers GetSMMemberById(string smmemberId)
        {
            string sql = "select MemberId, MemberName, Points, PhoneNumber, MemberAddress, OpenTime, MemberStatus from ";
            sql += "SMMembers where MemberId={0}";
            sql = string.Format(sql, smmemberId);
            SMMembers objSMMember = null;
            try
            {
                SqlDataReader objReader = SQLHelp.GetResult(sql, null);
                if (objReader.Read())
                {
                    objSMMember = new SMMembers()
                    {
                        MemberAddress = objReader["MemberAddress"].ToString(),
                        MemberId =objReader["MemberId"].ToString(),
                        MemberName = objReader["MemberName"].ToString(),
                        MemberStatus = Convert.ToInt32(objReader["MemberStatus"]),
                        OpenTime = Convert.ToDateTime(objReader["OpenTime"]),
                        PhoneNumber = objReader["PhoneNumber"].ToString(),
                        Points = Convert.ToInt32(objReader["Points"])

                    };
                }
                
                objReader.Close();
                return objSMMember;
            }
            catch (System.Data.SqlClient.SqlException ex)
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
