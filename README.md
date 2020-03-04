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
|      TRUE      | `true`                   | Boolean value true                      |
|     FALSE      | `false`                  | Boolean value false                     |
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
<program>   => <stmts>

<stmts>     => ( <stmt> ";" )*

<stmt>      => "var" IDENT ":" <type> [ ":=" <expr> ]
            | IDENT ":=" <expr>
            | "for" IDENT "in" <expr> ".." <expr> "do"
                  <stmts>
              "end" "for"
            | "read" IDENT
            | "print" <expr>
            | "assert" "(" <expr> ")"
            | <expr>

<type>      => "int" | "string" | "bool"

<expr>      => <equality>

<equality>  => <comparison> (("=" | "&") <comparison>)*

<comparison>=> <addition> (("<") <addition>)*

<addition>  => <mult> (("+" | "-") <mult>)*

<mult>      => <unary> (("/" | "*") <unary>)*

<unary>     => "!" (<unary> | <primary>)

<primary>   => INT | STRING | IDENT | "true" | "false" | "(" <expr> ")"
```

### Abstract syntax trees

### Error handling

#### In scanner

Upon noticing an invalid/unexpected character the scanner rejects that character
and returns a `SCAN_ERROR` token instead.
The scanner also reports the inconsistency using `Program.Error();` with a user-friendly message.
It continues to parse the characters after reporting the error.

#### In parser

Parser defines a `ParseError` which is thrown if the next token is unexpected or if a statement boundary is missing/unexpected.
This stops the execution of the program.

#### In semantic analyzer

#### In interpreter

During the runtime the interpreter checks that the operands are of correct type for the given operator before evaluating the expression.
If they're not the interpreter throws a `RuntimeError` with an error message.
This stops the execution of the program.

## Work hour log

| Date  | Time (h) | Work done                                                                     |
| :---: | :------- | :---------------------------------------------------------------------------- |
| 6.2.  | 3        | Setting up the project and familiarizing myself with C# and the requirements. |
| 7.2.  | 5        | Writing documentation, reading C# docs and writing basics.                    |
| 8.2.  | 4        | Updating documentation, writing the lexer.                                    |
| 10.2. | 6        | Fixing multiline comment parsing and starting on ASTs.                        |
| 11.2. | 2        | Some refactoring and updating documentation.                                  |
| 14.2. | 3        | Big refactor to work with upcoming Parser.                                    |
| 17.2. | 1        | Reading course material. Writing documentation.                               |
| 18.2. | 4        | Parser pretty much working.                                                   |
| 20.2. | 5        | Updating grammar and code according to it.                                    |
| 21.2. | 4        | Updating parser to work with statements.                                      |
| 27.2. | 1        | Implementing tree parsing for the interpreter.                                |
| 28.2. | 4        | Refactoring and reimplementing statements. Begin working on the environment.  |
| 2.3.  | 2        | Working on the interpreter. Fixing bugs and done some cleanup.                |
| 4.3.  | 5        | Finishing up the interpreter and environment. Updating documentation.         |

**Total:** 49h
