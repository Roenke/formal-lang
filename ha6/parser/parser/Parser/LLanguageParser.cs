using System;
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
        public bool GetTree(string programToParse)
        {
            Parser<Term>[] exprlazy = {null};
            var expr = Prim.Lazy(() => exprlazy[0]);
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

            var number = from n in intLex
                         select new Number(n) as Term;

            var variable = from id in identifier
                           select new Variable(id) as Term;

            var read =
                from r in reserved("read")
                select new OperationIo() as Term;

            var write =
                from w in reserved("write")
                select new OperationIo() as Term;

            var subexpr = 
                from p in parens(expr)
                select new Expression() as Term;

            var factor = from f in @try(number)
                                 | @try(variable)
                                 | subexpr
                         select f;

            var assign =
                from v in identifier
                from op in reservedOp(":=")
                from e in expr
                select new Assign(v, e) as Term;

            var skipStatement = from s in reserved("skip")
                select new SkipStatement() as Term;

            var statement =
                from st in read | write | skipStatement | assign
                select new Statement() as Term;

            var whileDoStatement =
                from _ in reserved("while")
                from e in expr
                from __ in reserved("do")
                from s in statement
                select new WhileDoStatement(e, s) as Term;

            var ifThenElseStatement =
                from _ in reserved("if")
                from e in expr
                from t in reserved("then")
                from st in statement
                from el in reserved("else")
                from se in statement
                select new IfStatement(e as Expression, st as Statement, se as Statement) as Term;

            var fewStatements = from lst in many(
                from sc in reservedOp(";")
                from st in statement | ifThenElseStatement | whileDoStatement
                select new Statement() as Term
                )
                select new Statement(lst) as Term;

            var program = 
                from s1 in statement | ifThenElseStatement | whileDoStatement
                from ts in fewStatements 
                select new Program(ts as Statement) as Term;

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

            var equals = new Infix<T>("==", resOp("=="), Assoc.Left);
            var mult = new Infix<T>("*", resOp("*"), Assoc.Left);
            var divide = new Infix<T>("/", resOp("/"), Assoc.Left);
            var plus = new Infix<T>("+", resOp("+"), Assoc.Left);
            var minus = new Infix<T>("-", resOp("-"), Assoc.Left);
            var lessThan = new Infix<T>("<", resOp("<"), Assoc.Left);

            var binops = new OperatorTable<T>();
            binops.AddRow().Add(equals)
                  .AddRow().Add(mult).Add(divide)
                  .AddRow().Add(plus).Add(minus)
                  .AddRow().Add(lessThan);

            return binops;
        }
    }
}