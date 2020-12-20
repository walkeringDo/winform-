using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FansheTeXing
{
    public class Class1
    {
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum, Inherited = false)]
        public class DemoAttribute : Attribute
        {
            public string Name { get; set; }
            public int Age{get;set;}
            public DemoAttribute(int age)
            {
                Age = age;
            }
        }
        [Demo(10, Name = "测试")]
         public class Student {
            int Class { get; set; }
            int Num { get; set; }
            public void sum()
            {
                int a = 10;
            }
        }        
    }
}
