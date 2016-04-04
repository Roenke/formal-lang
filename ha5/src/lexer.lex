%{
#include <string.h>
#include <cstdint>
#include <iostream>
#include <map>
#include "parser.hpp"

uint64_t position = 1;
uint64_t line_num = 1;
const uint32_t TAB_SIZE = 4;
std::map<std::string, yytokentype> kw_types{
	{ "skip", SKIP },
	{ "do", DO },
	{ "while", WHILE },
	{ "read", READ },
	{ "write", WRITE },
	{ "if", IF },
	{ "then", THEN },
	{ "else", ELSE }
};

void log_semicolon();
void log_token(char const * token_type);

%}

%option ansi-prototypes noyywrap yy_scan_string

KEY_WORD "do"|"while"|"skip"|"read"|"write"|"if"|"then"|"else"
OPERATION \+|-|\*|\/|%|==|!=|<|<=|>|>=|&&|\|\|
VARIABLE  [a-zA-Z][a-zA-Z0-9]*
NUMBER    [0-9]+
ASSIGN    :=
SPACE     [ ]
NEW_LINE  \n
TAB       \t
SEMICOLON ;
UNKNOWN   .

%%

{KEY_WORD} {
	log_token("kw");
	position += strlen(yytext);
	std::cout << "return " << kw_types[yytext] << std::endl;
	return kw_types[yytext];
}

{OPERATION} {
	log_token("op");
	position += strlen(yytext);
	return OPERATION;
}

{VARIABLE} {
	log_token("var");
	position += strlen(yytext);
	return VARIABLE;
}

{NUMBER} {
	log_token("num");
	position += strlen(yytext);
	return NUMBER;
}

{ASSIGN} {
	log_token("assign");
	position += strlen(yytext);
	return ASSIGN;
}

{SPACE} { 
	++position; 
}

{NEW_LINE} { 
	++line_num; 
	position = 1; 
}

{TAB} { 
	position += TAB_SIZE; 
}

{SEMICOLON}  {
	log_semicolon();
	++position;
	return SEMICOLON;
}

{UNKNOWN} {
	log_token("unknown");
	++position;
}

%%

void log_position() {
	std::cout << "(line = " << line_num << ", pos = " << position << ")";
}

void log_semicolon() {
	std::cout << "semicolon" << "\t";
	log_position();
	std::cout << std::endl;
}

void log_token(const char * type) {
	std::cout << type << "\t" << yytext << "\t";
	log_position();
	std::cout << std::endl;
}
