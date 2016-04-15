using System;
using System.Linq;
using Monad.Parsec;
using Monad.Parsec.Expr;
using Monad.Parsec.Token;
using parser.Expressions;
using parser.Statements;

namespace parser.Parser
{
    public class LLanguageParser
    {
        // Terminal statements
        private static Parser<Statement> _skip;
        private static Parser<Statement> _read;
        private static Parser<Statement> _write;
        private static Parser<Statement> _assign;
        private static Parser<Statement> _whileDo;
        private static Parser<Statement> _ifThenElse;

        // Non-terminal statements
        private static Parser<Statement> _termStatement;
        private static Parser<Statement> _statement; 
        private static Parser<Statement> _semiStatement;

        // Expressions
        private static Parser<Term> _number;
        private static Parser<Term> _variable;
        private static Parser<Term> _expr;

        private static Parser<Program> _program; 

        public Program GetTree(string programToParse)
        {
            Parser<Term>[] exprlazy = {null};
            _expr = Prim.Lazy(() => exprlazy[0]);
            Func<Parser<Term>, Parser<Term>> @try = Prim.Try;

            var def = new Language();
            var lexer = Tok.MakeTokenParser<Term>(def);
            var binops = BuildOperatorsTable(lexer);

            var parens = lexer.Parens;
            var reserved = lexer.Reserved;
            var reservedOp = lexer.ReservedOp;

            _number = from n in lexer.Integer
                      select new Number(n) as Term;

            _variable = from id in lexer.Identifier
                        select new Variable(id) as Term;

            _read =
                from r in reserved("read")
                from e in _expr
                select new OperationIo(IoOperationType.Read, e as Expression, r.Location) as Statement;

            _write =
                from w in reserved("write")
                from e in _expr
                select new OperationIo(IoOperationType.Write, e as Expression, w.Location) as Statement;

            _skip = from s in reserved("skip")
                    select new SkipStatement(s.Location) as Statement;

            _assign =
                from v in _variable
                from op in reservedOp(":=")
                from e in _expr
                select new AssignStatement(v as Variable, e, v.Location) as Statement;

            _whileDo =
                from _ in reserved("while")
                from e in _expr
                from __ in reserved("do")
                from s in _termStatement
                select new WhileDoStatement(e, s) as Statement;

            _ifThenElse =
                from _ in reserved("if")
                from c in _expr
                from then in reserved("then")
                from st1 in _termStatement
                from else_ in reserved("else")
                from st2 in _termStatement
                select new IfStatement(null, null, null) as Statement;

            _termStatement =
                from s in _skip | _assign | _read | _write | _whileDo | _ifThenElse
                select s;

            _semiStatement =
                from t in _termStatement
                from _ in reservedOp(";")
                from st in _semiStatement
                select new SemiStatement(t, st) as Statement;

            _statement =
                from s in _semiStatement | _termStatement
                select s;

            var subexpr =
                from p in parens(_expr)
                select p;

            var factor = 
                from f in @try(_number)
                                   | @try(_variable)
                                   | subexpr
                select f;

            _program = 
                from s in _statement
                select new Program(s);

            exprlazy[0] = Ex.BuildExpressionParser(binops, factor);

            var result = _program.Parse(programToParse);

            if (!result.IsFaulted && result.Value.Length > 1)
            {
                throw new NotUniqueParseException("Result of parsing \"" + programToParse + "\" not unique");
            }

            return result.IsFaulted ? null : result.Value.First().Item1;
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