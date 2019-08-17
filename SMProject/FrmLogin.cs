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
            SalePerson objPerson = new SalePerson()
            {
                SalesPersonId = Convert.ToInt32(this.txtLoginId.Text.Trim()),
                LoginPwd = this.txtLoignPwd.Text.Trim()
            };
            if (objSalePersonService.UserLogin(objPerson)!= null){
                this.DialogResult = DialogResult.OK;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
