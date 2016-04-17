using System.Text;
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

        private readonly Statement _statement;

        public string PrettyPrint()
        {
            var sb = new StringBuilder();
            _statement.PrettyPrint(sb, 0);
            return sb.ToString();
        }
    }
}