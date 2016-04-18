using System;
using NUnit.Framework;
using parser.Expressions;
using parser.Parser;
using parser.Statements;

namespace Tests
{
    public class ParserTests
    {
        private static readonly LLanguageParser Parser = new LLanguageParser(new LLanguageDefinition());

        [Test]
        public void ValidNumberTest()
        {
            var tree = Parser.GetTree("write 123");
            var write = (OperationIo)tree.Statement;
            Assert.True(write.Expression is Number);
            Assert.AreEqual(123, ((Number)write.Expression).Value);
        }

        [Test]
        public void ValidVariableTest()
        {
            var tree = Parser.GetTree("write y");
            var read = (OperationIo)tree.Statement;
            Assert.True(read.Expression is Variable);
            Assert.AreEqual("y", ((Variable)read.Expression).Name);
        }

        [Test]
        public void ValidBinaryArithmeticOperationTest()
        {
            var tree = Parser.GetTree("a := (((((x + y) - z) * 2) / 5) % 3)");
            var binOp = (BinOperation)((AssignStatement)tree.Statement).RightPart;
            Assert.AreEqual(BinOp.Modulo, binOp.Operation.Op);
            Assert.AreEqual(3, ((Number)binOp.Right).Value);

            var left = (BinOperation)binOp.Left;
            Assert.AreEqual(BinOp.Div, left.Operation.Op);
            Assert.AreEqual(5, ((Number)left.Right).Value);

            left = (BinOperation)left.Left;
            Assert.AreEqual(BinOp.Mul, left.Operation.Op);
            Assert.AreEqual(2, ((Number)left.Right).Value);

            left = (BinOperation)left.Left;
            Assert.AreEqual(BinOp.Minus, left.Operation.Op);
            Assert.AreEqual("z", ((Variable)left.Right).Name);

            left = (BinOperation)left.Left;
            Assert.AreEqual(BinOp.Plus, left.Operation.Op);
            Assert.AreEqual("y", ((Variable)left.Right).Name);
            Assert.AreEqual("x", ((Variable)left.Left).Name);
        }

        [Test]
        public void ValidBinaryLogicalOperationTest()
        {
            var tree = Parser.GetTree("a := ((x && y) || z)");
            var binOp = (BinOperation)((AssignStatement)tree.Statement).RightPart;
            Assert.AreEqual(BinOp.Or, binOp.Operation.Op);
            Assert.AreEqual("z", ((Variable)binOp.Right).Name);

            var left = (BinOperation)binOp.Left;
            Assert.AreEqual(BinOp.And, left.Operation.Op);
            Assert.AreEqual("y", ((Variable)left.Right).Name);
            Assert.AreEqual("x", ((Variable)left.Left).Name);
        }

        [Test]
        public void ValidBinaryCompareOperationTest()
        {
            var tree = Parser.GetTree("a := (((((x < 1) <= 2) > 0) >= 5) == 3) != 6)");
            var binOp = (BinOperation)((AssignStatement)tree.Statement).RightPart;
            Assert.AreEqual(BinOp.Neq, binOp.Operation.Op);
            Assert.AreEqual(6, ((Number)binOp.Right).Value);

            var left = (BinOperation)binOp.Left;
            Assert.AreEqual(BinOp.Eq, left.Operation.Op);
            Assert.AreEqual(3, ((Number)left.Right).Value);

            left = (BinOperation)left.Left;
            Assert.AreEqual(BinOp.Ge, left.Operation.Op);
            Assert.AreEqual(5, ((Number)left.Right).Value);

            left = (BinOperation)left.Left;
            Assert.AreEqual(BinOp.Gt, left.Operation.Op);
            Assert.AreEqual(0, ((Number)left.Right).Value);

            left = (BinOperation)left.Left;
            Assert.AreEqual(BinOp.Le, left.Operation.Op);
            Assert.AreEqual(2, ((Number)left.Right).Value);

            left = (BinOperation)left.Left;
            Assert.AreEqual(BinOp.Lt, left.Operation.Op);
            Assert.AreEqual(1, ((Number)left.Right).Value);

            Assert.AreEqual("x", ((Variable)left.Left).Name);
        }

        [Test]
        public void ValidSkipTreeTest()
        {
            var tree = Parser.GetTree("skip; skip");
            Assert.True(tree.Statement is SemiStatement);
            var semi = (SemiStatement)tree.Statement;
            Assert.True(semi.FirstStatement is SkipStatement);
            Assert.True(semi.SecondStatement is SkipStatement);
        }

        [Test]
        public void ValidIfTreeTest()
        {
            var tree = Parser.GetTree("if 1 then skip else read x");

            Assert.True(tree.Statement is IfStatement);
            var ifStatement = (IfStatement)tree.Statement;
            Assert.True(ifStatement.Condition is Number);
            Assert.AreEqual(1, ((Number)ifStatement.Condition).Value);
            Assert.True(ifStatement.ThenStatement is SkipStatement);
            Assert.True(ifStatement.ElseStatement is OperationIo);
        }

        [Test]
        public void ValidWhileTest()
        {
            var tree = Parser.GetTree("while 0 do x := x + 1");

            Assert.True(tree.Statement is WhileDoStatement);
            var whileDoWtatement = (WhileDoStatement)tree.Statement;
            Assert.AreEqual(0, ((Number)whileDoWtatement.Condition).Value);
            Assert.True(whileDoWtatement.LoopBody is AssignStatement);
            Assert.AreEqual("x", ((AssignStatement)whileDoWtatement.LoopBody).Var.Name);
            Assert.AreEqual(BinOp.Plus, ((BinOperation)((AssignStatement)whileDoWtatement.LoopBody).RightPart).Operation.Op);
        }

        [Test]
        public void ValidInOutTest()
        {
            var tree = Parser.GetTree("read x; write 10");
            Assert.True(tree.Statement is SemiStatement);

            var semi = (SemiStatement)tree.Statement;
            Assert.True(semi.FirstStatement is OperationIo);
            Assert.True(semi.SecondStatement is OperationIo);

            var read = (OperationIo)semi.FirstStatement;
            var write = (OperationIo)semi.SecondStatement;

            Assert.AreEqual(IoOperationType.Read, read.OperationType);
            Assert.AreEqual(IoOperationType.Write, write.OperationType);
        }

        [Test]
        public void ValidAssigmentTest()
        {
            var tree = Parser.GetTree("x := 123");
            Assert.True(tree.Statement is AssignStatement);
            var assign = (AssignStatement)tree.Statement;
            Assert.AreEqual("x", assign.Var.Name);
            Assert.AreEqual(123, ((Number)assign.RightPart).Value);
        }

        [Test]
        public void SimpleProgramTest()
        {
            Assert.IsNotNull(Parser.GetTree("skip"));
            Assert.IsNull(Parser.GetTree("unknown asds"));
            Assert.IsNotNull(Parser.GetTree("x := 1 + 1"));
            Assert.IsNotNull(Parser.GetTree("if (1 <    3) then         x := 3 else skip"));
            Assert.IsNotNull(Parser.GetTree("skip      ; skip"));
            Assert.IsNotNull(Parser.GetTree("while (x < 10) do x := (x + (10 * 120))"));
        }

        [Test]
        public void PrettyPrinterTest()
        {
            Console.WriteLine(Parser.GetTree("x := ((y==2)&&((p==3)||((y!=2)&&(p!=4))))").PrettyPrint()); Console.WriteLine();
            Console.WriteLine(Parser.GetTree("skip;skip;skip").PrettyPrint()); Console.WriteLine();
            Console.WriteLine(Parser.GetTree("x := (1 + 1); y := (2 + 3)").PrettyPrint()); Console.WriteLine();
            Console.WriteLine(Parser
                .GetTree("if (1 < 3) then x := (2 + 4) else while (y < 10) do while (z < y) do if (z < 3) then z := z + 1 else y := y - 1; skip;skip")
                .PrettyPrint()); Console.WriteLine();
            Console.WriteLine(Parser.GetTree("if (x < 10) then write x else read z").PrettyPrint());
        }
    }
}
