using System.Text;
using Monad.Parsec;
using parser.Parser;

namespace parser.Statements
{
    public class WhileDoStatement : Statement
    {
        public WhileDoStatement(SrcLoc location = null) : base(location)
        {

        }

        public WhileDoStatement(Term location, Term term) : base(SrcLoc.EndOfSource)
        {
            
        }

        public override void PrettyPrint(StringBuilder sb)
        {
            throw new System.NotImplementedException();
        }

        public override void Optimize()
        {
            throw new System.NotImplementedException();
        }
    }
}