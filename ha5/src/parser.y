%{
#include <iostream>
#include <string>
#include <memory>
#include <cstdint>
#include "parser.hpp"
#include <fstream>
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

void yyerror(const char *s) {
	std::cerr << "Parse error! " << 
	"Message: " << s << std::endl;
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

%type <node> program
%type <node> statement
%type <node> expr_term
%type <node> expr
%type <node> statement_term

%start program

%%
// TODO: Add delete for lexer buffer;
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
		static size_t local_num = 1;
		$$ = new tree_node(std::string("expr_term") + std::to_string(local_num++));
		$$->add_child($1);
	}
	|
	expr OPERATION expr_term
	{
		$$ = new tree_node("expr " + std::string($2));
		$$->add_child($1);
		$$->add_child($3);
		delete[] $2;
	}
	;

statement:
	statement_term
	{
		static size_t local_num = 1;
		$$ = new tree_node(std::string("statement") + std::to_string(local_num++));
		$$->add_child($1);
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
		$$ = new tree_node(std::string($1) + $2);
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
	READ expr
	{
		$$ = new tree_node(std::string($1));
		$$->add_child($2);
		delete[] $1;
	}
	|
	WHILE expr DO statement_term
	{
		$$ = new tree_node(std::string($1) + " expr " + $3 + " statement_term ");
		$$->add_child($2);
		$$->add_child($4);
		delete[] $1;
		delete[] $3;
	}
	|
	IF expr THEN statement ELSE statement_term 
	{
		$$ = new tree_node(std::string($1) + "expr" + $3 + " statement " + $5 + " statement_term");
		$$->add_child($2);
		$$->add_child($4);
		$$->add_child($6);
		delete[] $1; delete[] $3; delete[] $5;
	}
	;
%%