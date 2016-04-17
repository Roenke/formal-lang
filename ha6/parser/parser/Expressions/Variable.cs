using System;
using System.Linq;
using System.Text;
using Monad.Parsec;
using Monad.Parsec.Token;

namespace parser.Expressions
{
    public class Variable : Expression
    {
        public Variable(StringToken id, SrcLoc location = null)
                :
                base(id.Location)
        {
            _name = string.Join("", id.Value.Select(x => x.Value));
        }

        public override void Print(StringBuilder sb)
        {
            sb.Append(_name);
        }

        public override void Simplify()
        {
            throw new System.NotImplementedException();
        }

        private string _name;
    }
}