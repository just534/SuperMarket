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
                    objProduct = new Product();
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

        #region 保存商品销售信息
        public bool SaveSaleInfo(SaleList objSaleList,SMMembers objSMMembers)
        {
            List<string> sqllist = new List<string>();
            string mainsql = "insert into SalesList(SerialNum, TotalMoney, RealReceive, ReturnMoney, SalesPersonId) ";
            mainsql += "values('{0}', {1}, {2}, {3}, {4})";
            sqllist.Add(string.Format(mainsql,objSaleList.SerialNum,objSaleList.TotalMoney,objSaleList.RealReceive,
                objSaleList.ReturnMoney,objSaleList.SalesPersonId));
            foreach (SaleListDetail itemDetail in objSaleList.SaleListDetails)
            {
                string detailsql = "insert into SalesListDetail(SerialNum, ProductId, ProductName, UnitPrice, Discount, Quantity, SubTotalMoney) ";
                detailsql += "values('{0}','{1}', '{2}',{3}, {4}, {5}, {6})";
                detailsql = string.Format(detailsql, itemDetail.SerialNum, itemDetail.ProductId, itemDetail.ProductName, itemDetail.UnitPrice,
                    itemDetail.Discount, itemDetail.Quantity, itemDetail.SubTotalMoney);
                sqllist.Add(detailsql);
                string updatesql = "update ProductInventory Set TotalCount=TotalCount-{0} where ProductId='{1}'";
                updatesql = string.Format(updatesql, itemDetail.Quantity, itemDetail.ProductId);
                sqllist.Add(updatesql);
            }
            if (objSMMembers != null)
            {
                string pointsql = "update SMMembers Set Points+={0} where MemberId={1}";
                pointsql = string.Format(pointsql, objSMMembers.Points, objSMMembers.MemberId);
                sqllist.Add(pointsql);
            }

            try
            {
                return SQLHelp.UpdateByTran(sqllist, null);
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
