using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace sbdotnet.Demotest.tests
{
    internal class Extensions : ITest
    {
        private void test_ContainsDigits2(string value)
        {
            Console.WriteLine(value);
            Console.WriteLine($"ContainsDigits: {value.ContainsDigits()}");
        }
        public void test_ContainsDigits()
        {
            string string1 = "abcdefg";
            string string2 = "12345";
            string string3 = "a1b2c3";
            string string4 = "&#^&as";
            test_ContainsDigits2(string1);
            test_ContainsDigits2(string2);
            test_ContainsDigits2(string3);
            test_ContainsDigits2(string4);
        }

        public void Run()
        {
            test_ContainsDigits();
        }
    }
}
