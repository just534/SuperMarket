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
using System.Net;



namespace SMProject
{
    
    public partial class FrmLogin : Form
    {
        private SalePersonService objSalePersonService = new SalePersonService();
        public FrmLogin()
        {
            InitializeComponent();
        }
        //登录按钮
        private void btnLogin_Click(object sender, EventArgs e)
        {
            if(this.txtLoginId.Text.Trim().Length==0 || this.txtLoignPwd.Text.Trim().Length == 0)
            {
                MessageBox.Show("账号密码必须填写完整", "错误提示");
                return;
            }
            SalePerson objPerson = new SalePerson()
            {
                SalesPersonId = Convert.ToInt32(this.txtLoginId.Text.Trim()),
                LoginPwd = this.txtLoignPwd.Text.Trim()
            };
            try
            {
                objPerson = objSalePersonService.UserLogin(objPerson);

                if (objPerson != null)
                {
                    Program.CurrentPerson = objPerson;
                    Program.CurrentPerson.LoginLogId = objSalePersonService.WriteLog(new LoginLog()
                    {
                        LoginId = Convert.ToInt32(objPerson.SalesPersonId),
                        SPName = objPerson.SPName,
                        ServerName = Dns.GetHostName()
                    });
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("账户或密码错误，请重新输入", "错误提示");
                    this.txtLoginId.Focus();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "错误信息");
            }
          
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtLoginId_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.txtLoginId.Text.Trim().Length != 0 && e.KeyValue==13)
            {
                this.txtLoignPwd.Focus();
            }
        }

        private void txtLoignPwd_KeyDown(object sender, KeyEventArgs e)
        {
            if(this.txtLoignPwd.Text.Trim().Length!=0 && e.KeyValue == 13)
            {
                btnLogin_Click(null, null);
            }
        }
    }
}
