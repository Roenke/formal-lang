using System;
using System.Text;
using Monad.Parsec;
using parser.Expressions;

namespace parser.Statements
{
    public class AssignStatement : Statement
    {
        public AssignStatement(SrcLoc location = null) : base(location)
        {
        }

        public AssignStatement(Variable leftVar, Expression rightExpr, SrcLoc location) : base(location)
        {
            _var = leftVar;
            _rightExpression = rightExpr;
        }

        public override void PrettyPrint(StringBuilder sb, int tabCount)
        {
            var tabs = new string('\t', tabCount);
            sb.Append(tabs);
            _var.Print(sb);
            sb.Append(" := ");
            _rightExpression.Print(sb);
        }

        public override void Optimize()
        {
            _rightExpression.Simplify();
        }

        private readonly Variable _var;
        private readonly Expression _rightExpression;
    }
}