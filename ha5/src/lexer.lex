%{
#include <string.h>
#include <cstdint>
#include <iostream>
#include <fstream>
#include <sstream>
#include <map>
#include "parser.hpp"

const size_t BUFFER_SIZE = 1024;

std::ofstream lexer_log("lexer.log");

size_t position = 1;
size_t line_num = 1;
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

const char* describe_token();
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
	yylval.str = describe_token();
	return kw_types[yytext];
}

{OPERATION} {
	log_token("op");
	position += strlen(yytext);
	yylval.str = describe_token();
	return OPERATION;
}

{VARIABLE} {
	log_token("var");
	position += strlen(yytext);
	yylval.str = describe_token();
	return VARIABLE;
}

{NUMBER} {
	log_token("num");
	position += strlen(yytext);
	yylval.str = describe_token();
	return NUMBER;
}

{ASSIGN} {
	log_token("assign");
	position += strlen(yytext);
	yylval.str = describe_token();
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
	yylval.str = describe_token();
	return SEMICOLON;
}

{UNKNOWN} {
	log_token("unknown");
	++position;
}

%%

const char* describe_token() {
	char* buf = new char[BUFFER_SIZE];
	sprintf(buf, "%s(%zd : %zd)", yytext, line_num, position);
	return buf;
}

void log_position() {
	lexer_log << "(line = " << line_num << ", pos = " << position << ")";
}

void log_semicolon() {
	lexer_log << "semicolon" << "\t\t";
	log_position();
	lexer_log << std::endl;
}

void log_token(const char * type) {
	lexer_log << type << "\t\t" << yytext << "\t\t";
	log_position();
	lexer_log << std::endl;
}
