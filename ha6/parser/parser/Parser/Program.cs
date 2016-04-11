using Monad.Parsec;
using parser.Statements;

namespace parser.Parser
{
    public class Program : Term
    {
        public Program(Statement statement, SrcLoc location = null) : base(location)
        {
            _statement = statement;
        }

        private Statement _statement;
    }
}