using System;
using System.Collections.Generic;


using System.Windows.Forms;

namespace PTLIS
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            DbConn dbConn = new DbConn();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (DbConn.mzzybz == "1")
            {
                Application.Run(new Form1());
            }
            else if (DbConn.mzzybz == "2")
            {
                Application.Run(new FrmTP());
            }
        }
    }
}
