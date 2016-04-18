using System.Text;
using Monad.Parsec;
using parser.Expressions;
using parser.Optimization;

namespace parser.Statements
{
    public class AssignStatement : Statement
    {
        public AssignStatement(Variable leftVar, Expression rightExpr, SrcLoc location) : base(location)
        {
            Var = leftVar;
            RightPart = rightExpr;
        }

        public Variable Var { get; }
        public Expression RightPart { get; private set; }

        public override void PrettyPrint(StringBuilder sb, int tabCount)
        {
            var tabs = new string('\t', tabCount);
            sb.Append(tabs);
            Var.Print(sb);
            sb.Append(" := ");
            RightPart.Print(sb);
        }

        public override bool OptimizeExpression(IExpressionOptimizer optimizer)
        {
            if (RightPart.Accept(optimizer))
                RightPart = RightPart.Optimized;

            var right = RightPart as Number;
            if (right == null) return false;

            if (!optimizer.IsNested)
                optimizer.Context.Addvalue(Var.Name, right.Value);
            else
                optimizer.Context.ClearKey(Var.Name);

            return false;
        }

        public override bool OptimizeStatement(IStatementOptimizer optimizer)
        {
            return false;
        }
    }
}