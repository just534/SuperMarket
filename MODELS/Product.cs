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
        public string  ProductId { get; set; }
        public string  ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Discount { get; set; }
        public int CategotyId { get; set; }



        //扩展属性:供商品扫描的时候使用
        public int Num { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal { get; set; }

        //直接扩展：商品分类名称

        public string CategoryName { get; set; }

    }
}
