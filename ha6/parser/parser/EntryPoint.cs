using System;
using System.IO;
using parser.Parser;

namespace parser
{
    internal class EntryPoint
    {
        public static void Main(string[] args)
        {
            var count = args.Length;
            if (count != 1 || !File.Exists(args[0]))
            {
                Usage();
            }

            var content = File.ReadAllText(args[0]);

            var parser = new LLanguageParser(new LLanguageDefinition());
            parser.GetTree(content);
        }

        private static void Usage()
        {
            var path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            Console.WriteLine("Usage:");
            Console.WriteLine("\t {0} <path/to/programm>", path);
        }
    }
}
