using System.Text;
using Monad.Parsec;
using Monad.Parsec.Token;
using parser.Parser;

namespace parser.Expressions
{
    public class Number : Expression
    {
        public Number(IntegerToken t, SrcLoc location = null)
            : base(location)
        {
            _value = t.Value;
        }

        public override void Print(StringBuilder sb)
        {
            sb.Append(_value);
        }

        public override void Simplify()
        {
            throw new System.NotImplementedException();
        }

        private readonly int _value;
    }
}