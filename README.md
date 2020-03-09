# Mini-PL interpreter

Interpreter for Mini-PL language.
This is an assignment for University of Helsinki course on Compilers.

## Token patterns

|     token      | regex or token           | explanation                             |
| :------------: | :----------------------- | :-------------------------------------- |
|  **Keywords**  |
|      VAR       | `var`                    | Variable assignment                     |
|      FOR       | `for`                    | For-loop constructor                    |
|      END       | `end`                    | End of code block                       |
|       IN       | `in`                     | Range specifier                         |
|       DO       | `do`                     | Start of code block                     |
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
|     STRING     | r`\"(\\.\|[^"\\])*\"`    | String constant                         |
|    BOOLEAN     | r`true|false`            | Boolean constants                       |
|   **Other**    |
|     COLON      | `:`                      | Variable type assignment                |
|   SEMICOLON    | `;`                      | End of statement                        |
|     RANGE      | `..`                     | Range of integers                       |
|   LEFT_PAREN   | `(`                      | Start nested expression                 |
|  RIGHT_PAREN   | `)`                      | End nested expression                   |
|     ASSIGN     | `:=`                     | Variable assignment                     |
|      EOF       |                          | End of file                             |
|   SCAN_ERROR   |                          | Unexpected token                        |

## Modified context-free grammar

```
<program>   => <stmts>

<stmts>     => ( <stmt> ";" )*

<stmt>      => "var" IDENT ":" <type> [ ":=" <expr> ]
             | "for" IDENT "in" <expr> ".." <expr> "do"
                   <stmts>
               "end" "for"
             | "read" IDENT
             | "print" <expr>
             | "assert" "(" <expr> ")"
             | <expr>

<type>      => "int" | "string" | "bool"

<expr>      => <assignment>

<assignment>=> IDENT ":=" <assignment>
             | <equality>

<equality>  => <comparison> (("=" | "&") <comparison>)*

<comparison>=> <addition> (("<") <addition>)*

<addition>  => <mult> (("+" | "-") <mult>)*

<mult>      => <unary> (("/" | "*") <unary>)*

<unary>     => "!" (<unary> | <primary>)

<primary>   => | IDENT | INTEGER | STRING | BOOLEAN | "(" <expr> ")"
```
