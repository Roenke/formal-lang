using System.Text;
using Monad.Parsec;
using Monad.Parsec.Token;
using parser.Parser;

namespace parser.Expressions
{
    public class Number : Expression
    {
        public IntegerToken Value;
        public Number(IntegerToken t, SrcLoc location = null)
            : base(location)
        {
            Value = t;
        }

        public override void Print(StringBuilder sb)
        {
            throw new System.NotImplementedException();
        }

        public override void Simplify()
        {
            throw new System.NotImplementedException();
        }
    }
}