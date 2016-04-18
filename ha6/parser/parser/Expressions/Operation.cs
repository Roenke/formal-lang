using System.Collections.Generic;
using System.Linq;
using Monad.Parsec;
using Monad.Parsec.Token;
using parser.Parser;

namespace parser.Expressions
{
    public class Operation : Term
    {
        private static readonly IDictionary<string, BinOp> TokenNameBinOpMapping = new Dictionary<string, BinOp>
        {
            { "+", BinOp.Plus },
            { "-", BinOp.Minus },
            { "*", BinOp.Mul },
            { "/", BinOp.Div },
            { "%", BinOp.Modulo },

            { "==", BinOp.Eq },
            { "!=", BinOp.Neq },
            { "<",  BinOp.Lt },
            { "<=", BinOp.Le },
            { ">",  BinOp.Gt },
            { ">=", BinOp.Ge },
            { "&&", BinOp.And },
            { "||", BinOp.Or }
        };

        public Operation(StringToken opToken, SrcLoc location) : base(location)
        {
            Op = TokenNameBinOpMapping[string.Join("", opToken.Value.Select(x => x.Value))];
        }

        public BinOp Op { get; }

        public override string ToString()
        {
            return TokenNameBinOpMapping.First(x => x.Value == Op).Key;
        }
    }
}