using Monad.Parsec;
using Monad.Utility;
using parser.Parser;

namespace parser.Statements
{
    public class Statement : Term
    {
        private ImmutableList<Term> _terms;

        public Statement(ImmutableList<Term> lst)
            : base(null)
        {
            _terms = lst;
        }

        public Statement(SrcLoc location = null) : base(location)
        {
        }
    }
}