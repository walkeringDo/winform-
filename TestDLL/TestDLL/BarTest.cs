using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestDLL
{
    public partial class BarTest : Form
    {
        public int current = 0;
        public int max = 100;
        public BarTest()
        {
            InitializeComponent();

            progressBar1.Maximum = max;
            for (int i = 0; i < max; i++)
            {
                progressBar1.Value += 1;
                progressBar1.Update();
                Application.DoEvents();
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Update();

            for (int i = 0; i < max; i++)
            {
                progressBar1.Value += 1;
                progressBar1.Update();
                Application.DoEvents();
            }
        }
    }
}
