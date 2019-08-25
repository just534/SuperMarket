using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DAL;
using MODELS;


namespace SMProject
{
    public partial class FrmSaleManage : Form
    {


        #region  窗体拖动、关闭【实际项目中不用】

        private Point mouseOff;//鼠标移动位置变量
        private bool leftFlag;//标签是否为左键
        private void FrmMain_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseOff = new Point(-e.X, -e.Y); //得到变量的值
                leftFlag = true;                  //点击左键按下时标注为true;
            }
        }
        private void FrmMain_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                Point mouseSet = Control.MousePosition;
                mouseSet.Offset(mouseOff.X, mouseOff.Y);  //设置移动后的位置
                Location = mouseSet;
            }
        }
        private void FrmMain_MouseUp(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                leftFlag = false;//释放鼠标后标注为false;
            }
        }

        #endregion

        private SalePersonService objSalePersonService = new SalePersonService();
        private ProductService objProductService = new ProductService();
        private List<Product> productlist = new List<Product>();
        private BindingSource bs = new BindingSource();
        
        
        public FrmSaleManage()
        {
            InitializeComponent();
            this.lblSerialNum .Text= this.CreateSerialNum();
            this.lblSalePerson.Text = Program.CurrentPerson.SPName;
            this.dgvProdutList.AutoGenerateColumns = false;
        }
        #region 创建流水单号
        private string CreateSerialNum()
        {
            string serialNum = SQLHelp.GetServerTime().ToString("yyyyMMddHHmmssms");
            Random rd = new Random();
            serialNum += rd.Next(10, 99).ToString();
            return serialNum;
        }
        #endregion

        private void FrmSaleManage_FormClosing(object sender, FormClosingEventArgs e)
        {
            //退出程序确认，并保存退出的时间到日志
            if (MessageBox.Show("确认退出程序吗?", "退出确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel) e.Cancel=true;
            try
            {
                DateTime dt = SQLHelp.GetServerTime();
                objSalePersonService.WriteExitLog(Program.CurrentPerson.LoginLogId, dt);//获取当前的操作员的日志ID，写入退出时间
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"错误提示");
            }
        }

        private void txtProductId_KeyDown(object sender, KeyEventArgs e)
        {
            if( e.KeyValue == 13)
            #region 添加商品
            {
                //[1]验证信息
                //[2]如果当前集合中不存在该商品那么添加该商品
                var plist = from p in this.productlist
                            where p.ProductId.Equals(this.txtProductId.Text.Trim())
                            select p;
                if (plist.Count()==0)
                {
                    AddNewProduct();
                }else//如果商品已经存在，那么更新数量和小计金额
                {
                    Product objproduct = plist.FirstOrDefault<Product>();
                    objproduct.Quantity += Convert.ToInt32(this.txtQuantity.Text.Trim());
                    objproduct.SubTotal = Convert.ToDecimal(objproduct.Quantity * objproduct.UnitPrice);
                    if (objproduct.Discount != 0)
                    {
                        objproduct.SubTotal *= Convert.ToDecimal(objproduct.Discount/10);
                    }
                }
                this.bs.DataSource = this.productlist;
                this.dgvProdutList.DataSource = null;
                this.dgvProdutList.DataSource = this.bs;//显示商品列表
                this.lblTotalMoney.Text = (from p in this.productlist select p.SubTotal).Sum().ToString();
                this.txtProductId.Clear();
                this.txtQuantity.Text = "1";
                this.txtUnitPrice.Text = "0.00";
                this.txtDiscount.Text = "0";
                this.lblReceivedMoney.Text = "0.00";
                this.txtProductId.Focus();

            }
            #endregion
            else if(e.KeyValue==38)//向上移动
            {
                if (this.dgvProdutList.RowCount == 0 || this.dgvProdutList.RowCount == 1) return;
                {
                    this.bs.MovePrevious();
                }
            }
            else if (e.KeyValue == 40)
            {
                if (this.dgvProdutList.RowCount == 0 || this.dgvProdutList.RowCount == 1) return;
                {
                    this.bs.MoveNext();
                }
            }
            else if (e.KeyValue == 46)//删除一行
            {
                if (this.dgvProdutList.RowCount == 0) return;
                this.bs.RemoveCurrent();//这里删除列表的同时会删除集合中的数据
                this.dgvProdutList.DataSource = null;
                 this.dgvProdutList.DataSource = this.bs;
            }
            else if (e.KeyValue == 112)//按F4进入结算
            {
                if (this.dgvProdutList.RowCount == 0) return;
                Balance();
            }

        }

        #region 商品结算方法
        private void Balance()
        {
            FrmBalance objFrmBalance = new FrmBalance(this.lblTotalMoney.Text.Trim());
            if (objFrmBalance.ShowDialog() != DialogResult.OK)
            {
                if (objFrmBalance.Tag.ToString() == "F4")//用户放弃本次购买
                {
                    //重新生成流水号等待结算下一个客户
                    RestForm();
                }
                else if (objFrmBalance.Tag.ToString() == "F5")//用户放弃部分商品购买
                {
                    this.txtProductId.Focus();
                }
            }else//进入正式结算，获取用户的实付金额和会员卡
            {
                SMMembers objSMM = null;
                if (objFrmBalance.Tag.ToString().Contains('|'))//如果有会员卡号
                {
                    string[] info = objFrmBalance.Tag.ToString().Split('|');
                    this.lblReceivedMoney.Text = info[0];
                    objSMM = new SMMembers()//新建一个会员卡对象
                    {
                        MemberId = info[1],
                        Points = (int)(Convert.ToDouble(this.lblTotalMoney.Text) / 10.0)
                    };
                }
                else
                {
                    this.lblReceivedMoney.Text = objFrmBalance.Tag.ToString();
                }
                //显示找零
                this.lblReturnMoney.Text = (Convert.ToDecimal(this.lblReceivedMoney.Text.Trim()) -
                    Convert.ToDecimal(this.lblTotalMoney.Text)).ToString();
                SaleList objSaleList = new SaleList()
                {
                    SerialNum = this.lblSerialNum.Text.Trim(),
                    TotalMoney = Convert.ToDecimal(this.lblTotalMoney.Text.Trim()),
                    RealReceive = Convert.ToDecimal(this.lblReceivedMoney.Text.Trim()),
                    ReturnMoney = Convert.ToDecimal(this.lblReturnMoney.Text.Trim()),
                    SalesPersonId = Program.CurrentPerson.SalesPersonId
                };
                foreach (Product item in this.productlist)
                {
                    objSaleList.SaleListDetails.Add(new SaleListDetail()
                    {
                        SerialNum = this.lblSerialNum.Text.Trim(),
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        Quantity = item.Quantity,
                        Discount = item.Discount,
                        SubTotalMoney = item.SubTotal,
                        UnitPrice = item.UnitPrice
                    });
                }
                try
                {
                    objProductService.SaveSaleInfo(objSaleList, objSMM);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("保存销售数据到数据库时发生错误"+ex.Message, "错误信息");
                    return;
                }
                RestForm();
            }
        }

        private void RestForm()
        {
            this.lblSerialNum.Text = CreateSerialNum();
            this.dgvProdutList.DataSource = null;
            this.productlist.Clear();
            this.txtProductId.Focus();
            this.txtQuantity.Text = "1";
            this.txtUnitPrice.Text = "0.00";
            this.txtDiscount.Text = "0";
            this.lblReceivedMoney.Text = "0.00";
        }


        #endregion

        //新增加一个商品，前提是列表中没有该商品
        private void AddNewProduct()
        {
            Product objproduct = objProductService.GetProductById(this.txtProductId.Text.Trim());
            if (objproduct == null)
            {
                objproduct = new Product()
                {
                    ProductName = "暂时没有商品信息",
                    ProductId = this.txtProductId.Text.Trim(),
                    UnitPrice = Convert.ToDecimal(this.txtUnitPrice.Text.Trim()),
                    Discount = Convert.ToInt32(this.txtDiscount.Text.Trim())
                };
            }
            else
            {
                this.txtUnitPrice.Text = objproduct.UnitPrice.ToString();
                this.txtDiscount.Text = objproduct.Discount.ToString();
            }
            objproduct.Quantity = Convert.ToInt32(this.txtQuantity.Text.Trim());
            objproduct.SubTotal = Convert.ToDecimal(objproduct.Quantity * objproduct.UnitPrice);
            if (objproduct.Discount != 0)//如果有折扣
            {
                objproduct.SubTotal*= Convert.ToDecimal(objproduct.Discount / 10);
            }
            objproduct.Num = this.productlist.Count + 1;//商品序号
            this.productlist.Add(objproduct);
            this.bs.MoveLast();
        }

        #region 数量单价折后回车后直接回商品ID 输入框
        private void txtOther_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                this.txtProductId.Focus();
            }
        }

        #endregion

        private void dgvProdutList_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            this.lblTotalMoney.Text = (from p in this.productlist select p.SubTotal).Sum().ToString();//更新总金额
            for (int i = 0; i < this.productlist.Count; i++)
            {
                this.productlist[i].Num = i + 1;//更新商品序号
            }
        }
    }
}
