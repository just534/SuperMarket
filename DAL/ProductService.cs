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
            SqlParameter[] parameter = {
                new SqlParameter("@SerialNum",objSaleList.SerialNum),
                new SqlParameter("@TotalMoney",objSaleList.TotalMoney),
                new SqlParameter("@RealReceive",objSaleList.RealReceive),
                new SqlParameter("@ReturnMoney",objSaleList.ReturnMoney),
                new SqlParameter("@SalesPersonId",objSaleList.SalesPersonId),
                new SqlParameter("@ProductId",null),
                new SqlParameter("@ProductName",null),
                new SqlParameter("@UnitPrice",null),
                new SqlParameter("@Discount",null),
                new SqlParameter("@Quantity",null),
                new SqlParameter("@SubTotalMoney",null),
                new SqlParameter("@Points",null),
                new SqlParameter("@MemberId",null)
            };
            string mainsql = "insert into SalesList(SerialNum, TotalMoney, RealReceive, ReturnMoney, SalesPersonId) ";
            mainsql += "values(@SerialNum, @TotalMoney, @RealReceive, @ReturnMoney, @SalesPersonId)";
            sqllist.Add(mainsql);
            foreach (SaleListDetail itemDetail in objSaleList.SaleListDetails)
            {
                parameter[5] = new SqlParameter("@ProductId", itemDetail.ProductId);
                parameter[6] = new SqlParameter("@ProductName", itemDetail.ProductName);
                parameter[7] = new SqlParameter("@UnitPrice", itemDetail.UnitPrice);
                parameter[8] = new SqlParameter("@Discount", itemDetail.Discount);
                parameter[9] = new SqlParameter("@Quantity", itemDetail.Quantity);
                parameter[10] = new SqlParameter("@SubTotalMoney", itemDetail.SubTotalMoney);
                string detailsql = "insert into SaleListDetail(SerialNum, ProductId, ProductName, UnitPrice, Discount, Quantity, SubTotalMoney) ";
                detailsql += "values(@SerialNum,@ProductId, @ProductName,@UnitPrice, @Discount, @Quantity, @SubTotalMoney)";
                sqllist.Add(detailsql);
                string updatesql = " update ProductInventory TotalCount=TotalCount-@Quantity where ProductId=@ProductId";
                sqllist.Add(updatesql);
            }
            if (objSMMembers != null)
            {
                parameter[11] = new SqlParameter("@Points",objSMMembers.Points);
                parameter[12] = new SqlParameter("@MemberId", objSMMembers.Points);

                string pointsql = "update SMMembers Points+=@Points where MemberId=@MemberId";
                sqllist.Add(pointsql);
            }

            try
            {
                return SQLHelp.UpdateByTran(sqllist, parameter);
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
