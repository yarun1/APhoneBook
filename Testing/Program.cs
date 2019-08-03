using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testing
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> list1 = new List<int>();
            list1.Add(0);
            list1.Add(2);

            List<int> list2 = list1;
            list2.RemoveAt(0);

            foreach (int l in list1)
            {
                Console.WriteLine(l);
            }

            Console.WriteLine(list1.IndexOf(0));
        }
    }
}
