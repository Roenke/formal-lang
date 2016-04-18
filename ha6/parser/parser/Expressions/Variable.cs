using System;
using System.Text;
using Monad;
using Monad.Parsec;
using Monad.Parsec.Token;
using parser.Optimization;

namespace parser.Expressions
{
    public class Variable : Expression
    {
        public Variable(StringToken id, SrcLoc location = null)
                :
                base(id.Location)
        {
            Name = string.Join("", id.Value.Select(x => x.Value));
        }

        public string Name { get; }

        public override void Print(StringBuilder sb)
        {
            sb.Append(Name);
        }

        public override bool Accept(IExpressionOptimizer optimizer)
        {
            if (!optimizer.Context.Contains(Name)) return false;

            Optimized = new Number(optimizer.Context.GetValue(Name), Location);
            return true;
        }

        public override int? EvalInContext(IExpressionOptimizer optimizer)
        {
            return optimizer.Context.Contains(Name) ? (int?)optimizer.Context.GetValue(Name) : null;
        }
    }
}