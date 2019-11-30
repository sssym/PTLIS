using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Xml;
using System.IO;

namespace PTLIS
{
    class fun_addLog
    {
        public static void addLog(String str,string fun)
        {
            if (File.Exists(@"\SQLlog.txt"))
            {



                StreamWriter sw = new StreamWriter(@"\SQLlog.txt", true);
                sw.WriteLine(DateTime.Now+":"+ fun);
                sw.WriteLine(str);
                sw.Flush();
                sw.Close();

            }
            else
            {
                FileStream sf = new FileStream(@"\SQLlog.txt", FileMode.Create);
                sf.Close();
                addLog(str,fun);
            }

        }
    }
}
