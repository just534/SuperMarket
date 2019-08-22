using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//商品实体类
namespace MODELS
{
    [Serializable]
    public class Product
    {
        public string  ProductId { get; set; }//商品编号
        public string  ProductName { get; set; }//商品名称
        public decimal UnitPrice { get; set; }//单价
        public int Discount { get; set; }//折扣
        public int CategoryId { get; set; }//商品类别编号



        //扩展属性:供商品扫描的时候使用
        public int Num { get; set; }//商品序号
        public int Quantity { get; set; }//商品数量
        public decimal SubTotal { get; set; }//小计金额

        //直接扩展：商品分类名称

        public string CategoryName { get; set; }//商品分类

    }
}
