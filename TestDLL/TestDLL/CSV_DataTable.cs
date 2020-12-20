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
    public partial class CSV_DataTable : Form
    {
        public  DataTable AddDataXKZ = new DataTable();
        public CSV_DataTable()
        {
            InitializeComponent();
            CreadDatatableXKZ(AddDataXKZ);
        }
        /// <summary>
        /// 创建datatable:相控阵
        /// </summary>
        /// <param name="dt"></param>
        public void CreadDatatableXKZ(DataTable dt)
        {
            dt.Columns.Add(new DataColumn("象限", Type.GetType("System.Int32")));
            dt.Columns.Add(new DataColumn("组件号", Type.GetType("System.Int32")));
            dt.Columns.Add(new DataColumn("通道号", Type.GetType("System.Int32")));
            dt.Columns.Add(new DataColumn("衰减", Type.GetType("System.Double")));
            dt.Columns.Add(new DataColumn("相位", Type.GetType("System.Double")));

        }
         /// <summary>
        /// 传入的double数组 扫描架
        /// </summary>
        /// <param name="i"></param>
        /// <param name="Xcontrol"></param>
        /// <param name="Ycontrol"></param>
        /// <param name="Zcontrol"></param>
        /// <param name="Ucontrol"></param>
        /// <returns></returns>
        public double[] AddDataXKZToDoubles(Int64 i, double A, double B)
        {
            double[] str = new double[6];
            //象限1
            if (i <= 256)
            {
                //象限
                str[0] = 1;
                if (i % 16 != 0)
                {
                    str[1] = i / 16 + 1;
                }
                else
                {
                    str[1] = i / 16;
                }
                str[1] = str[1] % 16;
                if (str[1] % 16 == 0)
                    str[1] = 16;

                if (i % 16 == 0)
                {
                    str[2] = 16;
                }
                else
                    str[2] = i % 16;
            }
            //象限2
            if (i > 256 && i <= 512)
            {
                //象限
                str[0] = 2;
                if (i % 16 != 0)
                {
                    str[1] = i / 16 + 1;
                }
                else
                {
                    str[1] = i / 16;
                }
                str[1] = str[1] % 16;
                if (str[1] % 16 == 0)
                    str[1] = 16;

                if (i % 16 == 0)
                {
                    str[2] = 16;
                }
                else
                    str[2] = i % 16;

            }
            //象限3
            else if (i > 512 && i <= 768)
            {
                //象限
                str[0] = 3;
                if (i % 16 != 0)
                {
                    str[1] = i / 16 + 1;
                }
                else
                {
                    str[1] = i / 16;
                }
                str[1] = str[1] % 16;
                if (str[1] % 16 == 0)
                    str[1] = 16;

                if (i % 16 == 0)
                {
                    str[2] = 16;
                }
                else
                    str[2] = i % 16;

            }
            //象限4
            else if (i > 768 && i <= 1024)
            {
                //象限
                str[0] = 4;
                if (i % 16 != 0)
                {
                    str[1] = i / 16 + 1;
                }
                else
                {
                    str[1] = i / 16;
                }
                str[1] = str[1] % 16;
                if (str[1] % 16 == 0)
                    str[1] = 16;

                if (i % 16 == 0)
                {
                    str[2] = 16;
                }
                else
                    str[2] = i % 16;

            }
            str[3] = A;
            str[4] = B;
            return str;
        }
        /// <summary>
        /// 绑定数据到gridview中 ：相控阵
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="dr"></param>
        /// <param name="dgv"></param>
        /// <param name="str"></param>
        public DataTable  AddDataToGridViweXKZ(DataTable dt, DataRow dr, double[] str)
        {
            dr["象限"] = str[0];
            dr["组件号"] = str[1];
            dr["通道号"] = str[2];
            dr["衰减"] = str[3];
            dr["相位"] = str[4];

            dt.Rows.Add(dr);

            //dgv.Invoke(new Action(() =>
            //{
            //    dgv.DataSource = dt;
            //}));
            return dt;
        }
        /// <summary>
        /// Datatable转CSV
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            double A = 0;
            double B = 0;
            int m = 1;
            DataTable dt = new DataTable ();
            for (int ll = 0; ll < 4;ll++ )
            {
                for (int xx = 0; xx < 4; xx++)
                {
                    for (int yy = 0; yy < 4; yy++)
                    {
                        for (int l = 0; l < 4; l++)
                        {
                            for (int n = 0; n < 4; n++)
                            {
                                DataRow dr = AddDataXKZ.NewRow();
                                double[] dataInt = new double[5];

                                dataInt = AddDataXKZToDoubles(m, A, B);

                                dt = AddDataToGridViweXKZ(AddDataXKZ, dr, dataInt);

                                m++;
                            }
                        }
                    }
                }
            }

            DotNet.Utilities.CSVHelper.CSVHelper.DataTableToCSV(dt, "D:/一楼三维扫描架天线系统数据/相控阵补偿分析数据.CSV");
        }
        /// <summary>
        /// CSV转Datatable 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            DataTable dt = DotNet.Utilities.CSVHelper.CSVHelper.CSVToDataTableByOledb("D:/一楼三维扫描架天线系统数据/相控阵补偿分析数据.CSV");//CSVToDataTableByOledb()

            bool sta = ExcelHelper.OutputToExcel(dt, "D:/一楼三维扫描架天线系统数据/" + "最新" + ".xls");
        }

    }
}
