%{
#include <string.h>
#include <cstdint>
#include <iostream>
uint64_t position = 1;
uint64_t line_num = 1;

const uint32_t TAB_SIZE = 4;

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
}

{OPERATION} {
	log_token("op");
	position += strlen(yytext);
}

{VARIABLE} {
	log_token("var");
	position += strlen(yytext);
}

{NUMBER} {
	log_token("num");
	position += strlen(yytext);
}

{ASSIGN} {
	log_token("assign");
	position += strlen(yytext);
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

int main() {
	yylex();
}
