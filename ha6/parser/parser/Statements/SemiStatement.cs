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

        public override void PrettyPrint(StringBuilder sb)
        {
            _left.PrettyPrint(sb);
            sb.AppendLine();
            _right.PrettyPrint(sb);
        }

        public override void Optimize()
        {
            _left.Optimize();
            _right.Optimize();
        }

        private readonly Statement _left;
        private readonly Statement _right;
    }
}