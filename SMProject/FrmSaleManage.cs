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
        public FrmSaleManage()
        {
            InitializeComponent();
         
        }

        private void FrmSaleManage_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("确认退出程序吗?", "退出确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel) e.Cancel=true;
            try
            {
                DateTime dt = SQLHelp.GetServerTime();
                objSalePersonService.WriteExitLog(Program.CurrentPerson.LoginLogId, dt);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message,"错误提示");
            }
        }
    }
}
