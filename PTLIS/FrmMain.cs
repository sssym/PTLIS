using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;

using System.Windows.Forms;

namespace PTLIS
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            f.ShowDialog();
        }
    }
}
