using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;

using System.Windows.Forms;
using System.Data.OracleClient;

using System.Drawing.Printing;
using System.Collections;
namespace PTLIS
{
    public partial class Form1 : Form
    {
       public static string strSql = "";
        Image image;
       public static OracleDataAdapter oracleDataAdapter;
        DbConn dbConn;
        DataSet dataSet;
        string Str_txt3 = "";
        ArrayList arrayList;
        int i = 0;
        int j = 0;
        int int_selectRow = 0;
        public Form1()
        {
            InitializeComponent();
            dbConn = new DbConn();
            textBox1.Text = "";
            textBox1.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.SelectedIndex == 0)
                {
                    Str_txt3 = textBox1.Text;
                    if (textBox1.Text.Length < 10 && textBox1.Text.Length != 0)
                    {
                        for (int i = 0; i < (10 - textBox1.Text.Length); i++)
                        {
                            Str_txt3 = "0" + Str_txt3;
                        }
                    }
                    textBox1.Text = Str_txt3;
                }

                strSql = "select * from (select t1.application_id_chr as 申请单号,"
                    + "t1.application_dat as 申请日期,"
                    + "t1.patient_name_vchr as 患者姓名,"
                    + "t1.check_content_vchr as 检验内容,"
                    + "t1.sex_chr as 性别,"
                    + "t1.age_chr as 年龄,"
                    + "case when t1.patient_type_id_chr = '2' then '门诊' "
                    + "when t1.patient_type_id_chr = '1' then '住院' else '其他' end as 就诊类别,"
                    + "t1.diagnose_vchr as 诊断,"
                    + "t1.bedno_chr as 床号,"
                    + "t4.lastname_vchr as 申请医生,"
                    + "t5.deptname_vchr as 申请科室,"
                    + "t1.patient_inhospitalno_chr as 住院号,"
                    + " t1.printed_num"
                    + " from t_opr_lis_application t1,"
                    + "t_bse_employee t4,"
                    + " t_bse_deptdesc t5"
                    + " where t1.pstatus_int = 2 "
                    + " and t1.appl_empid_chr = t4.empid_chr"
                    + " and t1.appl_deptid_chr = t5.deptid_chr";
                // +"and t1.patientcardid_chr = '"++"'"
                if (checkBox1.Checked)
                {
                    
                    strSql = strSql + " and (t1.application_dat>=to_date('" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "','yyyy-MM-dd') and  t1.application_dat<to_date('" + dateTimePicker2.Value.AddDays(1).ToString("yyyy-MM-dd") + "','yyyy-MM-dd'))";
                }
                else
                {
                   
                    strSql = strSql + " and to_char(t1.application_dat,'yyyy-MM-dd')=to_char(sysdate,'yyyy-MM-dd') ";
                }
                //+ "and t1.application_dat >= sysdate - 7";

                if (comboBox1.SelectedIndex == 0 && textBox1.Text.Length != 0)
                {
                    strSql = strSql + " and t1.patientcardid_chr='" + textBox1.Text + "'";
                }
                else if (comboBox1.SelectedIndex == 1 && textBox1.Text.Length != 0)
                {

                    strSql = strSql + " and  t1.patient_inhospitalno_chr='" + textBox1.Text + "'";
                }
                else if (comboBox1.SelectedIndex == 2 && textBox1.Text.Length != 0)
                {

                    strSql = strSql + " and  t1.patient_name_vchr like '%" + textBox1.Text + "%'";
                }
                strSql = strSql + "order by t1.application_id_chr desc) where rownum=1";
                oracleDataAdapter = dbConn.GetDataAdapter(strSql);
                dataSet = new DataSet();


                oracleDataAdapter.Fill(dataSet, "table1");
                dataGridView1.DataSource = dataSet.Tables["table1"].DefaultView;
                dataGridView1.Columns[4].Visible = true;
                dataGridView1.Columns[5].Visible = true;
                dataGridView1.Columns[6].Visible = true;
                dataGridView1.Columns[7].Visible = false;
                dataGridView1.Columns[8].Visible = false;
                dataGridView1.Columns[9].Visible = false;
                dataGridView1.Columns[10].Visible = false;
                dataGridView1.Columns[11].Visible = false;
                dataGridView1.Columns[12].Visible = false;
                fun_addLog.addLog(strSql, "button1_Click");
                if (dataGridView1.RowCount != 0)
                {
                    if (comboBox1.SelectedIndex == 0&&checkBox1.Checked==false)
                    {
                        button2_Click(sender, e);
                        //ArrayList al = new ArrayList();
                        //strSql = "update t_opr_lis_application set printed_num=printed_num+1 where patientcardid_chr='" + textBox1.Text + "'";
                        //al.Add(strSql);
                        //if (!dbConn.GetTransaction(al))
                        //{
                        //    MessageBox.Show("数据保存失败，请联系管理员！");

                        //}

                    }
                    else
                    {
                        DialogResult dir = MessageBox.Show("是否打印？", "提示", MessageBoxButtons.YesNo);
                        if (dir == DialogResult.Yes)
                        {
                            button2_Click(sender, e);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            textBox1.Text = "";
            textBox1.Focus();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (DbConn.mzzybz == "1")
            {
                comboBox1.SelectedIndex = 0;
                textBox1.Text = "";
                textBox1.Focus();

            }
            else if (DbConn.mzzybz == "2")
            {


            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PaperSize pkCustomSize = new PaperSize("First Custom Size", 197, 118);
            PDoc.DefaultPageSettings.PaperSize = pkCustomSize;
            ((System.Windows.Forms.Form)Pvd).StartPosition = FormStartPosition.CenterScreen;
            ((System.Windows.Forms.Form)Pvd).Width = 197;
            ((System.Windows.Forms.Form)Pvd).Height = 118;
            ((System.Windows.Forms.Form)Pvd).Icon = this.Icon;
            PDoc.DocumentName = "1" ;
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
            int y2 = 20;
            int x3 = 0;
            int y3 = 100;
            int x4 = 0;
            int y4 = 40;

            //Psd.PageSettings.Landscape = true;
            int nLeft = Psd.PageSettings.Margins.Left;
            int nTop = Psd.PageSettings.Margins.Top;
            arrayList = new ArrayList();


            if (i < dataGridView1.RowCount)
            {

                Image image2;
                ClsPublic.GetBarcode(70, 180, BarcodeLib.TYPE.CODE128, dataGridView1[0, i].Value.ToString(), out image2);
                image = image2;
                arrayList = ClsPublic.getItem1(dataGridView1[3, i].Value.ToString());
                if (j < arrayList.Count)
                {
                    e.Graphics.DrawString(" 申请日期：" + DateTime.Parse(dataGridView1.Rows[i].Cells["申请日期"].Value.ToString()).ToString("yyyy-MM-dd")+"  "+ dataGridView1.Rows[i].Cells["就诊类别"].Value.ToString(), objFont, Brushes.Black, x1, y1);
                    e.Graphics.DrawString(" 姓名:" + dataGridView1.Rows[i].Cells["患者姓名"].Value.ToString()
                        + "  性别:" + dataGridView1.Rows[i].Cells["性别"].Value.ToString()
                        + "  年龄:" + dataGridView1.Rows[i].Cells["年龄"].Value.ToString(), objFont, Brushes.Black, x2, y2);
                  //  e.Graphics.DrawString("" + arrayList[j].ToString(), objFont, Brushes.Black, x3, y3);
                    e.Graphics.DrawImage(image, x4, y4, 180, 50);

                    e.HasMorePages = true;
                    j++;
                    if (j == arrayList.Count)
                    {

                        i++;
                        j = 0;
                    }

                }
                if (i == dataGridView1.RowCount)
                {
                    e.HasMorePages = false;
                    i = 0; j = 0;
                }
            }
            else
            {

            }

            // e.HasMorePages = false;

        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

            PaperSize pkCustomSize = new PaperSize("First Custom Size", 197, 118);
            PDoc2.DefaultPageSettings.PaperSize = pkCustomSize;
            ((System.Windows.Forms.Form)Pvd2).StartPosition = FormStartPosition.CenterScreen;
            ((System.Windows.Forms.Form)Pvd2).Width = 197;
            ((System.Windows.Forms.Form)Pvd2).Height = 118;
            ((System.Windows.Forms.Form)Pvd2).Icon = this.Icon;
            PDoc2.DocumentName = "1";
            
            Pvd2.Document = PDoc2;
            if (DbConn.print != "1")
            {
                Pvd2.ShowDialog();
            }
            else
            {
                PDoc2.Print();
            }

        }

        private void PDoc2_PrintPage(object sender, PrintPageEventArgs e)
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
            int y2 = 20;
            int x3 = 0;
            int y3 = 100;
            int x4 = 0;
            int y4 = 40;
            //Psd.PageSettings.Landscape = true;
            int nLeft = Psd.PageSettings.Margins.Left;
            int nTop = Psd.PageSettings.Margins.Top;
            arrayList = new ArrayList();


            DataGridViewSelectedRowCollection aa = dataGridView1.SelectedRows;

            if (i < aa.Count)
            {

                // i = int_selectRow;
                Image image2;
                ClsPublic.GetBarcode(70, 180, BarcodeLib.TYPE.CODE128, aa[i].Cells[0].Value.ToString(), out image2);
                image = image2;
                arrayList = ClsPublic.getItem1(aa[i].Cells[3].Value.ToString());
                if (j < arrayList.Count)
                {
                    e.Graphics.DrawString(" 申请日期：" + DateTime.Parse(aa[i].Cells[1].Value.ToString()).ToString("yyyy-MM-dd")+"  "+ dataGridView1.Rows[i].Cells["就诊类别"].Value.ToString(), objFont, Brushes.Black, x1, y1);
                    e.Graphics.DrawString(" 姓名:" + aa[i].Cells["患者姓名"].Value.ToString()
                        + "  性别:" + aa[i].Cells["性别"].Value.ToString()
                        + "  年龄:" + aa[i].Cells["年龄"].Value.ToString(), objFont, Brushes.Black, x2, y2);
                   // e.Graphics.DrawString("" + arrayList[j].ToString(), objFont, Brushes.Black, x3, y3);
                    e.Graphics.DrawImage(image, x4, y4, 180, 50);

                    e.HasMorePages = true;
                    j++;
                    if (j == arrayList.Count)
                    {

                        i++;
                        j = 0;

                        // e.HasMorePages = false;
                    }

                }
                if (i == aa.Count)
                {
                    e.HasMorePages = false;
                    i = 0; j = 0;
                }

            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int_selectRow = e.RowIndex;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
