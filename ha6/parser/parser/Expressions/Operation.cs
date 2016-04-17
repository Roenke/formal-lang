using Monad.Parsec;
using Monad.Parsec.Token;
using parser.Parser;

namespace parser.Expressions
{
    public class Operation : Term
    {
        public Operation(StringToken opToken, SrcLoc location) : base(location)
        {
            _op = string.Join("", opToken.Value.Select(x => x.Value));
        }

        public override string ToString()
        {
            return _op;
        }

        private readonly string _op;
    }
}