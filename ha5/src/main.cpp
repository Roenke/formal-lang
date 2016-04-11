#include <iostream>
#include "tree.hpp"
extern int yyparse(void);
extern FILE* yyin;
std::unique_ptr<tree_node> root;
void usage(const char* filename) {
    std::cerr << "Usage: " << std::endl << "\t" << filename 
    << " <input-file>" << std::endl;
}

void add_header(std::ostream& os){
    char const * header =
        "digraph AST {\n"
        "rankdir=TB;\n"
        "rotate=90\n"
        "size=\"8,5\"\n"
        "node [shape = circle];\n\n";

    os << header;
}

void add_footer(std::ostream& os) {
    char const * footer = "}\n";

    os << footer;
} 

int main(int argc, char* argv[])
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

    std::ofstream asf_file("ast.dot");
    add_header(asf_file);
    root->serialize(asf_file);
    add_footer(asf_file);
} 