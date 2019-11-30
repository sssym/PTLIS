using System;
using System.Collections.Generic;

using System.Text;

using System.Collections;
using System.Drawing;
using System.IO;

namespace PTLIS
{
    class ClsPublic
    {
       public static ArrayList arrayList;
        public static ArrayList getItem(string s)
        {
            arrayList = new ArrayList();
            string s1 = "";
            int i_index = 0;
            do
            {
                if (s.IndexOf('|') >= 0)
                {
                    if (s.IndexOf('>') > 0)
                    { s1 = s.Substring(0, s.IndexOf('>')); }
                    else
                    {
                        s1 = s;
                    }
                    i_index = s.IndexOf('|') + 1;
                    s = s.Substring(i_index, s.Length - i_index);
                    arrayList.Add(s1);
                }
                else
                {
                    s1 = s.Substring(0, s.IndexOf('>'));
                    arrayList.Add(s1);
                    break;
                }
            } while (true);

            return arrayList;
        }
        public static ArrayList getItem1(string s)
        {
            arrayList = new ArrayList();
            string s1 = "";
            do
            {
               
                    if (s.IndexOf(',') > 0)
                    { s1 = s.Substring(0, s.IndexOf(','));
                    arrayList.Add(s1);
                }
                    else
                    {
                        s1 = s;
                    arrayList.Add(s1);
                    break;
                    }
                s = s.Substring(s.IndexOf(',')+1,s.Length-s.IndexOf(',')-1);
            } while (true);

            return arrayList;
        }
        public static Image GetBarcode(int height, int width, BarcodeLib.TYPE type, string code, out System.Drawing.Image image)
        {
            try
            {
                image = null;
                BarcodeLib.Barcode b = new BarcodeLib.Barcode();
                b.BackColor = System.Drawing.Color.White;//图片背景颜色
                b.ForeColor = System.Drawing.Color.Black;//条码颜色
                b.IncludeLabel = true;
                b.Alignment = BarcodeLib.AlignmentPositions.LEFT;
                b.LabelPosition = BarcodeLib.LabelPositions.BOTTOMLEFT;//.BOTTOMCENTER;
                b.ImageFormat = System.Drawing.Imaging.ImageFormat.Jpeg;//图片格式
                System.Drawing.Font font = new System.Drawing.Font("verdana", 10f);//字体设置
                b.LabelFont = font;
                b.Height = height;//图片高度设置(px单位)
                b.Width = width;//图片宽度设置(px单位)

                image = b.Encode(type, code);//生成图片


            }
            catch (Exception ex)
            {

                image = null;
            }
            return image;
        }

        public static Image getBarCodeImage(string str_no)
        {

            System.Drawing.Image image;
            int width = 148, height = 55;
            string fileSavePath = AppDomain.CurrentDomain.BaseDirectory + "BarcodePattern.jpg";
            if (File.Exists(fileSavePath))
                File.Delete(fileSavePath);
           // GetBarcode(height, width, BarcodeLib.TYPE.CODE128, "111111", out image, fileSavePath);

           return Image.FromFile("BarcodePattern.jpg");
        }

    }
}
