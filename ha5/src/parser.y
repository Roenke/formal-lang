%{
#include <iostream>
#include <string>
#include "parser.hpp"

extern int yyparse(void);
extern int yylex(void);  

// Set new yyin and return 0, or 
// return 1, and complete all input.
int yywrap()
{
    return 1;
}

void yyerror(const char *s) {
	std::cout << "Parse error!  Message: " << s << std::endl;
}

main()
{
	std::cout << "Start parsing." <<std::endl;
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

expr:
	  VARIABLE
	| NUMBER
	| expr OPERATION expr
	{
		std::cout << "expr" << std::endl;
	}
	;

statement:
	  SKIP 
	| VARIABLE ASSIGN expr 
	| statement SEMICOLON statement
	| WRITE expr
	| READ expr
	| WHILE expr DO expr
	| IF expr THEN expr ELSE expr 
	{
		std::cout << "statement" << std::endl;
	}
	;
%%