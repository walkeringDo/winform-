using Common.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestDLL
{
    public partial class 事件 : Form
    {
        public delegate void CallBackRecv(Socket socket);
        public static event CallBackRecv CallBackRecvEvent;
        Socket sockt;
        Thread th;
            
        public 事件()
        {
            InitializeComponent();
            sockt = NetHelper.CreateTcpSocket();
            NetHelper.Connect(sockt, "192.168.20.10", 6666);

            th = new Thread(RecvDataFunc);
            th.Start();
            
        }

        private void RecvDataFunc()
        {
            while (true)
            {
                if (CallBackRecvEvent != null)
                    CallBackRecvEvent(sockt);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int x = int.Parse (textBox1.Text);

            byte[] bte = inttobytearray(x);
            NetHelper.SendMsg(sockt, bte);

            Thread.Sleep(10);
            CallBackRecvEvent += new CallBackRecv(ReceiveMsg);

        }

        public void ReceiveMsg(Socket socket) {
            string str = NetHelper.ReceiveMsg(socket);
            textBox2.Invoke(new Action(() => { textBox2.Text = str; }));
        }
        /// <summary>
        /// int转byte数组
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static byte[] inttobytearray(int i)
        {
            byte[] result = new byte[2];
            result[0] = (byte)((i >> 8) & 0xFF);
            result[1] = (byte)(i & 0XFF);
            return result;
        }

        private void 事件_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (th != null)
                th.Abort();
        }
    }
}
