using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace EffectSome
{
    public static class ConsolePrompt
    {
        // Most likely to be removed overall (or just be developed in the late days of the program - AKA in 4 years)

        public static void Main(string[] args)
        {
            // Make this shit work
            AllocConsole();
            AttachConsole(-1);
            Console.WriteLine("Welcome to the console prompt of EffectSome! Type -help for help.");
            Console.ReadKey();
            // you know the rest...
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool AllocConsole();

        [DllImport("kernel32", SetLastError = true)]
        public static extern bool AttachConsole(int dwProcessId);
    }
}
