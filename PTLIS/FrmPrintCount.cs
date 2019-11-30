using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PTLIS
{
    public partial class FrmPrintCount : Form
    {
        public static int int_prinCount = 0;
        public FrmPrintCount()
        {
            InitializeComponent();
        }

        private void FrmPrintCount_Load(object sender, EventArgs e)
        {
            int_prinCount = 0;
            textBox1.Text = "";
            textBox1.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim()=="")
            {
                return;
            }
            int_prinCount = int.Parse(textBox1.Text);
            this.Close();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 0x20) e.KeyChar = (char)0;  //禁止空格键  
            if ((e.KeyChar == 0x2D) && (((TextBox)sender).Text.Length == 0)) e.KeyChar = (char)0; ;   //处理负数  
            if (e.KeyChar > 0x20)
            {
                try
                {
                    double.Parse(((TextBox)sender).Text + e.KeyChar.ToString());
                }
                catch
                {
                    e.KeyChar = (char)0;   //处理非法字符  
                }
            }

        }
    }
}
