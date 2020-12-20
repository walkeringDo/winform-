using Common.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestDLL
{
    public partial class 二维码 : Form
    {
        public 二维码()
        {
            InitializeComponent();
            //pictureBox1.SizeMode = PictureBoxSizeMode .Zoom ;
        }

        QRcode qrcode = new QRcode();


        private void button1_Click(object sender, EventArgs e)
        {
            //string str = textBox1.Text;
            ////string strpath = QRcode.Create(str, 5, System.Windows.Forms.Application.StartupPath + "\\");

            //string[] strs = strpath.Split('\\');

            //int count = strs.Count();

            //string strph = strs[count-1];
            ////6b9d1b5c-06d9-48be-974a-d41ba1bc3360.jpg 3f03684e-826a-43f3-b882-e6319b0cc7f7.jpg
            //pictureBox1.BackgroundImage = Image.FromFile(strph);
        }
    }
}
