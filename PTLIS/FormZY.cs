using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Data.OracleClient;
using System.Drawing.Printing;
namespace PTLIS
{
    public partial class FormZY : Form
    {
        ArrayList arrayCheckBox = null;
        Image image;
        ArrayList al1 = null;
        ArrayList al2 = null;
        string strsql = "";
        DbConn dbConn = null;
        ArrayList arrayList = null;
        int i = 0;
        int k = 0;
        OracleDataReader oracleDataReader = null;
        OracleDataAdapter oracleDataAdapter = null;
        public FormZY()
        {
            InitializeComponent();
            dbConn = new DbConn();
            
        }
        public ArrayList getChuangwei()
        {
            arrayCheckBox = new ArrayList();
            arrayCheckBox.Add(checkBox0);
            arrayCheckBox.Add(checkBox1);
            arrayCheckBox.Add(checkBox2);
            arrayCheckBox.Add(checkBox3);
            arrayCheckBox.Add(checkBox4);
            arrayCheckBox.Add(checkBox5);
            arrayCheckBox.Add(checkBox6);
            arrayCheckBox.Add(checkBox7);
            arrayCheckBox.Add(checkBox8);
            arrayCheckBox.Add(checkBox9);
            arrayCheckBox.Add(checkBox10);
            arrayCheckBox.Add(checkBox11);
            arrayCheckBox.Add(checkBox12);
            arrayCheckBox.Add(checkBox13);
            arrayCheckBox.Add(checkBox14);
            arrayCheckBox.Add(checkBox15);
            arrayCheckBox.Add(checkBox16);
            arrayCheckBox.Add(checkBox17);
            arrayCheckBox.Add(checkBox18);
            arrayCheckBox.Add(checkBox19);
            arrayCheckBox.Add(checkBox20);
            arrayCheckBox.Add(checkBox21);
            arrayCheckBox.Add(checkBox22);
            arrayCheckBox.Add(checkBox23);
            arrayCheckBox.Add(checkBox24);
            arrayCheckBox.Add(checkBox25);
            arrayCheckBox.Add(checkBox26);
            arrayCheckBox.Add(checkBox27);
            arrayCheckBox.Add(checkBox28);
            arrayCheckBox.Add(checkBox29);
            arrayCheckBox.Add(checkBox30);
            arrayCheckBox.Add(checkBox31);
            arrayCheckBox.Add(checkBox32);
            arrayCheckBox.Add(checkBox33);
            arrayCheckBox.Add(checkBox34);
            arrayCheckBox.Add(checkBox35);
            arrayCheckBox.Add(checkBox36);
            arrayCheckBox.Add(checkBox37);
            arrayCheckBox.Add(checkBox38);
            arrayCheckBox.Add(checkBox39);
            arrayCheckBox.Add(checkBox40);
            arrayCheckBox.Add(checkBox41);
            arrayCheckBox.Add(checkBox42);
            arrayCheckBox.Add(checkBox43);
            arrayCheckBox.Add(checkBox44);
            arrayCheckBox.Add(checkBox45);
            arrayCheckBox.Add(checkBox46);
            arrayCheckBox.Add(checkBox47);
            arrayCheckBox.Add(checkBox48);
            arrayCheckBox.Add(checkBox49);
            arrayCheckBox.Add(checkBox50);
            arrayCheckBox.Add(checkBox51);
            arrayCheckBox.Add(checkBox52);
            arrayCheckBox.Add(checkBox53);
            arrayCheckBox.Add(checkBox54);
            arrayCheckBox.Add(checkBox55);
            arrayCheckBox.Add(checkBox56);
            arrayCheckBox.Add(checkBox57);
            arrayCheckBox.Add(checkBox58);
            arrayCheckBox.Add(checkBox59);
            return arrayCheckBox;
        }
        public ArrayList getZYBR(String str_ksdm)
        {
           
            ArrayList arrayListBR = new ArrayList();
            strsql = "select  t1.code_chr,t3.lastname_vchr "
                +" from t_bse_bed t1 ,t_opr_bih_register t2, t_bse_patient t3,t_bse_deptdesc t4 "
                +" where t3.patientid_chr = t2.patientid_chr and t2.registerid_chr = t1.bihregisterid_chr "
                +" and t4.deptid_chr = t1.nurseunitid_vchr "
                +" and t4.code_vchr = '"+ str_ksdm + "' ";
            oracleDataReader = dbConn.GetDataReader(strsql);
            while (oracleDataReader.HasRows)
            {
                while (oracleDataReader.Read())
                {
                    arrayListBR.Add(oracleDataReader.GetValue(0).ToString()+" "+ oracleDataReader.GetValue(1).ToString());
                }
                oracleDataReader.NextResult();
            }
            return arrayListBR;
        }
        public void setCheckBoxVisible(ArrayList allCheckBox)
        {
            for (int i = 0; i < allCheckBox.Count; i++)
            {
                CheckBox cb = (CheckBox)allCheckBox[i];
                cb.Visible = false;
                cb.Checked = false;
            }

        }
        public string getPatientId(string strCheckBoxText)
        {
            string strCWH = strCheckBoxText.Substring(0, strCheckBoxText.IndexOf(" "));
            strsql = "select t2.patientid_chr from t_bse_bed t1, t_opr_bih_register t2 "
                +" where t1.bihregisterid_chr = t2.registerid_chr and code_chr='"+strCWH+"'";
            oracleDataReader = dbConn.GetDataReader(strsql);
            if (oracleDataReader.Read())
            {
                return oracleDataReader.GetValue(0).ToString();

            }
            else
            {
                return "";
            }
        }
        public ArrayList getCheckedPatientId(ArrayList allShowChechBox)
        {
            ArrayList arrayList = new ArrayList();
            for (int i = 0; i < allShowChechBox.Count; i++)
            {
                CheckBox cb = (CheckBox)allShowChechBox[i];
                if (cb.Checked)
                {
                    arrayList.Add(getPatientId(cb.Text));
                }
                cb.Checked = false;
            }
            return arrayList;
        }
        private void button5_Click(object sender, EventArgs e)
        {
           
                if (LoadData() == null)
                {
                    return;
                }
                k = FrmPrintCount.int_prinCount;
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
        public DataGridView LoadData()
        {
            ArrayList al = getCheckedPatientId(getChuangwei());
            if (al.Count > 0)
            {
                FrmPrintCount f = new FrmPrintCount();
                f.ShowDialog();
                if (FrmPrintCount.int_prinCount == 0) { return null; }
                strsql = "select t1.application_id_chr as 申请单号,"
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
                      + " and t1.patientid_chr in ('";
                for (int i = 0; i < al.Count - 1; i++)
                {
                    strsql = strsql + al[i].ToString() + "','";
                }
                strsql = strsql + al[al.Count - 1].ToString() + "')";
               // strsql = strsql + " group by t1.patient_name_vchr,t1.sex_chr,t1.age_chr,t1.patient_type_id_chr";
                oracleDataAdapter = dbConn.GetDataAdapter(strsql);
                DataSet dataSet = new DataSet();
                oracleDataAdapter.Fill(dataSet,"table1");

                dataGridView1.DataSource = dataSet.Tables["table1"].DefaultView;
                return dataGridView1;
            }
            else
            {
                MessageBox.Show("没有选择待打印数据！");
                return null;
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
                for (; k > 0;)
                {
                    Image image2;
                    ClsPublic.GetBarcode(70, 180, BarcodeLib.TYPE.CODE128, dataGridView1[0, i].Value.ToString(), out image2);
                    image = image2;

                    //arrayList = ClsPublic.getItem1(dataGridView1[3, i].Value.ToString());


                    e.Graphics.DrawString(" "+DateTime.Parse(dataGridView1.Rows[i].Cells["申请日期"].Value.ToString()).ToString("MM-dd") 
                        + "/" + dataGridView1.Rows[i].Cells["科室"].Value.ToString()
                        +"/" + dataGridView1.Rows[i].Cells["床位号"].Value.ToString()
                        +"/"+ dataGridView1.Rows[i].Cells["住院号"].Value.ToString(), objFont, Brushes.Black, x1, y1);
                    e.Graphics.DrawString(" 姓名:" + dataGridView1.Rows[i].Cells["患者姓名"].Value.ToString()
                        + "  性别:" + dataGridView1.Rows[i].Cells["性别"].Value.ToString()
                        + "  年龄:" + dataGridView1.Rows[i].Cells["年龄"].Value.ToString(), objFont, Brushes.Black, x2, y2);
                    // e.Graphics.DrawString("" + arrayList[j].ToString(), objFont, Brushes.Black, x3, y3);
                    e.Graphics.DrawImage(image, x4, y4, 180, 50);
                 
                    e.Graphics.DrawString(" "+ dataGridView1.Rows[i].Cells["检查项目"].Value.ToString(), objFont, Brushes.Black, x5, y5);


                    e.HasMorePages = true;
                    k--;
                    if (k == 0)
                    {
                        i++; if (i == dataGridView1.RowCount)
                        {
                            e.HasMorePages = false;
                            i = 0;
                        }
                        else
                        {
                            k = FrmPrintCount.int_prinCount;
                        }
                        break;
                    }
                    else { break; }




                }
            }
            else
            {

            }
          
            // e.HasMorePages = false;

        }

        private void FormZY_Load(object sender, EventArgs e)
        {
            button4_Click(sender,e);
            strsql = "select deptname_vchr from t_bse_deptdesc "
                +"where code_vchr = '"+DbConn.ksdm+"' ";
            oracleDataReader = dbConn.GetDataReader(strsql);
            if(oracleDataReader.Read())
            {

                toolStripStatusLabel1.Text = "使用科室："+oracleDataReader.GetValue(0).ToString();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            al1 = getChuangwei();
            setCheckBoxVisible(al1);
            al2 = getZYBR(DbConn.ksdm);
            for (int i = 0; i < al2.Count; i++)
            {
                CheckBox cb = (CheckBox)al1[i];
                cb.Text = al2[i].ToString();
                cb.Visible = true;
            }
        }
    }
}
