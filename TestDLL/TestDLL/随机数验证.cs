using HD.Helper.Common;
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
    public partial class 随机数验证 : Form
    {
        public static string str = string.Empty;
        public 随机数验证()
        {
            InitializeComponent();
            str = VerifyCodeRand.Str(5, false);
            label1.Text = str;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == label1.Text)
                MessageBox.Show("OK");
            else
                MessageBox.Show("Bad");

            str = VerifyCodeRand.Str(5, false);
            label1.Text = str;
        }
    }
}
