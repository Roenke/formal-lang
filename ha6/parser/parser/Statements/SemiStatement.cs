using System.Text;
using Monad.Parsec;
using parser.Parser;

namespace parser.Statements
{
    public class SemiStatement : Statement
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

        public override void PrettyPrint(StringBuilder sb)
        {
            throw new System.NotImplementedException();
        }

        public override void Optimize()
        {
            throw new System.NotImplementedException();
        }
    }
}