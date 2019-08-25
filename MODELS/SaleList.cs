using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODELS
{
    [Serializable]
   public class SaleList
    {
        public SaleList()
        {
            this.SaleListDetails = new List<SaleListDetail>();
        }
        public string SerialNum { get; set; }
        public decimal TotalMoney { get; set; }
        public decimal RealReceive { get; set; }
        public decimal ReturnMoney { get; set; }
        public int SalesPersonId { get; set; }
        public DateTime SaleDate { get; set; }
        public List<SaleListDetail> SaleListDetails { get; set; }
    }

    [Serializable]
    public class SaleListDetail
    {
        public int Id { get; set; }
        public string SerialNum { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Discount { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotalMoney { get; set; }
    }

}
