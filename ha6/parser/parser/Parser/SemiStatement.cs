using Monad.Parsec;
using parser.Statements;

namespace parser.Parser
{
    public class SemiStatement : Term
    {
        public SemiStatement(Statement left, Statement right)
            : base(SrcLoc.EndOfSource)
        {
            _left = left;
            _right = right;
        }

        public Statement Left => _left;
        public Statement Right => _right;

        private Statement _left;
        private Statement _right;
    }
}