using System.Text;
using Monad.Parsec;
using parser.Expressions;

namespace parser.Statements
{
    public class WhileDoStatement : Statement
    {
        public WhileDoStatement(Expression expr, Statement statement, SrcLoc location) : base(location)
        {
            _condition = expr;
            _statement = statement;
        }

        public override void PrettyPrint(StringBuilder sb)
        {
            sb.Append("while ");
            _condition.Print(sb);
            sb.AppendLine("do");
            _statement.PrettyPrint(sb);
            sb.AppendLine();
        }

        public override void Optimize()
        {
            _condition.Simplify();
            _statement.Optimize();
        }

        private readonly Expression _condition;
        private readonly Statement _statement;
    }
}