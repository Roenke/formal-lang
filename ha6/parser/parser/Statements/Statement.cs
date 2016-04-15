using System.Text;
using Monad.Parsec;
using parser.Parser;

namespace parser.Statements
{
    public abstract class Statement : Term
    {
        public Statement()
            : base(null)
        {
        }

        public abstract void PrettyPrint(StringBuilder sb);

        public abstract void Optimize();

        public Statement(SrcLoc location = null) : base(location)
        {
        }
    }
}