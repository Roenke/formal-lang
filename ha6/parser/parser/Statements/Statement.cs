using System.Text;
using Monad.Parsec;
using parser.Parser;

namespace parser.Statements
{
    public abstract class Statement : Term
    {
        public abstract void PrettyPrint(StringBuilder sb, int tabCount);

        public abstract void Optimize();

        protected Statement(SrcLoc location = null) : base(location)
        {
        }
    }
}