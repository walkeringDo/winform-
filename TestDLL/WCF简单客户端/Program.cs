using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCF简单客户端
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceReference1.Service1Client client = new ServiceReference1.Service1Client();

            int count = client.GetCount(1, 2);
            string str = client.StrName();
            string str1 = client.Str();
            Console.WriteLine(count.ToString() + "\n");
            Console.WriteLine(str + "\n");
            Console.WriteLine(str1 + "\n");
            Console.WriteLine();
            Console.Read();

        }
    }
}
