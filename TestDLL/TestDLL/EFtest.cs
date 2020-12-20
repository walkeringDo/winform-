using Common.Utilities;
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
    public partial class EFtest : Form
    {
        public EFtest()
        {
            InitializeComponent();
        }

        public static int id = 1;
     

        public List < testEF> GetStudent(string StudentName)
        {

            using (testEntities entity = new testEntities())
            {
                List<testEF> StudentEF = entity.testEF.ToList ();
                return StudentEF;
            }
        }

        public testEF UpData(string StudentName, int Age, int Class)
        {
            using (testEntities entity = new testEntities())
            {
                testEF StudentEF = entity.testEF.FirstOrDefault(x => x.StudentName == StudentName);
                if (StudentEF != null)
                {
                    StudentEF.Age = Age;
                    StudentEF.Class = Class;
                    entity.SaveChanges();
                }
                return StudentEF;
            }
        }

        public testEF Add(testEF StudentEF)
        {
            using (testEntities entity = new testEntities())
            {
                entity.testEF.Add(
                    new testEF() {
                        ID = StudentEF.ID,
                        StudentName = StudentEF.StudentName,
                        Age = StudentEF.Age,
                        Class = StudentEF.Class,
                        NowTime = StudentEF.NowTime 
                    });
                if (entity.SaveChanges() > 0)
                {
                    return StudentEF;
                }
                else
                    return null;
            }
        }

        public void Deleted(string StudentName)
        {
            using (testEntities entity = new testEntities())
            {
                testEF StudentEF = entity.testEF.FirstOrDefault(x => x.StudentName == StudentName);
                if(StudentEF != null)
                {
                    entity.testEF.Remove(StudentEF);
                }

                if (entity.SaveChanges() > 0)
                {
                    return;
                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Add(new testEF()
            {
                ID = id,
                StudentName = id.ToString() + "STU",
                Age = 18,
                Class = 1,
                NowTime = DateTime.Now
            });
            id++;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Deleted(textBox1.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
             testEF StudentEF = UpData(textBox1.Text, int.Parse (textBox2.Text), int.Parse (textBox3.Text));
        }

        private void button4_Click(object sender, EventArgs e)
        {


            List<testEF> list = new List<testEF> ();
            list = GetStudent(textBox1.Text);
            DataTable dt = new DataTable ();
            if(list != null)
                dt = ListExtensions.ToDataTable<testEF>(list);

            dataGridView1.DataSource = dt;
        }

    }
}
