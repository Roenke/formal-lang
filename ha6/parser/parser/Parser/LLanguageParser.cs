using System;
using System.Net.Sockets;
using Monad.Parsec;
using Monad.Parsec.Expr;
using Monad.Parsec.Token;
using Monad.Utility;
using parser.Expressions;
using parser.Statements;

namespace parser.Parser
{
    public class LLanguageParser
    {
        // Terminal statements
        private static Parser<Term> _skip;
        private static Parser<Term> _read;
        private static Parser<Term> _write;
        private static Parser<Term> _assign;
        private static Parser<Term> _whileDo;
        private static Parser<Term> _ifThenElse;

        // Non-terminal statements
        private static Parser<Term> _termStatement;
        private static Parser<Term> _statement; 
        private static Parser<Term> _semiStatement;

        // Expressions
        private static Parser<Term> _number;
        private static Parser<Term> _variable;
        private static Parser<Term> _expr; 

        public bool GetTree(string programToParse)
        {
            Parser<Term>[] exprlazy = {null};
            _expr = Prim.Lazy(() => exprlazy[0]);
            Func<Parser<Term>, Parser<ImmutableList<Term>>> many = Prim.Many;
            Func<Parser<Term>, Parser<Term>> @try = Prim.Try;

            var def = new Language();
            var lexer = Tok.MakeTokenParser<Term>(def);
            var binops = BuildOperatorsTable(lexer);

            var intLex = lexer.Integer;
            var parens = lexer.Parens;
            var identifier = lexer.Identifier;
            var reserved = lexer.Reserved;
            var reservedOp = lexer.ReservedOp;

            _read =
                from r in reserved("read")
                select new OperationIo() as Term;

            _write =
                from w in reserved("write")
                select new OperationIo() as Term;

            _skip = from s in reserved("skip")
                    select new SkipStatement() as Term;

            _assign =
                from v in identifier
                from op in reservedOp(":=")
                from e in _expr
                select new Assign(v, e) as Term;

            _whileDo =
                from _ in reserved("while")
                from e in _expr
                from __ in reserved("do")
                from s in _statement
                select new Statement() as Term;

            _ifThenElse =
                from _ in reserved("if")
                from c in _expr
                from then in reserved("then")
                from st1 in _statement
                from else_ in reserved("else")
                from st2 in _statement
                select new IfStatement(null, null, null) as Term;

            _termStatement =
                from s in _skip | _assign | _read | _write | _whileDo | _ifThenElse
                select new Statement() as Term;

            _semiStatement =
                from t in _termStatement
                from _ in reservedOp(";")
                from st in _semiStatement
                select new Statement() as Term;

            _statement =
                from s in _semiStatement | _termStatement
                select new Statement() as Term;

            _number = from n in intLex
                      select new Number(n) as Term;

            _variable = from id in identifier
                        select new Variable(id) as Term;

            var subexpr = 
                from p in parens(_expr)
                select new Expression() as Term;

            var factor = 
                from f in @try(_number)
                                   | @try(_variable)
                                   | subexpr
                select f;

            var program = 
                from s in _statement
                select new Program(s as Statement) as Term;

            exprlazy[0] = Ex.BuildExpressionParser(binops, factor);

            var result = program.Parse(programToParse);

            return !result.IsFaulted;
        }

        private static OperatorTable<T> BuildOperatorsTable<T>(GenTokenParser<T> lexer)
            where T : Token
        {
            Func<T, T, ReservedOpToken, T> fn = 
                    (lhs, rhs, op) => new BinaryOperation(lhs, rhs, op) as T;

            Func<ReservedOpToken, Func<T, T, T>> binop = 
                    op => ((lhs, rhs) => fn(lhs, rhs, op));

            Func<string, Parser<Func<T, T, T>>> resOp = 
                    name => from op in lexer.ReservedOp(name) select binop(op);

            var assign = new Infix<T>(":=", resOp(":="), Assoc.Left);
            var eq = new Infix<T>("==", resOp("=="), Assoc.None);
            var neq = new Infix<T>("!=", resOp("!="), Assoc.None);
            var lt = new Infix<T>("<", resOp("<"), Assoc.None);
            var le = new Infix<T>("<=", resOp("<="), Assoc.None);
            var gt = new Infix<T>(">", resOp(">"), Assoc.None);
            var ge = new Infix<T>(">=", resOp(">="), Assoc.None);
            var mult = new Infix<T>("*", resOp("*"), Assoc.Left);
            var divide = new Infix<T>("/", resOp("/"), Assoc.Left);
            var module = new Infix<T>("%", resOp("%"), Assoc.Left);
            var land = new Infix<T>("&&", resOp("&&"), Assoc.Left);
            var lor = new Infix<T>("||", resOp("||"), Assoc.Left);

            var plus = new Infix<T>("+", resOp("+"), Assoc.Left);
            var minus = new Infix<T>("-", resOp("-"), Assoc.Left);

            var binops = new OperatorTable<T>();
            binops.AddRow().Add(assign)
                .AddRow().Add(mult).Add(divide).Add(module)
                .AddRow().Add(plus).Add(minus)
                .AddRow().Add(eq).Add(neq).Add(lt).Add(le).Add(gt).Add(ge)
                .AddRow().Add(land)
                .AddRow().Add(lor);

            return binops;
        }
    }
}