﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetBarManageSystem
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)  //退出系统按钮
        {
            //获取上机时间
            DateTime dts = Convert.ToDateTime(lblLoginTime.Text);
            string logintime = dts.GetDateTimeFormats('g')[0].ToString();
            //获取下机时间
            DateTime dtr = DateTime.Now;
            string rq = dtr.ToShortDateString().ToString();
            DateTime dts2 = Convert.ToDateTime(rq + " " + n1.Value + ":" + n2.Value);
            string offtime = dts2.GetDateTimeFormats('g')[0].ToString();
            //得出时间间隔
            TimeSpan dt = Convert.ToDateTime(offtime) - Convert.ToDateTime(logintime);
            int jg = Convert.ToInt32(Convert.ToString(dt.TotalMinutes));
            //获得单位时间金额设置值

            int i, j;
            int k = 1;
            for (int num = 0; num < 100; num++)
            {
                i = 5 * num;
                j = 5 * (num + 1);
                if (i < jg && j >= jg)
                {
                    k = 1 + num;
                }
            }
            //计算剩余金额
            int sy = Convert.ToInt32(lblLoginMoney.Text.Trim()) - k;
            //更新数据库余额
            string id = lblLoginId.Text.Trim();
            Facade.Consumers cs = new Facade.Consumers();
            cs.UpdateMoney(id, sy);
            //在消费记录表中插入一条新的消费信息
            Entity.Money consumeinfo = new Entity.Money();
            consumeinfo.cardno = lblLoginId.Text.Trim();
            consumeinfo.logintime = Convert.ToDateTime(lblLoginTime.Text.Trim());
            consumeinfo.offtime = Convert.ToDateTime(offtime);
            consumeinfo.consumetime = jg;
            consumeinfo.consume = k;
            consumeinfo.remaindermoney = sy;
            cs.InsertConsumeInfo(consumeinfo);

            //退出系统  
            MessageBox.Show("上机时间为：" + jg + "," + "消费金额为：" + k + "," + "剩余金额为：" + sy, "温馨提示");
            
        }

        private void ExitSystem() //退出系统给出询问提示
        {
            if (DialogResult.Yes == MessageBox.Show("是否退出系统？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                this.Dispose();
                Application.Exit();
            }
        }

        private void btnAddUser_Click(object sender, EventArgs e) //添加用户按钮
        {
            frmAddManager f = new frmAddManager();
            f.Show();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e) //点击窗体右上角红叉
        {
            if (DialogResult.Yes == MessageBox.Show("是否退出系统？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                this.Dispose();
                Application.Exit();
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void frmMain_Load(object sender, EventArgs e) //窗体加载事件
        {
            if (Entity.GoAnyWhere.level.Trim() == "用户")
            {
                panel2.Hide();
                panel3.Hide();
                label3.Visible = true;
                lblLoginTime.Visible = true;
                label5.Visible = true;
                lblLoginId.Visible = true;
                label7.Visible = true;
                lblLoginMoney.Visible = true;
                label9.Visible = true;
                label10.Visible = true;
                label11.Visible = true;
                label12.Visible = true;
                label13.Visible = true;
                label14.Visible = true;

                DateTime dt = DateTime.Now;
                lblLoginTime.Text = dt.GetDateTimeFormats('g')[0].ToString();
                lblLoginId.Text = Entity.GoAnyWhere.id;
                //根据id获取用户余额
                Facade.Consumers cs = new Facade.Consumers();
                lblLoginMoney.Text = cs.CheckMoney(Entity.GoAnyWhere.id);

                label10.Text = "5";
                label12.Text = "1";
                label14.Text = Convert.ToString(Convert.ToInt32(lblLoginMoney.Text) - 1);
            }
            else if (Entity.GoAnyWhere.level.Trim() == "收银员")
            {
                panel1.Hide();
                panel3.Hide();
                panel4.Hide();
            }
            else
            {
                panel1.Hide();
                panel2.Hide();
                panel4.Hide();
            }
        }

        private void btnAddUsers_Click(object sender, EventArgs e) //收银员添加消费用户
        {
            frmAddUsers f = new frmAddUsers();
            f.ShowDialog();
        }

        private void txtCheck_Click(object sender, EventArgs e)  //查看所有消费者数据
        {
            frmConsumersInfo f = new frmConsumersInfo();
            f.ShowDialog();
        }

        private void btnRecharge_Click(object sender, EventArgs e) //单击用户充值按钮
        {
            frmConsumerRecharge f = new frmConsumerRecharge();
            f.ShowDialog();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //获取上机时间
            DateTime dts = Convert.ToDateTime(lblLoginTime.Text);
            string logintime = dts.GetDateTimeFormats('g')[0].ToString();
            //获取下机时间
            DateTime dts2 = DateTime.Now;
            string offtime = dts2.GetDateTimeFormats('g')[0].ToString();
            //得出时间间隔
            TimeSpan dt = Convert.ToDateTime(offtime) - Convert.ToDateTime(logintime);
            int jg = Convert.ToInt32(Convert.ToString(dt.TotalMinutes));

            label10.Text = jg.ToString();
        }

        private void button1_Click_1(object sender, EventArgs e) //点击消费情况按钮
        {
            frmCheckConsumeStation f = new frmCheckConsumeStation();
            f.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e) //点击用户退卡按钮
        {
            frmConsumerBackMoney f = new frmConsumerBackMoney();
            f.ShowDialog();
        }

        private void btnBill_Click(object sender, EventArgs e) //点击账单按钮
        {
            frmBill f = new frmBill();
            f.ShowDialog();
        }
    }
}
