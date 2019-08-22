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
    public class ProductService
    {
        #region 根据商品编号返回商品实体类
        public Product GetProductById(string productId)
        {
            string sql = "select ProductId,ProductName,UnitPrice,Discount,CategoryId from Products where ProductId=@ProductId";
            SqlParameter[] param = new SqlParameter[] {
            new SqlParameter("@ProductId",productId)};
            Product objProduct =null;
            try
            {
                SqlDataReader objReader = SQLHelp.GetResult(sql, param);
                if (objReader.Read())
                {
                    objProduct.UnitPrice =Convert.ToDecimal(objReader["UnitPrice"]);
                    objProduct.ProductId = objReader["ProductId"].ToString();
                    objProduct.ProductName = objReader["ProductName"].ToString();
                    objProduct.CategoryId = Convert.ToInt32(objReader["CategoryId"]);
                    objProduct.Discount = Convert.ToInt32(objReader["Discount"]);
                }
                objReader.Close();
                return objProduct;
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
