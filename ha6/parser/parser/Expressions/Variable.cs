using Monad.Parsec;
using Monad.Parsec.Token;
using parser.Parser;

namespace parser.Expressions
{
    public class Variable : Term
    {
        public IdentifierToken Id;
        public Variable(IdentifierToken id, SrcLoc location = null)
                :
                base(location)
        {
            Id = id;
        }
    }
}