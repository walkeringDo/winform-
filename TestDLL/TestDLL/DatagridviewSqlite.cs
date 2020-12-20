using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Threading;
using HD.DBHelper;

namespace TestDLL
{
    public partial class DatagridviewSqlite : Form
    {
        /// <summary>
        /// 读取数据库到控件显示的线程
        /// </summary>
        Thread th;

        /// <summary>
        /// 获取连接字符串
        /// </summary>
        string ConnectStr = "Data Sourse = id_cache.db;Version = 3;Pooling = true;FailIfMissing=false";
        public DatagridviewSqlite()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 控件显示
        /// </summary>
        private void ShowDataToGdv()
        {
            DataSet dataSetData = DbHelperSQLite.Query("select * from TestNetWorkAnalyzer");
            DataTable dt = dataSetData.Tables[0];
            dataGridView1.Invoke(new Action(() => { dataGridView1.DataSource = dt; }));


            Thread.Sleep(5000);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataSet dataSetData = DbHelperSQLite.Query("select * from TestNetWorkAnalyzer");
            DataTable dt = dataSetData.Tables[0];
            dataGridView1.DataSource = dt;
        }
        public static int intId = 1;
        private void button2_Click(object sender, EventArgs e)
        {
            //INSERT INTO TestNetWorkAnalyzer (ID,X轴位置,Y轴位置,Z轴位置,U轴位置,衰减,相位,RI,CHi,A,Φ,频点集合) VALUES(2,2,2,2,2,2.0,2.0,"2",2.0,2.0,2.0,"2");
            //int count = DbHelperSQLite.ExecuteSql("INSERT INTO TestNetWorkAnalyzer (ID,X轴位置,Y轴位置,Z轴位置,U轴位置,衰减,相位,RI,CHi,A,Φ,频点集合) VALUES(" + intId.ToString () + ",2,2,2,2,2.0,2.0,"2",2.0,2.0,2.0,"2")");


            InsertCommend(intId, 1, 1, 1, 1, 1.0, 1.0, "123", 2.0, 2.0, 2.0, " ");
            intId++;

            ShowDataToGdv();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int count = DbHelperSQLite.ExecuteSql("DELETE  FROM TestNetWorkAnalyzer");

            ShowDataToGdv();
        }

        private void InsertCommend(int ID, int AxisX, int AxisY, int AxisZ, int AxisU, double ParaSJ, double ParaXW, string RI, double CHi, double ParaA, double ParaFi, string ParaPD)
        {
            string tmp = "INSERT INTO TestNetWorkAnalyzer (ID,X轴位置,Y轴位置,Z轴位置,U轴位置,衰减,相位,RI,CHi,A,Φ,频点集合) VALUES(" + ID.ToString() + "," + AxisX.ToString() + "," +
                AxisY.ToString() + "," + AxisZ.ToString() + "," + AxisU.ToString() + "," + ParaSJ.ToString() + "," + ParaXW.ToString() + "," + "'" + RI + "'" + "," + CHi.ToString() + "," + ParaA.ToString() +
                "," + ParaFi.ToString() + "," + "'" + ParaPD.ToString() + "'" + ")";
            int count = DbHelperSQLite.ExecuteSql(tmp);

        }
    }
}
