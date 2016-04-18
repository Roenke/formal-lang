using System;
using NUnit.Framework;
using parser.Optimization;
using parser.Parser;

namespace Tests
{
    [TestFixture]
    public class OptimizationTest
    {
        [Test]
        public void ExpressionOptimizerTest()
        {
            var parser = new LLanguageParser(new LLanguageDefinition());

            var tree = parser.GetTree("write (((1 + 2) * 3) / 4)");
            Console.WriteLine(tree.PrettyPrint());
            tree.OptimizeExpressions(new BinaryOperationOptimizer());
            Console.WriteLine(tree.PrettyPrint());
        }

        [Test]
        public void StatementSemiOptimizerTest()
        {
            var parser = new LLanguageParser(new LLanguageDefinition());

            var tree = parser.GetTree("skip;skip;skip");
            Console.WriteLine(tree.PrettyPrint()); Console.WriteLine();
            tree.OptimizeStatement(new StatementOptimizer());
            Console.WriteLine(tree.PrettyPrint());
        }

        [Test]
        public void StatementIfOptimizerTest()
        {
            var parser = new LLanguageParser(new LLanguageDefinition());

            var tree = parser.GetTree("if 1 then read x else write (2 + 2)");
            Console.WriteLine(tree.PrettyPrint()); Console.WriteLine();
            tree.OptimizeStatement(new StatementOptimizer());
            Console.WriteLine(tree.PrettyPrint());
        }

        [Test]
        public void StatementWhileOptimizerTest()
        {
            var parser = new LLanguageParser(new LLanguageDefinition());

            var tree = parser.GetTree("while ((1 + 4) == 4) do x := x + 1;skip");
            tree.OptimizeExpressions(new BinaryOperationOptimizer());
            Console.WriteLine(tree.PrettyPrint()); Console.WriteLine();
            tree.OptimizeStatement(new StatementOptimizer());
            Console.WriteLine(tree.PrettyPrint());
        }
    }
}