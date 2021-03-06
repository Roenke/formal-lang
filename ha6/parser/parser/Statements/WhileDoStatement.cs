﻿using System.Text;
using Monad.Parsec;
using parser.Expressions;
using parser.Optimization;

namespace parser.Statements
{
    public class WhileDoStatement : Statement
    {
        public WhileDoStatement(Expression expr, Statement statement, SrcLoc location) : base(location)
        {
            Condition = expr;
            LoopBody = statement;
        }

        public Expression Condition { get; private set; }
        public Statement LoopBody { get; private set; }

        public override void PrettyPrint(StringBuilder sb, int tabCount)
        {
            var tabs = new string('\t', tabCount);
            sb.Append(tabs).Append("while ");
            Condition.Print(sb);
            sb.AppendLine(" do");
            LoopBody.PrettyPrint(sb, tabCount + 1);
            sb.AppendLine().Append(tabs).Append("enddo");
        }

        public override bool OptimizeExpression(IExpressionOptimizer optimizer)
        {
            var value = Condition.EvalInContext(optimizer);
            if (value.HasValue && value.Value == 0)
            {
                Condition = new Number(0, Condition.Location);
                return false;
            }

            optimizer.PopContext();
            if (Condition.Accept(optimizer))
                Condition = Condition.Optimized;
            LoopBody.OptimizeExpression(optimizer);
            
            return false;
        }

        public override bool OptimizeStatement(IStatementOptimizer optimizer)
        {
            if (LoopBody.OptimizeStatement(optimizer))
            {
                LoopBody = LoopBody.Optimized;
            }

            return optimizer.Visit(this);
        }
    }
}