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
    }
}
