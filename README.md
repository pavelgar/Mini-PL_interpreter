# Mini-PL interpreter

Interpreter for Mini-PL language.
This is assignement for University of Helsinki course on Compilers.

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

## Abstract syntax trees

## Error handling

### In scanner

Upon noticing an invalid or unexpected character (or unterminated string) the scanner
rejects that character and returns a `SCAN_ERROR` token instead.
The scanner also reports the inconsistency using `Program.Error()` with a user-friendly
message.

It continues to parse characters after reporting the error to seek out any other
possible syntax errors.

### In parser

Parser throws `ParseError` if the next token is not what it's expecting or if a
statement boundary is missing/unexpected.
Thrown `ParseError` contains a helpful message and the token which produced the error.
It catches the `ParseError` and tries to sync the program back to the next statement
and continue parsing.

### In semantic analyzer and interpreter

Error handling of the semantic analyzer and the interpreter are bound together.

During the runtime the interpreter checks that the operands are of correct type for the
given operator before evaluating the expression.
If they're not the interpreter throws a `RuntimeError` with an error message.

The interpreter (namely the environment) has checks for program's variable management
such as _Undefined variable_, _Variable already defined_,
_No assignment to control variable_, etc. which also throw a `RuntimeError` with a
helpful message about the error.

Assert statement also throws a `RuntimeError` if the asserted expression evaluates to `false`.

The interpreter does not catch these `RuntimeError`s therefore the execution halts on
first such error.

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
| 5.3.  | 4        | Adding variable assignment, boolean type and updating documentation.          |

**Total:** 53h
