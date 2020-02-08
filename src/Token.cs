public enum Type {
    VAR,
    FOR,
    END,
    IN,
    DO,
    READ,
    PRINT,
    INT,
    STR,
    BOOL,
    ASSERT,
    ADD,
    SUB,
    MULT,
    DIV,
    LT,
    EQ,
    AND,
    NOT,
    IDENT,
    INTEGER,
    STRING,
    MLCS,
    MLCE,
    COMMENT,
    SEMI,
    RANGE,
    EXPS,
    EXPE,
    ASSIGN
}

public struct Token {
    public Type type;
    public string value;
    public Token(Type type, string value) {
        this.type = type;
        this.value = value;
    }
}
