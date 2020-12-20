using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestDLL
{
    public partial class 存储过程 : Form
    {
        public 存储过程()
        {
            InitializeComponent();
        }
        #region
        private void button1_Click(object sender, EventArgs e)
        {
            IDataParameter[] parameters = new IDataParameter[4];

            parameters[0] = new SqlParameter("a", SqlDbType.NChar, 50);
            parameters[1] = new SqlParameter("b", SqlDbType.NChar, 50);
            parameters[2] = new SqlParameter("c", SqlDbType.NChar, 50);


            parameters[0].Value = "laowang";
            parameters[1].Value = "laowang";
            parameters[2].Value = "laowang";
            RunProcedure("pro_abc1", parameters);

            DataTable dt = RunProc();
        }
        /// <summary>
        /// 执行存储过程，返回SqlDataReader ( 注意：调用该方法后，一定要对SqlDataReader进行Close )
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader RunProcedure(string storedProcName, IDataParameter[] parameters)
        {
            SqlConnection connection = new SqlConnection("data source=PC-20200102UPAO\\SQLEXPRESS;initial catalog=Mytest;integrated security=True;MultipleActiveResultSets=True");
            SqlDataReader returnReader;
            connection.Open();
            SqlCommand command = BuildQueryCommand(connection, storedProcName, parameters);
            
            command.CommandType = CommandType.StoredProcedure;
            returnReader = command.ExecuteReader(CommandBehavior.CloseConnection);
            return returnReader;
        }
        /// <summary>
        /// 构建 SqlCommand 对象(用来返回一个结果集，而不是一个整数值)
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlCommand</returns>
        private static SqlCommand BuildQueryCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            SqlCommand command = new SqlCommand(storedProcName, connection);
            command.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter parameter in parameters)
            {
                if (parameter != null)
                {
                    // 检查未分配值的输出参数,将其分配以DBNull.Value.
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    command.Parameters.Add(parameter);
                }
            }

            return command;
        }
        #endregion
        #region abc
        private DataTable  RunProc()
        {
            DataSet DS = new DataSet();
            //输入参数
            SqlParameter para = new SqlParameter("a", SqlDbType.VarChar, 10);
            para.Direction = ParameterDirection.Input;
            para.Value = "1";

            //输出参数
            SqlParameter para1 = new SqlParameter("@outvalue", SqlDbType.NChar, 4);
            para1.Direction = ParameterDirection.Output;

            //返回参数
            SqlParameter para2 = new SqlParameter("@returnvalue", SqlDbType.NChar, 4);
            para2.Direction = ParameterDirection.ReturnValue;

            SqlParameter[] paras = { para, para1, para2 };
            //执行存储过程返回查询结果
            DS = runprocessdure("pro_abc", paras, "tt");

            DataTable dt = DS.Tables[0];

            return dt;
        }

        public static DataSet runprocessdure(string strname, IDataParameter[] parameters, string tablename, int timeout=30)
        {
            using (SqlConnection connect = new SqlConnection("data source=PC-20200102UPAO\\SQLEXPRESS;initial catalog=Mytest;integrated security=True;MultipleActiveResultSets=True"))
            {
                DataSet dataset = new DataSet();
                connect.Open();

                SqlDataAdapter sqlDA = new SqlDataAdapter();
                sqlDA.SelectCommand = BuildQueryCommand(connect, strname, parameters);

                sqlDA.SelectCommand.CommandTimeout = timeout;
                sqlDA.Fill(dataset,tablename);

                connect.Close();

                return dataset;
            }
        }
        #endregion

        SqlConnection conn = new SqlConnection("data source=PC-20200102UPAO\\SQLEXPRESS;initial catalog=Stu;integrated security=True;MultipleActiveResultSets=True");
        private void RunQuerySql()
        {
            SqlCommand cmd = new SqlCommand("proc_Query_S",conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Clear();
            SqlParameter parSno = new SqlParameter("@sNo",SqlDbType.Char,4);
            parSno.Value=this.txtSno.Text.Trim();//学号
            SqlParameter parSname = new SqlParameter("@sName", SqlDbType.Char, 10);
            parSname.Direction = ParameterDirection.Output;//姓名
            SqlParameter parSAge = new SqlParameter("@sAge", SqlDbType.Int);
            parSAge.Direction = ParameterDirection.Output;//年龄
            SqlParameter parSSex = new SqlParameter("@sSex", SqlDbType.Char, 2);
            parSSex.Direction = ParameterDirection.Output;//性别
            cmd.Parameters.Add(parSno);
            cmd.Parameters.Add(parSname);
            cmd.Parameters.Add(parSAge);
            cmd.Parameters.Add(parSSex);
            try
            {
            conn.Open();
            cmd.ExecuteNonQuery();
            this.txtS.Text = parSno.Value.ToString();
            this.txtName.Text = parSname.Value.ToString();
            this.txtAge.Text = parSAge.Value.ToString();
            this.txtSex.Text = parSSex.Value.ToString();
            }
            catch (Exception ex)
            {
            MessageBox.Show("异常：" + ex.Message);
            }
            finally { conn.Close(); }
        }
        private void Querysql()
        {
            //conn是连接字符串data source=ServerName;initial catalog=DatabaseName;user id=UserName;pwd=Password
            //  server=ServerName;database=DatabaseName ;UID=UserName;pwd=Password
            //proc_Query_SC_More 是存储过程名称
            SqlCommand cmd = new SqlCommand("proc_Query_SC_More", conn);//新建一个SqlCommand对象
            cmd.CommandType = CommandType.StoredProcedure;//设置CommandType属性为StoredProcedure,及存储过程
            cmd.Parameters.Clear();//清除cmd的参数，不清除，第二次调用的时候则会出错
            SqlParameter parSno = new SqlParameter("@sNo", SqlDbType.Char, 4);//新建参数实列new SqlParameter('存储过程的变量',数据类型,数据长度)
            parSno.Value = this.txtSno.Text.Trim();//给参数parSno赋值
            cmd.Parameters.Add(parSno);//把参数加入到cmd中
            conn.Open();//打开连接
            SqlDataReader dr = cmd.ExecuteReader();//实列化一个SqlDataReader
            //以下是对ListView控件的操作
            while (dr.Read())
            {
            string sno = dr["sNo"].ToString();//取值
            string cno = dr["cNo"].ToString();
            string score = dr["score"].ToString();
            ListViewItem lvi = new ListViewItem(sno);//实列化一个ListViewItem对象
            lvScore.Items.Add(lvi);//把ListViewItem 对象添加到ListView控件中 lvScore为ListView控件
            lvi.SubItems.AddRange(new string[]{cno,score});//给ListViewItem赋值
            } 
            conn.Close();//关闭连接

        }

        private void loadsql()
        {
            //新建一个SqlDataAdapter对象，proc_Query_SC_More为存储过程，conn为连接对象SqlConnection
            SqlDataAdapter da = new SqlDataAdapter("proc_Query_SC_More", conn);
            //设置CommandType为StroeProcedure,及存储过程
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            //新建DataSet实例
            DataSet ds = new DataSet();
            //参数
            SqlParameter parSno = new SqlParameter("@sNo", SqlDbType.Char, 4);
            parSno.Value = "s001";
            da.SelectCommand.Parameters.Add(parSno);
            //填充数据
            da.Fill(ds, "score");
            //设置DataGriedView数据源
            this.dataGridView1.DataSource = ds.Tables["score"];

        }

        private void button2_Click(object sender, EventArgs e)
        {
            loadsql();
            RunQuerySql();
            Querysql();
        }
    }
}
