using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OracleClient;
using System.Collections;

using System.Drawing.Printing;
namespace PTLIS
{
    public partial class FrmTP : Form
    {
        String Strsql = "";
        ArrayList arrayList = null;

        Image image;
        String Str = "";
        String Str_OrderSql = "";
        String Str_ExcuteSql = "";
        int j = 0;
        int i = 0;
        Boolean hasRow = false;
        int itianshu = 0;
        static String StrPrintDate = "";
        int ipage = 0;
        OracleDataAdapter sda = null;
        DataSet ds = new DataSet();
        DbConn dbc = null;
        public static int i_flag = 0;
        ArrayList al = null;

        int icount = 0;
        int izuhao = 0;//组号
        OracleDataReader odr;
        OracleDataAdapter oracleDataAdapter;
        string str_patientid = "";
        public FrmTP()
        {
            InitializeComponent();
            dbc = new DbConn();
        }

        private void FrmTP_Load(object sender, EventArgs e)
        {
            dbc = new DbConn();
            //Strsql = "select a.hospital_name_chr from t_bse_hospitalinfo a";
            //odr = dbc.GetDataReader(Strsql);
            //odr.Read();
            //if (odr.GetValue(0).ToString() != OrclIcareConn.str_key)
            //{
            //    MessageBox.Show("系统无法启动，请联系管理员！" + odr.GetValue(0).ToString());
            //    Application.Exit();
            //}
            try
            {
                dateTimePicker1.Value = DateTime.Now;
                Strsql = "SELECT deptname_vchr FROM T_BSE_DEPTDESC WHERE STATUS_INT=1 AND ATTRIBUTEID='0000003' and code_vchr='" + DbConn.ksdm + "'";
               odr = dbc.GetDataReader(Strsql);
                while (odr.HasRows)
                {
                    while (odr.Read())
                    {
                        comboBox1.Items.Add(odr.GetValue(0).ToString());

                    }
                    odr.NextResult();
                }
                comboBox1.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public String getTypeString(String str)
        {
            int length = 0;
            String Str1 = "";
            int i = 0;
            do
            {
                length = str.IndexOf(',');
                if (length > 0)
                {
                    if (i == 0)
                    {
                        Str1 = Str1 + "'" + str.Substring(0, length) + "'";
                        i++;
                    }
                    else
                    {
                        Str1 = Str1 + ",'" + str.Substring(0, length) + "'";
                        i++;
                    }
                    str = str.Substring(length + 1);
                }
                else if (length == -1)
                {
                    if (i == 0)
                    {
                        Str1 = "'" + str + "'";
                    }
                    else
                    {
                        Str1 = Str1 + ",'" + str + "'";
                    }
                }
            }
            while (length > 0);

            return Str1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            checkedListBox2.Items.Clear();
            int icheckcount = 0;
            for (int ii = 0; ii < checkedListBox1.Items.Count; ii++)
            {
                if (checkedListBox1.GetItemChecked(ii))
                {
                    icheckcount++;
                }
            }
            if (icheckcount < 1)
            {
                MessageBox.Show("没有选中床号！");
            }
            else if (icheckcount > 1)
            {
                MessageBox.Show("选择多项不允许加载方号！");
            }
            else
            {
                Strsql = "select t2.patientid_chr from t_bse_bed t1,t_opr_bih_register t2 "
                    + "where t1.bihregisterid_chr=t2.registerid_chr "
                    + "and t1.code_chr ='" + checkedListBox1.CheckedItems[0].ToString() + "'";
                odr = dbc.GetDataReader(Strsql);
                if (odr.Read())
                {

                    str_patientid = odr.GetValue(0).ToString();

                    Strsql = "select "
                         + "t1.check_content_vchr as 检查项目"
                         + " from t_opr_lis_application t1"
                         + " where t1.pstatus_int = 2 "
                         + " and t1.patientid_chr ='" + str_patientid + "'";

                    odr = dbc.GetDataReader(Strsql);
                    // comboBox3.Items.Add("全部");
                    while (odr.HasRows)
                    {
                        while (odr.Read())
                        {
                            checkedListBox2.Items.Add(odr.GetValue(0).ToString());

                        }
                        odr.NextResult();
                    }
                }

            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkedListBox1.Items.Clear();
            Strsql = "select  t1.code_chr "
             + " from t_bse_bed t1 ,t_opr_bih_register t2, t_bse_patient t3,t_bse_deptdesc t4 "
             + " where t3.patientid_chr = t2.patientid_chr and t2.registerid_chr = t1.bihregisterid_chr "
             + " and t4.deptid_chr = t1.nurseunitid_vchr "
             + " and t4.code_vchr = '" + DbConn.ksdm + "' "
             + " order by t1.code_chr";
            OracleDataReader odr = dbc.GetDataReader(Strsql);
            //  checkedListBox1.Items.Add("全部");
            //  = null;
            while (odr.HasRows)
            {

                while (odr.Read())
                {

                    checkedListBox1.Items.Add(odr.GetValue(0).ToString());
                    // comboBox2.Items.Add(odr.GetValue(0).ToString());

                }
                odr.NextResult();
            }
            //omboBox2.SelectedIndex = 0;
        }

        private void 查询_Click(object sender, EventArgs e)
        {

            arrayList = new ArrayList();
            if (checkedListBox2.Items.Count != 0)
            {
                for (int i = 0; i < checkedListBox2.Items.Count; i++)
                {
                    if (checkedListBox2.GetItemChecked(i))
                    {
                        arrayList.Add(checkedListBox2.Items[i].ToString());
                    }
                }
                Strsql = "select t1.application_id_chr as 申请单号,"
                         + "t1.application_dat as 申请日期,"
                         + "t1.patient_name_vchr as 患者姓名,"
                         + "t1.sex_chr as 性别,"
                         + "t1.age_chr as 年龄,"
                         + "t1.check_content_vchr as 检查项目,"
                         + "t1.bedno_chr as 床位号,"
                         + "t1.patient_inhospitalno_chr as 住院号,"
                         + "t5.deptname_vchr as 科室,"
                         + "case when t1.patient_type_id_chr = '2' then '门诊' "
                         + "when t1.patient_type_id_chr = '1' then '住院' else '其他' end as 就诊类别 "
                         + " from t_opr_lis_application t1,"
                         + "t_bse_employee t4,"
                         + " t_bse_deptdesc t5"
                         + " where t1.pstatus_int = 2 "
                         + " and t1.appl_empid_chr = t4.empid_chr"
                         + " and t1.appl_deptid_chr = t5.deptid_chr"
                         + " and t1.patientid_chr ='" + str_patientid + "'"
                         ;
                if (arrayList.Count != 0)
                {
                    Strsql = Strsql + " and t1.check_content_vchr in (";
                    for (int i = 0; i < arrayList.Count - 1; i++)
                    {
                        Strsql = Strsql + "'" + arrayList[i].ToString() + "',";
                    }
                    Strsql = Strsql + "'" + arrayList[arrayList.Count - 1].ToString() + "')";
                }
            }
            else
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    if (checkedListBox1.GetItemChecked(i))
                    {
                        arrayList.Add(checkedListBox1.Items[i].ToString());
                    }
                }
                Strsql = "select t1.application_id_chr as 申请单号,"
                         + "t1.application_dat as 申请日期,"
                         + "t1.patient_name_vchr as 患者姓名,"
                         + "t1.sex_chr as 性别,"
                         + "t1.age_chr as 年龄,"
                         + "t1.check_content_vchr as 检查项目,"
                         + "t1.bedno_chr as 床位号,"
                         + "t1.patient_inhospitalno_chr as 住院号,"
                         + "t5.deptname_vchr as 科室,"
                         + "case when t1.patient_type_id_chr = '2' then '门诊' "
                         + "when t1.patient_type_id_chr = '1' then '住院' else '其他' end as 就诊类别 "
                         + " from t_opr_lis_application t1,t_bse_bed t2,t_opr_bih_register t3,"
                         + " t_bse_deptdesc t5"
                         + " where t1.pstatus_int = 2 "
                         + " and t2.status_int=2"
                         + " and t1.appl_deptid_chr = t5.deptid_chr"
                         + " and t1.bedno_chr=t2.code_chr "
                         + " and t2.bihregisterid_chr=t3.registerid_chr"
                         + " and t1.patientid_chr = t3.patientid_chr"
                         + " and t1.bedno_chr in (";

                if (arrayList.Count != 0)
                {
                    for (int i = 0; i < arrayList.Count - 1; i++)
                    {
                        Strsql = Strsql + "'" + arrayList[i].ToString() + "',";
                    }
                    Strsql = Strsql + "'" + arrayList[arrayList.Count - 1].ToString() + "')";
                }
                else
                {
                    return;
                }
               
            }
            Strsql = Strsql + " order by t1.application_id_chr desc";
            oracleDataAdapter = dbc.GetDataAdapter(Strsql);
            DataSet dataSet = new DataSet();
            oracleDataAdapter.Fill(dataSet, "table1");

            dataGridView1.DataSource = dataSet.Tables["table1"].DefaultView;
            dataGridView1.Columns[0].Width = 130;
            dataGridView1.Columns[1].Width = 130;

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    checkedListBox1.SetItemChecked(i, true);
                }
            }
            else
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    checkedListBox1.SetItemChecked(i, false);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount == 0)
            {
                return;
            }
            PaperSize pkCustomSize = new PaperSize("First Custom Size", 197, 118);
            PDoc.DefaultPageSettings.PaperSize = pkCustomSize;
            ((System.Windows.Forms.Form)Pvd).StartPosition = FormStartPosition.CenterScreen;
            ((System.Windows.Forms.Form)Pvd).Width = 197;
            ((System.Windows.Forms.Form)Pvd).Height = 118;
            ((System.Windows.Forms.Form)Pvd).Icon = this.Icon;
            PDoc.DocumentName = "1";
            Pvd.Document = PDoc;
            if (DbConn.print != "1")
            {
                Pvd.ShowDialog();
            }
            else
            {
                PDoc.Print();
            }
        }

        private void PDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            Font objFont = new Font("宋体", 8, FontStyle.Bold);
            Font objFont2 = new Font("宋体", 4, FontStyle.Regular);
            Brush objBrush = Brushes.Black;
            Pen objPen = new Pen(objBrush);
            objPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            Psd.PageSettings = new System.Drawing.Printing.PageSettings();
            Psd.PageSettings.Margins.Left = 5;
            Psd.PageSettings.Margins.Top = 5;
            Psd.PageSettings.Margins.Right = 5;
            Psd.PageSettings.Margins.Bottom = 5;
            int x1 = 0;
            int y1 = 0;
            int x2 = 0;
            int y2 = 15;
            int x3 = 0;
            int y3 = 30;
            int x4 = 30;
            int y4 = 30;
            int x5 = 0;
            int y5 = 100;

            //Psd.PageSettings.Landscape = true;
            int nLeft = Psd.PageSettings.Margins.Left;
            int nTop = Psd.PageSettings.Margins.Top;
            arrayList = new ArrayList();

            if (i < dataGridView1.RowCount)
            {

                Image image2;
                ClsPublic.GetBarcode(70, 180, BarcodeLib.TYPE.CODE128, dataGridView1[0, i].Value.ToString(), out image2);
                image = image2;

                //arrayList = ClsPublic.getItem1(dataGridView1[3, i].Value.ToString());


                e.Graphics.DrawString(" " + DateTime.Parse(dataGridView1.Rows[i].Cells["申请日期"].Value.ToString()).ToString("MM-dd")
                    + "/" + dataGridView1.Rows[i].Cells["科室"].Value.ToString()
                    + "/" + dataGridView1.Rows[i].Cells["床位号"].Value.ToString()
                    + "/" + dataGridView1.Rows[i].Cells["住院号"].Value.ToString(), objFont, Brushes.Black, x1, y1);
                e.Graphics.DrawString(" 姓名:" + dataGridView1.Rows[i].Cells["患者姓名"].Value.ToString()
                    + "  性别:" + dataGridView1.Rows[i].Cells["性别"].Value.ToString()
                    + "  年龄:" + dataGridView1.Rows[i].Cells["年龄"].Value.ToString(), objFont, Brushes.Black, x2, y2);
                // e.Graphics.DrawString("" + arrayList[j].ToString(), objFont, Brushes.Black, x3, y3);
                e.Graphics.DrawImage(image, x4, y4, 180, 50);

                e.Graphics.DrawString(" " + dataGridView1.Rows[i].Cells["检查项目"].Value.ToString(), objFont, Brushes.Black, x5, y5);


                e.HasMorePages = true;

                i++;
                if (i == dataGridView1.RowCount)
                {
                    e.HasMorePages = false;
                    i = 0;
                }
                





            }
            else
            {

            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                for (int i = 0; i < checkedListBox2.Items.Count; i++)
                {
                    checkedListBox2.SetItemChecked(i, true);
                }
            }
            else
            {
                for (int i = 0; i < checkedListBox2.Items.Count; i++)
                {
                    checkedListBox2.SetItemChecked(i, false);
                }
            }
        }
    }

}
