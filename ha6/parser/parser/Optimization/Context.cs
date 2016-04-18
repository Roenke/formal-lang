using System;
using System.Collections.Generic;

namespace parser.Optimization
{
    public class Context
    {
        private readonly IDictionary<string, int> _context; 
        public Context()
        {
            _context = new Dictionary<string, int>();
        }

        public bool Contains(string val)
        {
            return _context.ContainsKey(val);
        }

        public int GetValue(string val)
        {
            return _context[val];
        }

        public void Addvalue(string var, int val)
        {
            _context[var] = val;
        }

        public void ClearKey(string var)
        {
            _context.Remove(var);
        }
    }
}