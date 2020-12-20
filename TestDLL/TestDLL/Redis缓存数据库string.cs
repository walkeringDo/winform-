using ServiceStack.Redis;
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
    public partial class Redis缓存数据库string : Form
    {
        public Redis缓存数据库string()
        {
            InitializeComponent();
        }
        //首先new 一个客户端
        RedisClient client = new RedisClient("localhost", 6379);

        private void Redis缓存数据库string_Load(object sender, EventArgs e)
        {
            
        }
        /// <summary>
        /// 存储数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetBtn_Click(object sender, EventArgs e)
        {
            //缓存字符串类型
            bool sta = client.Set(textBox1.Text, textBox2.Text);
            if(sta)
                MessageBox.Show("存储成功!");
            else
                MessageBox.Show("存储失败!");
           
        }

        private void GetBtn_Click(object sender, EventArgs e)
        {
            string userName = Encoding.ASCII.GetString(client.Get(textBox1.Text));//Encoding.ASCII.GetString(data)

            MessageBox.Show(userName);
        }
    }
}
