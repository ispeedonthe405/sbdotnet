using sbdotnet.Demotest.tests;

namespace sbdotnet.Demotest
{
    internal class Program
    {
        static void Main(string[] args)
        {

            tests.Extensions extensions = new();
            extensions.Run();
        }
    }
}
