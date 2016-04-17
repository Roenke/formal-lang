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

        public override void PrettyPrint(StringBuilder sb, int tabCount)
        {
            var tabs = new string('\t', tabCount);
            sb.Append(tabs).Append("while ");
            _condition.Print(sb);
            sb.AppendLine().Append(tabs).AppendLine("do");
            _statement.PrettyPrint(sb, tabCount + 1);
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