%{
#include <iostream>
#include <string>
#include <memory>
#include <cstdint>
#include "parser.hpp"
#include <fstream>
#include <sstream>
#include "../src/tree.hpp"

extern size_t position;
extern size_t line_num;

extern int yylex(void);

extern std::unique_ptr<tree_node> root;

// Set new yyin and return 0, or 
// return 1, and complete all input.
int yywrap()
{
    return 1;
}

void yyerror(const char* message) {
	std::cerr << "Parse error! " << 
	"Message: " << message << std::endl;
	std::cerr << "line = " << line_num << ", pos = " << position << std::endl;
}

%}

%union{
    const char* str;
    struct tree_node* node;
}

%token <str> DO
%token <str> WHILE
%token <str> WRITE
%token <str> READ
%token <str> IF
%token <str> THEN
%token <str> ELSE
%token <str> VARIABLE 
%token <str> NUMBER
%token <str> ASSIGN
%token <str> SEMICOLON
%token <str> OPERATION
%token <str> SKIP
%token <str> ENDIF
%token <str> ENDDO
%token LPAREN
%token RPAREN

%type <node> program
%type <node> statement
%type <node> expr_term
%type <node> expr
%type <node> statement_term

%start program

%%
program:
	statement
	{
		$$ = $1;
		root.reset($$);
	}
	;

expr_term:
	VARIABLE
	{
		$$ = new tree_node(std::string($1));
		delete[] $1;
	}
	|
	NUMBER
	{
		$$ = new tree_node(std::string($1));
		delete[] $1;
	}
	;

expr:
	expr_term
	{
		$$ = $1;
	}
	|
	LPAREN expr OPERATION expr RPAREN
	{
		$$ = new tree_node(std::string($3));
		$$->add_child($2);
		$$->add_child($4);
		delete[] $3;
	}
	;

statement:
	statement_term
	{
		$$ = $1;
	}
	|
	statement SEMICOLON statement_term
	{
		$$ = new tree_node(std::string($2));
		$$->add_child($1);
		$$->add_child($3);
		delete[] $2;
	}
	;

statement_term:
	SKIP 
	{
		$$ = new tree_node(std::string($1));
		delete[] $1;
	}
	|
	VARIABLE ASSIGN expr
	{
		$$ = new tree_node($2);
		auto variable_node = new tree_node($1);
		$$->add_child(variable_node);
		$$->add_child($3);
		delete[] $1; delete[] $2;
	}
	|
	WRITE expr
	{
		$$ = new tree_node(std::string($1));
		$$->add_child($2);
		delete[] $1;
	}
	|
	READ VARIABLE
	{
		$$ = new tree_node(std::string($1));
		$$->add_child(new tree_node(std::string($2)));
		delete[] $1;
	}
	|
	WHILE expr DO statement ENDDO
	{
		$$ = new tree_node(std::string($1) + " " + $3 + " " + $5);
		$$->add_child($2);
		$$->add_child($4);
		delete[] $1;
		delete[] $3;
	}
	|
	IF expr THEN statement ELSE statement ENDIF
	{
		std::stringstream ss;
		ss << $1 << " " << $3 << " " << $5 << " " << $7;
		$$ = new tree_node(ss.str());
		$$->add_child($2);
		$$->add_child($4);
		$$->add_child($6);
		delete[] $1; delete[] $3; delete[] $5;
	}
	;
%%