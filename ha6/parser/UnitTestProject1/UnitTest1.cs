using NUnit.Framework;
using parser.Parser;

namespace Tests
{
    public class ParserTests
    {
        [Test]
        public void SimpleProgramTest()
        {
            var parser = new LLanguageParser();
            Assert.NotNull(parser.GetTree("skip"));
            Assert.Null(parser.GetTree("unknown asds"));
            Assert.NotNull(parser.GetTree("x := 1 + 1"));
            Assert.NotNull(parser.GetTree("if (1 < 3) then x := 3 else skip"));
            Assert.NotNull(parser.GetTree("skip ; skip"));
            Assert.NotNull(parser.GetTree("while (x < 10) do x := (x + (10 * 120))"));
            Assert.NotNull(parser.GetTree("if (1 < 3) then x := 3 else skip; while x < 10 do x := ((x + 10) * 120); skip"));
        }

        [Test]
        public void OperatorsTest()
        {
            var parser = new LLanguageParser();
            Assert.NotNull(parser.GetTree("x := ((1 + 2) * ((4 / 3) % (3 - 1)))"));
            //Assert.NotNull(parser.GetTree("x := y == 2 && p == 3 || y != 2 && p != 4"));
            //Assert.NotNull(parser.GetTree("x := z < 3 || x > 6 || t <= 3 && v >= 3"));
        }

        [Test]
        public void MyExpressionsTest()
        {
            var parser = new LLanguageParser();
            Assert.NotNull(parser.GetTree("x := 23"));
            Assert.NotNull(parser.GetTree("x := y"));
            Assert.NotNull(parser.GetTree("x := (1 + 1); y := (2 + 3)"));
            //Assert.NotNull(parser.GetTree("if (x + y) then x := 1 else y := 0"));
        }

        [Test]
        public void InOutTest()
        {
            var parser = new LLanguageParser();
            Assert.NotNull(parser.GetTree("read x"));
            Assert.NotNull(parser.GetTree("write x"));
            Assert.NotNull(parser.GetTree("write (4 * 2)"));
        }

        [Test]
        public void PrettyPrinterTest()
        {
            
        }

        [Test]
        public void ValidTreeTest()
        {
            
        }
    }
}
