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
        private static Parser<Program> _program;

        public LLanguageParser()
        {
            Init();
        }

        public Program[] GetTree(string programToParse)
        {
            var result = _program.Parse(programToParse);

            return result.Value.Select(x => x.Item1).ToArray();
        }

        private static void Init()
        {
            Parser<Statement>[] termStatement = {null};
            Parser<Expression>[] exprlazy = { null };
            var expr = Prim.Lazy(() => exprlazy[0]);
            Func<Parser<Expression>, Parser<Expression>> @try = Prim.Try;

            var def = new Language();
            var lexer = Tok.MakeTokenParser<Expression>(def);
            var binops = BuildOperatorsTable(lexer);

            var parens = lexer.Parens;
            var reserved = lexer.Reserved;

            var number = from n in lexer.Integer
                      select new Number(n) as Expression;

             var variable = from id in lexer.Identifier
                        select new Variable(id) as Expression;

            var read =
                from r in reserved("read")
                from e in expr
                select new OperationIo(IoOperationType.Read, e, r.Location) as Statement;

            var write =
                from w in reserved("write")
                from e in expr
                select new OperationIo(IoOperationType.Write, e, w.Location) as Statement;

            var skip = from s in reserved("skip")
                    select new SkipStatement(s.Location) as Statement;

            var assign =
                from v in variable
                from op in reserved(":=")
                from e in expr
                select new AssignStatement(v as Variable, e, v.Location) as Statement;

            var whileDo =
                from w in reserved("while")
                from e in expr
                from __ in reserved("do")
                from s in termStatement[0]
                select new WhileDoStatement(e, s, w.Location) as Statement;

            var ifThenElse =
                from i in reserved("if")
                from c in expr
                from then in reserved("then")
                from st1 in termStatement[0]
                from else_ in reserved("else")
                from st2 in termStatement[0]
                select new IfStatement(c, st1, st2, i.Location) as Statement;

            termStatement[0] =
                from s in Prim.Try(skip) | Prim.Try(assign) | Prim.Try(read) | Prim.Try(write) | Prim.Try(whileDo) | Prim.Try(ifThenElse)
                select s;

            Parser<Statement>[] statement = { null };
            var semiStatement =
                from t in termStatement[0]
                from _ in lexer.ReservedOp(";")
                from st in statement[0]
                select new SemiStatement(t, st) as Statement;

            statement[0] =
                from s in Prim.Try(semiStatement) | termStatement[0]
                select s;

            var subexpr =
                from p in parens(expr)
                select p;

            var factor =
                from f in @try(number) | @try(variable) | subexpr
                select f;

            _program =
                from s in statement[0]
                select new Program(s);

            exprlazy[0] = Ex.BuildExpressionParser(binops, factor);
        }

        private static OperatorTable<Expression> BuildOperatorsTable<T>(GenTokenParser<T> lexer)
            where T : Expression
        {
            Func<Expression, Expression, ReservedOpToken, BinOperation> fn =
                    (lhs, rhs, op) => new BinOperation(lhs, rhs, new Operation(op, op.Location));

            Func<ReservedOpToken, Func<Expression, Expression, Expression>> binop =
                    op => ((lhs, rhs) => fn(lhs, rhs, op));

            Func<string, Parser<Func<Expression, Expression, Expression>>> resOp =
                    name => from op in lexer.ReservedOp(name) select binop(op);

            var eq = new Infix<Expression>("==", resOp("=="), Assoc.None);
            var neq = new Infix<Expression>("!=", resOp("!="), Assoc.None);
            var lt = new Infix<Expression>("<", resOp("<"), Assoc.None);
            var le = new Infix<Expression>("<=", resOp("<="), Assoc.None);
            var gt = new Infix<Expression>(">", resOp(">"), Assoc.None);
            var ge = new Infix<Expression>(">=", resOp(">="), Assoc.None);
            var mult = new Infix<Expression>("*", resOp("*"), Assoc.Left);
            var divide = new Infix<Expression>("/", resOp("/"), Assoc.Left);
            var module = new Infix<Expression>("%", resOp("%"), Assoc.Left);
            var land = new Infix<Expression>("&&", resOp("&&"), Assoc.Left);
            var lor = new Infix<Expression>("||", resOp("||"), Assoc.Left);

            var plus = new Infix<Expression>("+", resOp("+"), Assoc.Left);
            var minus = new Infix<Expression>("-", resOp("-"), Assoc.Left);

            var binops = new OperatorTable<Expression>();
            binops.AddRow()
                .AddRow().Add(mult).Add(divide).Add(module)
                .AddRow().Add(plus).Add(minus)
                .AddRow().Add(eq).Add(neq).Add(lt).Add(le).Add(gt).Add(ge)
                .AddRow().Add(land)
                .AddRow().Add(lor);

            return binops;
        }
    }
}