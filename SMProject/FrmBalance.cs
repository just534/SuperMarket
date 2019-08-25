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
    public partial class FrmBalance : Form
    {
        private SMMembersService objSMM = new SMMembersService();
        //构造方法
        public FrmBalance(string TotalMoney)
        {
            InitializeComponent();
            this.lblTotalMoney.Text = TotalMoney;
            this.txtRealReceive.Text = TotalMoney;
            this.txtRealReceive.SelectAll();
            this.txtRealReceive.Focus();
        }

        private void txtMemberId_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                if (this.txtMemberId.Text.Trim().Length == 0)
                {
                    this.Tag = this.txtRealReceive.Text.Trim();//将实收款保存
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else//如果有会员卡，将实收款和会员卡号同时返回
                {
                    //返回形式 150|98987888888
                    //判断会员卡号是否存在
                    if (objSMM.GetSMMemberById(this.txtMemberId.Text.Trim()) == null)
                    {
                        MessageBox.Show("当前会员卡号不存在");
                        this.txtMemberId.SelectAll();
                        this.txtMemberId.Focus();
                        return;
                    }
                    this.Tag = this.txtRealReceive.Text.Trim() + "|" + this.txtMemberId.Text.Trim();
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            else if (e.KeyValue == 115)//F4 用户放弃所有商品购买
            {
                this.Tag = "F4";
                this.Close();
            }
            else if (e.KeyValue == 116)//F5 用户放弃部分商品购买
            {
                this.Tag = "F5";
                this.Close();
            }
            
        }
    }
}
