using Monad.Parsec;
using parser.Parser;

namespace parser.Statements
{
    public class WhileDoStatement : Term
    {
        public WhileDoStatement(SrcLoc location = null) : base(location)
        {
        }

        public WhileDoStatement(Term location, Term term) : base(SrcLoc.EndOfSource)
        {
            
        }
    }
}