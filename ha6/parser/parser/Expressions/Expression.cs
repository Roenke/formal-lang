﻿using System;
using System.Text;
using Monad;
using Monad.Parsec;
using parser.Optimization;
using parser.Parser;

namespace parser.Expressions
{
    public abstract class Expression : Term
    {
        protected Expression(SrcLoc location = null) : base(location)
        {
        }

        public abstract void Print(StringBuilder sb);

        public abstract bool Accept(IExpressionOptimizer optimizer);

        public abstract int? EvalInContext(IExpressionOptimizer optimizer); 

        public Expression Optimized;
    }
}