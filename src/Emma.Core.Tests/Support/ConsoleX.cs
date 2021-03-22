using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Emma.Common;

namespace Emma.Core.Tests.Support
{
    public static class ConsoleX
    {
        private static int _folderDepth;

        public static void Dump(IEnumerable<ExtensionMethod> methods, string source = null)
        {
            Console.WriteLine();
            if (!string.IsNullOrEmpty(source))
            {
                Console.WriteLine(source);
            }
            foreach (var mi in methods)
            {
                Dump(mi);
            }
        }
        public static void Dump(ExtensionMethod method)
        {
        Console.WriteLine($"{method}");
        }
    }
}