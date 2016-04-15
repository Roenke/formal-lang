using System.Text;
using Monad.Parsec;
using Monad.Parsec.Token;

namespace parser.Expressions
{
    public class Variable : Expression
    {
        public IdentifierToken Id;
        public Variable(IdentifierToken id, SrcLoc location = null)
                :
                base(location)
        {
            Id = id;
        }

        public override void Print(StringBuilder sb)
        {
            throw new System.NotImplementedException();
        }

        public override void Simplify()
        {
            throw new System.NotImplementedException();
        }
    }
}