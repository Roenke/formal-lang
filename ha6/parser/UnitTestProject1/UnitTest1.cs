using NUnit.Framework;
using parser.Parser;

namespace Tests
{
    public class ParserTests
    {
        [Test]
        public void SimpleTest()
        {
            var parser = new LLanguageParser();
            Assert.True(parser.GetTree("skip"));
            Assert.False(parser.GetTree("unknown"));
            Assert.True(parser.GetTree("x := 1"));
            Assert.True(parser.GetTree("if 1 < 3 then x := 3 else skip"));
            Assert.True(parser.GetTree("skip ; skip"));
            Assert.True(parser.GetTree("while x < 10 do x := x + 10 * 120"));
            Assert.True(parser.GetTree("if 1 < 3 then x := 3 else skip; while x < 10 do x := x + 10 * 120; skip"));
        }

        [Test]
        public void OperatorsTest()
        {
            var parser = new LLanguageParser();
            Assert.True(parser.GetTree("x := 1 + 2 * 4 / 3 % 3 - 1"));
            Assert.True(parser.GetTree("x := y == 2 && p == 3 || y != 2 && p != 4"));
            Assert.True(parser.GetTree("x := z < 3 || x > 6 || t <= 3 && v >= 3"));
        }
    }
}
