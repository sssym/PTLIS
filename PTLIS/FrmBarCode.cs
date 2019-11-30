using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;

using System.Windows.Forms;
using System.IO;
namespace PTLIS
{
    public partial class FrmBarCode : Form
    {
        public FrmBarCode()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            System.Drawing.Image image;
            int width = 148, height = 55;
            string fileSavePath = AppDomain.CurrentDomain.BaseDirectory + "BarcodePattern.jpg";
            if (File.Exists(fileSavePath))
                File.Delete(fileSavePath);
          //ClsPublic.GetBarcode(height, width, BarcodeLib.TYPE.CODE128, "111111", out image, fileSavePath);

            pictureBox1.Image = Image.FromFile("BarcodePattern.jpg");
        }
       
    }
}
