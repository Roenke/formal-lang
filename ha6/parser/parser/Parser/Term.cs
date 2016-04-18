using Monad.Parsec;
using Monad.Parsec.Token;

namespace parser.Parser
{
    public class Term : Token
    {
        public Term(SrcLoc location) : base(location)
        {
        }
    }
}