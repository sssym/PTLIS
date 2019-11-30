using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace MainApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            start_craw(sender,e);
        }
        public bool StartProcess(string runFilePath, params string[] args)
        {
            string s = "";
            foreach (string arg in args)
            {
                s = s + arg + " ";
            }
            s = s.Trim();
            Process process = new Process();//创建进程对象    
            ProcessStartInfo startInfo = new ProcessStartInfo(runFilePath, s); // 括号里是(程序名,参数)
            process.StartInfo = startInfo;
            process.Start();
            return true;
        }

        private void start_craw(object sender, EventArgs e)
        {
            string exe_path = @"D:\project\坡头人民医院LIS\PTLIS - 特殊版本\PTLIS\bin\x86\HIS条码打印系统\HIS条码打印系统.exe";  // 被调exe
            string[] the_args = { "1", "2", "3", "4" };   // 被调exe接受的参数
            StartProcess(exe_path, the_args);
        }


    }
}
