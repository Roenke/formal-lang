%{
#include <iostream>
#include <string>
#include <cstdint>
#include "parser.hpp"
#include <fstream>

extern uint64_t position;
extern uint64_t line_num;
extern int yyparse(void);
extern int yylex(void);

extern FILE* yyin;
void usage(const char* filename) {
	std::cerr << "Usage: " << std::endl << "\t" << filename 
	<< " <input-file>" << std::endl;
}

// Set new yyin and return 0, or 
// return 1, and complete all input.
int yywrap()
{
    return 1;
}

void yyerror(const char *s) {
	std::cerr << "Parse error! " << 
	"Message: " << s << std::endl;
}

main(int argc, char* argv[])
{
	if (argc != 2) {
		usage(argv[0]);
		return 1;
	}

	yyin = fopen(argv[1], "r");
	if(yyin == NULL) {
		std::cerr << "Cannot open input file. Try again." << std::endl;
		usage;
	}

	yyparse();
} 
%}

%token SKIP DO WHILE WRITE READ IF THEN ELSE
%token VARIABLE NUMBER ASSIGN SEMICOLON
%token OPERATION
%%

program:
	statement
	{
		std::cout << "program" << std::endl;
	}
	;

expr_term:
	VARIABLE
	{
		std::cout << "var" << std::endl;
	}
	|
	NUMBER
	{
		std::cout << "num" << std::endl;
	}
	;

expr:
	expr_term
	|
	expr OPERATION expr_term
	{
		std::cout << "expr" << std::endl;
	}
	;

statement:
	statement_term
	|
	statement SEMICOLON statement_term
	{
		std::cout << "few statement" << std::endl;
	}
	;

statement_term:
	SKIP 
	{
		std::cout << "skip" << std::endl;
	}
	|
	VARIABLE ASSIGN expr
	{
		std::cout << "assign" << std::endl;
	}
	|
	WRITE expr
	{
		std::cout << "write" << std::endl;
	}
	|
	READ expr
	{
		std::cout << "read" << std::endl;
	}
	|
	WHILE expr DO statement_term
	{
		std::cout << "while-cycle" << std::endl;
	}
	|
	IF expr THEN statement ELSE statement_term 
	{
		std::cout << "statement" << std::endl;
	}
	;
%%