using System;
using System.Diagnostics;
using System.IO;
using parser.Optimization;
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
                Environment.Exit(1);
            }

            var content = File.ReadAllText(args[0]);

            var parser = new LLanguageParser(new LLanguageDefinition());

            var sw = new Stopwatch();
            var totalStopWatch = new Stopwatch();
            totalStopWatch.Start();
            sw.Start();
            var tree = parser.GetTree(content);
            sw.Stop();
            Console.WriteLine("Building tree completed. Time = {0} ms", sw.ElapsedMilliseconds);
            File.WriteAllText("../../Example/pretty_res.txt", tree.PrettyPrint());

            sw.Restart();
            tree.OptimizeExpressions(new BinaryOperationOptimizer());
            sw.Stop();
            File.WriteAllText("../../Example/expr_opt_pretty_res.txt", tree.PrettyPrint());
            Console.WriteLine("Expression optimization completed. Time = {0}", sw.ElapsedMilliseconds);

            sw.Restart();
            tree.OptimizeStatement(new StatementOptimizer());
            sw.Stop();
            File.WriteAllText("../../Example/all_opts_pretty_res.txt", tree.PrettyPrint());
            Console.WriteLine("Statements optimization completed. Time = {0}", sw.ElapsedMilliseconds);

            totalStopWatch.Stop();
            Console.WriteLine("Completed all. Total time = {0} ms", totalStopWatch.ElapsedMilliseconds);
        }

        private static void Usage()
        {
            var path = Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
            Console.WriteLine("Usage:");
            Console.WriteLine("\t {0} <path/to/programm>", path);
        }
    }
}
