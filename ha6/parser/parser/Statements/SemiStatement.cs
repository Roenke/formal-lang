using System.Text;
using Monad.Parsec;
using parser.Optimization;

namespace parser.Statements
{
    public class SemiStatement : Statement
    {
        public SemiStatement(Statement left, Statement right)
            : base(SrcLoc.EndOfSource)
        {
            FirstStatement = left;
            SecondStatement = right;
        }

        public Statement FirstStatement { get; set; }
        public Statement SecondStatement { get; set; }

        public override void PrettyPrint(StringBuilder sb, int tabCount)
        {
            FirstStatement.PrettyPrint(sb, tabCount);
            sb.AppendLine(";");
            SecondStatement.PrettyPrint(sb, tabCount);
        }

        public override bool OptimizeExpression(IExpressionOptimizer optimizer)
        {
            FirstStatement.OptimizeExpression(optimizer);
            SecondStatement.OptimizeExpression(optimizer);
            return false;
        }

        public override bool OptimizeStatement(IStatementOptimizer optimizer)
        {
            if (FirstStatement.OptimizeStatement(optimizer))
                FirstStatement = FirstStatement.Optimized;
            if (SecondStatement.OptimizeStatement(optimizer))
                SecondStatement = SecondStatement.Optimized;

            return optimizer.Visit(this);
        }
    }
}