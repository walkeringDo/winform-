using Common.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestDLL
{
    public partial class UdpForm : Form
    {
        public UdpForm()
        {
            InitializeComponent();
        }

        Socket udpsocket = NetHelper.CreateUdpSocket();

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] buffer = new byte[200];

            //NetHelper.StartListen(udpsocket, textBox1.Text, int.Parse(textBox2.Text), 100);

            string srt = NetHelper.ReceiveMsg(udpsocket);

            MessageBox .Show ( IpHelper.GetUserIp());
        }

    }
}
