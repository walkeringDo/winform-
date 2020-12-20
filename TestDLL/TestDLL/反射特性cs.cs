using FansheTeXing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestDLL
{
    public partial class 反射特性cs : Form
    {
        public 反射特性cs()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            var stuType = typeof(Class1.Student);
            
            //反射第一个属性
            //var Class = stuType.GetProperties();
            //var attrs = Class[0].GetCustomAttributes(typeof(Class1.DemoAttribute), false);
            //发射第一个方法
            var method = stuType.GetMethods()[0];
            var attrs1 = method.GetCustomAttributes(typeof(Class1.DemoAttribute),false);

            //testone
            Assembly a = testone();

            //testtwo
            StringBuilder sb = new StringBuilder(1000);
            Type[] typ = testtwo();
            foreach (Type tp in typ)
            {
                sb.Append(tp.FullName +"\r");
            }
            MessageBox.Show(sb.ToString());

            //testthree
            sb.Clear();
            sb =testthree();
            MessageBox.Show(sb.ToString ());

            //testfour
            int aa = 1,bb=2;

            int c = testfour(aa, bb);
            MessageBox.Show(c.ToString ());
        }

        [AttributeUsage(AttributeTargets.Property,AllowMultiple = false,Inherited = false)]
        public class FieldNameAttribute : Attribute
        {
            private string name;
            public FieldNameAttribute(string name)
            {
                this.name = name;
            }
        }

        private Assembly  testone()
        {
            Assembly assem = typeof(object).Module.Assembly;
            return assem;
        }

        private Type[] testtwo()
        {
            Assembly assem = Assembly.LoadFile(@"F:\BaiduNetdiskDownload\c#自用相关\TestDLL\TestDLL\bin\Debug\二维矩形旋转公式.exe");
            Type[] tp = assem.GetTypes();

            return tp;
        }

        private StringBuilder testthree()
        {
            StringBuilder sb = new StringBuilder(1000);
            Type t = typeof(System.String);

            ConstructorInfo[] ms = t.GetConstructors(BindingFlags .Public | BindingFlags.Instance);
            foreach (MemberInfo mi in ms)
            {
                sb.Append(mi.ToString ());
            }
            return sb;
        }

        [Obsolete ("将在下一个版本淘汰")]
        private int testfour(int a ,int b)
        { 
            return a + b;
        }
    }
}
