# Mini-PL interpreter

Interpreter for Mini-PL language.
This is assignement for University of Helsinki course on Compilers.

## Documentation

### Token patterns

|     token      | regex or token           | explanation                             |
| :------------: | :----------------------- | :-------------------------------------- |
|  **Keywords**  |
|      VAR       | `var`                    | Variable assignment                     |
|      FOR       | `for`                    | For-loop constructor                    |
|      END       | `end`                    | End code block                          |
|       IN       | `in`                     | Range specifier                         |
|       DO       | `do`                     | Start code block                        |
|      READ      | `read`                   | Read from stdin                         |
|     PRINT      | `print`                  | Write to stdout                         |
|      INT       | `int`                    | Integer type                            |
|      STR       | `string`                 | String type                             |
|      BOOL      | `bool`                   | Boolean type                            |
|     ASSERT     | `assert`                 | Program state verification              |
| **Arithmetic** |
|      ADD       | `+`                      | Arithmetic add and string concatenation |
|      SUB       | `-`                      | Arithmetic subtraction                  |
|      MULT      | `*`                      | Arithmetic multiplication               |
|      DIV       | `/`                      | Arithmetic division                     |
|   **Logic**    |
|       LT       | `<`                      | Less-than comparison                    |
|       EQ       | `=`                      | Equality comparison                     |
|      AND       | `&`                      | Logical AND operator                    |
|      NOT       | `!`                      | Logical NOT operator                    |
|  **Literals**  |
|     IDENT      | r`[a-zA-Z][a-zA-Z0-9_]*` | Identifier                              |
|    INTEGER     | r`[0-9]+`                | Integer constant                        |
|     STRING     | r`\"(\\.|[^"\\])*\"`     | String constant                         |
|   **Other**    |
|     COLON      | `:`                      | Variable type assignment(?)             |
|   SEMICOLON    | `;`                      | End of statement                        |
|     RANGE      | `..`                     | Create sequence of integers             |
|   LEFT_PAREN   | `(`                      | Start nested expression                 |
|  RIGHT_PAREN   | `)`                      | End nested expression                   |
|     ASSIGN     | `:=`                     | Variable assignment                     |
|      EOF       |                          | End of file                             |

### Modified context-free grammar

```
<prog>  ::= <stmts>

<stmts> ::= ( <stmt> ";" )+

<stmt>  ::= "var" <var_ident> ":" <type> [ ":=" <expr> ]
        | <var_ident> ":=" <expr>
        | "for" <var_ident> "in" <expr> ".." <expr> "do"
              <stmts>
          "end" "for"
        | "read" <var_ident>
        | "print" <expr>
        | "assert" "(" <expr> ")"

<expr>  ::= <opnd> <op> <opnd>
        | [ <unary_op> ] <opnd>

<opnd>  ::= <int> | <string> | <var_ident> | "(" expr ")"

<op>    ::= "+" | "-" | "*" | "/" | "<" | "=" | "&"

<unary_op> ::= "!"

<type>  ::= "int" | "string" | "bool"

<var_ident> ::= <ident>
```

### Abstract syntax trees

### Error handling

#### In scanner

1. Reject the invalid character
1. Report the error and skip over

#### In parser

#### In semantic analyzer

#### In interpreter

## Work hour log

| Date  | Time (h) | Work done                                                                     |
| :---: | :------- | :---------------------------------------------------------------------------- |
| 6.2.  | 3        | Setting up the project and familiarizing myself with C# and the requirements. |
| 7.2.  | 5        | Writing documentation, reading C# docs and writing basics.                    |
| 8.2.  | 4        | Updating documentation, writing the lexer.                                    |
| 10.2. | 6        | Fixing multiline comment parsing and starting on ASTs.                        |

**Total:** 18h
