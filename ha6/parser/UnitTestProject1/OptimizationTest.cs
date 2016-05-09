using System;
using NUnit.Framework;
using parser.Expressions;
using parser.Optimization;
using parser.Parser;
using parser.Statements;

namespace Tests
{
    [TestFixture]
    public class OptimizationTest
    {
        private static readonly LLanguageParser Parser = new LLanguageParser(new LLanguageDefinition());
        private static readonly IStatementOptimizer StatementOptimizer = new StatementOptimizer();
        private static readonly IExpressionOptimizer ExpressionOptimizer =new BinaryOperationOptimizer();

        [Test]
        public void SimpleExpressionOptimizerTest()
        {
            var tree = Parser.GetTree("write (((1 + 2) * 3) / 4)");
            Console.WriteLine(tree.PrettyPrint());
            tree.OptimizeExpressions(new BinaryOperationOptimizer());
            Console.WriteLine(tree.PrettyPrint());
        }

        [Test]
        public void SimpleStatementIfOptimizerTest()
        {
            var tree = Parser.GetTree("if 1 then read x else write (2 + 2) endif");
            Console.WriteLine(tree.PrettyPrint()); Console.WriteLine();
            tree.OptimizeStatement(new StatementOptimizer());
            Console.WriteLine(tree.PrettyPrint());
        }

        [Test]
        public void SimpleStatementWhileOptimizerTest()
        {
            var tree = Parser.GetTree("while ((1 + 4) == 4) do x := (x + 1);skip enddo");
            tree.OptimizeExpressions(new BinaryOperationOptimizer());
            Console.WriteLine(tree.PrettyPrint()); Console.WriteLine();
            tree.OptimizeStatement(new StatementOptimizer());
            Console.WriteLine(tree.PrettyPrint());
        }

        [Test]
        public void SkipFewSkipStatementsTestTest()
        {
            var tree = Parser.GetTree("skip;skip;skip;skip");
            Assert.True(tree.Statement is SemiStatement);
            tree.OptimizeStatement(StatementOptimizer);
            Assert.True(tree.Statement is SkipStatement);
        }

        [Test]
        public void OptimizeExpressionsTest()
        {
            var tree = Parser.GetTree("x := ((((1 + 10) * 2) - 2) / 5)");
            Assert.True(tree.Statement is AssignStatement);
            tree.OptimizeExpressions(ExpressionOptimizer);
            var assign = (AssignStatement) tree.Statement;
            Assert.True(assign.RightPart is Number);
            Assert.AreEqual(4, ((Number)assign.RightPart).Value);
        }

        [Test]
        public void OptimizeExpressionsWithContextTest()
        {
            var tree = Parser.GetTree("y := 10; x := 12; x := (x * y)");
            tree.OptimizeExpressions(ExpressionOptimizer);
            var rightist = (AssignStatement)((SemiStatement) ((SemiStatement) tree.Statement).SecondStatement).SecondStatement;
            Assert.AreEqual(120, ((Number)rightist.RightPart).Value);
        }

        [Test]
        public void RemoveUnusedIfTest()
        {
            var tree = Parser.GetTree("if 0 then skip else x := 10 endif");
            Assert.True(tree.Statement is IfStatement);
            tree.OptimizeStatement(StatementOptimizer);
            Assert.True(tree.Statement is AssignStatement);

            tree = Parser.GetTree("if 1 then skip else x := 10 endif");
            Assert.True(tree.Statement is IfStatement);
            tree.OptimizeStatement(StatementOptimizer);
            Assert.True(tree.Statement is SkipStatement);
        }

        [Test]
        public void RemoveUnusedIfWithContextTest()
        {
            var tree = Parser.GetTree("x := 12; if (x < 15) then skip else x := 10 endif");
            Assert.True(tree.Statement is SemiStatement);
            tree.OptimizeExpressions(ExpressionOptimizer);
            tree.OptimizeStatement(StatementOptimizer);
            var semi = (SemiStatement) tree.Statement;
            Assert.True(semi.SecondStatement is SkipStatement);
        }

        [Test]
        public void RemoveUnusedWhileTest()
        {
            var tree = Parser.GetTree("while 0 do x := (x + 1) enddo");
            tree.OptimizeStatement(StatementOptimizer);
            Assert.True(tree.Statement is SkipStatement);
        }

        [Test]
        public void RemoveUnusedWhileWithContextTest()
        {
            var tree = Parser.GetTree("x := 10; while (x > 12) do x := (x - 1) enddo");
            tree.OptimizeExpressions(ExpressionOptimizer);
            tree.OptimizeStatement(StatementOptimizer);

            Assert.True(((SemiStatement)tree.Statement).SecondStatement is SkipStatement);
        }

        [Test]
        public void NoOptimizeTrueWhileConditionWithContextTest()
        {
            var tree = Parser.GetTree("x := 10; while (x < 12) do x := (x + 1) enddo");
            tree.OptimizeExpressions(ExpressionOptimizer);
            tree.OptimizeStatement(StatementOptimizer);

            Assert.True(((SemiStatement)tree.Statement).SecondStatement is WhileDoStatement);
        }

        [Test]
        public void CleanupContextAfterIfAssignTest()
        {
            var tree = Parser.GetTree("x := 10; if (y < 10) then x := 34 else skip endif; y := (x * 2)");
            tree.OptimizeExpressions(ExpressionOptimizer);
            var rightist = (AssignStatement)((SemiStatement)((SemiStatement)tree.Statement).SecondStatement).SecondStatement;
            Assert.False(rightist.RightPart is Number);
            Assert.True(rightist.RightPart is BinOperation);
        }

        [Test]
        public void NoCleanupContextAfterIfAssignOtherVariableTest()
        {
            var tree = Parser.GetTree("x := 10; if (y < 10) then z := 34 else skip endif; y := (x * 2)");
            tree.OptimizeExpressions(ExpressionOptimizer);
            var rightist = (AssignStatement)((SemiStatement)((SemiStatement)tree.Statement).SecondStatement).SecondStatement;
            Assert.True(rightist.RightPart is Number);
        }

        [Test]
        public void CleanupContextAfterReadTest()
        {
            var tree = Parser.GetTree("x := 10; read x; y := (x * 2)");
            tree.OptimizeExpressions(ExpressionOptimizer);
            var rightist = (AssignStatement)((SemiStatement)((SemiStatement)tree.Statement).SecondStatement).SecondStatement;
            Assert.False(rightist.RightPart is Number);
            Assert.True(rightist.RightPart is BinOperation);
        }

        [Test]
        public void CleanupContexInWhileLoopBodyTest()
        {
            var tree = Parser.GetTree("x := 10; while y < 200 do z := 2 * x enddo; y := (x * 2)");
            tree.OptimizeExpressions(ExpressionOptimizer);
            var whileStatement = (WhileDoStatement)((SemiStatement)((SemiStatement)tree.Statement).SecondStatement).FirstStatement;
            Assert.True(whileStatement.LoopBody is AssignStatement);

            var assign = (AssignStatement) whileStatement.LoopBody;
            Assert.True(assign.RightPart is BinOperation);
        }

        [Test]
        public void TrivialMulOptimizeTest()
        {
            var tree = Parser.GetTree("x := (0 * y)");
            tree.OptimizeExpressions(ExpressionOptimizer);
            var expr = ((AssignStatement) tree.Statement).RightPart;
            Assert.True(expr is Number);
            Assert.AreEqual(0, ((Number)expr).Value);
        }

        [Test]
        public void TrivialAndOptimizeTest()
        {
            var tree = Parser.GetTree("x := (y && 0)");
            tree.OptimizeExpressions(ExpressionOptimizer);
            var expr = ((AssignStatement)tree.Statement).RightPart;
            Assert.True(expr is Number);
            Assert.AreEqual(0, ((Number)expr).Value);
        }
    }
}