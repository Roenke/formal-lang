using System;
using NUnit.Framework;
using parser.Parser;

namespace Tests
{
    public class ParserTests
    {
        [Test]
        public void SimpleProgramTest()
        {
            var parser = new LLanguageParser(new LLanguageDefinition());
            Assert.IsNotNull(parser.GetTree("skip"));
            Assert.IsNull(parser.GetTree("unknown asds"));
            Assert.IsNotNull(parser.GetTree("x := 1 + 1"));
            Assert.IsNotNull(parser.GetTree("if (1 < 3) then x := 3 else skip"));
            Assert.IsNotNull(parser.GetTree("skip ; skip"));
            Assert.IsNotNull(parser.GetTree("while (x < 10) do x := (x + (10 * 120))"));

            var nonUnique =
                parser.GetTree("if (1 < 3) then x := 3 else skip; while x < 10 do x := ((x + 10) * 120); skip");
            Assert.IsNotNull(nonUnique);
            Console.WriteLine(nonUnique.PrettyPrint());

        }

        [Test]
        public void OperatorsTest()
        {
            var parser = new LLanguageParser(new LLanguageDefinition());
            Assert.NotNull(parser.GetTree("x := ((1 + 2) * ((4 / 3) % (3 - 1)))"));
            Assert.NotNull(parser.GetTree("x := y == 2 && p == 3 || y != 2 && p != 4").PrettyPrint());
            Assert.NotNull(parser.GetTree("x := z < 3 || x > 6 || t <= 3 && v >= 3"));
        }

        [Test]
        public void InOutTest()
        {
            var parser = new LLanguageParser(new LLanguageDefinition());
            Assert.NotNull(parser.GetTree("read x"));
            Assert.NotNull(parser.GetTree("write x"));
            Assert.NotNull(parser.GetTree("write (4 * 2)"));
        }

        [Test]
        public void PrettyPrinterTest()
        {
            var parser = new LLanguageParser(new LLanguageDefinition());
            
            Console.WriteLine(parser.GetTree("x := ((y==2)&&((p==3)||((y!=2)&&(p!=4))))").PrettyPrint()); Console.WriteLine();
            Console.WriteLine(parser.GetTree("skip;skip;skip").PrettyPrint()); Console.WriteLine();
            Console.WriteLine(parser.GetTree("x := (1 + 1); y := (2 + 3)").PrettyPrint()); Console.WriteLine();
            Console.WriteLine(parser
                .GetTree("if (1 < 3) then x := (2 + 4) else while (y < 10) do while (z < y) do if (z < 3) then z := z + 1 else y := y - 1; skip")
                .PrettyPrint()); Console.WriteLine();
            Console.WriteLine(parser.GetTree("if x < 10 then write x else read z").PrettyPrint());
        }

        [Test]
        public void ValidTreeTest()
        {
            
        }
    }
}
