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
    public partial class Redis缓存数据库 : Form
    {
        public Redis缓存数据库()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Redis缓存数据库string stringForm = new Redis缓存数据库string();
            stringForm.ShowDialog();
        }
        
    }
}
