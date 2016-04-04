BIN = ./bin/
SRC = ./src/

CXX = g++
LEX = lex
BISON = bison

LEXER_GEN = $(BIN)lexer.cpp
LEXER_LEX = $(SRC)lexer.lex

PARSER_SRC = $(SRC)parser.y
PARSER_GEN = $(BIN)parser.cpp

TARGET = ./parser
CXXFLAGS = -std=c++11

all: bin gen
	$(CXX) $(CXXFLAGS) $(LEXER_GEN) -o $(TARGET)

gen: $(LEXER_LEX)
	$(LEX) -o $(LEXER_GEN) $(LEXER_LEX) 

bin:
	mkdir -p bin

clean:
	rm -r $(BIN)*
	rm $(TARGET)

tests: all
	cat tests/1.test && ./lexer < tests/1.test
	cat tests/2.test && ./lexer < tests/2.test