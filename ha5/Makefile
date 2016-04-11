BIN = ./bin/
SRC = ./src/

CXX = g++
LEX = lex
BISON = bison

LEXER_GEN = $(BIN)lexer.cpp
LEXER_LEX = $(SRC)lexer.lex

PARSER_SRC = $(SRC)parser.y
PARSER_GEN = $(BIN)parser.cpp

MAIN = $(SRC)main.cpp

TARGET = ./parser
CXXFLAGS = -std=c++11

all: bin gen
	$(CXX) $(CXXFLAGS) $(LEXER_GEN) $(PARSER_GEN) $(MAIN) -o $(TARGET)

gen: $(LEXER_LEX) $(PARSER_SRC)
	$(BISON) -o $(PARSER_GEN) -d -v $(PARSER_SRC)
	$(LEX) -o $(LEXER_GEN) $(LEXER_LEX)

bin:
	mkdir -p bin

clean:
	rm -r $(BIN)*
	rm $(TARGET)
	rm lexer.log

tests: all
	cat tests/1.test && $(TARGET) tests/1.test
