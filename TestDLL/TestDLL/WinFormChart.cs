using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace TestDLL
{
    public partial class WinFormChart : Form
    {
        Thread th;
        DataTable dt;
        public WinFormChart()
        {
            InitializeComponent();

            //dt = Common.Utility.ExcelHelper.InputFromExcel("C:\\Users\\Administrator\\Desktop\\一楼三维扫描架天线系统数据\\一楼三维扫描架天线系统数据0518\\20200518161854象限1,2,3,4.xls", "Sheet0");
            ////曲线初始化
            //InitChart(chart1);
            //timer1.Interval = 1000;
            //timer1.Start();

            //InitChartThread(chart2);
            //th = new Thread(drawchart2);//new th(drawchart2);
            //th.Start();

            List<int> listX = new List<int>();
            List<double[]> listY = new List<double[]>();
            double[,] tmp = new double[1024, 5];
            double[,] tmpXW = new double[1024, 5];
            int tmplistX = 0;
            double[] tmplistY = new double[5];
            double[] tmplistYXW = new double[5];

            bool sta = IsExistFile("D:\\一楼三维扫描架天线系统数据\\最新1,2,3,4.xls");
            if (!sta)
            {
                MessageBox.Show("文件不存在.");
                return;
            }
            //else
            //{
            //    File.Delete("D:\\一楼三维扫描架天线系统数据\\最新1,2,3,4.xls");
            //}
            dt = InputFromExcel("D:\\一楼三维扫描架天线系统数据\\最新1,2,3,4.xls", "Sheet0");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                tmplistX = i + 1;
                tmplistY[0] = Convert.ToDouble(dt.Rows[i][10]);
                tmplistY[1] = Convert.ToDouble(dt.Rows[i][12]);
                tmplistY[2] = Convert.ToDouble(dt.Rows[i][14]);
                tmplistY[3] = Convert.ToDouble(dt.Rows[i][16]);
                tmplistY[4] = Convert.ToDouble(dt.Rows[i][18]);

                tmp[i, 0] = tmplistY[0];
                tmp[i, 1] = tmplistY[1];
                tmp[i, 2] = tmplistY[2];
                tmp[i, 3] = tmplistY[3];
                tmp[i, 4] = tmplistY[4];

                listX.Add(tmplistX);
                //listY.Add(tmplistY);

                tmplistYXW[0] = Convert.ToDouble(dt.Rows[i][11]);
                tmplistYXW[1] = Convert.ToDouble(dt.Rows[i][13]);
                tmplistYXW[2] = Convert.ToDouble(dt.Rows[i][15]);
                tmplistYXW[3] = Convert.ToDouble(dt.Rows[i][17]);
                tmplistYXW[4] = Convert.ToDouble(dt.Rows[i][19]);

                tmpXW[i, 0] = tmplistYXW[0];
                tmpXW[i, 1] = tmplistYXW[1];
                tmpXW[i, 2] = tmplistYXW[2];
                tmpXW[i, 3] = tmplistYXW[3];
                tmpXW[i, 4] = tmplistYXW[4];
            }
            DrawSpline(listX, chart1, tmp);
            DrawSplineXW(listX, chart2, tmpXW);
            
        }
        /// <summary>
        /// 获取Excel文件数据表列表
        /// </summary>
        public static ArrayList GetExcelTables(string ExcelFileName)
        {
            DataTable dt = new DataTable();
            ArrayList TablesList = new ArrayList();
            if (File.Exists(ExcelFileName))
            {
                using (OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Extended Properties=Excel 8.0;Data Source=" + ExcelFileName))
                {
                    try
                    {
                        conn.Open();
                        dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                    }
                    catch (Exception exp)
                    {
                        throw exp;
                    }

                    //获取数据表个数
                    int tablecount = dt.Rows.Count;
                    for (int i = 0; i < tablecount; i++)
                    {
                        string tablename = dt.Rows[i][2].ToString().Trim().TrimEnd('$');
                        if (TablesList.IndexOf(tablename) < 0)
                        {
                            TablesList.Add(tablename);
                        }
                    }
                }
            }
            return TablesList;
        }
        /// <summary>
        /// 将Excel文件导出至DataTable(第一行作为表头)
        /// </summary>
        /// <param name="ExcelFilePath">Excel文件路径</param>
        /// <param name="TableName">数据表名，如果数据表名错误，默认为第一个数据表名</param>
        public static DataTable InputFromExcel(string ExcelFilePath, string TableName)
        {
            if (!File.Exists(ExcelFilePath))
            {
                throw new Exception("Excel文件不存在！");
            }

            //如果数据表名不存在，则数据表名为Excel文件的第一个数据表
            ArrayList TableList = new ArrayList();
            TableList = GetExcelTables(ExcelFilePath);

            if (TableName.IndexOf(TableName) < 0)
            {
                TableName = TableList[0].ToString().Trim();
            }

            DataTable table = new DataTable();
            OleDbConnection dbcon = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ExcelFilePath + ";Extended Properties=Excel 8.0");
            OleDbCommand cmd = new OleDbCommand("select * from [" + TableName + "$]", dbcon);
            OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);

            try
            {
                if (dbcon.State == ConnectionState.Closed)
                {
                    dbcon.Open();
                }
                adapter.Fill(table);
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (dbcon.State == ConnectionState.Open)
                {
                    dbcon.Close();
                }
            }
            return table;
        }
        /// <summary>
        /// 检测指定文件是否存在,如果存在则返回true。
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>        
        public static bool IsExistFile(string filePath)
        {
            return File.Exists(filePath);
        }
        public void drawchart2()
        {
            for (int i = 1; i <= dt.Rows.Count; i++)
            {
                double data1 = Convert.ToDouble(dt.Rows[i-1][10]);

                double data2 = Convert.ToDouble(dt.Rows[i-1][12]);

                double data3 = Convert.ToDouble(dt.Rows[i-1][14]);

                double data4 = Convert.ToDouble(dt.Rows[i-1][16]);

                double data5 = Convert.ToDouble(dt.Rows[i-1][18]);

                ShowChartThread(this.chart2, data1, data2, data3, data4, data5, i);
                Thread.Sleep(10);
            }
            th.Abort();
        }
        /// <summary>
        /// 初始化chart控件
        /// </summary>
        private void InitChart(Chart chart1)
        {
            chart1.Series[0].IsValueShownAsLabel = true;
            chart1.Series[0].SmartLabelStyle.Enabled = true;
            chart1.Series[0].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;

            chart1.Series[1].IsValueShownAsLabel = true;
            chart1.Series[1].SmartLabelStyle.Enabled = true;
            chart1.Series[1].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;

            chart1.Series[2].IsValueShownAsLabel = true;
            chart1.Series[2].SmartLabelStyle.Enabled = true;
            chart1.Series[2].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;

            chart1.ChartAreas[0].AxisX.ScrollBar.ButtonStyle = System.Windows.Forms.DataVisualization.Charting.ScrollBarButtonStyles.None;
            chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = false;
            chart1.ChartAreas[0].AxisX.ScrollBar.Size = 20;
            chart1.ChartAreas[0].AxisX.ScaleView.MinSizeType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Seconds;
            chart1.ChartAreas[0].AxisX.ScaleView.SizeType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Seconds;
            chart1.ChartAreas[0].AxisX.ScaleView.Size = 20;
            chart1.ChartAreas[0].AxisX.ScaleView.MinSize = 15;
            chart1.ChartAreas[0].AxisX.ScaleView.SmallScrollMinSize = 1;
            chart1.ChartAreas[0].AxisX.ScaleView.SmallScrollMinSizeType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Seconds;
            chart1.ChartAreas[0].AxisX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Seconds;

            chart1.ChartAreas[0].AxisX.Interval = DateTime.Parse("00:00:01").Second;
            chart1.ChartAreas[0].AxisX.TitleAlignment = StringAlignment.Near;
            chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = true;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineWidth = 1;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightGray;

            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss";
            chart1.ChartAreas[0].AxisY.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;

            chart1.Series[0].ChartType = SeriesChartType.Spline;
            chart1.Series[1].ChartType = SeriesChartType.Spline;
            chart1.Series[2].ChartType = SeriesChartType.Spline;
        }
        /// <summary>
        /// 初始化chart控件Thread
        /// </summary>
        private void InitChartThread(Chart chart1)
        {
            chart1.Series[0].IsValueShownAsLabel = true;
            chart1.Series[0].SmartLabelStyle.Enabled = true;
            chart1.Series[0].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;

            chart1.Series[1].IsValueShownAsLabel = true;
            chart1.Series[1].SmartLabelStyle.Enabled = true;
            chart1.Series[1].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;

            chart1.Series[2].IsValueShownAsLabel = true;
            chart1.Series[2].SmartLabelStyle.Enabled = true;
            chart1.Series[2].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;

            chart1.Series[3].IsValueShownAsLabel = true;
            chart1.Series[3].SmartLabelStyle.Enabled = true;
            chart1.Series[3].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;

            chart1.Series[4].IsValueShownAsLabel = true;
            chart1.Series[4].SmartLabelStyle.Enabled = true;
            chart1.Series[4].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;

            chart1.ChartAreas[0].AxisX.ScrollBar.ButtonStyle = System.Windows.Forms.DataVisualization.Charting.ScrollBarButtonStyles.None;
            chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = false;
            chart1.ChartAreas[0].AxisX.ScrollBar.Size = 20;
            chart1.ChartAreas[0].AxisX.ScaleView.MinSizeType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Seconds;
            chart1.ChartAreas[0].AxisX.ScaleView.SizeType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Seconds;
            chart1.ChartAreas[0].AxisX.ScaleView.Size = 20;
            chart1.ChartAreas[0].AxisX.ScaleView.MinSize = 15;
            chart1.ChartAreas[0].AxisX.ScaleView.SmallScrollMinSize = 1;
            chart1.ChartAreas[0].AxisX.ScaleView.SmallScrollMinSizeType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Seconds;
            chart1.ChartAreas[0].AxisX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Seconds;

            chart1.ChartAreas[0].AxisX.Interval = DateTime.Parse("00:00:01").Second;
            chart1.ChartAreas[0].AxisX.TitleAlignment = StringAlignment.Near;
            chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = true;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineWidth = 3;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightGray;

            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss";
            chart1.ChartAreas[0].AxisY.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;

            chart1.Series[0].ChartType = SeriesChartType.Spline;
            chart1.Series[1].ChartType = SeriesChartType.Spline;
            chart1.Series[2].ChartType = SeriesChartType.Spline;
            chart1.Series[3].ChartType = SeriesChartType.Spline;
            chart1.Series[4].ChartType = SeriesChartType.Spline;
        }
        /// <summary>
        /// 显示数据到chart控件
        /// </summary>
        /// <param name="chart1"></param>
        /// <param name="data1"></param>
        /// <param name="data2"></param>
        /// <param name="data3"></param>
        private void ShowChart(Chart chart1, double data1, double data2, double data3)
        {
            chart1.ChartAreas[0].AxisX.ScaleView.Scroll(ScrollType.Last);

            string now = DateTime.Now.ToLongTimeString();


            DateTime time = DateTime.Parse(now);

            chart1.Series[0].Points.AddXY(time, data1);//DateTime.Now.Second

            chart1.Series[1].Points.AddXY(time, data2);//DateTime.Now.Second

            chart1.Series[2].Points.AddXY(time, data3);//DateTime.Now.Second
        }
        private void ShowChartThread(Chart chart1, double data1, double data2, double data3,double data4,double data5,int flag)
        {
            //chart1.ChartAreas[0].AxisX.ScaleView.Scroll(ScrollType.Last);

            //string now = DateTime.Now.ToLongTimeString();


            //DateTime time = DateTime.Parse(now);
            chart1.Invoke(new Action(() => { chart1.Series[0].Points.AddXY(flag, data1); }));

            chart1.Invoke(new Action(() => { chart1.Series[1].Points.AddXY(flag, data2); }));

            chart1.Invoke(new Action(() => { chart1.Series[2].Points.AddXY(flag, data3); }));

            chart1.Invoke(new Action(() => { chart1.Series[3].Points.AddXY(flag, data4); }));

            chart1.Invoke(new Action(() => { chart1.Series[4].Points.AddXY(flag, data5); }));
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            double data1 = 1;

            double data2 = 2;

            double data3 = 3;

            ShowChart(this.chart1, data1, data2, data3);
        }



        #region 绘制曲线函数
        /// <summary>
        /// 绘制曲线函数 幅度
        /// </summary>
        /// <param name="listX">X值集合</param>
        /// <param name="listY">Y值集合</param>
        /// <param name="chart">Chart控件</param>
        public static void DrawSpline(List<int> listX, Chart chart, double[,] tmp)
        {
            try
            {
                List<double> listY0 = new List<double>();
                List<double> listY1 = new List<double>();
                List<double> listY2 = new List<double>();
                List<double> listY3 = new List<double>();
                List<double> listY4 = new List<double>();
                for (int i = 0; i < 1024; i++)
                {
                    listY0.Add(tmp[i, 0]);
                    listY1.Add(tmp[i, 1]);
                    listY2.Add(tmp[i, 2]);
                    listY3.Add(tmp[i, 3]);
                    listY4.Add(tmp[i, 4]);
                }

                //频点 1
                //X、Y值成员
                chart.Series[0].Points.DataBindXY(listX, listY0);
                chart.Series[0].Points.DataBindY(listY0);

                //点颜色
                //chart.Series[0].MarkerColor = Color.Green;
                //图表类型  设置为样条图曲线
                chart.Series[0].ChartType = SeriesChartType.Line;
                //设置点的大小
                chart.Series[0].MarkerSize = 5;
                //设置曲线的颜色
                // chart.Series[0].Color = Color.Orange;
                //设置曲线宽度
                chart.Series[0].BorderWidth = 2;
                //chart.Series[0].CustomProperties = "PointWidth=4";
                //设置是否显示坐标标注
                chart.Series[0].IsValueShownAsLabel = false;

                //频点 2
                //X、Y值成员
                chart.Series[1].Points.DataBindXY(listX, listY1);
                chart.Series[1].Points.DataBindY(listY1);

                //点颜色
                chart.Series[1].MarkerColor = Color.Green;
                //图表类型  设置为样条图曲线
                chart.Series[1].ChartType = SeriesChartType.Line;
                //设置点的大小
                chart.Series[1].MarkerSize = 5;
                //设置曲线的颜色
                // chart.Series[0].Color = Color.Orange;
                //设置曲线宽度
                chart.Series[1].BorderWidth = 2;
                //chart.Series[0].CustomProperties = "PointWidth=4";
                //设置是否显示坐标标注
                chart.Series[1].IsValueShownAsLabel = false;

                //频点 3
                //X、Y值成员
                chart.Series[2].Points.DataBindXY(listX, listY2);
                chart.Series[2].Points.DataBindY(listY2);

                //点颜色
                chart.Series[2].MarkerColor = Color.Green;
                //图表类型  设置为样条图曲线
                chart.Series[2].ChartType = SeriesChartType.Line;
                //设置点的大小
                chart.Series[2].MarkerSize = 5;
                //设置曲线的颜色
                // chart.Series[0].Color = Color.Orange;
                //设置曲线宽度
                chart.Series[2].BorderWidth = 2;
                //chart.Series[0].CustomProperties = "PointWidth=4";
                //设置是否显示坐标标注
                chart.Series[2].IsValueShownAsLabel = false;

                //频点 4
                //X、Y值成员
                chart.Series[3].Points.DataBindXY(listX, listY3);
                chart.Series[3].Points.DataBindY(listY3);

                //点颜色
                chart.Series[3].MarkerColor = Color.Green;
                //图表类型  设置为样条图曲线
                chart.Series[3].ChartType = SeriesChartType.Line;
                //设置点的大小
                chart.Series[3].MarkerSize = 5;
                //设置曲线的颜色
                // chart.Series[0].Color = Color.Orange;
                //设置曲线宽度
                chart.Series[3].BorderWidth = 2;
                //chart.Series[0].CustomProperties = "PointWidth=4";
                //设置是否显示坐标标注
                chart.Series[3].IsValueShownAsLabel = false;

                //频点 5 
                //X、Y值成员
                chart.Series[4].Points.DataBindXY(listX, listY4);
                chart.Series[4].Points.DataBindY(listY4);

                //点颜色
                chart.Series[4].MarkerColor = Color.Green;
                //图表类型  设置为样条图曲线
                chart.Series[4].ChartType = SeriesChartType.Line;
                //设置点的大小
                chart.Series[4].MarkerSize = 5;
                //设置曲线的颜色
                // chart.Series[0].Color = Color.Orange;
                //设置曲线宽度
                chart.Series[4].BorderWidth = 2;
                //chart.Series[0].CustomProperties = "PointWidth=4";
                //设置是否显示坐标标注
                chart.Series[4].IsValueShownAsLabel = false;

                //设置游标
                chart.ChartAreas[0].CursorX.IsUserEnabled = true;
                chart.ChartAreas[0].CursorX.AutoScroll = true;
                chart.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
                //设置X轴是否可以缩放
                chart.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
                //将滚动条放到图表外
                chart.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = false;
                // 设置滚动条的大小
                chart.ChartAreas[0].AxisX.ScrollBar.Size = 15;
                // 设置滚动条的按钮的风格，下面代码是将所有滚动条上的按钮都显示出来
                chart.ChartAreas[0].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.All;
                chart.ChartAreas[0].AxisX.ScrollBar.ButtonColor = Color.SkyBlue;
                // 设置自动放大与缩小的最小量
                chart.ChartAreas[0].AxisX.ScaleView.SmallScrollSize = double.NaN;
                chart.ChartAreas[0].AxisX.ScaleView.SmallScrollMinSize = 1;
                //设置刻度间隔
                chart.ChartAreas[0].AxisX.Interval = 1;
                //将X轴上格网取消
                chart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                //X轴、Y轴标题
                chart.ChartAreas[0].AxisX.Title = "点位";
                chart.ChartAreas[0].AxisY.Title = "幅度";
                //设置Y轴范围  可以根据实际情况重新修改
                double max = -20;//listY[0];
                double min = -100;//listY[0];
                //foreach (var yValue in listY[0])
                //{
                //    if (max < yValue)
                //    {
                //        max = yValue;
                //    }
                //    if (min > yValue)
                //    {
                //        min = yValue;
                //    }
                //}
                chart.ChartAreas[0].AxisY.Maximum = max;
                chart.ChartAreas[0].AxisY.Minimum = min;
                chart.ChartAreas[0].AxisY.Interval = (max - min) / 10;
                //绑定数据源
                chart.DataBind();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString());
            }
        }

        /// <summary>
        /// 绘制曲线函数 相位
        /// </summary>
        /// <param name="listX">X值集合</param>
        /// <param name="listY">Y值集合</param>
        /// <param name="chart">Chart控件</param>
        public static void DrawSplineXW(List<int> listX, Chart chart, double[,] tmp)
        {
            try
            {
                List<double> listY0 = new List<double>();
                List<double> listY1 = new List<double>();
                List<double> listY2 = new List<double>();
                List<double> listY3 = new List<double>();
                List<double> listY4 = new List<double>();
                for (int i = 0; i < 1024; i++)
                {
                    listY0.Add(tmp[i, 0]);
                    listY1.Add(tmp[i, 1]);
                    listY2.Add(tmp[i, 2]);
                    listY3.Add(tmp[i, 3]);
                    listY4.Add(tmp[i, 4]);
                }

                //相位 1
                //X、Y值成员
                chart.Series[0].Points.DataBindXY(listX, listY0);
                chart.Series[0].Points.DataBindY(listY0);

                //点颜色
                //chart.Series[0].MarkerColor = Color.Green;
                //图表类型  设置为样条图曲线
                chart.Series[0].ChartType = SeriesChartType.Line;
                //设置点的大小
                chart.Series[0].MarkerSize = 5;
                //设置曲线的颜色
                // chart.Series[0].Color = Color.Orange;
                //设置曲线宽度
                chart.Series[0].BorderWidth = 2;
                //chart.Series[0].CustomProperties = "PointWidth=4";
                //设置是否显示坐标标注
                chart.Series[0].IsValueShownAsLabel = false;

                //相位 2
                //X、Y值成员
                chart.Series[1].Points.DataBindXY(listX, listY1);
                chart.Series[1].Points.DataBindY(listY1);

                //点颜色
                chart.Series[1].MarkerColor = Color.Green;
                //图表类型  设置为样条图曲线
                chart.Series[1].ChartType = SeriesChartType.Line;
                //设置点的大小
                chart.Series[1].MarkerSize = 5;
                //设置曲线的颜色
                // chart.Series[0].Color = Color.Orange;
                //设置曲线宽度
                chart.Series[1].BorderWidth = 2;
                //chart.Series[0].CustomProperties = "PointWidth=4";
                //设置是否显示坐标标注
                chart.Series[1].IsValueShownAsLabel = false;

                //相位 3
                //X、Y值成员
                chart.Series[2].Points.DataBindXY(listX, listY2);
                chart.Series[2].Points.DataBindY(listY2);

                //点颜色
                chart.Series[2].MarkerColor = Color.Green;
                //图表类型  设置为样条图曲线
                chart.Series[2].ChartType = SeriesChartType.Line;
                //设置点的大小
                chart.Series[2].MarkerSize = 5;
                //设置曲线的颜色
                // chart.Series[0].Color = Color.Orange;
                //设置曲线宽度
                chart.Series[2].BorderWidth = 2;
                //chart.Series[0].CustomProperties = "PointWidth=4";
                //设置是否显示坐标标注
                chart.Series[2].IsValueShownAsLabel = false;

                //相位 4
                //X、Y值成员
                chart.Series[3].Points.DataBindXY(listX, listY3);
                chart.Series[3].Points.DataBindY(listY3);

                //点颜色
                chart.Series[3].MarkerColor = Color.Green;
                //图表类型  设置为样条图曲线
                chart.Series[3].ChartType = SeriesChartType.Line;
                //设置点的大小
                chart.Series[3].MarkerSize = 5;
                //设置曲线的颜色
                // chart.Series[0].Color = Color.Orange;
                //设置曲线宽度
                chart.Series[3].BorderWidth = 2;
                //chart.Series[0].CustomProperties = "PointWidth=4";
                //设置是否显示坐标标注
                chart.Series[3].IsValueShownAsLabel = false;

                //相位 5 
                //X、Y值成员
                chart.Series[4].Points.DataBindXY(listX, listY4);
                chart.Series[4].Points.DataBindY(listY4);

                //点颜色
                chart.Series[4].MarkerColor = Color.Green;
                //图表类型  设置为样条图曲线
                chart.Series[4].ChartType = SeriesChartType.Line;
                //设置点的大小
                chart.Series[4].MarkerSize = 5;
                //设置曲线的颜色
                // chart.Series[0].Color = Color.Orange;
                //设置曲线宽度
                chart.Series[4].BorderWidth = 2;
                //chart.Series[0].CustomProperties = "PointWidth=4";
                //设置是否显示坐标标注
                chart.Series[4].IsValueShownAsLabel = false;

                //设置游标
                chart.ChartAreas[0].CursorX.IsUserEnabled = true;
                chart.ChartAreas[0].CursorX.AutoScroll = true;
                chart.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
                //设置X轴是否可以缩放
                chart.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
                //将滚动条放到图表外
                chart.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = false;
                // 设置滚动条的大小
                chart.ChartAreas[0].AxisX.ScrollBar.Size = 15;
                // 设置滚动条的按钮的风格，下面代码是将所有滚动条上的按钮都显示出来
                chart.ChartAreas[0].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.All;
                chart.ChartAreas[0].AxisX.ScrollBar.ButtonColor = Color.SkyBlue;
                // 设置自动放大与缩小的最小量
                chart.ChartAreas[0].AxisX.ScaleView.SmallScrollSize = double.NaN;
                chart.ChartAreas[0].AxisX.ScaleView.SmallScrollMinSize = 1;
                //设置刻度间隔
                chart.ChartAreas[0].AxisX.Interval = 1;
                //将X轴上格网取消
                chart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                //X轴、Y轴标题
                chart.ChartAreas[0].AxisX.Title = "点位";
                chart.ChartAreas[0].AxisY.Title = "相位";
                //设置Y轴范围  可以根据实际情况重新修改
                double max = 200;//listY[0];
                double min = -200;//listY[0];
                //foreach (var yValue in listY[0])
                //{
                //    if (max < yValue)
                //    {
                //        max = yValue;
                //    }
                //    if (min > yValue)
                //    {
                //        min = yValue;
                //    }
                //}
                chart.ChartAreas[0].AxisY.Maximum = max;
                chart.ChartAreas[0].AxisY.Minimum = min;
                chart.ChartAreas[0].AxisY.Interval = (max - min) / 10;
                //绑定数据源
                chart.DataBind();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString());
            }
        }
        #endregion

    }
}
