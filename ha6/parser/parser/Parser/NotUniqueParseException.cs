using System;

namespace parser.Parser
{
    public class NotUniqueParseException : Exception
    {
        public NotUniqueParseException(string msg)
            : base(msg)
        {
            
        }
    }
}