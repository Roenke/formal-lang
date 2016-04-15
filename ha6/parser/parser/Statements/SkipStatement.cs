using System.Text;
using Monad.Parsec;
using parser.Parser;

namespace parser.Statements
{
    public class SkipStatement : Statement
    {
        public SkipStatement(SrcLoc location = null) : base(location)
        {
        }

        public override void PrettyPrint(StringBuilder sb)
        {
            sb.Append("skip");
        }

        public override void Optimize()
        {
            
        }
    }
}