using System.Collections.Generic;
using Monad.Parsec.Language;

namespace parser.Parser
{
    public class LLanguageDefinition : EmptyDef
    {
        public LLanguageDefinition()
        {
            ReservedOpNames = new List<string> { "+", "-", "*", "/", "%", "==", "!=", ">", ">=", "<", "<=", "&&", "||", ";",};
            ReservedNames = new List<string> { "skip", "write", "read", "while", "do", "if", "then", "else" , ":=", "enddo", "endif" };
            CaseSensitive = true;
        }
    }
}