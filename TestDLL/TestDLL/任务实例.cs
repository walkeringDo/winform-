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
    public partial class 任务实例 : Form
    {
        public 任务实例()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //TaskDo();
            //FactoryTaskDo();
            ConTaskDo();
        }
        void Download(object obj)
        {
            textBox1.Invoke(new Action(() => { textBox1.Text = "Download Begin ID " + Thread.CurrentThread.ManagedThreadId + " " + obj + " \r"; }));

            Thread.Sleep(1000);

            textBox1.Invoke(new Action(() => { textBox1.Text += "Download End \r"; }));
        }
        void ConDownload(Task task)
        {
            Thread.Sleep(1000);
            textBox1.Invoke(new Action(() => { textBox1.Text += "read news"; }));
        }
        /// <summary>
        /// 创建task
        /// </summary>
        void TaskDo()
        {
            //创建任务
            Task task = new Task(Download, "孔夫子 \r");
            //启动任务
            task.Start();

            textBox1.Invoke(new Action(() => { textBox1.Text += "123"; }));
        }

        /// <summary>
        /// 创建任务工厂
        /// </summary>
        void FactoryTaskDo()
        { 
            //创建任务工厂
            TaskFactory taskfactory = new TaskFactory();

            //开启新的任务
            taskfactory.StartNew(Download ,"孔夫子");
        }

        /// <summary>
        /// 连续任务1
        /// </summary>
        void ConTaskDo()
        {
            Task task = new Task(Download ,"孔夫子");
            Task task2 = task.ContinueWith(ConDownload); 
            task.Start();
        }

        //void ConTaskFactoryDo()
        //{
        //    TaskFactory taskfactory = new TaskFactory();

        //    Task task = new Task(Download, "孔夫子");

        //    //taskfactory.StartNew(Download, "孔夫子");
        //    CancellationToken ct = task.AsyncState;
        //    taskfactory.StartNew(task, (CancellationToken)task.AsyncState);
        //}
    }
}
