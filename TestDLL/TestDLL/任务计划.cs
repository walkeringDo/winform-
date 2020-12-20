using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;


namespace TestDLL
{
    public partial class 任务计划 : Form
    {
        public 任务计划()
        {
            InitializeComponent();
        }
        public class TimerJob : IJob
        {
            /// <summary>
            /// 定义执行任务:写程序
            /// </summary>
            /// <param name="context"></param>
            public void Execute(IJobExecutionContext context)
            {
                System.IO.File.AppendAllText(@"D:\Quartz.txt", DateTime.Now + Environment.NewLine);
            }
        }
        /// <summary>
        /// 开始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            //AreaRegistration.RegisterAllAreas();


            //创建调度器
            CreatScheduler();
            //创建任务
            CreatTask();
            //创建触发器
            CreatITrigger();
        }


        //调度器
        IScheduler scheduler;
        //调度器工厂
        ISchedulerFactory factory;
        //任务
        IJobDetail job;
        //触发器
        ITrigger trigger;

        /// <summary>
        /// 创建调度器
        /// </summary>
        public void CreatScheduler()
        {
            //创建一个调度器
            factory = new StdSchedulerFactory();
            scheduler = factory.GetScheduler();

            scheduler.Start();
        }

        /// <summary>
        /// 创建任务
        /// </summary>
        public void CreatTask()
        {
            job = JobBuilder.Create<TimerJob>().WithIdentity("job1", "group1").Build();
        }
        /// <summary>
        /// 创建触发器
        /// </summary>
        public void CreatITrigger()
        {
            trigger = TriggerBuilder.Create().WithIdentity("trigger1","group1").WithCronSchedule ("0/5 * * * * ?").Build ();//5秒执行一次
            //将任务与触发器添加到调度器中
            scheduler.ScheduleJob(job, trigger);
        }
        /// <summary>
        /// 关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 任务计划_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (scheduler != null)
            {
                scheduler.Shutdown(true);
            }
        }
    }
}
